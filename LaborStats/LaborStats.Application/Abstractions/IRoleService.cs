// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;
using LaborStats.Application.Roles;

namespace LaborStats.Application.Abstractions;

public interface IRoleService
{
    Task<RoleResponse> CreateAsync(CreateRoleRequest request, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<RoleResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<RoleResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}


