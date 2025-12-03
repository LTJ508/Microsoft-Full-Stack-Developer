using System;

public class Program
{
    static double CalculateTriangleArea(double baseLength, double height)
    {
        return 0.5 * baseLength * height;
    }

    static void Main()
    {
        Console.WriteLine("Enter the base length of the triangle: ");
        double baseLength = Convert.ToDouble(Console.ReadLine());

        Console.WriteLine("Enter the height of the triangle: ");
        double height = Convert.ToDouble(Console.ReadLine());

        double area = CalculateTriangleArea(baseLength, height);

        Console.WriteLine("The area of the triangle is: " + area);
    }
}