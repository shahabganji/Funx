using System;
using static Funx.Factories;

namespace Funx.Tests.Mocks
{
    class Age
    {
        public int Value { get; }

        internal static Option<Age> Of(int age)
        {
            if (age <= 0 || age > 120)
                return None;
//                throw new ArgumentOutOfRangeException();
            return Some(new Age(age));
        }

        internal static Age From(int age)
        {
            if (age <= 0 || age > 120)
                throw new ArgumentOutOfRangeException();

            return new Age(age);
        }

        private Age()
        {
        }

        private Age(int age)
        {
            this.Value = age;
        }
    }
}