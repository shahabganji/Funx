using static System.Console;


using Funx.Extensions;
using static Funx.Helpers;


namespace Funx.Console {
    
    internal static class Program 
    {
        private static void Main(string[] args) {

            var str = Some("value");

            Option<string> IsValid(string v)
            {
                if (v.StartsWith("v"))
                    return v;

                return None;
            }

            var upperCase = str.Bind(IsValid);

            upperCase.ForEach(WriteLine);


        }
    }
}
