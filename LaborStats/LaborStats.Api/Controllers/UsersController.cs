// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using LaborStats.Application.Abstractions;
using LaborStats.Application.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LaborStats.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize(Roles = "Admin")]
    public sealed class UsersController(IUserService userService) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<UserResponse>> Create(
            CreateUserRequest request,
            CancellationToken cancellationToken)
        {
            var result = await userService.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<UserResponse>>> GetAll(
            CancellationToken cancellationToken)
        {
            return Ok(await userService.GetAllAsync(cancellationToken));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserDetailResponse>> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var result = await userService.GetByIdAsync(id, cancellationToken);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpPut("{id:guid}/role")]
        public async Task<IActionResult> AssignRole(
            Guid id,
            AssignRoleRequest request,
            CancellationToken cancellationToken)
        {
            bool success = await userService.AssignRoleAsync(id, request, cancellationToken);
            return success ? NoContent() : NotFound();
        }
    }
}
