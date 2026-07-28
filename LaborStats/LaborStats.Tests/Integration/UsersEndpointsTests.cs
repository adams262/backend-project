using System.Net;
using System.Net.Http.Json;
using LaborStats.Application.Roles;
using LaborStats.Application.Users;

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
            await Fixture.WebApiClient.GetAsync(
                "/api/users");

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        UserResponse[]? users =
            await response.Content
                .ReadFromJsonAsync<UserResponse[]>();

        Assert.NotNull(users);
        Assert.NotEmpty(users);
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

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        UserDetailResponse? user =
            await response.Content
                .ReadFromJsonAsync<UserDetailResponse>();

        Assert.NotNull(user);

        Assert.Equal(
            userId,
            user.Id);

        Assert.Equal(
            login,
            user.Login);
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

        Assert.Equal(
            HttpStatusCode.Created,
            createResponse.StatusCode);

        Assert.NotNull(
            createResponse.Headers.Location);

        UserResponse? createdUser =
            await createResponse.Content
                .ReadFromJsonAsync<UserResponse>();

        Assert.NotNull(createdUser);

        Assert.Equal(
            login,
            createdUser.Login);

        Assert.Equal(
            email,
            createdUser.Email);

        using HttpResponseMessage getResponse =
            await Fixture.WebApiClient.GetAsync(
                $"/api/users/{createdUser.Id}");

        Assert.Equal(
            HttpStatusCode.OK,
            getResponse.StatusCode);

        UserDetailResponse? returnedUser =
            await getResponse.Content
                .ReadFromJsonAsync<UserDetailResponse>();

        Assert.NotNull(returnedUser);

        Assert.Equal(
            createdUser.Id,
            returnedUser.Id);

        Assert.Equal(
            login,
            returnedUser.Login);

        Assert.Equal(
            roleId,
            returnedUser.RoleId);
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

        Assert.Equal(
            HttpStatusCode.Conflict,
            response.StatusCode);
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

        Assert.Equal(
            HttpStatusCode.NoContent,
            assignResponse.StatusCode);

        using HttpResponseMessage getResponse =
            await Fixture.WebApiClient.GetAsync(
                $"/api/users/{userId}");

        Assert.Equal(
            HttpStatusCode.OK,
            getResponse.StatusCode);

        UserDetailResponse? returnedUser =
            await getResponse.Content
                .ReadFromJsonAsync<UserDetailResponse>();

        Assert.NotNull(returnedUser);

        Assert.Equal(
            adminRoleId,
            returnedUser.RoleId);
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

        Assert.Equal(
            HttpStatusCode.NoContent,
            changeResponse.StatusCode);
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

    private async Task<Guid> GetRoleIdAsync(
        string roleName)
    {
        using HttpResponseMessage response =
            await Fixture.WebApiClient.GetAsync(
                "/api/roles");

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        RoleResponse[]? roles =
            await response.Content
                .ReadFromJsonAsync<RoleResponse[]>();

        Assert.NotNull(roles);

        RoleResponse role =
            roles.Single(role =>
                role.Name == roleName);

        return role.Id;
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

        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);

        UserResponse? user =
            await response.Content
                .ReadFromJsonAsync<UserResponse>();

        Assert.NotNull(user);

        return user.Id;
    }
}
