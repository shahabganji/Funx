using static Funx.Helpers;


namespace Funx.Console {
    
    internal static class Program 
    {
        private static void Main(string[] args)
        {
            var either = Right<string>("right");
            var either2 = Either<string, int>.Left("left");

        }
    }
}
