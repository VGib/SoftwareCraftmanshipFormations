using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("StrongestOddTests")]

// See https://aka.ms/new-console-template for more information
Console.WriteLine(StrongestOdd.FindStrongestOdd(0, 17778378));

public static class StrongestOdd
{
    public static int FindStrongestOdd(int minValue, int maxValue)
    {
        if (minValue < 0 )
        {
            throw new ArgumentException("minValue or maxValue should not be negative");
        }

        if (maxValue < minValue)
        {
            throw new ArgumentException("minValue should not be greater than maxValue");
        }

        var strongestOddPower = FindOddPower(minValue);
        var strongestOdd = minValue;

        for (int value = minValue + 1; value <= maxValue; value++)
        {
            var thisValueOddPower = FindOddPower(value);

            if (thisValueOddPower > strongestOddPower)
            {
                strongestOddPower = thisValueOddPower;
                strongestOdd = value;
            }

            // Deal with int overflow with maxValue
            if(value == int.MaxValue)
            {
                return strongestOdd;
            }
        }

        return strongestOdd;
    }

    private static int FindOddPower(int value)
    {
        if(value == 0)
        {
            return 0;
        }

        var valueAsBitArray = BitConverter.GetBytes(value);
        var minIndexnotPowerOfTwo = FindMinIndexNotPowerOfTwo(valueAsBitArray);

        var power = 1;

        for (var index = 0; index <= minIndexnotPowerOfTwo; ++index)
        {
            if(index == minIndexnotPowerOfTwo)
            {
                power *= FindOddPowerofByte(valueAsBitArray[index]);
            }
            else
            {
                power *= 256;
            }

        }

        return power == 1 ? 0 : power;
    }

    private static int FindMinIndexNotPowerOfTwo(byte[] valueAsBitArray)
    {
        for (int index = 0; index < valueAsBitArray.Length; index++)
        {
           if(valueAsBitArray[index] != 0)
            {
                return index;
            }
        }

        return valueAsBitArray.Length - 1;

    }

    private static int FindOddPowerofByte(byte value)
    {

        if(value % 128 ==  0)
        {
            return 128;
        }
        if (value % 64 == 0)
        {
            return 64;
        }
        if (value % 32 == 0)
        {
            return 32;
        }
        if (value % 16 == 0)
        {
            return 16;
        }
        if (value % 8 == 0)
        {
            return 8;
        }
        if (value % 4 == 0)
        {
            return 4;
        }
        if (value % 2 == 0)
        {
            return 2;
        }

        return 1;
    }
}
