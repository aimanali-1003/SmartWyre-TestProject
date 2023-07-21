using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;
using System;

namespace Smartwyre.DeveloperTest.Runner;

class Program
{
     
    static void Main(string[] args)
    {
        try {

        CalculateRebateRequest crr = new CalculateRebateRequest();
        ProductDataStore pds = new ProductDataStore();
        RebateDataStore rds = new RebateDataStore();
        crr.ProductIdentifier = pds.PostProduct();
        crr.RebateIdentifier = rds.PostRebate();
        Console.Write("Please Enter the Volume: ");
        crr.Volume = int.Parse(Console.ReadLine());
        IRebateService rebateService = new RebateService();
        CalculateRebateResult result = rebateService.Calculate(crr);
            if (result.Success)
            {
                Console.WriteLine("Rebate Calculation is Updated");
            }
            else
            {
                Console.WriteLine("Rebate Calculation is not Update due to an issue");
            }
        }
        catch
        {
            Console.WriteLine("Calculate Rebate Request is not working");
        }
    }
}
