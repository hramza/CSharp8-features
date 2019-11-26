using CSharp8Features.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharp8Features
{
    public partial class Program
    {
        static ICollection<IExecutable> GetAllTypes() =>
            typeof(Program).Assembly.GetTypes()
            .Where(_ => typeof(IExecutable).IsAssignableFrom(_) && _ != typeof(IExecutable))
            .OrderBy(_ => _.Name)
            .Select(_ => (IExecutable)Activator.CreateInstance(_))
            .ToArray();

        static Dictionary<int, IExecutable> GetPairs()
        {
            int i = 0;
            return GetAllTypes().ToDictionary(k => ++i, k => k);
        }

        static void DisplayFeatures()
        {
            Action<Action> ChangeConsoleColor = body =>
            {
                var consoleColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;

                body();

                Console.ForegroundColor = consoleColor;
            };

            Func<IExecutable, string> TypeName = _ => _.GetType().Name;

            var pairs = GetPairs();
            ChangeConsoleColor(() =>
            {
                int maxLength = pairs.Values.Max(_ => TypeName(_).Length + 10);
                int middleIndex = pairs.Values.Count / 2;
                for (int i = 0; i < middleIndex; ++i)
                {
                    var left = pairs.ElementAt(i);
                    var right = pairs.ElementAt(middleIndex + i);

                    string leftString = $"{Padding(left.Key, 1)} - {TypeName(left.Value)}";
                    string rightString = $"{Padding(right.Key, 1)} - {TypeName(right.Value)}";

                    Console.WriteLine($"{leftString.PadRight(maxLength)}{rightString}");
                }

                if (pairs.Values.Count % 2 == 1)
                {
                    var last = pairs.Last();
                    var lastString = $"{Padding(last.Key, 1)} - {TypeName(last.Value)}";
                    Console.WriteLine($"{string.Empty.PadRight(maxLength / 2)}{lastString}");
                }

                Console.WriteLine();
                Console.WriteLine("- Select a feature to execute");
            });
        }

        static string Padding(int index, int length)
        {
            try
            {
                string source = index.ToString();

                return source.PadLeft((length - source.Length) / 2 + source.Length).PadRight(length);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}