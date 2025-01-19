## Projektbeskrivning

Examensprojektet är en uthyrningsplattform för bostäder i Halland, med fokus på att hantera allt från användarautentisering till bokningar och betalningar.
Backend-API:t är byggt med ASP.NET Core, och frontend-delen är utvecklad med React/Next.js.

Plattformen är strukturerad för att möjliggöra både enkel vidareutveckling och skalbarhet.
Du hittar länken till frontend-repot här: https://github.com/MisimoM/Examensarbete-UI

## Syfte med examensarbetet
Målet med examensarbetet är att bygga en fungerande uthyrningsplattform för bostäder i Halland, vilket ger praktisk erfarenhet inom systemdesign, API-utveckling och fullstackutveckling.
Projektet använder en modular monolith-arkitektur och Vertical Slice Architecture för att skapa skalbara och underhållbara lösningar, samt implementera moderna utvecklingsprinciper och teknologier som används i branschen.

## Arkitektur och uppbyggnad
Projektet är byggt som en modular monolith, en arkitektur som kombinerar enkelheten hos en monolit med möjligheten att i framtiden bryta ut moduler till separata microservices.
Applikationen är indelad i tydliga moduler med egna ansvarsområden, vilket gör den både lätt att förstå och utveckla.

För att separera data och ansvarsområden använder varje modul sitt eget databasschema i en gemensam databas.
Detta gör det möjligt att hantera modulernas data isolerat, samtidigt som hela applikationen körs som en monolitisk enhet. Varje modul har också sina egna migrationer och interna strukturer.

Jag valde att använda en modular monolith för att få erfarenhet av en modern och avancerad arkitektur samtidigt som jag utmanade mig själv med ett projekt som är både lärorikt och relevant för framtida utveckling.
Genom detta projekt har jag fått insikt i både fördelar och begränsningar med modular monolith och hur det kan underlätta övergången till microservices om behovet skulle uppstå.

För att skapa enkla och effektiva API-endpoints använder jag Minimal APIs, vilket gör koden mer kompakt och lättläst.

Varje modul är organiserad med **Vertical Slice Architecture**, och varje slice följer ett modifierat **REPR-pattern**:
- **Request:** Innehåller inkommande data från klienten.
- **Endpoint:** Definierar API-endpointen.
- **Handler:** Hanterar logiken för operationen.
- **Response:** Returnerar strukturerat svar till klienten.
- **Validator:** Validerar inkommande data med hjälp av FluentValidation (när det behövs).

### Moduler och Funktionalitet
**Users/Authentication**
- **Aktuella funktioner:** Hanterar inloggning och autentisering med JWT, inklusive funktioner för både AccessToken och RefreshToken.
- **Kommande funktioner:** CRUD för användarhantering.

**Listings**
- **Aktuella funktioner:** Grundläggande funktioner för att lägga till nya annonser och filtrera befintliga annonser.
- **Kommande funktioner:** Mer avancerad filtrering och CRUD för annonshantering.

**Bookings**
- **Aktuella funktioner:** Funktionalitet för att genomföra bokningar.
- **Kommande funktioner:** CRUD-funktioner för att ändra eller ta bort bokningar.

## Hur man använder projektet
Det här projektet kan köras med Microsoft SQL Server antingen lokalt eller via Docker. Du kan välja vilken metod du vill genom att justera connectionstringsen i appsettings.json.
Kommentera ut den anslutning du inte vill använda.

Migrering och seeding sker automatiskt vid uppstart för att hålla databasen uppdaterad med testdata.

För att testa inloggning och få access- och refresh-token, använd den seedade användaren:
- E-post: admin@admin.com
- Lösenord: Admin123

Inloggningen ger cookies med access-token och refresh-token, som används för autentisering vid bokningar (just nu är autentisering endast implementerad för boknings-API:et).

Istället för Swagger används Scalar som GUI för att interagera med API:t. Du kan nå Scalar-dokumentationen via scalar/v1 i URL:en, t.ex. http://localhost:{port}/scalar/v1.

En frontendapplikation för projektet ingår också i mitt examensarbete. Det är byggt i React/Next.js. Du hittar länken till frontend-repot här: https://github.com/MisimoM/Examensarbete-UI
