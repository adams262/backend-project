using System.Net;
using System.Text.Json;
using System.Net.Http.Json;

namespace LaborStats.Tests.Integration;

[Collection(TestCollections.Integration)]
public sealed class UsersEndpointsTests(
    TestSetup fixture)
    : IntegrationTestBase(fixture)
{
    [Fact]
    public async Task GetAll_WhenAuthenticatedAsAdmin_ReturnsOkAndUsers()
    {
        using HttpResponseMessage response =
            await Fixture.WebApiClient.GetAsync("/api/users");

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

        Assert.True(
            document.RootElement.GetArrayLength() > 0,
            "The users collection should contain the seeded administrator.");
    }

    [Fact]
    public async Task GetById_WhenUserExists_ReturnsOkAndUser()
    {
        Guid roleId =
            await GetRoleIdAsync("BaseUser");

        string suffix =
            Guid.NewGuid().ToString("N")[..8];

        string login =
            $"get-user-{suffix}";

        Guid userId =
            await CreateUserAsync(
                roleId,
                login,
                $"get-user-{suffix}@local.com",
                "IntegrationTest123");

        using HttpResponseMessage response =
            await Fixture.WebApiClient.GetAsync(
                $"/api/users/{userId}");

        string content =
            await response.Content.ReadAsStringAsync();

        Assert.True(
            response.StatusCode == HttpStatusCode.OK,
            $"Expected 200 OK, but received " +
            $"{(int)response.StatusCode} {response.StatusCode}. " +
            $"Response: {content}");

        using JsonDocument document =
            JsonDocument.Parse(content);

        Assert.Equal(
            userId,
            document.RootElement
                .GetProperty("id")
                .GetGuid());

        Assert.Equal(
            login,
            document.RootElement
                .GetProperty("login")
                .GetString());
    }

    [Fact]
    public async Task GetById_WhenUserDoesNotExist_ReturnsNotFound()
    {
        Guid missingUserId =
            Guid.NewGuid();

        using HttpResponseMessage response =
            await Fixture.WebApiClient.GetAsync(
                $"/api/users/{missingUserId}");

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task GetAll_WhenAuthenticatedWithoutAdminRole_ReturnsForbidden()
    {
        using HttpRequestMessage request =
            new(HttpMethod.Get, "/api/users");

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
            new(HttpMethod.Get, "/api/users");

        request.Headers.Add(
            TestAuthHandler.UnauthenticatedHeader,
            "true");

        using HttpResponseMessage response =
            await Fixture.WebApiClient.SendAsync(request);

        Assert.Equal(
            HttpStatusCode.Unauthorized,
            response.StatusCode);
    }

    [Fact]
    public async Task Create_WhenRequestIsValid_CreatesUser()
    {
        Guid roleId =
            await GetRoleIdAsync("BaseUser");

        string suffix =
            Guid.NewGuid().ToString("N")[..8];

        string login =
            $"test-user-{suffix}";

        string email =
            $"test-user-{suffix}@local";

        using HttpResponseMessage createResponse =
            await Fixture.WebApiClient.PostAsJsonAsync(
                "/api/users",
                new
                {
                    Name = "Integration Test User",
                    Login = login,
                    Email = email,
                    Password = "IntegrationTest123",
                    RoleId = roleId
                });

        string createContent =
            await createResponse.Content.ReadAsStringAsync();

        Assert.True(
            createResponse.StatusCode == HttpStatusCode.Created,
            $"Expected 201 Created, but received " +
            $"{(int)createResponse.StatusCode} " +
            $"{createResponse.StatusCode}. " +
            $"Response: {createContent}");

        Assert.NotNull(createResponse.Headers.Location);

        using JsonDocument createDocument =
            JsonDocument.Parse(createContent);

        Guid userId =
            createDocument.RootElement
                .GetProperty("id")
                .GetGuid();

        Assert.Equal(
            login,
            createDocument.RootElement
                .GetProperty("login")
                .GetString());

        Assert.Equal(
            email,
            createDocument.RootElement
                .GetProperty("email")
                .GetString());

        using HttpResponseMessage getResponse =
            await Fixture.WebApiClient.GetAsync(
                $"/api/users/{userId}");

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
            userId,
            getDocument.RootElement
                .GetProperty("id")
                .GetGuid());

        Assert.Equal(
            login,
            getDocument.RootElement
                .GetProperty("login")
                .GetString());

        Assert.Equal(
            roleId,
            getDocument.RootElement
                .GetProperty("roleId")
                .GetGuid());
    }

    private async Task<Guid> GetRoleIdAsync(
    string roleName)
    {
        using HttpResponseMessage response =
            await Fixture.WebApiClient.GetAsync(
                "/api/roles");

        string content =
            await response.Content.ReadAsStringAsync();

        Assert.True(
            response.StatusCode == HttpStatusCode.OK,
            $"Expected 200 OK, but received " +
            $"{(int)response.StatusCode} " +
            $"{response.StatusCode}. " +
            $"Response: {content}");

        using JsonDocument document =
            JsonDocument.Parse(content);

        JsonElement role =
            document.RootElement
                .EnumerateArray()
                .Single(element =>
                    element
                        .GetProperty("name")
                        .GetString() == roleName);

        return role
            .GetProperty("id")
            .GetGuid();
    }

    [Fact]
    public async Task Create_WhenLoginAlreadyExists_ReturnsConflict()
    {
        Guid roleId =
            await GetRoleIdAsync("BaseUser");

        string suffix =
            Guid.NewGuid().ToString("N")[..8];

        string login =
            $"duplicate-user-{suffix}";

        await CreateUserAsync(
            roleId,
            login,
            $"first-{suffix}@local",
            "IntegrationTest123");

        using HttpResponseMessage response =
            await Fixture.WebApiClient.PostAsJsonAsync(
                "/api/users",
                new
                {
                    Name = "Second Integration User",
                    Login = login,
                    Email = $"second-{suffix}@local",
                    Password = "IntegrationTest123",
                    RoleId = roleId
                });

        string content =
            await response.Content.ReadAsStringAsync();

        Assert.True(
            response.StatusCode == HttpStatusCode.Conflict,
            $"Expected 409 Conflict, but received " +
            $"{(int)response.StatusCode} " +
            $"{response.StatusCode}. " +
            $"Response: {content}");
    }

    [Fact]
    public async Task AssignRole_WhenUserAndRoleExist_UpdatesUserRole()
    {
        Guid baseUserRoleId =
            await GetRoleIdAsync("BaseUser");

        Guid adminRoleId =
            await GetRoleIdAsync("Admin");

        string suffix =
            Guid.NewGuid().ToString("N")[..8];

        Guid userId =
            await CreateUserAsync(
                baseUserRoleId,
                $"role-user-{suffix}",
                $"role-user-{suffix}@local",
                "IntegrationTest123");

        using HttpResponseMessage assignResponse =
            await Fixture.WebApiClient.PutAsJsonAsync(
                $"/api/users/{userId}/role",
                new
                {
                    RoleId = adminRoleId
                });

        string assignContent =
            await assignResponse.Content.ReadAsStringAsync();

        Assert.True(
            assignResponse.StatusCode ==
            HttpStatusCode.NoContent,
            $"Expected 204 No Content, but received " +
            $"{(int)assignResponse.StatusCode} " +
            $"{assignResponse.StatusCode}. " +
            $"Response: {assignContent}");

        using HttpResponseMessage getResponse =
            await Fixture.WebApiClient.GetAsync(
                $"/api/users/{userId}");

        string getContent =
            await getResponse.Content.ReadAsStringAsync();

        Assert.True(
            getResponse.StatusCode == HttpStatusCode.OK,
            $"Expected 200 OK, but received " +
            $"{(int)getResponse.StatusCode} " +
            $"{getResponse.StatusCode}. " +
            $"Response: {getContent}");

        using JsonDocument document =
            JsonDocument.Parse(getContent);

        Assert.Equal(
            adminRoleId,
            document.RootElement
                .GetProperty("roleId")
                .GetGuid());
    }

    [Fact]
    public async Task SetPassword_WhenUserExists_UpdatesPassword()
    {
        Guid roleId =
            await GetRoleIdAsync("BaseUser");

        string suffix =
            Guid.NewGuid().ToString("N")[..8];

        Guid userId =
            await CreateUserAsync(
                roleId,
                $"password-user-{suffix}",
                $"password-user-{suffix}@local",
                "OriginalPassword123");

        const string administratorPassword =
            "Administrator123";

        using HttpResponseMessage setPasswordResponse =
            await Fixture.WebApiClient.PutAsJsonAsync(
                $"/api/users/{userId}/password",
                new
                {
                    NewPassword = administratorPassword
                });

        Assert.Equal(
            HttpStatusCode.NoContent,
            setPasswordResponse.StatusCode);

        using HttpRequestMessage changeRequest =
            new(
                HttpMethod.Put,
                "/api/users/me/password")
            {
                Content = JsonContent.Create(
                    new
                    {
                        CurrentPassword =
                            administratorPassword,

                        NewPassword =
                            "FinalPassword123"
                    })
            };

        changeRequest.Headers.Add(
            TestAuthHandler.UserIdHeader,
            userId.ToString());

        using HttpResponseMessage changeResponse =
            await Fixture.WebApiClient.SendAsync(
                changeRequest);

        string changeContent =
            await changeResponse.Content.ReadAsStringAsync();

        Assert.True(
            changeResponse.StatusCode ==
            HttpStatusCode.NoContent,
            $"Expected 204 No Content, but received " +
            $"{(int)changeResponse.StatusCode} " +
            $"{changeResponse.StatusCode}. " +
            $"Response: {changeContent}");
    }

    [Fact]
    public async Task Create_WhenEmailAlreadyExists_ReturnsConflict()
    {
        Guid roleId =
            await GetRoleIdAsync("BaseUser");

        string suffix =
            Guid.NewGuid().ToString("N")[..8];

        string email =
            $"duplicate-email-{suffix}@local";

        await CreateUserAsync(
            roleId,
            $"email-user-{suffix}",
            email,
            "IntegrationTest123");

        using HttpResponseMessage response =
            await Fixture.WebApiClient.PostAsJsonAsync(
                "/api/users",
                new
                {
                    Name = "Email Integration User",
                    Login = $"email-user-{suffix}",
                    Email = email,
                    Password = "IntegrationTest123",
                    RoleId = roleId
                });

        Assert.Equal(
            HttpStatusCode.Conflict,
            response.StatusCode);
    }

    private async Task<Guid> CreateUserAsync(
        Guid roleId,
        string login,
        string email,
        string password)
    {
        using HttpResponseMessage response =
            await Fixture.WebApiClient.PostAsJsonAsync(
                "/api/users",
                new
                {
                    Name = "Integration Test User",
                    Login = login,
                    Email = email,
                    Password = password,
                    RoleId = roleId
                });

        string content =
            await response.Content.ReadAsStringAsync();

        Assert.True(
            response.StatusCode == HttpStatusCode.Created,
            $"Expected 201 Created, but received " +
            $"{(int)response.StatusCode} " +
            $"{response.StatusCode}. " +
            $"Response: {content}");

        using JsonDocument document =
            JsonDocument.Parse(content);

        return document.RootElement
            .GetProperty("id")
            .GetGuid();
    }
}
