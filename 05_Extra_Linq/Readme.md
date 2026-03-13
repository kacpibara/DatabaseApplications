# Dodatkowy Trening: LINQ to Objects

Ten projekt to zbiór 15 praktycznych zadań algorytmicznych i transformacyjnych rozwiązanych wyłącznie przy pomocy **Language Integrated Query (LINQ)** w języku C#.

## Czego dotyczy ten projekt?
Zamiast pisać wielokrotnie zagnieżdżone pętle `for`/`foreach` oraz instrukcje `if`, do transformacji danych użyto deklaratywnego podejścia LINQ. W projekcie symulowane są operacje, które w klasycznym podejściu wymagałyby zapytań SQL, ale tutaj wykonywane są bezpośrednio na kolekcjach w pamięci RAM.

## Przećwiczone mechanizmy LINQ:
1. **Podstawowe zapytania:** `Where()`, `Select()`, `OrderBy()`, `OrderByDescending()`.
2. **Agregacje:** Wyszukiwanie wartości ekstremalnych (`Max()`) oraz podliczanie (`Count()`).
3. **Funkcje łączące:** Zestawianie danych z dwóch niezależnych list (`Join()`) oraz sumowanie zbiorów (`Union()`).
4. **Grupowanie (GROUP BY):** Kategoryzowanie danych przy użyciu `GroupBy()`, a następnie filtrowanie grup (odpowiednik SQL-owego `HAVING`).
5. **Typy Anonimowe:** Tworzenie i zwracanie w locie nowych struktur danych bez konieczności definiowania osobnych klas DTO.
6. **Extension Methods:** Napisanie własnej metody rozszerzającej `GetEmpsWithSubordinates()`, która w sposób płynny wpina się w potok wywołań (fluent API) interfejsu `IEnumerable<T>`.

## Kontekst: LINQ to Objects vs LINQ to Entities
Choć składnia użyta w tym projekcie jest identyczna jak przy pracy z Entity Framework Core (Zadanie 5), tutaj operujemy na **LINQ to Objects**. Oznacza to, że cały kod wykonuje się w pamięci programu, a nie jest tłumaczony na język T-SQL i wysyłany do silnika bazy danych. Wymagało to szczególnej uwagi na optymalizację i poprawne konstruowanie delegatów (lambda expressions).