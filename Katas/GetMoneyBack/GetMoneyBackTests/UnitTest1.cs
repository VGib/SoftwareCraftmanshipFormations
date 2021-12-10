using NUnit.Framework;
using System;
using System.Text.Json;

public class GetMoneyBackTests
{

    [Test]
    public void CurrentStateofMoneyShouldBeInitialized()
    {
        var initialization = @"{
  ""500"": 5,
  ""200"": 1,
  ""100"": 6,
  ""50"": 20,
  ""20"": 11,
  ""10"": 5,
  ""5"": 17,
  ""2"": 31,
  ""1"": 29,
  ""0.5"": 150,
  ""0.2"": 721,
  ""0.1"": 600,
  ""0.05"": 1500,
  ""0.02"": 1600,
  ""0.01"": 2500
}";
        var moneyBackGetter = new MoneyBackManager(initialization);
        var status = moneyBackGetter.State;

        Assert.AreEqual(initialization, status);
    }

    [Test]
    public void wrongintializationshouldfail()
    {
        var initialization = @"{
  ""500"": 5,
  ""200"": 1,
  ""100"": 6,
  ""70"" : 30, 
 ""50"": 20,
  ""20"": 11,
  ""10"": 5,
  ""5"": 17,
  ""2"": 31,
  ""1"": 29,
  ""0.5"": 150,
  ""0.2"": 721,
  ""0.1"": 600,
  ""0.05"": 1500,
  ""0.02"": 1600,
  ""0.01"": 2500
}";
        Assert.Throws<MoneyBackManagerException>(() => new MoneyBackManager(initialization));
    }

    [Test]
    public void ICanAddSomeMoneyToCashbackToTheMachine()
    {
        var initialization = @"{
  ""500"": 5,
  ""200"": 1,
  ""100"": 6,
  ""50"": 20,
  ""20"": 11,
  ""10"": 5,
  ""5"": 17,
  ""2"": 31,
  ""1"": 29,
  ""0.5"": 150,
  ""0.2"": 721,
  ""0.1"": 600,
  ""0.05"": 1500,
  ""0.02"": 1600,
  ""0.01"": 2500
}";
        var moneyBackGetter = new MoneyBackManager(initialization);
        moneyBackGetter.AddMoney("20", 14);
        var newState = @"{
  ""500"": 5,
  ""200"": 1,
  ""100"": 6,
  ""50"": 20,
  ""20"": 25,
  ""10"": 5,
  ""5"": 17,
  ""2"": 31,
  ""1"": 29,
  ""0.5"": 150,
  ""0.2"": 721,
  ""0.1"": 600,
  ""0.05"": 1500,
  ""0.02"": 1600,
  ""0.01"": 2500
}";
        Assert.AreEqual(newState, moneyBackGetter.State);
    }

    [Test]
    public void ICantAddMoneyForAWrongPiece()
    {
        var initialization = @"{
  ""500"": 5,
  ""200"": 1,
  ""100"": 6,
  ""50"": 20,
  ""20"": 11,
  ""10"": 5,
  ""5"": 17,
  ""2"": 31,
  ""1"": 29,
  ""0.5"": 150,
  ""0.2"": 721,
  ""0.1"": 600,
  ""0.05"": 1500,
  ""0.02"": 1600,
  ""0.01"": 2500
}";
        var moneyBackGetter = new MoneyBackManager(initialization);
        Assert.Throws<MoneyBackManagerException>(() => moneyBackGetter.AddMoney("15", 30));
    }

    [Test]
    public void IfNotPayingEnougthThePaymentShouldBeReject()
    {
        var initialization = @"{
  ""500"": 5,
  ""200"": 1,
  ""100"": 6,
  ""50"": 20,
  ""20"": 11,
  ""10"": 5,
  ""5"": 17,
  ""2"": 31,
  ""1"": 29,
  ""0.5"": 150,
  ""0.2"": 721,
  ""0.1"": 600,
  ""0.05"": 1500,
  ""0.02"": 1600,
  ""0.01"": 2500
}";
        var moneyBackGetter = new MoneyBackManager(initialization);
        var payment = @"{
""20"" : 1,
""5"" : 2
}";

        var result = moneyBackGetter.Pay(31, payment);
        Assert.IsFalse(result.IsPaymentAccepted);
    }

    [Test]
    public void IfPayingEnougthThePaymentShouldBeAccepted()
    {
        var initialization = @"{
  ""500"": 5,
  ""200"": 1,
  ""100"": 6,
  ""50"": 20,
  ""20"": 11,
  ""10"": 5,
  ""5"": 17,
  ""2"": 31,
  ""1"": 29,
  ""0.5"": 150,
  ""0.2"": 721,
  ""0.1"": 600,
  ""0.05"": 1500,
  ""0.02"": 1600,
  ""0.01"": 2500
}";
        var moneyBackGetter = new MoneyBackManager(initialization);
        var payment = @"{
