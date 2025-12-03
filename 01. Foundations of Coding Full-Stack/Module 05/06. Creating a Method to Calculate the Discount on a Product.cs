using System;
public class Program {
   public static void Main()
   {
        Console.WriteLine("Enter the original price:");
        decimal originalPrice = Convert.ToDecimal(Console.ReadLine());

        Console.WriteLine("Enter the discount rate (as a decimal, e.g., 0.20 for 20%):");
        decimal discountRate = Convert.ToDecimal(Console.ReadLine());

        decimal finalPrice = CalculateDiscount(originalPrice, discountRate);
        Console.WriteLine("The final price after discount is: " + finalPrice);
    }

    static decimal CalculateDiscount(decimal originalPrice, decimal discountRate)
    {
        return originalPrice * (1 - discountRate);
    }
}