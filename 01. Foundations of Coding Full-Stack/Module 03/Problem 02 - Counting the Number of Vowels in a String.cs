using System;

class Program
{
    static void Main()
    {
        string text = "Hello World";
        int vowelCount = 0;

        foreach (char c in text.ToLower())
        {
            if(c == 'a' || c == 'e' || c == 'i' || c == 'o' || c == 'u')
            {
                vowelCount++;
            }
        }

        Console.WriteLine("The number of vowels in the string is: " + vowelCount);
    }
}