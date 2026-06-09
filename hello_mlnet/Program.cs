using Microsoft.ML;
using Microsoft.ML.Data;

var mlContext = new MLContext(seed: 1);

var samples = new[]
{
    new SentimentData { Text = "I love ML.NET", Label = true },
    new SentimentData { Text = "This is amazing", Label = true },
    new SentimentData { Text = "I hate bugs", Label = false },
    new SentimentData { Text = "This is terrible", Label = false },
    new SentimentData { Text = "ML.NET is fantastic", Label = true },
    new SentimentData { Text = "I dislike this", Label = false }
};

var trainingData = mlContext.Data.LoadFromEnumerable(samples);

var pipeline = mlContext.Transforms.Text.FeaturizeText(
        outputColumnName: "Features",
        inputColumnName: nameof(SentimentData.Text))
    .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(
        labelColumnName: nameof(SentimentData.Label),
        featureColumnName: "Features"));

var model = pipeline.Fit(trainingData);
var predictionEngine = mlContext.Model.CreatePredictionEngine<SentimentData, SentimentPrediction>(model);

var candidates = new[]
{
    (Category: "Positive", Data: new SentimentData { Text = "ML.NET is awesome" }),
    (Category: "Negative", Data: new SentimentData { Text = "I regret using this" }),
    (Category: "Neutral", Data: new SentimentData { Text = "ML.NET is a machine learning library" })
};
var selected = candidates[Random.Shared.Next(candidates.Length)];
var testInput = selected.Data;
var prediction = predictionEngine.Predict(testInput);

Console.WriteLine("Hello, ML.NET!");
Console.WriteLine(new string('-', 24));
Console.WriteLine($"Random category: {selected.Category}");
Console.WriteLine($"Input: {testInput.Text}");
Console.WriteLine($"Prediction: {(prediction.Prediction ? "Positive" : "Negative")}");
Console.WriteLine($"Probability: {prediction.Probability:F3}");
Console.WriteLine(new string('-', 24));
Console.WriteLine("Press any key to exit...");
Console.ReadKey();
Console.WriteLine("Read Key is" + Console.ReadKey());
Console.WriteLine(new string('-', 24));
Console.WriteLine("Goodbye, ML.NET!");

public sealed class SentimentData
{
    public string Text { get; set; } = string.Empty;

    public bool Label { get; set; }
}

public sealed class SentimentPrediction
{
    [ColumnName("PredictedLabel")]
    public bool Prediction { get; set; }

    public float Probability { get; set; }
}
