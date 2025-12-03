using System;

public class Program
{
    public static void Main()
    {
        int[] studentScores = {45, 60, 72, 38, 55};

        for (int i = 0; i < studentScores.Length; i++)
        {
            if (studentScores[i] >= 50)
            {
                Console.WriteLine("Student " + (i + 1) + ": Pass");
            }
            else
            {
                Console.WriteLine("Student " + (i + 1) + ": Fail");
            }
        }
    }
}