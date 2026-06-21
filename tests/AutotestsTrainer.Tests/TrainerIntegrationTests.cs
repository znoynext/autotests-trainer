using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;

namespace AutotestsTrainer.Tests;

public class TrainerIntegrationTests : IClassFixture<TrainerWebApplicationFactory>
{
    private readonly TrainerWebApplicationFactory _factory;

    public TrainerIntegrationTests(TrainerWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task HomePage_ShowsDemoCredentials()
    {
        using var client = _factory.CreateClient();

        var response = await client.GetAsync("/");
        response.EnsureSuccessStatusCode();

        var html = await response.Content.ReadAsStringAsync();

        Assert.Contains("admin", html);
    }

    [Fact]
    public async Task Login_AllowsAccessToDashboard()
    {
        using var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        var token = await GetAntiForgeryTokenAsync(client, "/account/login");
        var response = await client.PostAsync("/account/login", new FormUrlEncodedContent(new Dictionary<string, string?>
        {
            ["Username"] = "admin",
            ["Password"] = "admin",
            ["ReturnUrl"] = "",
            ["__RequestVerificationToken"] = token
        }));

        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);

        var dashboard = await client.GetAsync("/dashboard");
        dashboard.EnsureSuccessStatusCode();

        var html = await dashboard.Content.ReadAsStringAsync();
        Assert.Contains("Панель", html);
    }

    [Fact]
    public async Task ApiConsole_PageLoadsAndHasButtons()
    {
        using var client = await CreateAuthenticatedClientAsync();

        var response = await client.GetAsync("/api-console");
        response.EnsureSuccessStatusCode();

        var html = await response.Content.ReadAsStringAsync();
        Assert.Contains("API", html);
        Assert.Contains("Получить список задач", html);
    }

    [Fact]
    public async Task HomePage_StaysSeparateFromDashboard_WhenLoggedIn()
    {
        using var client = await CreateAuthenticatedClientAsync();

        var response = await client.GetAsync("/");
        response.EnsureSuccessStatusCode();

        var html = await response.Content.ReadAsStringAsync();
        Assert.Contains("АВТОТЕСТЫ", html);
        Assert.DoesNotContain("Всего задач", html);
    }

    [Fact]
    public async Task Api_CreatesItem_AndPersistsItInSqlite()
    {
        using var client = await CreateAuthenticatedClientAsync();

        var payload = JsonSerializer.Serialize(new
        {
            title = "Проверить сохранение",
            description = "Новая запись через API"
        });

        var postResponse = await client.PostAsync(
            "/api/workitems",
            new StringContent(payload, Encoding.UTF8, "application/json"));

        Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

        var listResponse = await client.GetAsync("/api/workitems");
        listResponse.EnsureSuccessStatusCode();

        var json = await listResponse.Content.ReadAsStringAsync();
        Assert.Contains("Проверить сохранение", json);

        using var connection = new SqliteConnection($"Data Source={_factory.DatabasePath}");
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = "select count(*) from WorkItems where Title = $title";
        command.Parameters.AddWithValue("$title", "Проверить сохранение");

        var count = Convert.ToInt32(await command.ExecuteScalarAsync());
        Assert.Equal(1, count);
    }

    private async Task<HttpClient> CreateAuthenticatedClientAsync()
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        var token = await GetAntiForgeryTokenAsync(client, "/account/login");
        var loginResponse = await client.PostAsync("/account/login", new FormUrlEncodedContent(new Dictionary<string, string?>
        {
            ["Username"] = "admin",
            ["Password"] = "admin",
            ["ReturnUrl"] = "",
            ["__RequestVerificationToken"] = token
        }));

        Assert.Equal(HttpStatusCode.Redirect, loginResponse.StatusCode);
        return client;
    }

    private static async Task<string> GetAntiForgeryTokenAsync(HttpClient client, string path)
    {
        var html = await client.GetStringAsync(path);
        const string marker = "name=\"__RequestVerificationToken\" type=\"hidden\" value=\"";
        var start = html.IndexOf(marker, StringComparison.Ordinal);
        Assert.True(start >= 0, "Не найден anti-forgery token");

        start += marker.Length;
        var end = html.IndexOf('"', start);
        Assert.True(end > start, "Не удалось прочитать anti-forgery token");

        return html[start..end];
    }
}
