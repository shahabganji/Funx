using System;
using System.Linq;
using FluentAssertions;
using Funx.Extensions;
using Xunit;
using static Funx.Helpers;

namespace Funx.Tests.Extensions
{
    public class ExceptionalExtensionsSpec
    {
        [Fact]
        public void ToValidation_should_change_Exceptional_to_a_Failed_Validation()
        {
            Exceptional<int> exceptional = new InvalidOperationException("invalid");

            var result = exceptional.ToValidation();

            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
            result.Errors.First().Message.Should().Be("invalid");

        }
        
        [Fact]
        public void ToResult_should_change_Exceptional_to_a_Successful_Validation()
        {
            Exceptional<int> exceptional = 1;

            var result = exceptional.ToValidation();

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeNullOrEmpty();
            result.Data.Should().BeOfType(typeof(int));
            result.Data.Should().Be(1);
        }

        [Fact]
        public void Select_should_map_a_success_exceptional_to_another_successful_exceptional()
        {
            Exceptional<int> exceptional = 1;

            var anotherExp = exceptional.Select((i) => i.ToString());

            anotherExp.Should().BeAssignableTo<Exceptional<string>>();
            anotherExp.IsSuccess.Should().BeTrue();
            anotherExp.OnSuccess(vs => vs.Should().Be("1"));
        }
        
        [Fact]
        public void Select_should_map_a_failed_exceptional_to_another_successful_exceptional()
        {
            Exceptional<int> exceptional = new InvalidOperationException("invalid");

            var anotherExp = exceptional.Select((i) => i.ToString());

            anotherExp.Should().BeAssignableTo<Exceptional<string>>();
            anotherExp.IsException.Should().BeTrue();
            anotherExp.OnException(vs =>
            {
                vs.Should().BeOfType<InvalidOperationException>();
                vs.Message.Should().Be("invalid");
            });
        }

        [Fact]
        public void ForEach_should_run_the_action_for_data_when_exceptional_is_success()
        {
            var exceptional = Success(1);

            exceptional.ForEach(i => i.Should().Be(1));
        }

        [Fact]
        public void Bind_should_map_a_successful_exceptional_to_another_exceptional()
        {
            var exceptional = Success(1);

            var mapped = 
                exceptional.Bind<int,Exceptional<string>>(i => new InvalidOperationException(i.ToString()));

            mapped.IsException.Should().BeTrue();
            mapped.OnException(ex =>
            {
                ex.Should().BeOfType<InvalidOperationException>();
                ex.Message.Should().Be("1");
            });
        }
        
        [Fact]
        public void Bind_should_map_a_failed_exceptional_to_another_FAILED_exceptional()
        {
            Exceptional<int> exceptional = new InvalidOperationException("invalid");

            var mapped = exceptional.Bind(i => Success(i.ToString()));

            mapped.IsException.Should().BeTrue();
            mapped.OnException(ex =>
            {
                ex.Should().BeOfType<InvalidOperationException>();
                ex.Message.Should().Be("invalid");
            });
        }

        [Fact]
        public void Bind_should_chain_to_a_None_Option_when_exceptional_is_exception()
        {
            Exceptional<int> ExceptionalFunc() => new InvalidOperationException("invalid");
            Option<string> OptionFunc(int i) => i.ToString();

            var result = ExceptionalFunc()
                .Bind(OptionFunc);

            result.Should().BeAssignableTo<Option<string>>();
            result.IsNone.Should().BeTrue();
            
        }
        
        [Fact]
        public void Bind_should_chain_to_an_Option_when_exceptional_is_success()
        {
            Exceptional<int> ExceptionalFunc() => 1;
            Option<string> OptionFunc(int i) => i.ToString();

            var result = ExceptionalFunc()
                .Bind(OptionFunc);

            result.IsSome.Should().BeTrue();
            result.Should().BeAssignableTo<Option<string>>();
            result.WhenSome(v => v.Should().Be("1"));

        }
        
    }
}
