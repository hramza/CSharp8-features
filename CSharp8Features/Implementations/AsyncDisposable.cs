using CSharp8Features.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CSharp8Features.Implementations
{
    public class AsyncDisposable : IExecutable
    {
        public async Task Execute()
        {
            string filePath = "stream_test.txt";
            await using (var writer = new StreamWrapper(filePath))
            {
                await writer.WriteToFile("this is a text").ConfigureAwait(false);
            }

            if (File.Exists(filePath)) Console.WriteLine(await File.ReadAllTextAsync(filePath));
        }
    }

    class StreamWrapper : IAsyncDisposable
    {
        private readonly StreamWriter _writer;

        public StreamWrapper(string filePath) => _writer = new StreamWriter(filePath);

        public async Task WriteToFile(string text) => await _writer.WriteLineAsync(text);

        public async ValueTask DisposeAsync()
        {
            await _writer.DisposeAsync();
            GC.SuppressFinalize(this);
        }
    }
}