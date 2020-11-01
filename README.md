# RoutingAssistant
Repository for RoutingAssistant .NET Core application codebase.

## Get it running
* Make sure Docker is installed 
* Run ```docker-compose up```

Once the app is up and running, you can issue requests like
```
curl --location --request POST 'http://localhost/route' --header 'ApiKey: 123123123' \
--data-raw '{
  "Stops": [
    {
      "Latitude": 46.94753769790697,
      "Longitude": 7.4395036697387695
    },
    {
      "Latitude": 46.90992922143335,
      "Longitude": 7.471003532409668
    },
    {
      "Latitude": 46.92040187442259,
      "Longitude": 7.413443326950073
    },

    {
      "Latitude": 46.92716147481122,
      "Longitude": 7.3767077922821045
    },
    {
      "Latitude": 46.95574020712597,
      "Longitude": 7.481389045715332
    }
  ]
}'
```
