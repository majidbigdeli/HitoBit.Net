using HitoBit.Net.Clients;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;

HitoBitRestClient.SetDefaultOptions(options =>
{
    options.ApiCredentials = new ApiCredentials("vHogz24Xv5ryqCUshAQ45DgsNMjHFagtvcFwWTXseIxeOXQ9mkwCJoqyX08RiPhf", "6e4edeTRIPqLGuTJPwmrJmXaskIIsPPpAgEEkrnGYtcGbMLDDhhpS9w625Iyo2sw"); // <- Provide you API key/secret in these fields to retrieve data related to your account
    options.OutputOriginalData = true;
});
HitoBitSocketClient.SetDefaultOptions(options =>
{
    options.ApiCredentials = new ApiCredentials("vHogz24Xv5ryqCUshAQ45DgsNMjHFagtvcFwWTXseIxeOXQ9mkwCJoqyX08RiPhf", "6e4edeTRIPqLGuTJPwmrJmXaskIIsPPpAgEEkrnGYtcGbMLDDhhpS9w625Iyo2sw");
});

string? read = "";
while (read != "R" && read != "S") 
{
    Console.WriteLine("Run [R]est or [S]ocket example?");
    read = Console.ReadLine();
}

if (read == "R")
{
    using (var client = new HitoBitRestClient())
    {
        await HandleRequest("Symbol list", () => client.SpotApi.ExchangeData.GetExchangeInfoAsync(), result => string.Join(", ", result.Symbols.Select(s => s.Name).Take(10)) + " etc");
        await HandleRequest("BTCUSDT book price", () => client.SpotApi.ExchangeData.GetBookPriceAsync("BTCUSDT"), result => $"Best Ask: {result.BestAskPrice}, Best Bid: {result.BestBidPrice}");
        await HandleRequest("ETHUSDT 24h change", () => client.SpotApi.ExchangeData.GetTickerAsync("ETHUSDT"), result => $"Change: {result.PriceChange}, Change percentage: {result.PriceChangePercent}");
    }
}
else
{
    Console.WriteLine("Press enter to subscribe to BTCUSDT trade stream");
    Console.ReadLine();
    var socketClient = new HitoBitSocketClient();
    var subscription = await socketClient.SpotApi.ExchangeData.SubscribeToOrderBookUpdatesAsync("BTCUSDT",null, data =>
    {
        Console.WriteLine($"{data.Data.LastUpdateId}: {data.Data.Asks} @ {data.Data.Bids}");
    });
    if (!subscription.Success)
    {
        Console.WriteLine("Failed to sub: " + subscription.Error);
        Console.ReadLine();
        return;
    }

    subscription.Data.ConnectionLost += () => Console.WriteLine("Connection lost, trying to reconnect..");
    subscription.Data.ConnectionRestored += (t) => Console.WriteLine("Connection restored");

    Console.ReadLine();
    /// Unsubscribe
    await socketClient.UnsubscribeAllAsync();
}

static async Task HandleRequest<T>(string action, Func<Task<WebCallResult<T>>> request, Func<T, string> outputData)
{
    Console.WriteLine("Press enter to continue");
    Console.ReadLine();
    Console.Clear();
    Console.WriteLine("Requesting " + action + " ..");
    var bookPrices = await request();
    if (bookPrices.Success)
        Console.WriteLine($"{action}: " + outputData(bookPrices.Data));
    else
        Console.WriteLine($"Failed to retrieve data: {bookPrices.Error}");
    Console.WriteLine();
}

//var socketClient = new HitoBitSocketClient();
//// Spot | Spot market and user subscription methods
//socketClient.Spot.SubscribeToAllBookTickerUpdatesAsync(data =>
//{
//    // Handle data
//});

//// FuturesCoin | Coin-M futures market and user subscription methods
//socketClient.FuturesCoin.SubscribeToAllBookTickerUpdatesAsync(data =>
//{
//    // Handle data
//});

//// FuturesUsdt | USDT-M futures market and user subscription methods
//socketClient.FuturesUsdt.SubscribeToAllBookTickerUpdatesAsync(data =>
//{
//    // Handle data
//});
Console.ReadLine();