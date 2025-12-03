using System;

public class Program
{
    public static void Main()
    {
        Console.WriteLine("What type of account are you opening? (savings/checking/business): ");
        string accountType = Console.ReadLine().ToLower();

        double interestRate = 0;
        int monthlyFee = 0;

        switch (accountType)
        {
            case "savings":
                interestRate = 0.02;
                Console.WriteLine("Interest rate is 2%");
                break;
            case "checking":
                monthlyFee = 10;
                Console.WriteLine("Monthly fee is $10");
                break;
            case "business":
                interestRate = 0.01;
                monthlyFee = 20;
                Console.WriteLine("Interest rate is 1% and monthly fee is $20");
                break;
            default:
                Console.WriteLine("Invalid account type.");
                break;
        }
    }
}