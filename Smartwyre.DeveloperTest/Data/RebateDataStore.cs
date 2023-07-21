using Microsoft.VisualBasic;
using Smartwyre.DeveloperTest.Types;
using System;
using System.Data.SqlClient;

namespace Smartwyre.DeveloperTest.Data;

public class RebateDataStore
{

    private readonly IDbConnectionWrapper connectionWrapper = new SqlConnectionWrapper();

    

    public Rebate GetRebate(string rebateIdentifier)
    {
        try
        {
            connectionWrapper.Open();

            string getRebateQuery = "SELECT * FROM Rebate WHERE Identifier = @Identifier";
            SqlCommand getRebateCommand = connectionWrapper.CreateCommand();
            getRebateCommand.CommandText = getRebateQuery;
            getRebateCommand.Parameters.AddWithValue("@Identifier", rebateIdentifier);

            SqlDataReader reader = getRebateCommand.ExecuteReader();

            Rebate rebate = null;

            if (reader.Read())
            {
                rebate = new Rebate();
                rebate.Identifier = reader.GetString(reader.GetOrdinal("Identifier"));
                rebate.Amount = reader.GetDecimal(reader.GetOrdinal("Amount"));

                string incentiveTypeString = reader.GetString(reader.GetOrdinal("IncentiveType"));
                if (Enum.TryParse(incentiveTypeString, out IncentiveType incentiveType))
                {
                    rebate.Incentive = incentiveType;
                }
                else
                {
                    // Handle the case when the value in the database is not a valid IncentiveType
                    // You can assign a default value or take appropriate action.
                    rebate.Incentive = IncentiveType.FixedRateRebate;
                }
                rebate.Percentage = reader.GetDecimal(reader.GetOrdinal("Percentage"));
            }

            reader.Close();
            connectionWrapper.Close();

            return rebate;
        }
        catch
        {
            Console.WriteLine("Error : Product Get Request is not Working");
            Environment.Exit(0);
            return null;
        }
        
    }


    public void StoreCalculationResult(Rebate account, decimal rebateAmount)
    {
        try
        {
            connectionWrapper.Open();

            string updateRebateQuery = "UPDATE Rebate SET Amount = @RebateAmount WHERE Identifier = @Identifier";
            using (SqlCommand updateRebate = connectionWrapper.CreateCommand())
            {
                updateRebate.CommandText = updateRebateQuery;
                updateRebate.Parameters.AddWithValue("@RebateAmount", rebateAmount);
                updateRebate.Parameters.AddWithValue("@Identifier", account.Identifier);

                updateRebate.ExecuteNonQuery();
            }

            // Additional code for updating account in the database if necessary

            connectionWrapper.Close();
        }
        catch
        {
            Console.WriteLine("Error : Sorry We can't update Rebate Amount");
        }
        
    }

    public string PostRebate()
    {
        try
        {
            //Rebate
            Rebate rebate = new Rebate();
            connectionWrapper.Open();
            Console.WriteLine("Please Enter Rebate Details:");
            rebate.Identifier = Guid.NewGuid().ToString();
            Console.Write("Amount :");
            rebate.Amount = Decimal.Parse(Console.ReadLine());
            Console.Write("Percentage: ");
            rebate.Percentage = Decimal.Parse(Console.ReadLine());
            Console.WriteLine("Press 1 for Fixed Rate Rebate");
            Console.WriteLine("Press 2 for Amount Per Uom");
            Console.WriteLine("Press 3 for Fixed Cash Amount");
            Console.WriteLine("Press 1-3 for Selection of Incentive Types :");
            int selectIncentive = int.Parse(Console.ReadLine());

            switch (selectIncentive)
            {
                case 1:
                    rebate.Incentive = IncentiveType.FixedRateRebate;
                    break;
                case 2:
                    rebate.Incentive = IncentiveType.AmountPerUom;

                    break;
                case 3:
                    rebate.Incentive = IncentiveType.FixedCashAmount;
                    break;
                default:
                    Console.WriteLine("Invalid Options");
                    break;

            }

            string insertRebateQuery = "INSERT INTO Rebate (Identifier, Amount, IncentiveType, Percentage) VALUES (@Identifier, @Amount, @IncentiveType, @Percentage)";
            using (SqlCommand insertRebateCommand = connectionWrapper.CreateCommand())
            {
                insertRebateCommand.CommandText = insertRebateQuery;
                insertRebateCommand.Parameters.AddWithValue("@Identifier", rebate.Identifier);
                insertRebateCommand.Parameters.AddWithValue("@Amount", rebate.Amount);
                insertRebateCommand.Parameters.AddWithValue("@IncentiveType", rebate.Incentive);
                insertRebateCommand.Parameters.AddWithValue("@Percentage", rebate.Percentage);
                insertRebateCommand.ExecuteNonQuery();
            }
            Console.WriteLine("Rebate Data Inserted");
            Console.Write("Press anything to proceed...");
            Console.ReadLine();
            Console.Clear();
            connectionWrapper.Close();  


            return rebate.Identifier;
        }
        catch
        {
            Console.WriteLine("Invalid Input");
            Environment.Exit(0);
            return null;
        }
    }
}
