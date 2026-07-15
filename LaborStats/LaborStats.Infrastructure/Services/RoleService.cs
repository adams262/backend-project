// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;
using LaborStats.Application.Abstractions;
using LaborStats.Application.Roles;
using LaborStats.Domain.Entities;
using LaborStats.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LaborStats.Infrastructure.Services
{
    public sealed class RoleService(LaborStatsDbContext context) : IRoleService
    {
        public async Task<RoleResponse> CreateAsync(
            CreateRoleRequest request,
            CancellationToken cancellationToken = default)
        {
            bool exists = await context.Role
                .AnyAsync(r => r.Name == request.Name, cancellationToken);

            if (exists)
                throw new InvalidOperationException($"Role '{request.Name}' already exists.");

            var role = new Roles { Name = request.Name };

            context.Role.Add(role);
            await context.SaveChangesAsync(cancellationToken);

            return new RoleResponse(role.Id, role.Name);
        }

        public async Task<IReadOnlyList<RoleResponse>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await context.Role
                .OrderBy(r => r.Name)
                .Select(r => new RoleResponse(r.Id, r.Name))
                .ToListAsync(cancellationToken);
        }
    }
}
