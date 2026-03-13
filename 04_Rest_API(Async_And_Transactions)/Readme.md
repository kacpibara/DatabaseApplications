# Zadanie 4: Warehouse API (Async & Transactions)

Ten projekt zawiera implementację REST API do zarządzania logistyką magazynową. Głównym celem edukacyjnym było poprawne wdrożenie programowania asynchronicznego oraz transakcji bazodanowych w ADO.NET.

## Główne Funkcjonalności
1. **Endpoint Transakcyjny (`POST /api/warehouse`):** * Sprawdza istnienie produktu, magazynu oraz odpowiedniego zamówienia.
    * Aktualizuje datę realizacji zamówienia (`UPDATE`).
    * Wstawia nowy rekord do historii magazynu (`INSERT`).
    * Całość operacji jest objęta blokiem `SqlTransaction`. Jeśli którykolwiek etap zawiedzie, cała operacja jest wycofywana (`Rollback`), co zapobiega niespójności danych.
2. **Endpoint Procedury Składowanej (`POST /api/warehouse/procedure`):**
    * Alternatywna implementacja tego samego procesu, delegująca logikę biznesową i transakcyjną do prekompilowanej procedury po stronie SQL Servera.

## Struktura
W tym folderze, oprócz kodu C# podzielonego na warstwy (Controllers, Services, Repositories), znajdują się również skrypty dostarczone do zadania:
* `create.sql` - skrypt tworzący tabele i relacje.
* `proc.sql` - skrypt tworzący procedurę składowaną `AddProductToWarehouse`.

## Napotkane wyzwania i rozwiązania 

Podczas realizacji tego projektu natrafiłem na kilka klasycznych problemów architektonicznych i konfiguracyjnych, z których wyciągnąłem ważne wnioski:

* **Zarządzanie kontekstem transakcji w ADO.NET:** Początkowo aplikacja rzucała wyjątkiem `InvalidOperationException: BeginExecuteReader requires the command to have a transaction...`. Szybko zdiagnozowałem, że w ADO.NET otwarcie transakcji na obiekcie `SqlConnection` nie wystarczy – absolutnie każdy obiekt `SqlCommand` korzystający z tego połączenia musi mieć jawnie przekazany kontekst tej transakcji (poprzez argument konstruktora lub właściwość `.Transaction`).
* **Prawidłowa obsługa braku danych (`DBNull` vs `null`):**
  Przy odpytywaniu bazy o istniejące zamówienia za pomocą `ExecuteScalarAsync`, aplikacja ulegała awarii (`NullReferenceException`). Wynikało to z faktu, że w C# metoda ta zwraca natywnego `nulla`, gdy zapytanie nie dopasuje żadnego wiersza, natomiast `DBNull.Value` występuje, gdy wiersz istnieje, ale kolumna w bazie jest pusta. Dodanie kompleksowego warunku `if (result == null || result == DBNull.Value)` całkowicie ustabilizowało repozytorium.
* **Unikanie antywzorca "Połykania Wyjątków" (Exception Swallowing):**
  W początkowej fazie projektu używałem ogólnego bloku `try...catch` w warstwie Repozytorium, mapując każdy błąd bazy danych na biznesowy wyjątek `ConflictException`. To skutecznie maskowało natywne wyjątki `SqlException` i utrudniało debugowanie logiki transakcyjnej. Usunięcie tego antywzorca i pozwolenie na propagację błędów do kontrolera znacząco przyspieszyło proces stabilizacji aplikacji.
* **Konfiguracja środowiska uruchomieniowego (Content Root):**
  Zetknąłem się z problemem ignorowania pliku `appsettings.json` przy uruchamianiu skompilowanej aplikacji w środowisku JetBrains Rider. Błąd `ConnectionString property has not been initialized` wynikał z nieprawidłowego ustawienia `Working directory` na podfolder `bin/Debug`. Ręczne wskazanie głównego folderu projektu jako `Content Root` rozwiązało problem ładowania zmiennych środowiskowych.