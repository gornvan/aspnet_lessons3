# IO.Swagger - the C# library for the lesson7_WebApi

No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)

This C# SDK is automatically generated by the [Swagger Codegen](https://github.com/swagger-api/swagger-codegen) project:

- API version: 1.0
- SDK version: 1.0.0
- Build package: io.swagger.codegen.v3.generators.dotnet.CsharpDotNet2ClientCodegen

<a name="frameworks-supported"></a>
## Frameworks supported
- .NET 2.0

<a name="dependencies"></a>
## Dependencies
- Mono compiler
- Newtonsoft.Json.7.0.1
- RestSharp.Net2.1.1.11

Note: NuGet is downloaded by the mono compilation script and packages are installed with it. No dependency DLLs are bundled with this generator

<a name="installation"></a>
## Installation
Run the following command to generate the DLL
- [Mac/Linux] `/bin/sh compile-mono.sh`
- [Windows] TODO

Then include the DLL (under the `bin` folder) in the C# project, and use the namespaces:
```csharp
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;
```
<a name="getting-started"></a>
## Getting Started

```csharp
using System;
using System.Diagnostics;
using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace Example
{
    public class Example
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
            var apiInstance = new WeatherForecastApi();
            var minTemperatureC = 56;  // int? |  (optional) 
            var maxTemperatureC = 56;  // int? |  (optional) 
            var pageSize = 56;  // int? |  (optional)  (default to 5)
            var pageNumber = 56;  // int? |  (optional)  (default to 1)
            var fields = new List<string>(); // List<string> |  (optional) 

            try
            {
                List<WeatherForecast> result = apiInstance.GetWeatherForecast(minTemperatureC, maxTemperatureC, pageSize, pageNumber, fields);
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

<a name="documentation-for-api-endpoints"></a>
## Documentation for API Endpoints

All URIs are relative to *http://localhost:5016/api/2.0.0.0*

Class | Method | HTTP request | Description
------------ | ------------- | ------------- | -------------
*WeatherForecastApi* | [**GetWeathForecastForConcreteDayAhead**](docs/WeatherForecastApi.md#getweathforecastforconcretedayahead) | **GET** /WeatherForecast/{daysAhead} | 
*WeatherForecastApi* | [**GetWeatherForecast**](docs/WeatherForecastApi.md#getweatherforecast) | **GET** /WeatherForecast | 

<a name="documentation-for-models"></a>
## Documentation for Models

 - [IO.Swagger.Model.DateOnly](docs/DateOnly.md)
 - [IO.Swagger.Model.DayOfWeek](docs/DayOfWeek.md)
 - [IO.Swagger.Model.WeatherForecast](docs/WeatherForecast.md)

<a name="documentation-for-authorization"></a>
## Documentation for Authorization

All endpoints do not require authorization.