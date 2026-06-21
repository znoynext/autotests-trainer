# UI Tests with Playwright

Эта папка подготовлена для будущих UI-тестов. Готовые UI-тесты сюда не добавлены специально: идея проекта в том, чтобы писать их самостоятельно во время обучения.

## Что уже подключено

- `Microsoft.Playwright.Xunit`
- xUnit
- Chromium для Playwright
- тестовый проект `AutotestsTrainer.Tests`

## Как начать первый UI-тест

1. Создай в этой папке файл, например `LoginUiTests.cs`.
2. Подключи пространства имен:

```csharp
using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;
```

3. Создай класс, который наследуется от `PageTest`.
4. В тесте открой сайт:

```csharp
await Page.GotoAsync("http://localhost:5171");
```

## Полезные адреса стенда

- Главная: `http://localhost:5171`
- Логин: `http://localhost:5171/account/login`
- Панель: `http://localhost:5171/dashboard`
- Задачи: `http://localhost:5171/workitems`
- API-консоль: `http://localhost:5171/api-console`

## Демо-доступ

- Логин: `admin`
- Пароль: `admin`

## Что потренировать

1. Открыть главную страницу и проверить заголовок.
2. Перейти на страницу логина.
3. Ввести `admin` / `admin`.
4. Проверить, что после входа открылась панель.
5. Перейти в задачи.
6. Создать задачу через форму.
7. Проверить, что новая задача появилась в таблице.

