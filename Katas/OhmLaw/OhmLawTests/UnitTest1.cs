using NUnit.Framework;
using System;

public class OhmLowTests
{

    [Test]
    public void ShouldCalculateOhmLowWithTensionAndIntensity()
    {
        var result = OhmLow.Calculate("100 V et 10 A");
        Assert.AreEqual("10 Ohm", result);
    }

    [Test]
    public void ShouldCalculateOhmLowWithTensionAndIntensity_2()
    {
        var result = OhmLow.Calculate("5 A et 10 V");
        Assert.AreEqual("2 Ohm", result);
    }

    [Test]
    public void IfCalculatingWithTensionAndZeroIntensityAnErrorShouldBeThrpwn()
    {
        Assert.Throws<OhmLawException>(() => OhmLow.Calculate("100 V et 0 A"));

    }

    [Test]
    public void ShouldCalculateOhmLowWithTensionAndResitance()
    {
        var result = OhmLow.Calculate("100 V et 100 Ohm");
        Assert.AreEqual("1 A", result);
    }

    [Test]
    public void ShouldCalculateOhmLowWithTensionAndResitance_2()
    {
        var result = OhmLow.Calculate("50 Ohm et 2000 V");
        Assert.AreEqual("40 A", result);
    }

    [Test]
    public void ShouldCalculateOhmLowWithResistanceAndIntensity()
    {
        var result = OhmLow.Calculate("10 Ohm et 1 A");
        Assert.AreEqual("10 V", result);
    }

    [Test]
    public void ShouldCalculateOhmLowWithResistanceAndIntensity_2()
    {
        var result = OhmLow.Calculate("1 A et 100 Ohm");
        Assert.AreEqual("100 V", result);
    }

    [Test]
    public void ShouldReconizeExpressionWithAnd()
    {
        var result = OhmLow.Calculate("1 A and 100 Ohm");
        Assert.AreEqual("100 V", result);
    }

    [Test]
    public void ShouldReconizeExpressionWithEt()
    {
        var result = OhmLow.Calculate("1 A et 100 Ohm");
        Assert.AreEqual("100 V", result);
    }


    [Test]
    public void ShouldReconizeExpressionWithSemiColon()
    {
        var result = OhmLow.Calculate("1 A ; 100 Ohm");
        Assert.AreEqual("100 V", result);
    }

    [Test]
    public void ShouldReconizeExpressionWithComma()
    {
        var result = OhmLow.Calculate("1 A , 100 Ohm");
        Assert.AreEqual("100 V", result);
    }

    [Test]
    public void nospaceshouldnotbeanerrorinexpressionreconization()
    {
        var result = OhmLow.Calculate("1A , 100Ohm");
        Assert.AreEqual("100 V", result);
    }

    [Test]
    public void nospaceshouldnotbeanerrorinexpressionreconization_2()
    {
        var result = OhmLow.Calculate("1A,100Ohm");
        Assert.AreEqual("100 V", result);
    }

    [Test]
    public void lotOfSpacesShouldNotbeAnErrorInExpressionReconization()
    {
        var result = OhmLow.Calculate("   1A  et   100Ohm ");
        Assert.AreEqual("100 V", result);
    }

    [Test]
    public void ShouldDealWithDigitAfter0InInput()
    {
        var result = OhmLow.Calculate("5 A et 10 V");
        Assert.AreEqual("2 Ohm", result);
    }

    [Test]
    public void ShouldDealWithNanoInInput()
    {
        var result = OhmLow.Calculate("5 nA et 10 mV");
        Assert.AreEqual("2 MOhm", result);
    }

    [Test]
    public void ShouldDealWithMicroInInput()
    {
        var result = OhmLow.Calculate("5 µA et 10 V");
        Assert.AreEqual("2 MOhm", result);
    }

    [Test]
    public void ShouldDealWithMiliInInput()
    {
        var result = OhmLow.Calculate("5 nA et 10 mV");
        Assert.AreEqual("2 MOhm", result);
    }

    [Test]
    public void ShouldDealWithNoPuissanceInInput()
    {
        var result = OhmLow.Calculate("100 V et 10 A");
        Assert.AreEqual("10 Ohm", result);
    }


    [Test]
    public void ShouldDealWithKiloInput()
    {
        var result = OhmLow.Calculate("5 mA et 10 kOhm");
        Assert.AreEqual("50 V", result);
    }

    [Test]
    public void ShouldDealWithMegaInInput()
    {
        var result = OhmLow.Calculate("5 µA et 10 MOhm");
        Assert.AreEqual("50 V", result);
    }

    [Test]
    public void ShouldDealWithDigitAfter0InOutput()
    {
        var result = OhmLow.Calculate("1 A , 100 Ohm");
        Assert.AreEqual("100 V", result);
    }

    [Test]
    public void ShouldDealWithNanoInOutput()
    {
        var result = OhmLow.Calculate("1 A , 100 nOhm");
        Assert.AreEqual("100 nV", result);
    }

    [Test]
    public void ShouldDealWithMicroInOutput()
    {
        var result = OhmLow.Calculate("1 A , 100 µOhm");
        Assert.AreEqual("100 µV", result);
    }

    [Test]
    public void ShouldDealWithMiliInOutput()
    {
        var result = OhmLow.Calculate("1 mA , 100 Ohm");
        Assert.AreEqual("100 mV", result);
    }

    [Test]
    public void ShouldDealWithNoPuissanceInOutput()
    {
        var result = OhmLow.Calculate("1 mA , 100 kOhm");
        Assert.AreEqual("100 V", result);
    }


    [Test]
    public void ShouldDealWithKiloOutput()
    {
        var result = OhmLow.Calculate("1 A , 10 kOhm");
        Assert.AreEqual("10 kV", result);
    }

    [Test]
    public void ShouldDealWithMegaOutput()
    {
        var result = OhmLow.Calculate("1 A , 10 MOhm");
        Assert.AreEqual("10 MV", result);
    }

    [Test]
    public void OnlyTowDigitAfterCommaShouldBeDisplayed()
    {
        var result = OhmLow.Calculate("10 V , 30 mA");
        Assert.AreEqual("333.33 Ohm", result);
    }


}