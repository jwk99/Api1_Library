# Biblioteka/wypożyczalnia – system wypożyczeń (ASP.NET Core)

Projekt przedstawia prostą aplikację typu system biblioteczny z:
- listą czytelników,
- listą książek i kontrolą dostępności egzemplarzy,
- wypożyczaniem i zwrotem książek,
- zapisem danych w relacyjnej bazie danych.

Backend został zrealizowany w **ASP.NET Core Web API**, a frontend jako **minimalistyczny HTML + JavaScript**.

---

## Funkcjonalności

### Czytelnicy (Members)
- Dodawanie czytelnika (name, email)
- Walidacja unikalności adresu email
- Lista czytelników dostępna przez API

### Książki (Books)
- Dodawanie książek (title, author, copies)
- Walidacja liczby egzemplarzy (copies >= 0)
- Lista książek z informacją o dostępnych egzemplarzach

### Wypożyczenia (Loans)
- Wypożyczenie książki przez czytelnika
- Automatyczne ustawienie daty wypożyczenia i terminu zwrotu (+14 dni)
- Zwrot książki
- Blokada wypożyczenia, gdy brak dostępnych egzemplarzy

---

## Model danych

```
Members(Id, Name, Email UNIQUE)
Books(Id, Title, Author, Copies)
Loans(Id, MemberId, BookId, LoanDate, DueDate, ReturnDate NULL)
```
- Liczba dostępnych egzemplarzy wyliczana jest na podstawie:
  copies - liczba aktywnych wypożyczeń
- ReturnDate = NULL oznacza aktywne wypożyczenie
- Integralność danych zapewniają klucze obce i ograniczenia w bazie

---

## Technologie

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- HTML + JavaScript (bez frameworków)

---

## Uruchomienie projektu

1. Sklonuj repozytorium.
2. Skonfiguruj połączenie do SQL Server w pliku `appsettings.json`.
3. Utwórz bazę danych i uruchom dołączony skrypt SQL.
4. Uruchom aplikację:
```
dotnet run
```
5. Aplikacja będzie dostępna pod adresem:
```
https://localhost:XXXX
```
6. Swagger:
```
https://localhost:XXXX/swagger
```
7. Strona z UI:
```
https://localhost:XXXX/index.html
```
XXXX odpowiada portowi przydzielonemu automatycznie przy starcie aplikacji.

---

## UI

UI umożliwia:
- przeglądanie listy książek z liczbą dostępnych egzemplarzy,
- wypożyczanie książek,
- dodawanie czytelników,
- dodawanie książek,
- podgląd aktywnych wypożyczeń,
- zwrot książek.

---

## Testy API

Plik `tests.rest` zawiera przykładowe wywołania API:
- dodawanie czytelników,
- dodawanie książek,
- wypożyczanie i zwrot książek,
- próby wypożyczenia przy braku dostępnych egzemplarzy.

Testy przedstawiają poprawny scenariusz „happy path” oraz przypadki błędne (np. konflikt dostępności).

---
## Uwagi końcowe

- Walidacja danych realizowana jest na poziomie DTO, backendu oraz bazy danych
- API zwraca odpowiednie kody HTTP (400, 404, 409, 200, 201)
- Kontrolery nie powielają logiki walidacyjnej
- Zastosowano DTO (record) do komunikacji API
- Modele domenowe nie zawierają kolekcji nawigacyjnych (relacje realizowane przez ID)
