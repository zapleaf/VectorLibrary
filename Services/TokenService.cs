using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VectorLibrary.Services
{
    public class TokenService
    {
        /// <summary>
        /// This is a very simple Token Splitter that creates overlapping chunks from an input string.
        /// In a production setting you would use something like Azure AI Document Intelligence that 
        /// extracts text and structure (like chapter, paragraphs, tables, etc) from a variety document types
        /// </summary>
        /// <param name="token">The input text to chunk</param>
        /// <param name="chunkSize">Size (in characters) of each chunk</param>
        /// <param name="chunkOverlap">Amount of text (in characters) that overlaps between chunks</param>
        /// <returns>A list of text chunks</returns>
        public List<string> TokenSplitter(string token, int chunkSize, int chunkOverlap)
        {
            var chunks = new List<string>();
            int start = 0;

            while (start < token.Length)
            {
                int end = Math.Min(start + chunkSize, token.Length);
                string chunk = token.Substring(start, end - start);
                chunks.Add(chunk);

                start += chunkSize;
                if (start >= token.Length)
                    break;

                start = Math.Max(0, start - chunkOverlap);
            }

            return chunks;
        }
    }
}
