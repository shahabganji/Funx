// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

using System;

namespace Funx.Analyzer.Sample;

//
// public class Option<T>
// {
//     
// }

public static class Extensions
{
    public static double Bar(this int i , Func<int, Funx.Option<double>> converter)
    {
        var converted = converter(i);
        return converted.UnwrappedValue;
    }
}
public class OptionExample
{

    public Option<int> GetValue(string number)
    {
        var parsed = int.TryParse(number, out var n);
        if (!parsed)
            throw new InvalidOperationException("Could not parse the nu,ber");

        return n;
    }
    
    
    
    public void Foo()
    {
        var x = 12.Bar(x =>
        {
            if (x == o)
                throw new Exception("Exception from lambda");

            return x;
        });
    }
    public static Funx.Option<int> GetValue(int value)
    {
        var option = Funx.Option<int>.Some(11);
        
        if (value % 2 == 0)
        {
            if (value % 3 == 0)
                return Funx.Factories.None;
        }
        else
        {
            while (value % 2 != 0)
            {
                if(value != 0)
                    throw new Exception("Invalid Format");
            }
        }
        
        throw new Exception("Invalid Format");

        Funx.Option<double> Convert(int value)
        {
            if (value == 0)
                throw new Exception("Invalid Format");
            
            return 10.00;
        }
        
        double ConvertToDouble(int value)
        {
            if (value == 0)
                throw new Exception("Invalid operation"); // TODO: think about interceptors :)) 
            
            return 10.00;
        }
        
        return Funx.Factories.None;
    }
}
