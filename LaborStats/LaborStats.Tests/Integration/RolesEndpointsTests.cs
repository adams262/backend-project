using System.Net;
using System.Text.Json;
using System.Net.Http.Json;

namespace LaborStats.Tests.Integration;

[Collection(TestCollections.Integration)]
public sealed class RolesEndpointsTests(
    TestSetup fixture)
    : IntegrationTestBase(fixture)
{
    [Fact]
    public async Task GetAll_WhenAuthenticatedAsAdmin_ReturnsOkAndRoles()
    {
        using HttpResponseMessage response =
            await Fixture.WebApiClient.GetAsync("/api/roles");

        string content =
            await response.Content.ReadAsStringAsync();

        Assert.True(
            response.StatusCode == HttpStatusCode.OK,
            $"Expected 200 OK, but received " +
            $"{(int)response.StatusCode} {response.StatusCode}. " +
            $"Response: {content}");

        Assert.False(string.IsNullOrWhiteSpace(content));

        using JsonDocument document =
            JsonDocument.Parse(content);

        Assert.Equal(
            JsonValueKind.Array,
            document.RootElement.ValueKind);

        string?[] roleNames =
        [.. document.RootElement
            .EnumerateArray()
            .Select(role =>
                role.GetProperty("name").GetString())];

    Assert.Contains("Admin", roleNames);
    Assert.Contains("BaseUser", roleNames);
    }

    [Fact]
    public async Task GetById_WhenRoleDoesNotExist_ReturnsNotFound()
    {
        Guid missingRoleId =
            Guid.NewGuid();

        using HttpResponseMessage response =
            await Fixture.WebApiClient.GetAsync(
                $"/api/roles/{missingRoleId}");

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task Create_WhenRequestIsValid_CreatesRole()
    {
        string roleName =
            $"TestRole-{Guid.NewGuid().ToString("N")[..8]}";

        using HttpResponseMessage createResponse =
            await Fixture.WebApiClient.PostAsJsonAsync(
                "/api/roles",
                new
                {
                    Name = roleName
                });

        string createContent =
            await createResponse.Content.ReadAsStringAsync();

        Assert.True(
            createResponse.StatusCode == HttpStatusCode.Created,
            $"Expected 201 Created, but received " +
            $"{(int)createResponse.StatusCode} " +
            $"{createResponse.StatusCode}. " +
            $"Response: {createContent}");

        using JsonDocument createDocument =
            JsonDocument.Parse(createContent);

        Guid roleId =
            createDocument.RootElement
                .GetProperty("id")
                .GetGuid();

        Assert.Equal(
            roleName,
            createDocument.RootElement
                .GetProperty("name")
                .GetString());

        Assert.NotNull(createResponse.Headers.Location);

        using HttpResponseMessage getResponse =
            await Fixture.WebApiClient.GetAsync(
                $"/api/roles/{roleId}");

        string getContent =
            await getResponse.Content.ReadAsStringAsync();

        Assert.True(
            getResponse.StatusCode == HttpStatusCode.OK,
            $"Expected 200 OK, but received " +
            $"{(int)getResponse.StatusCode} " +
            $"{getResponse.StatusCode}. " +
            $"Response: {getContent}");

        using JsonDocument getDocument =
            JsonDocument.Parse(getContent);

        Assert.Equal(
            roleId,
            getDocument.RootElement
                .GetProperty("id")
                .GetGuid());

        Assert.Equal(
            roleName,
            getDocument.RootElement
                .GetProperty("name")
                .GetString());
    }

    [Fact]
    public async Task Create_WhenRoleNameAlreadyExists_ReturnsConflict()
    {
        string roleName =
            $"Duplicate-{Guid.NewGuid().ToString("N")[..8]}";

        using HttpResponseMessage firstResponse =
            await Fixture.WebApiClient.PostAsJsonAsync(
                "/api/roles",
                new
                {
                    Name = roleName
                });

        Assert.Equal(
            HttpStatusCode.Created,
            firstResponse.StatusCode);

        using HttpResponseMessage secondResponse =
            await Fixture.WebApiClient.PostAsJsonAsync(
                "/api/roles",
                new
                {
                    Name = roleName
                });

        string content =
            await secondResponse.Content.ReadAsStringAsync();

        Assert.True(
            secondResponse.StatusCode == HttpStatusCode.Conflict,
            $"Expected 409 Conflict, but received " +
            $"{(int)secondResponse.StatusCode} " +
            $"{secondResponse.StatusCode}. " +
            $"Response: {content}");
    }

    [Fact]
    public async Task Create_WhenRoleNameIsEmpty_ReturnsBadRequest()
    {
        using HttpResponseMessage response =
            await Fixture.WebApiClient.PostAsJsonAsync(
                "/api/roles",
                new
                {
                    Name = string.Empty
                });

        string content =
            await response.Content.ReadAsStringAsync();

        Assert.True(
            response.StatusCode == HttpStatusCode.BadRequest,
            $"Expected 400 Bad Request, but received " +
            $"{(int)response.StatusCode} " +
            $"{response.StatusCode}. " +
            $"Response: {content}");
    }

    [Fact]
    public async Task GetAll_WhenAuthenticatedWithoutAdminRole_ReturnsForbidden()
    {
        using HttpRequestMessage request =
            new(HttpMethod.Get, "/api/roles");

        request.Headers.Add(
            TestAuthHandler.RoleHeader,
            "BaseUser");

        using HttpResponseMessage response =
            await Fixture.WebApiClient.SendAsync(request);

        Assert.Equal(
            HttpStatusCode.Forbidden,
            response.StatusCode);
    }

    [Fact]
    public async Task GetAll_WhenNotAuthenticated_ReturnsUnauthorized()
    {
        using HttpRequestMessage request =
            new(HttpMethod.Get, "/api/roles");

        request.Headers.Add(
            TestAuthHandler.UnauthenticatedHeader,
            "true");

        using HttpResponseMessage response =
            await Fixture.WebApiClient.SendAsync(request);

        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }
}
