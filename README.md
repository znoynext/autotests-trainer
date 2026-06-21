# Autotests Trainer

Учебный pet-проект для практики автотестов на C#.

Проект содержит небольшой локальный веб-стенд с UI, авторизацией, API и SQLite-базой. Его можно использовать как безопасную площадку для обучения: писать UI-тесты, API-тесты, интеграционные проверки и проверки данных в БД.

## Как скачать проект

1. Установи Git и .NET 8 SDK.
2. Открой PowerShell или терминал.
3. Склонируй репозиторий:

```powershell
git clone https://github.com/znoynext/autotests-trainer.git
```

4. Перейди в папку проекта:

```powershell
cd autotests-trainer
```

5. Восстанови зависимости:

```powershell
dotnet restore
```

## Как запустить сайт

Из корня проекта выполни:

```powershell
dotnet run --project .\src\AutotestsTrainer.Web
```

После запуска открой в браузере:

```text
http://localhost:5171
```

Демо-доступ:

```text
login: admin
password: admin
```

## Как запустить тесты

Из корня проекта:

```powershell
dotnet test
```

В проекте уже есть несколько стартовых интеграционных тестов. UI-тесты специально не добавлены: папка и зависимости подготовлены, чтобы писать их самостоятельно во время обучения.

## Что есть в приложении

- Главная страница.
- Форма авторизации.
- Панель после входа.
- Список задач.
- Создание задач через UI.
- API-консоль: `/api-console`.
- API для задач:
  - `GET /api/workitems`
  - `POST /api/workitems`
  - `POST /api/workitems/{id}/complete`
- SQLite-база, которая создается локально при запуске.

## Где писать тесты

Проект тестов находится здесь:

```text
tests/AutotestsTrainer.Tests
```

Рекомендуемая структура:

```text
tests/AutotestsTrainer.Tests
  TrainerIntegrationTests.cs       # примеры интеграционных/API-проверок
  Ui/                              # место для твоих UI-тестов Playwright
```

## Подготовка Playwright

Playwright уже подключен как NuGet-пакет. После первого `dotnet build` установи браузер Chromium:

```powershell
dotnet build
powershell -ExecutionPolicy Bypass -File .\tests\AutotestsTrainer.Tests\bin\Debug\net8.0\playwright.ps1 install chromium
```

После этого можно писать UI-тесты на C# с `Microsoft.Playwright.Xunit`.

## Минимальный маршрут обучения

1. Запусти сайт.
2. Открой главную страницу.
3. Перейди на страницу логина.
4. Войди как `admin / admin`.
5. Проверь, что открылась панель.
6. Создай задачу через UI.
7. Проверь список задач через API.
8. Проверь, что задача есть в SQLite.
9. Напиши негативный тест на неправильный пароль.
10. Напиши тест на закрытие задачи.

## Структура проекта

```text
src/AutotestsTrainer.Web           # ASP.NET Core MVC приложение
tests/AutotestsTrainer.Tests       # xUnit-проект автотестов
```

## Что не хранится в репозитории

В репозиторий не добавляются локальные и личные артефакты:

- `bin/`
- `obj/`
- локальная SQLite-база `*.db`
- файлы SQLite `*.db-shm` и `*.db-wal`
- локальные логи `*.log`

База данных создается автоматически при запуске приложения.

## Требования

- .NET 8 SDK
- Git
- Браузер для ручной проверки UI
- Опционально: VS Code или Visual Studio
- Опционально: Postman для ручной проверки API
- Опционально: SQLiteStudio или DBeaver для просмотра SQLite-базы

