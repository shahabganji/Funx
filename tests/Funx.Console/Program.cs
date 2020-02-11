using static System.Console;



using static Funx.Helpers;


namespace Funx.Console {
    
    internal static class Program 
    {
        private static void Main(string[] args)
        {
            Either<string, string> either = Right("right");
            var either2 = Either<string, string>.Left("left");

        }
    }
}
