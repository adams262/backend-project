using System.Net;
using System.Net.Http.Json;
using LaborStats.Application.Roles;

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

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        RoleResponse[]? roles =
            await response.Content
                .ReadFromJsonAsync<RoleResponse[]>();

        Assert.NotNull(roles);

        string[] roleNames =
        [
            .. roles.Select(role => role.Name)
        ];

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

        Assert.Equal(
            HttpStatusCode.Created,
            createResponse.StatusCode);

        Assert.NotNull(
            createResponse.Headers.Location);

        RoleResponse? createdRole =
            await createResponse.Content
                .ReadFromJsonAsync<RoleResponse>();

        Assert.NotNull(createdRole);

        Assert.Equal(
            roleName,
            createdRole.Name);

        using HttpResponseMessage getResponse =
            await Fixture.WebApiClient.GetAsync(
                $"/api/roles/{createdRole.Id}");

        Assert.Equal(
            HttpStatusCode.OK,
            getResponse.StatusCode);

        RoleResponse? returnedRole =
            await getResponse.Content
                .ReadFromJsonAsync<RoleResponse>();

        Assert.NotNull(returnedRole);

        Assert.Equal(
            createdRole.Id,
            returnedRole.Id);

        Assert.Equal(
            roleName,
            returnedRole.Name);
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

        Assert.Equal(
            HttpStatusCode.Conflict,
            secondResponse.StatusCode);
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

        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
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
