using Microsoft.Extensions.Configuration;
using OpenAI.Embeddings;

namespace VectorLibrary.Services
{
    public class OpenAIEmbeddingService
    {
        private readonly IConfiguration _config;
        private readonly EmbeddingClient _client;

        // This is an OpenAI proprietary model. Most of the AI companies have their own proprietary models.
        // Though I believe there are also open models from Hugging Face, Chroma, and Weviate to name a few
        // When you setup Pinecone you want it to match this model
        private readonly string _model = "text-embedding-3-small";

        public OpenAIEmbeddingService(IConfiguration config)
        {
            _config = config;
            _client = new(model: _model, _config["APiKeys:OpenAI"]);
        }

        public async Task<List<ReadOnlyMemory<float>>> CreateEmbeddings(List<string> inputs)
        {
            // You can pass a list of strings to OpenAi in a single call
            OpenAIEmbeddingCollection embeddings = await _client.GenerateEmbeddingsAsync(inputs);

            // Convert each of the OpenAI Embeddings into something more generic
            List<ReadOnlyMemory<float>> vectors = new();
            foreach (var embedding in embeddings)
            {
                vectors.Add(embedding.ToFloats());
            }

            return vectors;
        }
    }
}
