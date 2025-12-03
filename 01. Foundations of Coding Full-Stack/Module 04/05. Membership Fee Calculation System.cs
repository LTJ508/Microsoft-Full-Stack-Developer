using System;

public class Program
{
    public static void Main()
    {
        Console.WriteLine("Enter your age: ");
        int age = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Enter the type of membership (basic/premium): ");
        string membershipType = Console.ReadLine().ToLower();

        int fee = 0;

        if (age < 18)
        {
            if(membershipType == "basic")
            {
                fee = 15;
            }
            else if (membershipType == "premium")
            {
                fee = 25;
            }
        } else if (age >= 18 && age <= 60)
        {
            if(membershipType == "basic")
            {
                fee = 30;
            }
            else if (membershipType == "premium")
            {
                fee = 50;
            }
        } else if (age > 60)
        {
            if(membershipType == "basic")
            {
                fee = 20;
            }
            else if (membershipType == "premium")
            {
                fee = 35;
            }
        }

        Console.WriteLine("The membership fee is: $" + fee);
    }
}