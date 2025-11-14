# LinkCut

LinkCut API is a simple and fast REST API service designed for shortening URLs. It allows developers to easily generate short links, redirect users to the original target URL on the frontend: https://github.com/JakubPelc19/LinkCut-frontend. The API is built using ASP.NET Core Web API (C#) and uses PostgreSQL as its database.

## Requirements
- .NET 9
- PostgreSQL
- appsettings.json with ConnectionString
- AllowedOrigins in appsettings.json

## Tech Stack
- C#
- ASP.NET Core Web API
- EF Core
- PostgreSQL

## Setup
- Clone the repo:
`git clone https://github.com/JakubPelc19/LinkCut`
- Create appsettings.json in root of the repo:
```json

{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=linkcut-postgres;Username=postgres;Password=password123"
  },

  "AllowedOrigins": ""


}
```

- Add your frontends origins to `AllowedOrigins` variable inside your appsettings.json (multiple origins seperate with `;`):
```json
"AllowedOrigins": "http://localhost:3000;http://localhost:1000"
```
- You can also leave it empty:
```json
  "AllowedOrigins": ""
```

- Use `dotnet ef` tool in LinkCut directory:
- Create migration:
```bash
dotnet ef migrations add InitialCreate
```
Then apply this migration:
```bash
dotnet ef database update
```

- And that's it :)

## ShortLink creation

- Send this request on this link `https://<your-address>/api/LinkCutter/createshortlink`:
```json
{
  "method": "POST",
  "header": {
    "Content-Type": "application/json"
  },
  
  "body": {
    "link": "http://www.example.com"
  }
}
```

- Response would be:
```json
{
  "message": "Short link found",
  "statusCode": 200,
  "isSuccessful": true,
  "data": {
    "id": 18,
    "originalLink": "http://www.example.com",
    "originalLinkId": "vyrfgl"
  }
}
```

- `originalLinkId` is shortened link, that is used to redirection on frontend for example: `http://<your-host>/vyrfgl` redirects to `http://www.example.com`
## Getting original link
- Send GET request on this link, you would get this response `https://<your-address>/api/LinkCutter/getorignallink/{originalLinkId}`:
```json
{
  "message": "Short link found, client can redirect to the original source",
  "statusCode": 308,
  "isSuccessful": true,
  "data": {
    "id": 17,
    "originalLink": "http://example.com",
    "originalLinkId": "7g0mor"
  }
}
```

- Frontend can redirect to `originalLink`
