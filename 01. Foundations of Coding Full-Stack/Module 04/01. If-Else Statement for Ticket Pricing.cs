using System;

public class Program
{
    public static void Main()
    {
        int age;
        Console.WriteLine("Enter your age: ");
        age = int.Parse(Console.ReadLine());

        if (age < 12)
        {
            Console.WriteLine("Half price ticket.");
        }
        else if(age <= 65)
        {
            Console.WriteLine("Full price ticket.");
        }
        else
        {
            Console.WriteLine("Senior discount ticket.");
        }
    }
}