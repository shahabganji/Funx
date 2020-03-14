using System;
using System.Threading.Tasks;
using FluentAssertions;
using Funx.Extensions;
using Xunit;
using Xunit.Sdk;

namespace Funx.Tests.Extensions
{
    public class AsyncExtensionsSpec
    {
        [Fact]
        public void SafeFireAndForget_should_raise_the_exception()
        {
            Task FuncAsync()
            {
                var tsc = new TaskCompletionSource<string>();

                Task.Run(async () =>
                {
                    await Task.Delay(100)
                        .ContinueWith(x => { tsc.SetException(new InvalidOperationException("invalid")); })
                        .ConfigureAwait(false);
                });

                return tsc.Task;
            }
            
            static void OnException(Exception ex)
            {
                ex.Message.Should().Be("invalid");
                ex.Should().BeAssignableTo<InvalidOperationException>();
            }

            FuncAsync().SafeFireAndForget(false, OnException);
        }
        
        [Fact]
        public void SafeFireAndForget_should_raise_the_exception_for_ValueTask()
        {
            ValueTask FuncAsync()
            {
                var tsc = new TaskCompletionSource<string>();

                Task.Run(async () =>
                {
                    await Task.Delay(100)
                        .ContinueWith(x => { tsc.SetException(new InvalidOperationException("invalid")); })
                        .ConfigureAwait(false);
                });

                return new ValueTask(tsc.Task);
            }
            
            static void OnException(Exception ex)
            {
                ex.Message.Should().Be("invalid");
                ex.Should().BeAssignableTo<InvalidOperationException>();
            }

            FuncAsync().SafeFireAndForget(false, OnException);
        }
    }
}
