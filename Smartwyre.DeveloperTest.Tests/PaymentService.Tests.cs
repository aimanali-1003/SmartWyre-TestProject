using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Smartwyre.DeveloperTest.Tests;

public class PaymentServiceTests
{

    private readonly IDbConnectionWrapper connectionWrapper = new SqlConnectionWrapper();

    
    // Get Product Method Testing()

    [Fact]
    [TestMethod]
    public void GetProduct_ValidIdentifier_ReturnsProduct()
    {
        // Arrange
        var target = new ProductDataStore(); // Replace YourClassName with the actual class containing the GetProduct method
        var expectedProduct = new Product
        {
            Identifier = "86d8eb1d-1c7c-4d76-897e-feb72a7793ea              ",
            Price = 50.00M,
            SupportedIncentives = SupportedIncentiveType.AmountPerUom,
            Uom = "RFT                 "
        };

        // Act
        var actualProduct = target.GetProduct("86d8eb1d-1c7c-4d76-897e-feb72a7793ea              ");

        // Assert
        Assert.AreEqual(expectedProduct.Identifier, actualProduct.Identifier);
        Assert.AreEqual(expectedProduct.Price, actualProduct.Price);
        Assert.AreEqual(expectedProduct.SupportedIncentives, actualProduct.SupportedIncentives);
        Assert.AreEqual(expectedProduct.Uom, actualProduct.Uom);
    }

    [TestMethod]
    [Fact]
    public void GetProduct_InvalidIdentifier_ReturnsNull()
    {
        // Arrange
        var target = new ProductDataStore(); // Replace ProductDataStore with the actual class containing the GetProduct method

        // Act
        var actualProduct = target.GetProduct("86d8eb1d-1c7c-4d76-897e-feb7657862a7793ea");

        // Assert
        Assert.IsNull(actualProduct);
    }

    //  Testing Methods For Rebate 
    [Fact]
    [TestMethod]
    public void GetRebate_ExistingRebate_ReturnsRebateObject()
    {
        // Arrange
        var target = new RebateDataStore(); // Replace YourClassName with the actual class containing the GetProduct method
        var actualRebate = target.GetRebate("b54f02f7-8005-459d-b494-017e9a094f80              ");
        var expectedRebate = new Rebate
        {
            Identifier = "b54f02f7-8005-459d-b494-017e9a094f80              ",
            Amount = actualRebate.Amount,
            Incentive = IncentiveType.AmountPerUom,
            Percentage = 2.00M
        };

        // Act

        // Assert
        Assert.AreEqual(expectedRebate.Identifier, actualRebate.Identifier);
        Assert.AreEqual(expectedRebate.Amount, actualRebate.Amount);
        Assert.AreEqual(expectedRebate.Incentive, actualRebate.Incentive);
        Assert.AreEqual(expectedRebate.Percentage, actualRebate.Percentage);
    }

    [Fact]
    [TestMethod]
    public void GetRebate_NonExistingRebate_ReturnsNull()
    {
        // Arrange
        var target = new RebateDataStore(); // Replace RebateDataStore with the actual class containing the GetRebate method

        // Act
        var actualRebate = target.GetRebate("86d8eb1d-1c7c-4d76-897e-feb7657862a7793ea");

        // Assert
        Assert.IsNull(actualRebate);
    }


    //Test for Calculate Rebate 

    [Fact]
    [TestMethod]
    public void CalculateRebateResult_Caluculation()
    {
        // Arrange
        IRebateService target = new RebateService(); // Replace YourClassName with the actual class containing the RebateService method
        var expectedRebate = new CalculateRebateRequest
        {
            ProductIdentifier = "86d8eb1d-1c7c-4d76-897e-feb72a7793ea              ",
            RebateIdentifier = "b54f02f7-8005-459d-b494-017e9a094f80              ",
            Volume = 60.00M,
        };

        // Act
        var actualRebate = target.Calculate(expectedRebate);

        // Assert
        Assert.AreEqual(true, actualRebate.Success);
    }

    [Fact]
    [TestMethod]
    public void CalculateRebateResult_InvalidCaluculation()
    {
        // Arrange
        IRebateService target = new RebateService(); // Replace YourClassName with the actual class containing the RebateService method
        var expectedRebate = new CalculateRebateRequest
        {
            ProductIdentifier = "86d8eb1d-1c7c-4d76-897e-feb72a7793ea              ",
            RebateIdentifier = "aa802ca6-68f3-447c-b887-61dc0ffdcd9b              ",
            Volume = 60.00M,
        };

        // Act
        var actualRebate = target.Calculate(expectedRebate);

        // Assert
        Assert.AreEqual(false, actualRebate.Success);
    }
}
