# Zadanie 5: Trip API (Entity Framework Core)

Ten projekt to implementacja REST API dla fikcyjnego biura podróży. Głównym celem edukacyjnym było zastąpienie niskopoziomowego dostępu do danych (ADO.NET) zaawansowanym narzędziem ORM – **Entity Framework Core**, pracującym w trybie *Database First*.

## Zrealizowane Endpointy
1. **`GET /api/trips`**
    * Pobiera listę wycieczek wraz z przypisanymi do nich krajami oraz zarejestrowanymi klientami.
    * Dane są optymalizowane pod kątem payloadu przy użyciu dedykowanych obiektów `DTO`.
    * Wyniki są sortowane malejąco po dacie rozpoczęcia wycieczki.
2. **`DELETE /api/clients/{idClient}`**
    * Usuwa klienta z bazy danych.
    * **Zabezpieczenie:** Przed usunięciem weryfikowane jest, czy klient posiada już przypisane wycieczki. Jeśli tak, operacja jest blokowana (zwracany jest błąd 400 Bad Request), aby chronić integralność danych.
3. **`POST /api/trips/{idTrip}/clients`**
    * Zapisuje klienta na wycieczkę.
    * Weryfikuje istnienie klienta po numerze PESEL (jeśli nie istnieje, tworzy nowy profil w bazie).
    * Blokuje próby wielokrotnego zapisu tej samej osoby na tę samą wycieczkę.

## Napotkane wyzwania i rozwiązania

Podczas pracy nad migracją do Entity Framework Core napotkałem i rozwiązałem kilka interesujących problemów konfiguracyjnych i architektonicznych:

* **Zależności silnika bazy danych (Brakujące Metody EF Core):**
  Początkowo kompilator nie rozpoznawał specyficznych dla SQL Servera metod w `DbContext` (takich jak `UseSqlServer` czy konfiguracje `ToTable`). Wynikało to z faktu, że sam rdzeń EF Core nie zawiera sterowników do konkretnych baz. Rozwiązaniem była celowa instalacja pakietu `Microsoft.EntityFrameworkCore.SqlServer` przez NuGet, co dostarczyło brakujące metody rozszerzające (extension methods).
* **Zagnieżdżone kontrolery a routing Swaggera:**
  Narzędzie Swagger/OpenAPI całkowicie ignorowało jeden z moich endpointów (`DELETE`). Po analizie kodu okazało się, że klasa kontrolera (`ClientsController`) została omyłkowo zagnieżdżona w innej klasie (`ClientController`). ASP.NET Core domyślnie ignoruje zagnieżdżone klasy przy mapowaniu endpointów. Wyniesienie kontrolera na najwyższy poziom w przestrzeni nazw natychmiast przywróciło pełną dokumentację i funkcjonalność API.
* **Projekcja DTO a logika biznesowa (Sortowanie LINQ):**
  Zrozumiałem, jak ważne jest umiejscowienie metod LINQ. Sortowanie wyników (`OrderByDescending`) musi nastąpić przed metodą `Select` rzutującą encje na DTO, aby operacja została poprawnie przetłumaczona na zapytanie SQL i wykonana po stronie silnika bazy danych, co znacząco optymalizuje wydajność.