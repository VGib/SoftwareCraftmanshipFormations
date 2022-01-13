// See https://aka.ms/new-console-template for more information
using System.Globalization;
using System.Text.RegularExpressions;

Console.WriteLine("Hello, World!");



[Serializable]
public class OhmLawException : Exception
{
    public OhmLawException() { }
    public OhmLawException(string message) : base(message) { }
    public OhmLawException(string message, Exception inner) : base(message, inner) { }
    protected OhmLawException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}

public static class OhmLow
{
    enum Type
    {
        V,
        A,
        Ohm
    }

    record WorkingValue (double Value, Type Type);

    public static string Calculate(string expression)
    {
        var toCalculate = ReadExpression(expression);
        var workingValue =  Calculate(toCalculate);

        if( double.IsInfinity(workingValue.Value))
        {
            throw new OhmLawException("divising by 0");
        }

        return Format(workingValue);
    }

    private static string Format(WorkingValue workingValue)
    {
        (var valueToDisplay, var unit) = GetValueAsUnit(workingValue.Value); 
        return  FormattableString.Invariant($"{valueToDisplay:###.##} {unit}{workingValue.Type}");
    }

    private static (double valueToDisplay, string unit) GetValueAsUnit(double value)
    {
        return value switch  
        {
            < 1e-6 => (value * 1e9 , "n"),
            < 1e-3 => (value * 1e6, "µ"),
            // Error with 0 , compilator bug ?
            < 0.99999999 => (value * 1e3, "m"),
            < 1e3 => ( value, ""),
            < 1e6 => (value * 1e-3, "k"),
            _ => (value * 1e-6, "M")
        };
        
    }

    private static WorkingValue Calculate(WorkingValue[] toCalculate)
    {
        if (toCalculate[0].Type == Type.V && toCalculate[1].Type == Type.A)
            return new WorkingValue(toCalculate[0].Value / toCalculate[1].Value, Type.Ohm);

        if (toCalculate[0].Type == Type.A && toCalculate[1].Type == Type.V)
            return new WorkingValue(toCalculate[1].Value / toCalculate[0].Value, Type.Ohm);

        if (toCalculate[0].Type == Type.V && toCalculate[1].Type == Type.Ohm)
            return new WorkingValue(toCalculate[0].Value / toCalculate[1].Value, Type.A);

        if (toCalculate[0].Type == Type.Ohm && toCalculate[1].Type == Type.V)
            return new WorkingValue(toCalculate[1].Value / toCalculate[0].Value, Type.A);


        return new WorkingValue(toCalculate[0].Value * toCalculate[1].Value, Type.V);
    }

    private static WorkingValue[] ReadExpression(string expression)
    {
        var match = readingExpressionRegex.Match(expression);

        if( !match.Success)
        {
            throw new OhmLawException($"Incorrect expression: {expression}");
        }

        return new WorkingValue[] { ReadValue(match.Groups["left"].Value), ReadValue(match.Groups["right"].Value) };
    }

    private static WorkingValue ReadValue(string expressionValue)
    {
        var matchExpressionValue = readingValueRegex.Match(expressionValue);

        if(!matchExpressionValue.Success)
        {
            throw new OhmLawException($"Unable to read: {expressionValue}");
        }

        if(!double.TryParse(matchExpressionValue.Groups["Value"].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
        {
            throw new OhmLawException($"Unable to read digit: {expressionValue}");
        }

        if(!Enum.TryParse<Type>(matchExpressionValue.Groups["Type"].Value, out var type))
        {
            throw new OhmLawException($"Unable to read unit: {expressionValue}");
        }

        var puissance = GetPuissance(matchExpressionValue.Groups["Puissance"].Value);

        return new WorkingValue(value * puissance, type);
    }

    private static double GetPuissance(string value)
    {
        return value switch
        {
            "n" => 1e-9,
            "µ" => 1e-6,
            "m" => 1e-3,
            "k" => 1e3,
            "M" => 1e6,
            _ => 1
        };

    }

    private static Regex readingExpressionRegex = new Regex(@"(?<left>\d+\s*(?:n|µ|m|k|M)?(?:V|A|Ohm))\s*(?:et|and|;|,)\s*(?<right>\d+\s*(?:n|µ|m|k|M)?(?:A|V|Ohm))");
    private static Regex readingValueRegex = new Regex(@"(?<Value>\d+)\s*(?<Puissance>n|µ|m|k|M)?(?<Type>.*)");
}
