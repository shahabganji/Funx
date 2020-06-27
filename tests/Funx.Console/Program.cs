using System;
using Funx.Extensions;
using static System.Console;
using Name = System.String;
using Greeting = System.String;
using PersonalizeGreeting = System.String;



namespace Funx.Console {
    
    internal static class Program 
    {
        private static void Main(string[] args)
        {
            Func<Greeting, Name, PersonalizeGreeting> greet = (gr, name) => $"{gr}, {name}";
            Func<Greeting, Func<Name, PersonalizeGreeting>> greetWith = gr => name => $"{gr}, {name}";

            Name[] names = {"Shahab", "AliAbbas"};

            names
                .Map(g => greet("Hello", g))
                .ForEach((string s) =>  WriteLine(s));


            var greetFormally = greetWith("Good evening");
            names
                .Map(greetFormally)
                .ForEach(Print);
            
            

        }

        static void Print(string s) => WriteLine(s);
    }
}
