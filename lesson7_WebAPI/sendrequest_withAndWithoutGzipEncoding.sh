# will return json - text is not supported on this endpoint
curl -X 'GET'   'https://localhost:7176/api/2.0.0.0/WeatherForecast'   -H 'accept: text/plain' -o response_unarchived.txt

# should return text
curl -X 'GET'   'https://localhost:7176/api/2.0.0.0/WeatherForecast/12'   -H 'accept: text/plain' -o response_unarchived.txt

curl -X 'GET'   'https://localhost:7176/api/2.0.0.0/WeatherForecast/12'   -H 'accept: text/plain' -H 'Accept-Encoding: gzip' -o response_archived