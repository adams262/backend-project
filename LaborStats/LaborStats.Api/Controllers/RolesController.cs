// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using LaborStats.Application.Abstractions;
using LaborStats.Application.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LaborStats.Api.Controllers
{
    [ApiController]
    [Route("api/roles")]
    [Authorize(Roles = "Admin")]
    public sealed class RolesController(IRoleService roleService) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<RoleResponse>> Create(
            CreateRoleRequest request,
            CancellationToken cancellationToken)
        {
            var result = await roleService.CreateAsync(request, cancellationToken);
            return CreatedAtAction(nameof(GetAll), new { }, result);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<RoleResponse>>> GetAll(
            CancellationToken cancellationToken)
        {
            return Ok(await roleService.GetAllAsync(cancellationToken));
        }
    }
}
