using CSharp8Features.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CSharp8Features.Implementations
{
    public class RangeIndex : IExecutable
    {
        public Task Execute()
        {
            Action<int[]> Display = arr => Console.WriteLine(string.Join('-', arr));

            var array = Enumerable.Range(1, 9).ToArray();
            Index index = 1;

            // Display 2
            Console.WriteLine(array[index]);

            index = ^1;
            // Display 9
            Console.WriteLine(array[index]);

            // counting 1 from the end of the array
            index = new Index(1, true);
            // Display 9
            Console.WriteLine(array[index]);

            // slice the array: Display from 3 to 7
            var slice = array[2..7];
            Display(slice);

            // skip the first and last elements of the array => display from 2 to 8
            slice = array[1..^1];
            Display(slice);

            // Display all elements
            slice = array[..];
            Display(slice);

            // Skip last 3 elements
            slice = array[..^3];
            Display(slice);

            // using Range struct
            var range = new Range(1, ^1);
            // skip the first and last elements of the array => display from 2 to 8
            Display(array[range]);

            return Task.CompletedTask;
        }
    }
}