using System.Linq;

#nullable disable

// See https://aka.ms/new-console-template for more information
while(true)
{
    Console.WriteLine("Input parameter (q for quit)");
    var input = Console.ReadLine();

    if(input == "q")
    {
        break;
    }

    try
    {
        Console.WriteLine($"result = {CalculExpression.Calculate(input)}");
    }
    catch (CalculExpressionException exception)
    {
        Console.WriteLine($"oh oh something happen: {exception.Message}");
    }
}

[Serializable]
public class CalculExpressionException : Exception
{
    public CalculExpressionException() { }
    public CalculExpressionException(string message) : base(message) { }
    public CalculExpressionException(string message, Exception inner) : base(message, inner) { }
    protected CalculExpressionException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}

public static class CalculExpression
{
    private interface Type
    {
    }

    private struct EmptyType : Type
    {
    }

    private struct BinaryOperation : Type
    {
        public Func<int, int, int> Operator { get; set; }
        public int Priority { get; set; }
    }

    private struct UnaryOperation : Type
    {
        public Func<int, int> Operator { get; set; }
    }

    private struct Value : Type
    {
        public int ValueInt { get; set; }
    }

    private struct Parenthesis : Type
    {
        public bool IsOpen { get; set; }
    }

    private interface DoOperation
    {
        int Calculate();
        bool IsComplete();
    }

    private class EmptyOperation : DoOperation
    {
        public int Calculate()
        {
            throw new CalculExpressionException("incorrect parsing!");
        }

        public bool IsComplete()
        {
            throw new NotImplementedException();
        }
    }

    private class DoBinaryOperation : DoOperation
    {
        public DoOperation Left { get; set; }
        public DoOperation Right { get; set; }

        public BinaryOperation Operator { get; set; }

        public int Calculate()
        {
            return Operator.Operator(Left.Calculate(), Right.Calculate());
        }

        public bool IsComplete()
        {
            return !(Left is EmptyOperation || Right is EmptyOperation);
        }
    }

    private class DoUnaryOperation : DoOperation
    {
        public DoOperation Value { get; set; }

        public UnaryOperation Operator { get; set; }

        public int Calculate()
        {
            return Operator.Operator(Value.Calculate());
        }

        public bool IsComplete()
        {
            return Value is not EmptyOperation;
        }
    }

    private struct ValueDoOperation : DoOperation
    {
        public int Value { get; set; }

        public int Calculate()
        {
            return Value;
        }

        public bool IsComplete()
        {
            return true;
        }
    }

    private class DoParenthesisOperation : DoOperation
    {
        public CurrentOperations Operations { get; set; }

        public bool IsOpen { get; set; }

        public int Calculate()
        {
            if(Operations == null)
            {
                throw new CalculExpressionException("incorrect parsing!");
            }

            return Operations.CreateOperation().Calculate();
        }

        public bool IsComplete()
        {
            return Operations != null;
        }
    }

    private class CurrentOperations
    {
        private readonly List<DoOperation> operations = new();

        public void AddOperation(DoOperation operation)
        {
            operations.Add(operation);
        }

        public DoOperation CreateOperation()
        {
            FillParenthesis();

            FillUnaryOperations();

            FillBinaryOperations(2);
            FillBinaryOperations(1);

            if (operations.Count != 1)
            {
                throw new CalculExpressionException("incorrect parsing!");
            }

            return operations[0];
        }

        private void FillParenthesis()
        {
            bool foundParenthesis = true;
            do
            {
                (var openParenthesis, var firstOpenParenthesisIndex) = SearchOperation<DoParenthesisOperation>(x => x.IsOpen);

                if(openParenthesis is not DoParenthesisOperation thisParenthesis)
                {
                    foundParenthesis = false;
                    continue;
                }

                var nextCloseParenthesisIndex = SearchNextCloseParenthesisIndex(firstOpenParenthesisIndex);

                if(nextCloseParenthesisIndex < 0)
                {
                    foundParenthesis = false;
                    continue;
                }

                thisParenthesis.Operations = new CurrentOperations();

                for (int index = firstOpenParenthesisIndex + 1; index < nextCloseParenthesisIndex; index++)
                {
                    thisParenthesis.Operations.AddOperation(operations[index]);
                }

                operations.RemoveRange(firstOpenParenthesisIndex + 1, nextCloseParenthesisIndex - firstOpenParenthesisIndex);

            } while (foundParenthesis);
        }

        private int SearchNextCloseParenthesisIndex(int firstOpenParenthesisIndex)
        {
            int parenthesisPower = 0;

            for (int index = firstOpenParenthesisIndex + 1; index < operations.Count; index++)
            {
                if(operations[index] is DoParenthesisOperation thisParenthesis)
                {
                    if(thisParenthesis.IsOpen)
                    {
                        ++parenthesisPower;
                    }
                    else
                    {
                        if(parenthesisPower == 0)
                        {
                            return index;
                        }

                        --parenthesisPower;
                    }
                }
            }

            return -1;
        }

