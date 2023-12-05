#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;

namespace Funx;

public interface IResult
{
    bool IsSuccess { get; }
    bool IsFailure { get; }
    ResultStatus Status { get; }
    IReadOnlyCollection<ErrorMessage> Errors { get; }
    IReadOnlyCollection<ResultMessage> Infos { get; }

    void AddInfo(ResultMessage message);
    void AddInfo(string code, string message);
}

internal interface IConvertibleToResult : IResult
{
    internal Result ParentResult { get; }
}

public class Result<T> : IConvertibleToResult
{
    public bool IsSuccess => ParentResult.IsSuccess;
    public bool IsFailure => ParentResult.IsFailure;

    public ResultStatus Status => ParentResult.Status;

    public IReadOnlyCollection<ErrorMessage> Errors => ParentResult.Errors;
    public IReadOnlyCollection<ResultMessage> Infos => ParentResult.Infos;

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public T? Data { get; }

    private Result ParentResult { get; }

    Result IConvertibleToResult.ParentResult => ParentResult;

    internal Result(Result result) =>
        ParentResult = result ?? throw new NullReferenceException(nameof(result));

    internal Result(Result result, T data) : this(result)
        => Data = data ?? throw new NullReferenceException(nameof(data));

    public static implicit operator Result<T>(T data) => new(new Result(), data);
    public static implicit operator Result<T>(ErrorMessage resultMessage) => Result.Fail<T>(resultMessage);
    public static implicit operator Result<T>(ErrorMessage[] errorMessage) => Result.Fail<T>(errorMessage);

    public static implicit operator Result(Result<T> result)
    {
        ArgumentNullException.ThrowIfNull(result);

        return result.ParentResult;
    }

    public Result<T> ToResult(T data) => new(new Result(), data);

    public void AddInfo(ResultMessage message) => ParentResult.AddInfo(message);
    public void AddInfo(string code, string message) => ParentResult.AddInfo(code, message);
}

public enum ResultStatus
{
    Success = 0,
    Failure = 1,
    Conflicted = 2
}

public class Result : IResult
{
    public ResultStatus Status { get; }

    public bool IsSuccess => Status == ResultStatus.Success;
    public bool IsFailure => Status == ResultStatus.Failure;

    private readonly List<ErrorMessage> _messages;
    private readonly List<ResultMessage> _infos;
    public IReadOnlyCollection<ErrorMessage> Errors => _messages;
    public IReadOnlyCollection<ResultMessage> Infos => _infos;

    private Result(params ErrorMessage[] messages)
    {
        Status = ResultStatus.Failure;
        _messages = new List<ErrorMessage>(messages);
        _infos = new List<ResultMessage>();
    }

    private Result(IEnumerable<ErrorMessage>? messages)
    {
        Status = ResultStatus.Failure;
        _messages = messages?.ToList() ?? new List<ErrorMessage>();
        _infos = new List<ResultMessage>();
    }

    internal Result(ResultStatus status = ResultStatus.Success)
    {
        Status = status;
        _messages = new List<ErrorMessage>();
        _infos = new List<ResultMessage>();
    }

    public static Result Success() => new();
    public static Result Conflict() => new(ResultStatus.Conflicted);
    public static Result Fail(params ErrorMessage[] messages) => new(messages);
    public static Result Fail(IEnumerable<ErrorMessage> messages) => new(messages);
    public static Result Fail(string code, string messages) => new(new ErrorMessage(code, messages));

    public static Result<T> Success<T>(T data) => new(new Result(), data);
    public static Result<T> Conflict<T>(T data) => new(new Result(ResultStatus.Conflicted), data);
    public static Result<T> Fail<T>(params ErrorMessage[] messages) => new(messages);
    public static Result<T> Fail<T>(IEnumerable<ErrorMessage> messages) => new(messages.ToArray());

    public static Result<T> Fail<T>(string code, string messages) =>
        new(new Result(new ErrorMessage(code, messages)));


    public static implicit operator Result(ErrorMessage resultMessage) => Fail(resultMessage);
    public static implicit operator Result(ErrorMessage[] errorMessage) => Fail(errorMessage);
    public static Result ToResult(ErrorMessage resultMessage) => new(resultMessage);

    public void AddInfo(ResultMessage message)
    {
        ArgumentNullException.ThrowIfNull(message);
        
        AddInfo(message.Code, message.Message);
    }

    public void AddInfo(string code, string message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(message);

        _infos.Add(new ResultMessage(code, message));
    }
}

public class ResultMessage : IEqualityComparer<ResultMessage>
{
    private const string Separator = "||";
    public string Code { get; private set; }
    public string Message { get; private set; }

    public ResultMessage(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public static bool operator ==(ResultMessage? vm1, ResultMessage vm2) => vm1 is not null && vm1.Equals(vm2);
    public static bool operator !=(ResultMessage vm1, ResultMessage vm2) => !(vm1 == vm2);

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((ResultMessage)obj);
    }

    public bool Equals(ResultMessage? x, ResultMessage? y)
    {
        if (ReferenceEquals(x, y)) return true;
        
        if (x is null) return false;
        if (y is null) return false;
        if (x.GetType() != y.GetType()) return false;

        return x.Code == y.Code;
    }

    private bool Equals(ResultMessage? other) => other is not null && Message == other.Message;

    public int GetHashCode(ResultMessage obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        return Code.GetHashCode();
    }

    public override int GetHashCode() => GetHashCode(this);


    public override string ToString() => $"{Code}{Separator}{Message}";

    public static ResultMessage Parse(string serialized)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(serialized);
        
        var data = serialized.Split(new[] { Separator }, StringSplitOptions.RemoveEmptyEntries);

        if (data.Length < 2)
            throw new Exception($"Invalid error serialization: '{serialized}'");

        return new ResultMessage(data[0], data[1]);
    }
}

public sealed class ErrorMessage : ResultMessage
{
    public ErrorMessage(string code, string message) : base(code, message)
    {
    }
}
