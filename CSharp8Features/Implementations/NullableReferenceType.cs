﻿using CSharp8Features.Interfaces;
using System;
using System.Threading.Tasks;

namespace CSharp8Features.Implementations
{
    public class NullableReferenceType : IExecutable
    {
        public Task Execute()
        {
            Console.WriteLine("Not implemented yet");

            return Task.CompletedTask;
        }
    }
}
