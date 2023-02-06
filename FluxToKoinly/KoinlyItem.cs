using CsvHelper.Configuration.Attributes;

namespace FluxToKoinly;

public class KoinlyItem
{
    [Name("Date")] public string Date { get; set; }

    [Name("Sent Amount")] public double SentAmount { get; set; }

    [Name("Sent Currency")] public string SentCurrency { get; set; } = "FLUX";

    [Name("Received Amount")] public double ReceivedAmount { get; set; }

    [Name("Received Currency")] public string ReceivedCurrency { get; set; } = "FLUX";

    [Name("Label")] public string Label { get; set; }
    [Name("Description")] public string Description { get; set; }
    [Name("TxHash")] public string TxHash { get; set; }
}