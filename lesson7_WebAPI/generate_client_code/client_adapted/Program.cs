using IO.Swagger.Api;
using System.Diagnostics;
using System.Text.Json;

var apiInstance = new WeatherForecastApi();
var minTemperatureC = 20;  // int? |  (optional) 
var maxTemperatureC = 56;  // int? |  (optional) 
var pageSize = 5;  // int? |  (optional)  (default to 5)
var pageNumber = 3;  // int? |  (optional)  (default to 1)

try
{
    var result = apiInstance
        .GetWeatherForecast(minTemperatureC, maxTemperatureC, pageSize, pageNumber, null);

    Console.WriteLine(JsonSerializer.Serialize(result));
}
catch (Exception e)
{
    Console.WriteLine
        ("Exception when calling WeatherForecastApi.GetWeatherForecast: " + e.Message);
}