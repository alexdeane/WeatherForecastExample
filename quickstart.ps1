$env:ASPNETCORE_ENVIRONMENT = 'Development'

cd .\WeatherForecastExample.Web\ClientApp\
npm install
cd ..
dotnet run