# Hello World (ML.NET)

A simple Hello World sample for ML.NET using sentiment classification.

## Features

- **ML.NET** — Build and run machine learning pipelines in .NET
- **Text featurization** — Converts sentence text into numeric features
- **Binary classification** — Predicts positive/negative sentiment
- **Console output** — Prints prediction and probability for quick local verification

## Requirements

- [.NET SDK](https://dotnet.microsoft.com/download)

## Run locally

```sh
cd hello_mlnet
dotnet restore
dotnet run
```

Example output:

```text
Hello, ML.NET!
Input: ML.NET is awesome
Prediction: Positive
Probability: 0.5xx
```
