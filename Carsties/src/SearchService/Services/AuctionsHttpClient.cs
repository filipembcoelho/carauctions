using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Services;

public class AuctionsHttpClient
{
    private readonly IConfiguration _config;
    private readonly HttpClient _httpClient;

    public AuctionsHttpClient(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<List<Item>> GetItemsForSearchDb()
    {
        var lastUpdated = await DB.Find<Item, string>()
            .Sort(x => x.Descending(a => a.UpdatedAt))
            .Project(x => x.UpdatedAt.ToString())
            .ExecuteFirstAsync();

        var path = $"{_config["AuctionServiceUrl"]}/api/auctions?date={lastUpdated}";
        return await _httpClient.GetFromJsonAsync<List<Item>>(path);
    }
}