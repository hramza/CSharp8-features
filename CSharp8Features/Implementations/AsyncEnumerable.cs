using CSharp8Features.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSharp8Features.Implementations
{
    internal class AsyncEnumerable : IExecutable
    {
        public async Task Execute()
        {
            await foreach (string result in GetValues(new[] { 5, 6, 7, 8 }).ConfigureAwait(false))
            {
                Console.WriteLine(result);
            }
        }

        static async IAsyncEnumerable<string> GetValues(int[] array)
        {
            foreach (int number in array)
            {
                string value = await GetRandomValue(number);
                yield return value;
            }
        }

        static async Task<string> GetRandomValue(int number)
        {
            await Task.Delay(15);
            return $"CSharp - {number}";
        }
    }
}
