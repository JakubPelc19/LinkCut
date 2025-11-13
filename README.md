# LinkCut

LinkCut API je jednoduchá a rychlá REST API služba určená ke zkracování URL adres. Umožňuje vývojářům snadno generovat krátké odkazy, přesměrovávat uživatele na původní cílové URL a spravovat vytvořené záznamy. API je postavené na ASP.NET Core Web API (C#) a jako databázi využívá PostgreSQL.

## Požadavky
- .NET 9
- PostgreSQL
- appsettings.json s ConnectionStringem
- AllowedOrigins v  appsettings.json

## Tech Stack
- C#
- ASP.NET Core Web API
- EF Core
- PostgreSQL

## Setup
- Naklokujte repozitář přes git clone:
`git clone https://github.com/JakubPelc19/LinkCut`
- Vytvořte appsettings.json v adresáři LinkCut:
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

- Do AllowedOrigins přidejte všechny své frontendy, které chcete přípojit (oddělujte ';'):
```json
"AllowedOrigins": "http://localhost:3000;http://localhost:1000"
```
- Taktéž může zůstat prázdný:
```json
  "AllowedOrigins": ""
```

- V adresáři LinkCut použíjte nástroj `dotnet ef`:
- Nejprve vytvoře migrace:
```bash
dotnet ef migrations add InitialCreate
```
poté tuto migraci aplikujte:
```bash
dotnet ef database update
```

- A nyní můžete spustit :)

## Vytvoření krátkého odkazu

- Na tento odkaz pošlete tento požadavek `https://<your-address>/api/LinkCutter/createshortlink`:
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

- Jako odpověď Vám příjde např:
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

- `originalLinkId` je zkrácený odkaz, který se bude používat na frontendu k redirectutí např: `http://<your-host>/vyrfgl` redirectone na `http://www.example.com`
## Získání původního odkazu
- Na tento odkaz methodou GET dostanete tuto odpověď `https://<your-address>/api/LinkCutter/getorignallink/{originalLinkId}`:
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

- Frontend může redirectonout na `originalLinkId`
