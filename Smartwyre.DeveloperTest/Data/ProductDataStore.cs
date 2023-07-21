using Smartwyre.DeveloperTest.Types;
using System;
using System.Data.SqlClient;

namespace Smartwyre.DeveloperTest.Data;

public class ProductDataStore
{
    private readonly IDbConnectionWrapper connectionWrapper = new SqlConnectionWrapper();

    public Product GetProduct(string identifier)
    {
        try
        {
            connectionWrapper.Open();

            string selectProductQuery = "SELECT Identifier, Price, Uom, SupportedIncentive FROM Product WHERE Identifier = @Identifier";

            using (SqlCommand selectCommand = connectionWrapper.CreateCommand())
            {
                selectCommand.CommandText = selectProductQuery;
                selectCommand.Parameters.AddWithValue("@Identifier", identifier);

                using (SqlDataReader reader = selectCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Product product = new Product
                        {
                            Identifier = reader.GetString(0),
                            Price = reader.GetDecimal(1),
                            Uom = reader.GetString(2),
                            SupportedIncentives = (SupportedIncentiveType)Enum.Parse(typeof(SupportedIncentiveType), reader.GetString(3))
                        };
                        return product;
                    }
                }
            }

            return null;
        }
        catch
        {
            Console.WriteLine("Error: Get Request is not working");
            Environment.Exit(0);
            return null;
        }
        finally
        {
            connectionWrapper.Close();
        }
    }





    public string PostProduct()
    {
        try
        {
            Product product = new Product();
            connectionWrapper.Open();
            Console.WriteLine("Please Enter Product Details:");
            product.Identifier = Guid.NewGuid().ToString();
            Console.Write("Price: ");
            product.Price = int.Parse(Console.ReadLine());
            Console.Write("UOM: ");
            product.Uom = Console.ReadLine();
            Console.WriteLine("Press 1 for Fixed Rate Rebate");
            Console.WriteLine("Press 2 for Amount Per Uom");
            Console.WriteLine("Press 3 for Fixed Cash Amount");
            Console.Write("Press 1-3 for Selection of Incentive Types: ");
            int select = int.Parse(Console.ReadLine());

            switch (select)
            {
                case 1:
                    product.SupportedIncentives = SupportedIncentiveType.FixedRateRebate;
                    break;
                case 2:
                    product.SupportedIncentives = SupportedIncentiveType.AmountPerUom;
                    Console.WriteLine(product.SupportedIncentives);
                    break;
                case 3:
                    product.SupportedIncentives = SupportedIncentiveType.FixedCashAmount;
                    break;
                default:
                    Console.WriteLine("Invalid Options");
                    break;
            }

            string insertProductQuery = "INSERT INTO Product (Identifier, Price, Uom, SupportedIncentive) VALUES (@Identifier, @Price, @Uom, @SupportedIncentive)";
            using (SqlCommand insertCommand = connectionWrapper.CreateCommand())
            {
                insertCommand.CommandText = insertProductQuery;
                insertCommand.Parameters.AddWithValue("@Identifier", product.Identifier);
                insertCommand.Parameters.AddWithValue("@Price", product.Price);
                insertCommand.Parameters.AddWithValue("@Uom", product.Uom);
                insertCommand.Parameters.AddWithValue("@SupportedIncentive", product.SupportedIncentives);
                insertCommand.ExecuteNonQuery();
            }

            Console.WriteLine("Product Data Inserted");
            Console.Write("Press anything to proceed...");
            Console.ReadLine();
            Console.Clear();
            connectionWrapper.Close();
            return product.Identifier;
        }
        catch
        {
            Console.WriteLine("Invalid Input");
            Environment.Exit(0);
            return null;
        }
    }



}
