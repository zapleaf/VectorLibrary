using Microsoft.Extensions.Configuration;
using VectorLibrary.Services;

namespace VectorLibrary
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // Setup the appsettings.json as the configuration file
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json")
                .Build();

            try
            {
                // Get the current directory (or specify your own path)
                // string directoryPath = Directory.GetCurrentDirectory();
                string directoryPath = @"**Update location of txt files**";

                // Get all .txt files
                var txtFiles = Directory
                    .GetFiles(directoryPath, "*.txt")
                    .Select(path => new FileInfo(path))
                    .ToList();

                if (!txtFiles.Any())
                {
                    Console.WriteLine("No .txt files found in the directory.");
                    return;
                }

                // Get user selection
                while (true)
                {
                    // Display the files with numbers
                    Console.WriteLine("Available .txt files:");
                    Console.WriteLine("--------------------");

                    for (int i = 0; i < txtFiles.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {txtFiles[i].Name} ({txtFiles[i].Length} bytes)");
                    }


                    Console.WriteLine("\nEnter the number of the file you want to read (or 0 to exit):");
                    if (!int.TryParse(Console.ReadLine(), out int selection))
                    {
                        Console.WriteLine("Please enter a valid number.");
                        continue;
                    }

                    if (selection == 0)
                    {
                        Console.WriteLine("Goodbye!");
                        break;
                    }

                    if (selection < 1 || selection > txtFiles.Count)
                    {
                        Console.WriteLine($"Please enter a number between 1 and {txtFiles.Count}");
                        continue;
                    }

                    // Read and display the selected file
                    var selectedFile = txtFiles[selection - 1];
                    Console.WriteLine($"\nReading file: {selectedFile.Name}");
                    Console.WriteLine("--------------------");

                    try
                    {
                        string content = await File.ReadAllTextAsync(selectedFile.FullName);
                        if (content.Length > 0)
                        {
                            if (content?.Length > 200)
                            {
                                Console.WriteLine(content.Substring(0, 200));
                            }
                            else
                            {
                                Console.WriteLine(content);
                            }

                            // Create chunks from a txt file
                            // Ideally you would chunk the text in a more structured way 
                            // Like by chapters and end at the end of a paragraph or sentence. 
                            TokenService chunkerService = new();
                            var chunks = chunkerService.TokenSplitter(content, 5000, 500);
                            Console.WriteLine($"We created {chunks.Count} chunks.");

                            // Using OpenAI API to turn those chunks into embeddings (list of ReadOnlyMemory floats)
                            // Most of the AI companies have endpoints for creating embeddings
                            var embeddingServices = new OpenAIEmbeddingService(config);
                            var embeddings = await embeddingServices.CreateEmbeddings(chunks);
                            Console.WriteLine($"We created {embeddings.Count} embeddings.");

                            // Turn the embeddings into Vectors that can be sent to Pinecone
                            var pineconeService = new PineconeService(config);
                            var vectors = pineconeService.CreateVectors(embeddings, 1, selectedFile.Name);
                            Console.WriteLine($"We created {vectors.Count} vectors.");

                            // Upsert the vectors to Pinecone vector database
                            // For simplicity, We are just using the file name for the vector namespace
                            var vectorCount = await pineconeService.UpsertVectors(vectors, selectedFile.Name);
                            Console.WriteLine($"We upserted {vectorCount} vectors.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error reading file: {ex.Message}");
                    }

                    Console.WriteLine("\nPress Enter to continue...");
                    Console.ReadLine();
                    Console.Clear();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
