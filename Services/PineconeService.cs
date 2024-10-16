using Microsoft.Extensions.Configuration;
using Pinecone;

namespace VectorLibrary.Services
{
    internal class PineconeService
    {
        private readonly IConfiguration _config;
        private PineconeClient _pineconeClient;
        private IndexClient _indexClient;

        public PineconeService(IConfiguration config)
        {
            _config = config;

            // Used to interact with the Pinecone service based on the provided api key
            _pineconeClient = new PineconeClient(_config["APiKeys:Pinecone"]);

            // Used to interact with a specific index in the Pinecone vector database.
            // If you are on the free account you will only be able to create 1 index
            // This needs to match the index name you setup in Pinecone
            _indexClient = _pineconeClient.Index("testdex");
        }

        /// <summary>
        /// Creates a list of Pinecone Vectors with consecutive ids and metadata
        /// </summary>
        public List<Vector> CreateVectors(List<ReadOnlyMemory<float>> vectorValues, int initialId, string documentTitle)
        {
            var vectors = new List<Vector>();

            try
            {
                // Add whatever metadata you want here as key value pairs
                var metaData = new Metadata
                {
                    ["DocumentTitle"] = documentTitle
                };

                // Iterate through each set of Floats (vectorValues) add metadata and an id to create a vector
                foreach (var value in vectorValues)
                {
                    // Add a simple numeric id (this can be any string) but must be unique in a given namespace
                    var id = initialId++;

                    vectors.Add(new Vector
                    {
                        Id = id.ToString(),
                        Values = value,         // This is the actual vector values the text chunks were converted into
                        Metadata = metaData     // Pre-built metadata from above
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            return vectors;
        }

        /// <summary>
        /// Performs both insert AND update.
        /// If a vector doesn't exist (by id), it creates a new one
        /// If it already exists, it overwrites it completely
        /// Requires the full vector data (embeddings + metadata) to be provided
        /// </summary>
        public async Task<uint> UpsertVectors(List<Vector> vectors, string vectorNamespace)
        {
            try
            {
                // Removed Sparse values
                var response = await _indexClient.UpsertAsync(
                new UpsertRequest
                {
                    Vectors = vectors,
                    Namespace = vectorNamespace
                }
                );

                return response.UpsertedCount ?? 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0;
        }
    }
}
