# AI Terms — Ollama + Semantic Kernel Demo

A hands-on .NET 8 console application that demonstrates core AI/LLM concepts using **locally running Ollama models** and **Microsoft Semantic Kernel**. No cloud API keys required — everything runs on your machine.

---

## What This Demo Covers

The project walks through five progressive AI concepts, each in its own runnable module:

| # | Module | Concept |
|---|--------|---------|
| 1 | `llm` | Basic LLM chat completion |
| 2 | `embeddings` | Text embeddings & cosine similarity |
| 3 | `vector` | In-memory vector search |
| 4 | `rag` | Retrieval-Augmented Generation (RAG) |
| 5 | `hallucination` | Hallucination detection & validation guardrails |

---

## Prerequisites

### 1. .NET 8 SDK
Download from [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download).

### 2. Ollama
Install Ollama from [https://ollama.com](https://ollama.com) and pull the required models:

```bash
ollama pull llama3.1
ollama pull mxbai-embed-large
```

Ollama must be running before you start the demo (`ollama serve` or the desktop app).

---

## Configuration

The app reads configuration from environment variables or a `.env` file in the project root.

Create a `.env` file (optional — defaults are shown below):

```
OLLAMA_ENDPOINT=http://localhost:11434
OLLAMA_CHAT_MODEL=llama3.1
OLLAMA_EMBEDDING_MODEL=mxbai-embed-large
```

| Variable | Default | Description |
|----------|---------|-------------|
| `OLLAMA_ENDPOINT` | `http://localhost:11434` | Ollama server URL |
| `OLLAMA_CHAT_MODEL` | `llama3.1` | Model used for chat/completion |
| `OLLAMA_EMBEDDING_MODEL` | `mxbai-embed-large` | Model used for text embeddings |

---

## Running the Demo

```bash
# Run all demos sequentially
dotnet run -- all

# Run a specific demo
dotnet run -- llm
dotnet run -- embeddings
dotnet run -- vector
dotnet run -- rag
dotnet run -- hallucination

# Show help
dotnet run -- help
```

---

## Demo Modules

### 1. LLM Demo (`llm`)
Sends a chat message to the locally running Ollama model via Semantic Kernel's `IChatCompletionService`.

- **Prompt:** "Explain Redis cache."
- **Takeaway:** LLM = text in, text out — powered by a locally served Ollama model.

---

### 2. Embeddings + Cosine Similarity Demo (`embeddings`)
Converts three phrases into high-dimensional float vectors and computes cosine similarity between them.

- **Phrases compared:**
  - `"password reset issue"`
  - `"unable to sign in"` ← semantically close to phrase A
  - `"this is desi architecture demo code"` ← semantically distant from phrase A
- **Takeaway:** Semantically similar phrases yield higher cosine similarity scores.

---

### 3. Vector Search Demo (`vector`)
Performs an in-memory nearest-neighbour search over a small runbook knowledge base.

- **Query:** `"user cannot access their account"`
- Embeddings are pre-computed for all documents at startup; cosine distance ranks the results.
- Also shows the equivalent **pgvector SQL** query for reference:
  ```sql
  SELECT doc_text
  FROM support_docs
  ORDER BY embedding <=> CAST(@queryEmbedding AS vector)
  LIMIT 3;
  ```
- **Takeaway:** Vector search finds semantically relevant documents without keyword matching.

---

### 4. RAG — Retrieval-Augmented Generation Demo (`rag`)
Grounds the LLM's answer in retrieved context from a policy knowledge base.

- **Question:** "Can unused leaves be carried forward to the next calendar year?"
- Top-2 matching policy documents are retrieved via vector search and injected into the system prompt.
- The model is instructed to answer only from the provided context.
- **Takeaway:** RAG = retrieve relevant context first, then generate an answer grounded in that context.

---

### 5. Hallucination + Validation Guardrails Demo (`hallucination`)
Demonstrates how to catch and reject an LLM response that falls outside an allowed set of values.

- The model is asked to suggest a "premium-sounding leave type for executives."
- The response is validated against an allowlist: `{ Casual, Sick, Paid }`.
- Any response not in the list is **rejected**, regardless of how convincing it sounds.
- **Takeaway:** Even a convincingly worded model response can be caught and rejected by a business-layer validation guard.

---

## Project Structure

```
├── Program.cs                  # Entry point
├── DemoRunner.cs               # CLI dispatcher — routes commands to demo modules
├── AiTermsOllamaSkDemo.csproj  # Project file
│
├── Configuration/
│   └── AppConfig.cs            # Loads settings from env vars / .env file
│
├── Demos/
│   ├── IDemoModule.cs          # Interface for all demo modules
│   ├── LlmDemo.cs              # Demo 1: basic chat completion
│   ├── EmbeddingsDemo.cs       # Demo 2: embeddings + cosine similarity
│   ├── VectorSearchDemo.cs     # Demo 3: in-memory vector search
│   ├── RagDemo.cs              # Demo 4: RAG pipeline
│   └── HallucinationDemo.cs    # Demo 5: hallucination guardrails
│
├── Infrastructure/
│   └── KernelFactory.cs        # Builds and configures the Semantic Kernel instance
│
├── Knowledge/
│   └── KnowledgeStore.cs       # In-memory documents (runbooks + policies) with embeddings
│
├── Models/
│   ├── KnowledgeDoc.cs         # Document model (id, title, text, vector)
│   └── SearchHit.cs            # Search result model (doc + similarity score)
│
├── Plugins/
│   ├── PolicyPlugin.cs         # Kernel plugin: exposes leave carry-forward policy
│   └── TicketPlugin.cs         # Kernel plugin: exposes open support ticket summary
│
├── Services/
│   └── VectorSearchService.cs  # Cosine similarity search over a list of KnowledgeDocs
│
└── Utilities/
    └── ConsoleHelper.cs        # Formatted console output helpers
```

---

## NuGet Packages

| Package | Version | Purpose |
|---------|---------|---------|
| `Microsoft.SemanticKernel` | 1.45.0 | Core Semantic Kernel framework |
| `Microsoft.SemanticKernel.Connectors.Ollama` | 1.45.0-alpha | Ollama connector for chat & embeddings |

---

## Key Concepts Illustrated

| Concept | Where Used |
|---------|-----------|
| **Chat Completion** | `LlmDemo`, `RagDemo`, `HallucinationDemo` |
| **Text Embeddings** | `EmbeddingsDemo`, `VectorSearchDemo`, `RagDemo` |
| **Cosine Similarity** | `VectorSearchService.Cosine()` |
| **Vector Search** | `VectorSearchDemo`, `RagDemo` |
| **RAG Pipeline** | `RagDemo` |
| **Guardrails / Validation** | `HallucinationDemo` |
| **Semantic Kernel Plugins** | `PolicyPlugin`, `TicketPlugin` (registered in `KernelFactory`) |
| **Semantic Kernel DI** | `KernelFactory.Build()` wires services via `IKernelBuilder` |

---

## Notes

- All AI inference runs **locally** — no data leaves your machine.
- The knowledge base (`KnowledgeStore`) is intentionally kept in-memory for simplicity. In a real application, embeddings would be stored in a vector database such as pgvector, Qdrant, or Azure AI Search.
- Plugins (`PolicyPlugin`, `TicketPlugin`) are registered with Semantic Kernel and could be invoked via function calling in a more advanced agentic scenario.
