using System;

public class Program
{
    public static void Main()
    {
        string mode;
        Console.WriteLine("Select a travel mode: Bus, Train, Flight");
        mode = Console.ReadLine();

        switch (mode)
        {
            case "Bus":
                Console.WriteLine("Booking a bus ticket.");
                break;
            case "Train":
                Console.WriteLine("Booking a train ticket.");
                break;
            case "Flight":
                Console.WriteLine("Booking a flight ticket.");
                break;
            default:
                Console.WriteLine("Invalid selection. Please choose Bus, Train, or Flight.");
                break;
        }
    }
}