        private void FillUnaryOperations()
        {
            DoOperation tmp = new EmptyOperation();
            int index;
            do
            {
                (tmp, index) = SearchOperation<DoUnaryOperation>(_ => true);

                if (tmp is DoUnaryOperation doUnaryOperation)
                {
                    SetUnaryOperation(doUnaryOperation, index);
                }
            }
            while (tmp is DoUnaryOperation);
        }

        private void FillBinaryOperations(int minPriority)
        {
            DoOperation tmp = new EmptyOperation();
            int index;

            do
            {
                (tmp, index) = SearchOperation<DoBinaryOperation>(x => x.Operator.Priority >= minPriority);

                if (tmp is DoBinaryOperation doBinaryOperation)
                {
                    SetBinaryOperation(doBinaryOperation, index);
                }
            }
            while (tmp is DoBinaryOperation);
        }

        private void SetUnaryOperation(DoUnaryOperation tmp, int index)
        {
            // operand right to unary operation

            if(index == operations.Count - 1)
            {
                throw new CalculExpressionException("incorrect parsing!");
            }

            tmp.Value = operations[index + 1];
            operations.RemoveAt(index + 1);
        }

        private void SetBinaryOperation(DoBinaryOperation tmp, int index)
        {
            // operand left and right
           if(index == 0 || index == operations.Count - 1)
            {
                throw new CalculExpressionException("incorrect parsing!");
            }

            tmp.Left = operations[index - 1];
            tmp.Right = operations[index + 1];

            operations.RemoveAt(index - 1);
            operations.RemoveAt(index );
        }

        private (DoOperation tmp, int index) SearchOperation<T>(Predicate<T> acceptOperation) where T : DoOperation
        {
            for (int index = 0; index < operations.Count; index++)
            {
                if(operations[index] is T value && !value.IsComplete() && acceptOperation(value))
                {
                    return (value, index);
                }
            }

            return (new EmptyOperation(), -1);
        }
    }

    public static int Calculate(string expression)
    {
        var  operations = new CurrentOperations();

        foreach (var type in GetTypes(expression))
        {
            if (type is EmptyType)
            {
                continue;
            }

            operations.AddOperation(CreateOperation(type));
        }

        return operations.CreateOperation().Calculate();
    }

    private static DoOperation CreateOperation(Type type)
    {
        if(type is Parenthesis typeAsParenthesis)
        {
            return new DoParenthesisOperation { IsOpen = typeAsParenthesis.IsOpen };
        }
       if(type is Value typeAsValue)
        {
            return new ValueDoOperation { Value = typeAsValue.ValueInt };
        }
       else if(type is UnaryOperation typeAsUnaryOperation)
        {
            return new DoUnaryOperation { Value = new EmptyOperation(), Operator = typeAsUnaryOperation };
        }
       else if (type is BinaryOperation typeAsBinaryOperation )
        {
            return new DoBinaryOperation { Left = new EmptyOperation (), Operator = typeAsBinaryOperation, Right = new EmptyOperation() };
        }

        throw new CalculExpressionException("incorrect parsing! ");
    }

    private static IEnumerable<Type> GetTypes(string expression)
    {
        Type value = new EmptyType();

        foreach (var character in expression)
        {
            switch (character)
            {
                case '(':
                    yield return value;
                    yield return new Parenthesis { IsOpen = true };
                    value = new EmptyType();
                    break;
                case ')':
                    yield return value;
                    yield return new Parenthesis { IsOpen = false };
                    value = new EmptyType();
                    break;
                case '+':
                    yield return value;

                    yield return new BinaryOperation { Operator = (a, b) => a + b, Priority = 1 };
                    value = new EmptyType();
                    break;
                case '*':
                    yield return value;

                    yield return new BinaryOperation { Operator = (a, b) => a * b, Priority = 2 };
                    value = new EmptyType();
                    break;
                case '/':
                    yield return value;

                    yield return new BinaryOperation { Operator = (a, b) => a / b , Priority = 2 };
                    value = new EmptyType();
                    break;
                case '-':

                    yield return value;
                    if (value is Value)
                    {
                        yield return new BinaryOperation { Operator = (a, b) => a - b , Priority = 1 };
                    }
                    else
                    {
                        yield return new UnaryOperation { Operator = a => -a };
                    }
                    value = new EmptyType();
                    break;

                case >= '0' and <= '9':

                    if (value is Value valueAsValue)
                    {
                        value = new Value { ValueInt = valueAsValue.ValueInt * 10 + FromCharToIntDigit(character) };
                    }
                    else
                    {
                        yield return value;
                        value = new Value { ValueInt = FromCharToIntDigit(character) };
                    }

                    break;
            }
        }

        yield return value;
    }

    private static int FromCharToIntDigit(char character)
    {
        return character switch
        {
            '0' => 0,
            '1' => 1,
            '2' => 2,
            '3' => 3,
            '4' => 4,
            '5' => 5,
            '6' => 6,
            '7' => 7,
            '8' => 8,
            '9' => 9,
            _ => throw new CalculExpressionException($"unknow digit {character}")
        };
    }
}