""20"" : 1,
""5"" : 2
}";

        var result = moneyBackGetter.Pay(30, payment);
        Assert.IsTrue(result.IsPaymentAccepted);
    }

    [Test]
    public void IfPayingEnougthThePaymentShouldBeAccepted_2()
    {
        var initialization = @"{
  ""500"": 5,
  ""200"": 1,
  ""100"": 6,
  ""50"": 20,
  ""20"": 11,
  ""10"": 5,
  ""5"": 17,
  ""2"": 31,
  ""1"": 29,
  ""0.5"": 150,
  ""0.2"": 721,
  ""0.1"": 600,
  ""0.05"": 1500,
  ""0.02"": 1600,
  ""0.01"": 2500
}";
        var moneyBackGetter = new MoneyBackManager(initialization);
        var payment = @"{
""20"" : 1,
""5"" : 2
}";

        var result = moneyBackGetter.Pay(27, payment);
        Assert.IsTrue(result.IsPaymentAccepted);
    }

    [Test]
    public void TheMachineCanCashBack()
    {
        var initialization = @"{
  ""5"": 1,
  ""2"": 3,
  ""1"": 4,
  ""0.5"": 150,
  ""0.2"": 721,
  ""0.1"": 600,
  ""0.05"": 1500,
  ""0.02"": 1600,
  ""0.01"": 2500
}";
        var moneyBackGetter = new MoneyBackManager(initialization);
        var payment = @"{
""20"" : 1,
""5"" : 2
}";

        var result = moneyBackGetter.Pay(27, payment);

        Assert.IsTrue(result.IsPaymentAccepted);
    }

    [Test]
    public void WhenTheMachineCashBackSheHaveClientPaymentRetrivedByGivenMoney()
    {
        var initialization = @"{
  ""50"": 1,
  ""2"": 2,
  ""1"": 1,
  ""0.5"" : 3
}";
        var moneyBackGetter = new MoneyBackManager(initialization);
        var payment = @"{
""20"" : 1,
""5"" : 2
}";

        var result = moneyBackGetter.Pay(27, payment);

        var returnMoney = @"{
  ""500"": 0,
  ""200"": 0,
  ""100"": 0,
  ""50"": 1,
  ""20"": 1,
  ""10"": 0,
  ""5"": 2,
  ""2"": 1,
  ""1"": 0,
  ""0.5"": 3,
  ""0.2"": 0,
  ""0.1"": 0,
  ""0.05"": 0,
  ""0.02"": 0,
  ""0.01"": 0
}";
        Assert.AreEqual(returnMoney, moneyBackGetter.State);
    }

    [Test]
    public void WhenTheMachineCashBackThisGiveBackByPriorityTheMostGreaterPiece()
    {
        var initialization = @"{
  ""5"": 1,
  ""2"": 3,
  ""1"": 4,
  ""0.5"": 150,
  ""0.2"": 721,
  ""0.1"": 600,
  ""0.05"": 1500,
  ""0.02"": 1600,
  ""0.01"": 2500
}";
        var moneyBackGetter = new MoneyBackManager(initialization);
        var payment = @"{
""20"" : 1,
""5"" : 2
}";

        var result = moneyBackGetter.Pay(27, payment);

        var returnMoney = @"{
  ""500"": 0,
  ""200"": 0,
  ""100"": 0,
  ""50"": 0,
  ""20"": 0,
  ""10"": 0,
  ""5"": 0,
  ""2"": 1,
  ""1"": 1,
  ""0.5"": 0,
  ""0.2"": 0,
  ""0.1"": 0,
  ""0.05"": 0,
  ""0.02"": 0,
  ""0.01"": 0
}";
        Assert.AreEqual(returnMoney, result.MoneyBackState);
    }

    [Test]
    public void TheMachineCanNotGiveBackAPieceSheDontHaveForEuros()
    {
        var initialization = @"{
  ""5"": 1,
  ""2"": 3
}";
        var moneyBackGetter = new MoneyBackManager(initialization);
        var payment = @"{
""20"" : 1,
""5"" : 2
}";

        var result = moneyBackGetter.Pay(27, payment);

        Assert.IsFalse(result.IsPaymentAccepted);
    }

    [Test]
    public void WhenNotAcceptingPaymentTheClientCashShouldbeGivenBack()
    {
        var initialization = @"{
  ""5"": 1,
  ""2"": 3
}";
        var moneyBackGetter = new MoneyBackManager(initialization);
        var payment = @"{
