using System;

public class Program {
   public static void Main() {
        // Step 1: Prompt the user for input
        Console.WriteLine("Enter your income:");
        double income = Convert.ToDouble(Console.ReadLine());

        Console.WriteLine("Are you a resident? (yes/no):");
        string residentInput = Console.ReadLine();
        bool isResident = residentInput.ToLower() == "yes";

        double taxRate = 0;
        double tax;

        // Step 2: Use advanced if-else statements to calculate tax
        if (income < 50000) {
            if (isResident) {
                taxRate = 0.10;  // 10% tax for residents
            } else {
                taxRate = 0.15;  // 15% tax for non-residents
            }
        } else if (income >= 50000 && income <= 100000) {
            if (isResident) {
                taxRate = 0.20;  // 20% tax for residents
            } else {
                taxRate = 0.25;  // 25% tax for non-residents
            }
        } else {
            if (isResident) {
                taxRate = 0.30;  // 30% tax for residents
            } else {
                taxRate = 0.35;  // 35% tax for non-residents
            }
        }

        // Step 3: Calculate and print the tax
        tax = income * taxRate;
        Console.WriteLine("The tax is: $" + tax);
    }
}