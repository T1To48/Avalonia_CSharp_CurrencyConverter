using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json;

namespace RealTimeRate.Services;

public class RealtimeRate
{
    private readonly HttpClient _client=new ();
    public async Task<double> GetRealTimeRate()
    {
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"https://exchange-rates7.p.rapidapi.com/convert?base=USD&target=ILS"),

        };
        request.Headers.Add("x-rapidapi-key", "76ec758f96msh91f1b342af0b0e3p17a99djsn07bd79fa597b");
        request.Headers.Add("x-rapidapi-host", "exchange-rates7.p.rapidapi.com");
        
        var response=await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var json=  await response.Content.ReadAsStringAsync();
        var data = JsonDocument.Parse(json);
        return data.RootElement.GetProperty("convert_result").GetProperty("rate").GetDouble();
    }
}