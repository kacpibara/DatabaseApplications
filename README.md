# Aplikacje Baz Danych / Database Applications (APBD) - Zbiór Zadań

Repozytorium zawiera projekty realizowane w ramach przedmiotu Aplikacje Baz Danych (APBD). Celem projektów jest praktyczne zastosowanie technologii .NET oraz wzorców projektowych w komunikacji z bazami danych.

## Technologie i Narzędzia
* **Język:** C# 12
* **Framework:** .NET 8.0, ASP.NET Core Web API
* **Baza danych:** Microsoft SQL Server
* **Testy:** xUnit, NSubstitute (Mockowanie)
* **Narzędzia:** Swagger, ADO.NET, Git

## Struktura Zadań

### [Zadanie 2: Refaktoryzacja i SOLID](./02_Refactor)
Projekt polegający na refaktoryzacji istniejącego (Legacy) kodu.
* **Cel:** Poprawa czytelności, testowalności oraz zmniejszenie długu technologicznego.
* **Zastosowane praktyki:** * Wyodrębnienie metod (SRP z SOLID).
    * Wstrzykiwanie zależności (Dependency Injection / Dependency Inversion).
    * Zastosowanie wzorca projektowego **Adapter**.
    * Napisanie superszybkich testów jednostkowych przy użyciu biblioteki **NSubstitute** do mockowania warstwy dostępu do danych.

### [Zadanie 3: REST API & SQL Connection](./03_Rest_API_SQL_Connection)
Implementacja od podstaw serwera REST API do zarządzania zasobami (zwierzętami) w bazie danych SQL.
* **Cel:** Stworzenie w pełni funkcjonalnego API z zachowaniem czystej architektury.
* **Zastosowane praktyki:**
    * Podział na warstwy: `Controllers`, `Services`, `Repositories`, `DTOs`, `Models`.
    * Bezpośrednia komunikacja z bazą danych przy użyciu klas `SqlConnection` oraz `SqlCommand` (ADO.NET).
    * Obsługa pełnego cyklu CRUD (Create, Read, Update, Delete) z poprawnymi kodami statusów HTTP.
    * Walidacja danych wejściowych za pomocą `Data Annotations`.

---
*Projekt zrealizowany w celach edukacyjnych.*