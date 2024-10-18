# Vector Library

This is a simple example console application that demonstrates how to upload a `.txt` file into a [Pinecone](https://www.pinecone.io/) vector database. 

Vector databases are a way to efficiently store and retrieving high-dimensional data, such as embeddings, typically used in AI systems. These embeddings typically represent text, images, or audio relevant to a specific application and enable these AI systems to conduct searches, make classification, and make recommendations.

## Overview

- The application scans a directory for `.txt` files and provides a numbered list.
- The user selects a `.txt` file from that list.
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

