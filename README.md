# Vector Library

This is a simple example console application that demonstrates how to upload a `.txt` file into a vector database using [Pinecone](https://www.pinecone.io/).

## Overview

- The user selects a `.txt` file from a list.
- The application uses a basic Token Splitter to break the text into chunks.
- The chunks are converted to embeddings using the OpenAI endpoint and the model `text-embedding-3-small`. A proprietary OpenAI model.
- Each embedding is paired with metadata and a unique ID to create vectors.
- The vectors are uploaded to Pinecone through the Pinecone API.

## Requirements

- OpenAI API Key (will require an account with a balance)
- Pinecone API Key (has a free option)

## Usage

1. Clone the repository.
2. Configure your API keys for OpenAI and Pinecone in the appsettings.json file.
3. Provide a valid directory with at least one txt file, in Program.cs file.
4. Run the console application.
5. Select a file from the list when prompted.

The application will handle tokenization, embedding, and uploading to Pinecone automatically.

