using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Funx.Tests;

public class ResultSpec
{
    [Fact]
    public void A_Result_of_generic_type_has_message()
    {
        var intValueError = new ErrorMessage("int.value", "This is an integer value");

        var sut = Result.Fail<int>(intValueError);

        sut.IsSuccess.Should().BeFalse();
        sut.Errors.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void A_Result_of_generic_type_is_convertible_to_normal_Result()
    {
        var integerResult = Result.Success(11);

        Result sut = integerResult;

        sut.IsSuccess.Should().BeTrue();
        sut.Errors.Should().BeNullOrEmpty();
    }

    [Fact]
    public void A_Null_Result_of_generic_type_throws_an_exception()
    {
        Result<int> integerResult = null;

        var action = () =>
        {
            Result sut = integerResult;
        };

        action.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void A_Result_of_generic_type_with_an_error_is_also_a_normal_Result_with_an_Error()
    {
        var integerResult = Result.Fail<int>(new ErrorMessage("some.error", "some error"));

        Result sut = integerResult;

        sut.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public void Result_has_error_messages_on_failure()
    {
        var failure = new ErrorMessage("failure", "This is a failure message");
        var sut = Result.Fail(failure);

        sut.IsSuccess.Should().BeFalse();
        sut.Errors.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void A_none_null_object_is_a_Result_of_that_objects_type()
    {
        var someInteger = 11;

        Result<int> sut = someInteger;

        sut.IsSuccess.Should().BeTrue();
        sut.IsFailure.Should().BeFalse();
        sut.Data.Should().Be(11);
    }

    [Fact]
    public void ErrorMessage_can_be_cast_to_Result_with_Failure_status()
    {
        var error = new ErrorMessage("error.code", "This is an error message");

        Result result = error;

        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().NotBeNullOrEmpty();
        result.Errors.Count.Should().Be(1);
    }

    [Fact]
    public void ErrorMessage_can_be_cast_to_Generic_Result_of_T_withFailure_status()
    {
        var error = new ErrorMessage("error.code", "This is an error message");

        Result<int> result = error;

        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().NotBeNullOrEmpty();
        result.Errors.Count.Should().Be(1);
    }

    [Fact]
    public void Array_OfErrorMessage_can_be_cast_to_Generic_Result_of_T_withFailure_status()
    {
        var error = new[]
        {
            new ErrorMessage("error.code.1", "This is an error message"),
            new ErrorMessage("error.code.2", "This is another error message"),
        };

        Result<int> resultWithData = error;

        resultWithData.IsSuccess.Should().BeFalse();
        resultWithData.IsFailure.Should().BeTrue();
        resultWithData.Errors.Should().NotBeNullOrEmpty();
        resultWithData.Errors.Count.Should().Be(2);

        Result result = error;

        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().NotBeNullOrEmpty();
        result.Errors.Count.Should().Be(2);
    }

    [Fact]
    public void Fail_will_provide_a_Result_with_failure_status()
    {
        var result = Result.Fail("code", "message");
        var resultWithData = Result.Fail<string>("code", "message");

        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();

        resultWithData.IsFailure.Should().BeTrue();
        resultWithData.IsSuccess.Should().BeFalse();
        resultWithData.Data.Should().BeNull();
    }

    [Fact]
    public void Fail_with_array_will_provide_a_Result_with_failure_status()
    {
        var result = Result.Fail(new ErrorMessage("code1", "message1"), new ErrorMessage("code2", "message2"));
        var resultWithData = Result.Fail<string>(new ErrorMessage("code1", "message1"),
            new ErrorMessage("code2", "message2"));

        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        result.Errors.Count.Should().Be(2);

        resultWithData.IsFailure.Should().BeTrue();
        resultWithData.IsSuccess.Should().BeFalse();
        resultWithData.Data.Should().BeNull();
        resultWithData.Errors.Should().NotBeEmpty();
        resultWithData.Errors.Count.Should().Be(2);
    }

    [Fact]
    public void Fail_with_List_of_errors_will_provide_a_Result_with_failure_status()
    {
        var errors = new List<ErrorMessage>(new[]
        {
            new ErrorMessage("code1", "message1"),
            new ErrorMessage("code2", "message2")
        });
        var result = Result.Fail(errors);
        var resultWithData = Result.Fail<string>(errors);

        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
        result.Errors.Count.Should().Be(2);

        resultWithData.IsFailure.Should().BeTrue();
        resultWithData.IsSuccess.Should().BeFalse();
        resultWithData.Data.Should().BeNull();
        resultWithData.Errors.Should().NotBeEmpty();
        resultWithData.Errors.Count.Should().Be(2);
    }

    [Fact]
    public void Success_will_provide_a_Result_with_success_status()
    {
        var result = Result.Success();
        var resultWithData = Result.Success<string>("data");

        result.IsFailure.Should().BeFalse();
        result.IsSuccess.Should().BeTrue();

        resultWithData.IsFailure.Should().BeFalse();
        resultWithData.IsSuccess.Should().BeTrue();
        resultWithData.Data.Should().Be("data");
    }

    [Fact]
    public void Success_Result_could_have_informational_messages()
    {
        var result = Result.Success(11);
        result.AddInfo("test.message", "This is informational test message");

        result.IsSuccess.Should().BeTrue();
        result.Infos.Count.Should().Be(1);
    }


    [Fact]
    public void Error_Result_could_have_informational_messages()
    {
        var result = Result.Fail<int>("failed.test", "There should be some errors some where :(");
        result.AddInfo("test.message", "This is informational test message");

        result.IsSuccess.Should().BeFalse();
        result.Infos.Count.Should().Be(1);
    }

    [Fact]
    public void Conflict_sets_the_result_status_to_conflicted()
    {
        var result = Result.Conflict();

        result.Status.Should().Be(ResultStatus.Conflicted);
    }

    [Fact]
    public void Conflict_sets_the_result_status_to_conflicted_with_the_conflicted_data()
    {
        var result = Result.Conflict(11);

        result.Status.Should().Be(ResultStatus.Conflicted);
        result.Data.Should().Be(11);
    }

    [Fact]
    public void Implicit_conversion_should_keep_the_status_when_conflict()
    {
        var intResult = Result.Conflict(11);
        Result result = intResult;

        result.Status.Should().Be(ResultStatus.Conflicted);
    }

    [Fact]
    public void Implicit_conversion_should_keep_the_status_when_success()
    {
        var intResult = Result.Success(11);
        Result result = intResult;

        result.IsSuccess.Should().BeTrue();
        result.Status.Should().Be(ResultStatus.Success);
    }

    [Fact]
    public void Implicit_conversion_should_keep_the_status_when_error()
    {
        var intResult = Result.Fail<int>("error", "error happened");
        Result result = intResult;

        result.IsFailure.Should().BeTrue();
        result.Status.Should().Be(ResultStatus.Failure);
    }
}
