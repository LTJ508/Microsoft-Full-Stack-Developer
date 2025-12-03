using System;

public class Program
{
    public static void Main()
    {
        int number = 5;
        int factorial = 1;
        int i = 1;

        while (i <= number)
        {
            factorial *= i;
            i++;
        }

        Console.WriteLine("Factorial of " + number + " is: " + factorial);
    }
}