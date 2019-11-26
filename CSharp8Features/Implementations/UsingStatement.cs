using CSharp8Features.Interfaces;
using System;
using System.Threading.Tasks;

namespace CSharp8Features.Implementations
{
    public class UsingStatement : IExecutable
    {
        public Task Execute()
        {
            Console.WriteLine("See Cryptography");

            return Task.CompletedTask;
        }
    }
}
