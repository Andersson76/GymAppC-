# GymAppC

GymAppC är en fullstackapplikation för registrering och hantering av träningspass. Backend är byggd med ASP.NET Core Web API och följer Clean Architecture med CQRS/MediatR, Repository Pattern, Entity Framework Core, AutoMapper, FluentValidation samt JWT-baserad autentisering och rollstyrning. Frontend är byggd med Next.js och hela lösningen kan köras med Docker Compose.

Projektet är utformat för att uppfylla grundkraven för G och de tekniska tilläggskraven för VG i kursens Clean Architecture uppgift.

## Teknik

- .NET 9 och ASP.NET Core Web API
- Entity Framework Core 9 och SQL Server 2022/SQL Express
- MediatR med separerade Commands och Queries
- FluentValidation i ett MediatR `ValidationBehavior`
- AutoMapper och DTO:er vid API gränsen
- JWT Bearer autentisering och rollerna `Admin` och `User`
- Swagger/OpenAPI med stöd för Bearer token
- Next.js 16, React 19 och TypeScript
- Docker och Docker Compose

## Arkitektur

Backend består av fyra separata projekt. Beroenden pekar inåt mot Domain och controllers anropar endast Application lagret via MediatR.

## Författare

Martin Andersson<br>
Andersson Webb & System AB

## Repository

[github.com/Andersson76/GymAppC-](https://github.com/Andersson76/GymAppC-)
