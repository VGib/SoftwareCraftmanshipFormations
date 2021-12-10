// See https://aka.ms/new-console-template for more information
using System.Globalization;
using System.Text.Json;

Console.WriteLine("initialization");
var moneyBackManager = new MoneyBackManager();

Console.WriteLine("Initialisation");
InitializeMoneyBackManager(moneyBackManager);

do
{

    Console.WriteLine($"Solde : {moneyBackManager.TotalAmount}");
    Console.WriteLine("State:");
    Console.WriteLine(moneyBackManager.State);

    Console.WriteLine("Sum to pay (q to quit)");
    var tmp = Console.ReadLine();

    if(tmp == "q")
    {
        break;
    }

    if(!double.TryParse(tmp, out var toPay))
    {
        Console.WriteLine("incorrect amount");
        continue;
    }

    Console.WriteLine($"You need to pay {toPay}, please input your pieces");
    var payingMoneyBackManager = new MoneyBackManager();
    InitializeMoneyBackManager(payingMoneyBackManager);

    var result = moneyBackManager.Pay(toPay, payingMoneyBackManager.State);

    if(result.IsPaymentAccepted)
    {
        Console.WriteLine("Your payment has been accepted");
        Console.WriteLine("you get the following cashback:");
        Console.WriteLine(result.MoneyBackState);
    }
    else
    {
        Console.WriteLine("Your payment has been refused");
    }

    Console.WriteLine("***********************");

} while (true);

void InitializeMoneyBackManager(MoneyBackManager moneyBackManager)
{
    do
    {
        Console.WriteLine("State:");
        Console.WriteLine(moneyBackManager.State);

        Console.WriteLine("Bill or piece amount (q to quit)");
        var tmp1 = Console.ReadLine();

        if(tmp1 == "q")
        {
            break;
        }

        Console.WriteLine("Number of pieces or bills");
        var tmp2 = Console.ReadLine();

        if(!int.TryParse(tmp2, out var numberOfPieces))
        {
            Console.WriteLine("incorrect number of pieces");
        }

        try
        {
            moneyBackManager.AddMoney(tmp1, numberOfPieces);
        }
        catch (MoneyBackManagerException exception)
        {

            Console.WriteLine($"Something happen: {exception}");
        }

    } while (true);

    Console.WriteLine("Bill or piece amount");

}

[Serializable]
public class MoneyBackManagerException : Exception
{
    public MoneyBackManagerException() { }
    public MoneyBackManagerException(string message) : base(message) { }
    public MoneyBackManagerException(string message, Exception inner) : base(message, inner) { }
    protected MoneyBackManagerException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}

public record MoneyBackResult
{
    public string MoneyBackState { get; init; } = string.Empty;
    public bool IsPaymentAccepted { get; init; }
}

public class MoneyBackManager
{
    private const double usedForRoundingErrorInCalcul = 0.005;
    private int[] numberOfPieces = new int[15];

    private static readonly (int index, double price)[] piecePrices = new[]
    {
        (0,500),
        (1,200),
        (2,100),
        (3,50),
        (4,20),
        (5,10),
        (6,5),
        (7,2),
        (8,1),
        (9,0.5),
        (10,0.2),
        (11,0.1),
        (12,0.05),
        (13, 0.02),
        (14,0.01)
    };

    public MoneyBackManager(string initializationJson)
    {
        var initializationHashtable = JsonSerializer.Deserialize<Dictionary<string, int>>(initializationJson);

        if (initializationHashtable == null)
        {
            throw new MoneyBackManagerException("something goes wrong when parsing json");
        }

        foreach (var value in initializationHashtable)
        {
            AddMoney(value.Key, value.Value);
        }
    }

    public MoneyBackManager()
    {
    }

    public MoneyBackManager(MoneyBackManager paymentAndMyCash)
    {
        numberOfPieces = paymentAndMyCash.numberOfPieces.ToArray();
    }

    public void AddMoney(string value, int number)
    {
        var piece = piecePrices.SingleOrDefault(x => x.price.ToString(NumberFormatInfo.InvariantInfo) == value);

        if (piece == default)
        {
            throw new MoneyBackManagerException($"unknow piece or bill of value {value}");
        }

        numberOfPieces[piece.index] += number;
    }

    private static readonly JsonSerializerOptions SerializationOption = new() {  WriteIndented = true  };
                

