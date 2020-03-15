using System;
using System.Threading.Tasks;

namespace Funx.Extensions
{
    public static class AsyncExtensions
    {
        public static async void SafeFireAndForget(this Task tsk, bool continueOnCapturedContext = true,
            Action<Exception> onException = null)
        {
            try
            {
                await tsk.ConfigureAwait(continueOnCapturedContext);
            }
            catch (Exception e) when( onException != null)
            {
                onException(e);
            }
        }
        
        public static async void SafeFireAndForget(this ValueTask tsk, bool continueOnCapturedContext = true,
            Action<Exception> onException = null)
        {
            try
            {
                await tsk.ConfigureAwait(continueOnCapturedContext);
            }
            catch (Exception e) when( onException != null)
            {
                onException(e);
            }
        }

    }
}
