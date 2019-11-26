using CSharp8Features.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CSharp8Features.Implementations
{
    public class InterfaceImplementation : IExecutable
    {
        public async Task Execute()
        {
            IFileReader fileReader = new FileReader();
            Console.WriteLine(await fileReader.ReadContent("data_test.txt"));
        }
    }

    interface IFileReader
    {
        Task<string> ReadContent(string filePath) => ReadContent(new StreamReader(filePath));

        Task<string> ReadContent(StreamReader reader);
    }

    class FileReader : IFileReader
    {
        public async Task<string> ReadContent(StreamReader reader) => await reader.ReadToEndAsync();
    }
}
