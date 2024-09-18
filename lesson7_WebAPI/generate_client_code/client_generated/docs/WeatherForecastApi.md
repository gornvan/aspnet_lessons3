# IO.Swagger.Api.WeatherForecastApi

All URIs are relative to *http://localhost:5016/api/2.0.0.0*

Method | HTTP request | Description
------------- | ------------- | -------------
[**GetWeathForecastForConcreteDayAhead**](WeatherForecastApi.md#getweathforecastforconcretedayahead) | **GET** /WeatherForecast/{daysAhead} | 
[**GetWeatherForecast**](WeatherForecastApi.md#getweatherforecast) | **GET** /WeatherForecast | 

<a name="getweathforecastforconcretedayahead"></a>
# **GetWeathForecastForConcreteDayAhead**
> void GetWeathForecastForConcreteDayAhead (int? daysAhead)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetWeathForecastForConcreteDayAheadExample
    {
        public void main()
        {

            var apiInstance = new WeatherForecastApi();
            var daysAhead = 56;  // int? | 

            try
            {
                apiInstance.GetWeathForecastForConcreteDayAhead(daysAhead);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling WeatherForecastApi.GetWeathForecastForConcreteDayAhead: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **daysAhead** | **int?**|  | 

### Return type

void (empty response body)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a name="getweatherforecast"></a>
# **GetWeatherForecast**
> List<WeatherForecast> GetWeatherForecast (int? minTemperatureC, int? maxTemperatureC, int? pageSize, int? pageNumber, List<string> fields)



### Example
```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class GetWeatherForecastExample
    {
        public void main()
        {

            var apiInstance = new WeatherForecastApi();
            var minTemperatureC = 56;  // int? |  (optional) 
            var maxTemperatureC = 56;  // int? |  (optional) 
            var pageSize = 56;  // int? |  (optional)  (default to 5)
            var pageNumber = 56;  // int? |  (optional)  (default to 1)
            var fields = new List<string>(); // List<string> |  (optional) 

            try
            {
                List&lt;WeatherForecast&gt; result = apiInstance.GetWeatherForecast(minTemperatureC, maxTemperatureC, pageSize, pageNumber, fields);
                Debug.WriteLine(result);
            }
            catch (Exception e)
            {
                Debug.Print("Exception when calling WeatherForecastApi.GetWeatherForecast: " + e.Message );
            }
        }
    }
}
```

### Parameters

Name | Type | Description  | Notes
------------- | ------------- | ------------- | -------------
 **minTemperatureC** | **int?**|  | [optional] 
 **maxTemperatureC** | **int?**|  | [optional] 
 **pageSize** | **int?**|  | [optional] [default to 5]
 **pageNumber** | **int?**|  | [optional] [default to 1]
 **fields** | [**List<string>**](string.md)|  | [optional] 

### Return type

[**List<WeatherForecast>**](WeatherForecast.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

