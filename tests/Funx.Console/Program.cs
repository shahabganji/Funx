using System;
using System.ComponentModel.Design;
using System.Globalization;
using Funx.Extensions;
using static System.Console;
using Name = System.String;
using Greeting = System.String;
using PersonalizedGreeting = System.String;


namespace Funx.Console {
    
    internal static class Program 
    {
        

        private static void Main(string[] args)
        {
            Func<Greeting, Name, PersonalizedGreeting> greet = (gr, name) => $"{gr}, {name}";
            Func<Greeting, Func<Name, PersonalizedGreeting>> greetWith = gr => name => $"{gr}, {name}";

            Name[] names = {"Shahab", "AliAbbas"};

            names
                .Map(g => greet("Hello", g))
                .ForEach((string s) =>  WriteLine(s));

            var greetFormally = greet.Apply("Good evening, ");
            names
                .Map(greetFormally)
                .ForEach(Print);

            var greetWith2 = greet.Curry();
            var greetInformally = greetWith2("Hello");
            names.Map(greetInformally).ForEach(Print);

        }

        static void Print(string s) => WriteLine(s);
    }
}