""20"" : 1,
""5"" : 2
}";

        var result = moneyBackGetter.Pay(27, payment);

        var returnMoney = @"{
  ""500"": 0,
  ""200"": 0,
  ""100"": 0,
  ""50"": 0,
  ""20"": 0,
  ""10"": 0,
  ""5"": 1,
  ""2"": 3,
  ""1"": 0,
  ""0.5"": 0,
  ""0.2"": 0,
  ""0.1"": 0,
  ""0.05"": 0,
  ""0.02"": 0,
  ""0.01"": 0
}";
        Assert.AreEqual(returnMoney, moneyBackGetter.State);
    }

    [Test]
    public void TheMachinegiveCashBackInSellerDefavorWhenSheDontHaveEnougthCentimes()
    {
        var initialization = @"{
      ""5"": 1,
      ""2"": 3,
        ""1"" : 4,
    ""0.5"": 3,
    ""0.2"" : 4,
    ""0.1"" : 6,
    ""0.05"" : 7
    }";
        var moneyBackGetter = new MoneyBackManager(initialization);
        var payment = @"{
    ""20"" : 1,
    ""5"" : 2
    }";

        var result = moneyBackGetter.Pay(28.82, payment);

        Assert.IsTrue(result.IsPaymentAccepted);

        var returnMoney = @"{
  ""500"": 0,
  ""200"": 0,
  ""100"": 0,
  ""50"": 0,
  ""20"": 0,
  ""10"": 0,
  ""5"": 0,
  ""2"": 0,
  ""1"": 1,
  ""0.5"": 0,
  ""0.2"": 1,
  ""0.1"": 0,
  ""0.05"": 0,
  ""0.02"": 0,
  ""0.01"": 0
}";
        Assert.AreEqual(returnMoney, result.MoneyBackState);
    }

    [Test]
    public void TheMachinegiveCashBackInSellerDefavorWhenSheDontHaveEnougthCentimes_2()
    {
        var initialization = @"{
      ""5"": 1,
      ""2"": 3,
        ""1"" : 4,
    ""0.5"": 3,
    ""0.2"" : 4,
    ""0.1"" : 6,
    ""0.05"" : 7
    }";
        var moneyBackGetter = new MoneyBackManager(initialization);
        var payment = @"{
    ""20"" : 1,
    ""5"" : 2
    }";

        var result = moneyBackGetter.Pay(29.99, payment);

        Assert.IsTrue(result.IsPaymentAccepted);

        var returnMoney = @"{
  ""500"": 0,
  ""200"": 0,
  ""100"": 0,
  ""50"": 0,
  ""20"": 0,
  ""10"": 0,
  ""5"": 0,
  ""2"": 0,
  ""1"": 0,
  ""0.5"": 0,
  ""0.2"": 0,
  ""0.1"": 0,
  ""0.05"": 1,
  ""0.02"": 0,
  ""0.01"": 0
}";
        Assert.AreEqual(returnMoney, result.MoneyBackState);
    }

    [Test]
    public void WhenTheGoodPriceIsPayedThePaymentIsAccepted()
    {
        var initialization = @"{
      ""5"": 1,
      ""2"": 3,
        ""1"" : 4,
    ""0.5"": 3,
    ""0.2"" : 4,
    ""0.1"" : 6,
    ""0.05"" : 7
    }";
        var moneyBackGetter = new MoneyBackManager(initialization);
        var payment = @"{
    ""20"" : 1,
    ""5"" : 2
    }";

        var result = moneyBackGetter.Pay(30, payment);

        Assert.IsTrue(result.IsPaymentAccepted);
    }

    [Test]
    public void WhenTheGoodPriceIsPayedThePaymentNoMoneyIsgivenBack()
    {
        var initialization = @"{
      ""5"": 1,
      ""2"": 3,
        ""1"" : 4,
    ""0.5"": 3,
    ""0.2"" : 4,
    ""0.1"" : 6,
    ""0.05"" : 7
    }";
        var moneyBackGetter = new MoneyBackManager(initialization);
        var payment = @"{
    ""20"" : 1,
    ""5"" : 2
    }";

        var result = moneyBackGetter.Pay(30, payment);

        var returnMoney = @"{
  ""500"": 0,
  ""200"": 0,
  ""100"": 0,
  ""50"": 0,
  ""20"": 0,
  ""10"": 0,
  ""5"": 0,
  ""2"": 0,
  ""1"": 0,
  ""0.5"": 0,
  ""0.2"": 0,
  ""0.1"": 0,
  ""0.05"": 0,
  ""0.02"": 0,
  ""0.01"": 0
}";
        Assert.AreEqual(returnMoney, result.MoneyBackState);
    }
}

