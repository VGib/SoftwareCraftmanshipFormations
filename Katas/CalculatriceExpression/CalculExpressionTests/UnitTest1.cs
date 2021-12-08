using NUnit.Framework;

public class CalculExpressionTests
{

    [Test]
    public void ShouldDealWith0Digit()
    {
        var result = CalculExpression.Calculate("0+0");
        Assert.AreEqual(0, result);
    }

    [Test]
    public void ShouldDealWith1Digit()
    {
        var result = CalculExpression.Calculate("1+1");
        Assert.AreEqual(2, result);
    }

    [Test]
    public void ShouldDealWith2Digit()
    {
        var result = CalculExpression.Calculate("2+2");
        Assert.AreEqual(4, result);
    }

    [Test]
    public void ShouldDealWith3Digit()
    {
        var result = CalculExpression.Calculate("3+3");
        Assert.AreEqual(6, result);
    }

    [Test]
    public void ShouldDealWith4Digit()
    {
        var result = CalculExpression.Calculate("4+4");
        Assert.AreEqual(8, result);
    }

    [Test]
    public void ShouldDealWith5Digit()
    {
        var result = CalculExpression.Calculate("5+5");
        Assert.AreEqual(10, result);
    }

    [Test]
    public void ShouldDealWith6Digit()
    {
        var result = CalculExpression.Calculate("6+6");
        Assert.AreEqual(12, result);
    }

    [Test]
    public void ShouldDealWith7Digit()
    {
        var result = CalculExpression.Calculate("7+7");
        Assert.AreEqual(14, result);
    }

    [Test]
    public void ShouldDealWith8Digit()
    {
        var result = CalculExpression.Calculate("8+8");
        Assert.AreEqual(16, result);
    }

    [Test]
    public void ShouldDealWith9Digit()
    {
        var result = CalculExpression.Calculate("9+9");
        Assert.AreEqual(18, result);
    }

    [Test]
    public void ShouldDealWithPlus()
    {
        var result = CalculExpression.Calculate("5+3");
        Assert.AreEqual(8, result);
    }

    [Test]
    public void ShouldDealWithMultiplesDigits()
    {
        var result = CalculExpression.Calculate("52+3");
        Assert.AreEqual(55, result);
    }

    [Test]
    public void ShouldDealWithMultiplesDigits_2()
    {
        var result = CalculExpression.Calculate("3+52");
        Assert.AreEqual(55, result);
    }

    [Test]
    public void ShouldDealWithSpace()
    {
        var result = CalculExpression.Calculate("15 + 16");
        Assert.AreEqual(31, result);
    }

    [Test]
    public void ShouldDealWithMinus()
    {
        var result = CalculExpression.Calculate("- 32");
        Assert.AreEqual(-32, result);
    }

    [Test]
    public void ShouldDealWitOnlyNumbers()
    {
        var result = CalculExpression.Calculate("1523");
        Assert.AreEqual(1523, result);
    }

    [Test]
    public void ShouldAddminusNumbers()
    {
        var result = CalculExpression.Calculate("32 + - 5 ");
        Assert.AreEqual(27, result);
    }

    [Test]
    public void ShouldAddMultiplesnumbers()
    {
        var result = CalculExpression.Calculate("7 + -3 + 6 + 8");
        Assert.AreEqual(18, result);
    }

    [Test]
    public void ShouldRetriveNumbers()
    {
        var result = CalculExpression.Calculate("17 - 12 ");
        Assert.AreEqual(5, result);
    }

    [Test]
    public void ShouldAddAndRetriveMultiplesnumbers()
    {
        var result = CalculExpression.Calculate("7  -3 - 4 + 8 - 1 + 8 + 12");
        Assert.AreEqual(27, result);
    }

    [Test]
    public void ShouldDealWithMultiply()
    {
        var result = CalculExpression.Calculate(" 5  * 3");
        Assert.AreEqual(15, result);
    }

    [Test]
    public void ShouldDealWithDevide()
    {
        var result = CalculExpression.Calculate(" 9  / 3");
        Assert.AreEqual(3, result);
    }

    [Test]
    public void ShouldDealWithPriority()
    {
        var result = CalculExpression.Calculate(" 5 + 2 * 3");
        Assert.AreEqual(11, result);
    }

    [Test]
    public void ShouldDealWithPriority_2()
    {
        var result = CalculExpression.Calculate(" 3 * 5 + 2 ");
        Assert.AreEqual(17, result);
    }

    [Test]
    public void MultiplyAndDivideShouldHaveSamePriority()
    {
        var result = CalculExpression.Calculate(" 5  / 2  * 3");
        Assert.AreEqual(6, result);
    }

    [Test]
    public void MultiplyAndDivideShouldHaveSamePriority_2()
    {
        var result = CalculExpression.Calculate(" 2  * 3 / 5");
        Assert.AreEqual(1, result);
    }

    [Test]
    public void ShouldDealWithMultipleAdditionRetrieveAndMultiplicationAndDivisionWithPriority()
    {
        var result = CalculExpression.Calculate(" 2 + 3 * 5 + 7 / 7 - 1");
        Assert.AreEqual(17, result);
    }

    [Test]
    public void ShouldHandleParenthesis()
    {
        var result = CalculExpression.Calculate("( 5 + 2 ) * ( 3 + 1 ) ");
        Assert.AreEqual(28, result);
    }

    [Test]
    public void ShouldHandleParenthesis_2()
    {
        var result = CalculExpression.Calculate("( 5 + 2 ) * ( 3 + 1 ) + 4 ");
        Assert.AreEqual(32, result);
    }

    [Test]
    public void ShouldHandleMultipleParenthesis()
    {
        var result = CalculExpression.Calculate("(( 5 + 2 ) * 3 + 9) / 5 ");
        Assert.AreEqual(6, result);
    }

    [Test]
    public void ShouldHandleMultipleParenthesis_2()
    {
        var result = CalculExpression.Calculate("12 - (5 + ( 5 + 2 ) * 3 + 9) / 5 + (4 * ( 3 - 1 )) ");
        Assert.AreEqual(13, result);
    }
}
