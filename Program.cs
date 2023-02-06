// See https://aka.ms/new-console-template for more information

using System.Globalization;
using System.Reflection;
using CsvHelper;
using FluxApi.SDK;
using FluxToKoinly;

var accountId = args[0];

Console.WriteLine($"Flux to Koinly v{Assembly.GetExecutingAssembly().GetName().Version.ToString()}");
var transactions = await FluxApiService.GetAccountRawTransactions(accountId);

await using var writer = new StreamWriter($"{accountId}_transactions.csv");
await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
csv.WriteHeader<KoinlyItem>();
csv.NextRecord();
foreach (var transaction in transactions)
{
    var koinlyItem = new KoinlyItem
    {
        Date = UnixTimeStampToDateTime(transaction.Blocktime).ToString("yyyy-MM-dd HH:mm:ss"),
        TxHash = transaction.Blockhash
    };

    var isReceived = transaction.Vout.Any(x => x.ScriptPubKey.Addresses.Any(a => a == accountId));
    var vouts = transaction.Vout.FirstOrDefault(x => x.ScriptPubKey.Addresses.Any(a => a == accountId));
    if (vouts is not null)
    {
        koinlyItem.ReceivedAmount = vouts.Value;
        koinlyItem.ReceivedCurrency = "FLUX";
        koinlyItem.Label = "reward";
        koinlyItem.Description = "reward";
    }

    csv.WriteRecord(koinlyItem);
    csv.NextRecord();
}


static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
{
    // Unix timestamp is seconds past epoch
    var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
    dateTime = dateTime.AddSeconds(unixTimeStamp);
    return dateTime;
}