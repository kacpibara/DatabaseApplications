# Zadanie 6: Clinic API (Entity Framework Core - Code First)

Projekt implementuje REST API dla fikcyjnej kliniki medycznej, opierając się na architekturze **Code First**. Baza danych została wygenerowana automatycznie na podstawie modeli C#.

## Zrealizowane Endpointy
1. **`POST /api/prescriptions`**
    * Służy do wystawiania recept.
    * **Walidacja biznesowa:** Sprawdza, czy recepta ma max 10 leków i czy data ważności (`DueDate`) nie jest z przeszłości w stosunku do daty wystawienia (`Date`).
    * Zabezpiecza przed duplikacją pacjentów – weryfikuje ich istnienie w bazie przed dodaniem.
2. **`GET /api/prescriptions/{id}`**
    * Pobiera pełną kartotekę pacjenta.
    * Ładuje powiązane recepty, szczegóły lekarzy oraz przepisanych leków przy użyciu *Eager Loading* (`Include` / `ThenInclude`).
    * Wyniki są sortowane malejąco po dacie ważności recepty.

## Napotkane wyzwania
* **Zaufanie do certyfikatów deweloperskich (HTTPS/CORS):**
  Podczas testowania API przez Swaggera napotkałem błędy `ERR_CERT_AUTHORITY_INVALID` oraz blokady polityki CORS wynikające z nieufności przeglądarki do samopodpisanego certyfikatu .NET na localhost. Wyczyszczenie magazynu certyfikatów i wymuszenie zaufania komendami `dotnet dev-certs https --clean` oraz `--trust` ustabilizowało środowisko deweloperskie i pozwoliło na bezpieczną komunikację po HTTPS.
* **Precyzja w mapowaniu DTO:**
  Niezgodność nazewnictwa właściwości między klasami (np. wejściowy `PatientDto` w obiekcie, a odwołanie `Patient` w logice) prowadziła do błędów typu Null Reference. Wymusiło to ścisłą dyscyplinę w nazywaniu i odwoływaniu się do zagnieżdżonych obiektów podczas rzutowania DTO na Encje.