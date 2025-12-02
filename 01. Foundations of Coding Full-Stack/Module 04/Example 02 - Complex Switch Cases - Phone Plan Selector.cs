using System;

public class Program
{
    public static void Main()
    {
        Console.WriteLine("Enter your data usage (in GB): ");
        double dataUsage = Convert.ToDouble(Console.ReadLine());

        Console.WriteLine("Do you want international calling? (yes/no): ");
        string intlCallingInput = Console.ReadLine();
        bool wantsInternationalCalling = intlCallingInput.ToLower() == "yes";

        string plan = "";

        switch (dataUsage)
        {
            case 2:
                plan = "Basic Plan";
                Console.WriteLine("You are choosing the Basic Plan.");
                break;
            case 5:
                plan = "Standard Plan";
                Console.WriteLine("You are choosing the Standard Plan.");
                break;
            case 10:
                if(wantsInternationalCalling)
                {
                    plan = "Premium Plan";
                    Console.WriteLine("You are choosing the Premium Plan with International Calling.");
                }
                else
                {
                    plan = "Standard Plan";
                    Console.WriteLine("You are choosing the Standard Plan.");
                }
                break;
            case -1:
                plan = "Unlimited Plan";
                Console.WriteLine("You are choosing the Unlimited Plan with International Calling.");
                break;
            default:
                Console.WriteLine("Invalid data request");
                break;
        }
    }
}