    public string State
    {
        get
        {
            var statusHashTable = new Dictionary<string, int>();

            for (int pieceIndex = 0; pieceIndex < piecePrices.Length; pieceIndex++)
            {
                statusHashTable.Add(piecePrices[pieceIndex].price.ToString(NumberFormatInfo.InvariantInfo), numberOfPieces[pieceIndex]);
            }

            return JsonSerializer.Serialize(statusHashTable, SerializationOption);
        }
    }

    public double TotalAmount
    {
        get
        {
            double total = 0;

            for (int index = 0; index < numberOfPieces.Length; index++)
            {
                total += numberOfPieces[index] * piecePrices[index].price;
            }

            return total;
        }
    }

    public MoneyBackResult Pay(double priceToPay, string paymentJson)
    {
        var payment = new MoneyBackManager(paymentJson);

        double totalAmountPayment = payment.TotalAmount;
        if (totalAmountPayment < priceToPay)
        {
            return new MoneyBackResult
            {
                IsPaymentAccepted = false
            };
        }

        var paymentAndmyCash = Join(payment, this);

        (var result, var moneyAfterGivingMoney) =  Give(paymentAndmyCash , totalAmountPayment - priceToPay);

        if(result.IsPaymentAccepted)
        {
#nullable disable
            SetMoney(moneyAfterGivingMoney);
#nullable enable
        }

        return result;
    }

    private void SetMoney(MoneyBackManager moneyBackManager)
    {
        numberOfPieces = moneyBackManager.numberOfPieces.ToArray();
    }

    private static (MoneyBackResult result , MoneyBackManager? moneyAfterGivingMoney)  Give( MoneyBackManager paymentAndMyCash ,double priceToPay)
    {
        var moneyAfterGivingMoney = new MoneyBackManager(paymentAndMyCash);
        var givenMoney = new MoneyBackManager();
        double moneyINeedToGive = priceToPay;

        for (int index = 0; index < givenMoney.numberOfPieces.Length; index++)
        {
            if( piecePrices[index].price <= moneyINeedToGive + usedForRoundingErrorInCalcul && moneyAfterGivingMoney.numberOfPieces[index] > usedForRoundingErrorInCalcul)
            {
                var numberOfPieceToGive = Math.Min((int)(( moneyINeedToGive + usedForRoundingErrorInCalcul) / piecePrices[index].price), moneyAfterGivingMoney.numberOfPieces[index]);

                givenMoney.numberOfPieces[index] += numberOfPieceToGive;
                moneyAfterGivingMoney.numberOfPieces[index] -= numberOfPieceToGive;

                moneyINeedToGive -= piecePrices[index].price * numberOfPieceToGive;
            }
        }

        if(moneyINeedToGive >= 1 - usedForRoundingErrorInCalcul)
        {
            return (new MoneyBackResult { IsPaymentAccepted = false }, null);
        }

        if(moneyINeedToGive > usedForRoundingErrorInCalcul)
        {
            double newPriceToPay = priceToPay + moneyAfterGivingMoney.GetlastPieceICantGive(moneyINeedToGive) - moneyINeedToGive;
            return Give(paymentAndMyCash, newPriceToPay) ;
        }

        return (new MoneyBackResult { IsPaymentAccepted = true, MoneyBackState = givenMoney.State }, moneyAfterGivingMoney);
    }

    private double GetlastPieceICantGive(double moneyIWouldHaveGiven)
    {
        for (var index = numberOfPieces.Length - 1; index > 0 ; --index)
        {
            var missingPiecePriceForCurrentPiece = GetMissingPieceForCurrentPiecePrice(index, moneyIWouldHaveGiven);

            if ( missingPiecePriceForCurrentPiece >= piecePrices[index - 1].price  )
            {
                continue;
            }

            return GetMissingPieceForCurrentPiecePrice(index - 1, moneyIWouldHaveGiven);
        }

        // never used
        return 0;
    }

    private double GetMissingPieceForCurrentPiecePrice(int pieceIndex,  double moneyINeedToGive )
    {
        return (double)((int)(( moneyINeedToGive + usedForRoundingErrorInCalcul) / piecePrices[pieceIndex].price) + 1) * piecePrices[pieceIndex].price;
    }

    private static MoneyBackManager  Join(MoneyBackManager payment, MoneyBackManager moneyBackManager)
    {
        var newMoneyBackManager = new MoneyBackManager();

        for (int index = 0; index < newMoneyBackManager.numberOfPieces.Length; index++)
        {
            newMoneyBackManager.numberOfPieces[index] = payment.numberOfPieces[index] + moneyBackManager.numberOfPieces[index];
        }

        return newMoneyBackManager;
    }
}
