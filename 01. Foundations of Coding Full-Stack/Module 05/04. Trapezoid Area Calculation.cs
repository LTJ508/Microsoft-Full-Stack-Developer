using System;

public class Program
{
    static double CalculateTrapezoidArea(double a, double b, double height)
    {
        return (a + b) / 2 * height;
    }

    public static void Main()
    {
        Console.WriteLine("Enter the length of the first parallel side (a):");
        double a = Convert.ToDouble(Console.ReadLine());

        Console.WriteLine("Enter the length of the second parallel side (b):");
        double b = Convert.ToDouble(Console.ReadLine());

        Console.WriteLine("Enter the height of the trapezoid:");
        double height = Convert.ToDouble(Console.ReadLine());

        double area = CalculateTrapezoidArea(a, b, height);

        Console.WriteLine("The area of the trapezoid is: " + area);
    }
}