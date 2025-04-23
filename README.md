## Projektbeskrivning

Examensprojektet är en uthyrningsplattform för bostäder i Halland som omfattar hela flödet från användarautentisering till bokningar och betalningar.
Backend är byggd med ASP.NET Core och frontend utvecklad med React/Next.js.

Du hittar länken till frontend-repot här: https://github.com/MisimoM/Examensarbete-UI

## Arkitektur och uppbyggnad
Projektet är byggt som en **Modular Monolith**, vilket innebär att applikationen är uppdelad i tydligt avgränsade moduler med egna ansvarsområden.
Genom att hålla modulerna isolerade från varandra blir systemet enklare att förstå, testa och vidareutveckla.

Varje modul har ett eget databasschema i en gemensam databas.
Denna uppdelning gör datan mer organiserad och enklare att arbeta med eftersom varje modul hanterar sin egen data utan att påverka andra.

Varje modul är organiserad med **Vertical Slice Architecture** och varje slice följer **REPR-pattern**:
- **Request:** Innehåller inkommande data från klienten.
- **Endpoint:** Definierar API-endpointen.
- **Handler:** Hanterar logiken för operationen.
- **Response:** Returnerar strukturerat svar till klienten.
- **Validator:** Validerar inkommande data med hjälp av FluentValidation (när det behövs).

Varje slice testas med integrationstester som körs med Testcontainers för att verifiera att allt fungerar korrekt mot en riktig databas.
Testerna körs automatiskt i CI/CD-pipelinen via GitHub Actions

### Moduler och Funktionalitet
**Users/Authentication**
- **Aktuella funktioner:** Hanterar inloggning och autentisering med JWT, inklusive funktioner för både AccessToken och RefreshToken.
- **Kommande funktioner:** CRUD för användarhantering.

**Listings**
- **Aktuella funktioner:** Grundläggande funktioner för att lägga till nya annonser och filtrera befintliga annonser.
- **Kommande funktioner:** Mer avancerad filtrering och CRUD för annonshantering.

**Bookings**
- **Aktuella funktioner:** Funktionalitet för att genomföra bokningar samt integration med Klarna för betalningshantering.
- **Kommande funktioner:** CRUD-funktioner för att ändra eller ta bort bokningar.

## Hur man använder projektet
Det här projektet kan köras med Microsoft SQL Server antingen lokalt eller via Docker. Du kan välja vilken metod du vill genom att justera connectionstringsen i appsettings.json.
Kommentera ut den anslutning du inte vill använda.

Migrering och seeding sker automatiskt vid uppstart för att hålla databasen uppdaterad med testdata.

För att testa inloggning och få access- och refresh-token, använd den seedade användaren:
- E-post: admin@admin.com
- Lösenord: Admin123

Istället för Swagger används Scalar som GUI för att interagera med API:t. Du kan nå Scalar-dokumentationen via scalar/v1 i URL:en, t.ex. http://localhost:{port}/scalar/v1.
