using System;
public class Program {
	public static void Main() 
    {
        Console.WriteLine("Enter the first number:");
        int num1 = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Enter the second number:");
        int num2 = Convert.ToInt32(Console.ReadLine());

        int result = AddNumbers(num1, num2);

        Console.WriteLine("The sum of the numbers is: " + result);
    }
	static int AddNumbers(int a, int b)
    {
		return a + b;
	}
}