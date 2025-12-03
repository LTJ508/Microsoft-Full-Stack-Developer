using System;

public class Program
{
    public static void Main()
    {
        string[] weekDays = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday"};
        for (int i = 0; i < weekDays.Length; i++)
        {
            switch (weekDays[i])
            {
                case "Monday":
                    Console.WriteLine(weekDays[i] + ": Team Meeting");
                    break;
                case "Tuesday":
                    Console.WriteLine(weekDays[i] + ": Code Review");
                    break;
                case "Wednesday":
                    Console.WriteLine(weekDays[i] + ": Development");
                    break;
                case "Thursday":
                    Console.WriteLine(weekDays[i] + ": Testing");
                    break;
                case "Friday":
                    Console.WriteLine(weekDays[i] + ": Deployment");
                    break;
                default:
                    Console.WriteLine(weekDays[i] + ": No Tasks Scheduled");
                    break;
            }
        }
    }
}