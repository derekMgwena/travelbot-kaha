using System;
using KAHA.TravelBot.NETCoreReactApp.Models;
using Newtonsoft.Json.Linq;


public async Task<List<CountryModel>> GetAllCountries()
{
    using (var httpClient = new HttpClient())
    {
        var apiUrl = "https://restcountries.com/v3.1/all";

        var response = await httpClient.GetStringAsync(apiUrl);
        var parsedResponse = JArray.Parse(response);

        var countries = parsedResponse.Select(ParseCountry).ToList();

        return countries;
    }
}

private CountryModel ParseCountry(JToken countryJson)
{
    try
    {
        return new CountryModel
        {
            Name = countryJson["name"]["common"].ToString(),
            Capital = countryJson["capital"][0].ToString(),
            Latitude = float.Parse(countryJson["capitalInfo"]["latlng"][0].ToString()),
            Longitude = float.Parse(countryJson["capitalInfo"]["latlng"][1].ToString())
        };
    }
    catch (Exception ex)
    {
        // Log or handle the exception
        return null;
    }
}

public async Task<List<CountryModel>> GetTopFiveCountries()
{
    var allCountries = await GetAllCountries();

    var topFiveCountries = allCountries
        .OrderByDescending(c => c.Population)
        .Take(5)
        .ToList();

    return topFiveCountries;
}

public async Task<(string, string)> GetSunriseSunsetTimes(string countryName)
{
    using (var httpClient = new HttpClient())
    {
        var apiUrl = $"https://sunrise-sunset.org/api={countryName}";

        var response = await httpClient.GetStringAsync(apiUrl);
        var jsonResponse = JObject.Parse(response);

        var sunrise = jsonResponse["results"]["sunrise"].ToString();
        var sunset = jsonResponse["results"]["sunset"].ToString();

        return (sunrise, sunset);
    }
}

public CountrySummaryModel GetCountrySummary(string countryName)
{
    var country = Countries.FirstOrDefault(c => c.Name == countryName);

    if (country != null)
    {
        var (sunrise, sunset) = GetSunriseSunsetTimes(countryName);

        var summary = new CountrySummaryModel
        {
            CountryName = country.Name,
            Capital = country.Capital,
            Population = country.Population,
            TotalLanguages = country.Languages?.Count ?? 0,
            DriveSide = country.DriveSide,
            SunriseTime = sunrise,
            SunsetTime = sunset
            
        };

        return summary;
    }

    
    return null;
}





 
