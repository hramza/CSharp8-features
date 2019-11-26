using CSharp8Features.Interfaces;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CSharp8Features.Implementations
{
    // Process json without Newtonsoft.Json nuget package
    public class JsonWithSpan : IExecutable
    {
        public async Task Execute()
        {
            string json = await File.ReadAllTextAsync("random_data.json");

            // Read only fields as string type
            ReadOnlyStringValues(Encoding.UTF8.GetBytes(json));

            Console.WriteLine();

            // Create a sample json object
            CreateJsonObject();

            // Read json data
            ReadJson(json);
        }

        void ReadOnlyStringValues(ReadOnlySpan<byte> data)
        {
            var jsonReader = new Utf8JsonReader(data);
            while (jsonReader.Read())
            {
                JsonTokenType tokenType = jsonReader.TokenType;
                if (tokenType == JsonTokenType.String)
                {
                    Console.WriteLine(jsonReader.GetString());
                }
            }
        }

        void CreateJsonObject()
        {
            var stream = new MemoryStream();
            using (var json = new Utf8JsonWriter(stream))
            {
                json.WriteStartObject();
                json.WriteString("Customer", "Hamza");
                json.WriteEndObject();
            }

            Console.WriteLine(Encoding.UTF8.GetString(stream.ToArray()));
        }

        void ReadJson(string jsonString)
        {
            using var document = JsonDocument.Parse(jsonString);
            ReadElement(document.RootElement);

            void ReadElement(JsonElement element, string prefix = "")
            {
                switch (element.ValueKind)
                {
                    case JsonValueKind.Array:
                        for (var i = 0; i < element.GetArrayLength(); i++)
                        {
                            var prop = element[i];
                            switch (prop.ValueKind)
                            {
                                case JsonValueKind.Object:
                                case JsonValueKind.Array:
                                    Console.WriteLine($"{prefix}[{i}]:");
                                    ReadElement(prop, prefix + '\t');
                                    break;
                                case JsonValueKind.Null:
                                    Console.WriteLine($"{prefix}[{i}]: Null");
                                    break;
                                default:
                                    Console.WriteLine($"{prefix}[{i}]: {prop.ToString()}");
                                    break;
                            }
                        }
                        break;
                    case JsonValueKind.Object:
                        foreach (var prop in element.EnumerateObject())
                        {
                            switch (prop.Value.ValueKind)
                            {
                                case JsonValueKind.Object:
                                case JsonValueKind.Array:
                                    Console.WriteLine($"{prefix}{prop.Name}:");
                                    ReadElement(prop.Value, prefix + '\t');
                                    break;
                                case JsonValueKind.Null:
                                    Console.WriteLine($"{prefix}{prop.Name}: Null");
                                    break;
                                default:
                                    Console.WriteLine($"{prefix}{prop.Name}: {prop.Value}");
                                    break;
                            }
                        }
                        break;
                    default:
                        Console.WriteLine($"{prefix}{element.ToString()}");
                        break;
                }
            }
        }
    }
}