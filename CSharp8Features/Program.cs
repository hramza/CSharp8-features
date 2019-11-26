using CSharp8Features.Interfaces;
using System;
using System.Threading.Tasks;

namespace CSharp8Features
{
    public static partial class Program
    {
        static async Task Main(string[] args)
        {
            Console.Clear();

            DisplayFeatures();

            string input;
            while ((input = Console.ReadLine().ToLowerInvariant()) != "exit")
            {
                if (input.Equals("clear"))
                {
                    Console.Clear();
                    DisplayFeatures();
                }

                if (int.TryParse(input, out int option) && GetPairs().TryGetValue(option, out IExecutable feature))
                {
                    Console.Clear();
                    var currentColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Green;

                    try
                    {
                        await feature.Execute().ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"- An exception has been thrown by: {feature.GetType().Name}, the message is: {ex.Message}");
                    }
                    finally
                    {
                        Console.ForegroundColor = currentColor;

                        Console.WriteLine();
                        Console.WriteLine("- Enter 'clear' to clear the console");
                        Console.WriteLine("- Enter 'exit' to exit the console");
                    }
                }
            }
        }
    }
}
