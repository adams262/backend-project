// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;
using LaborStats.Application.Users;

namespace LaborStats.Application.Abstractions
{
    public interface IUserService
    {
        Task<UserResponse> CreateAsync(CreateUserRequest request, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<UserResponse>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<UserDetailResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> AssignRoleAsync(Guid userId, AssignRoleRequest request, CancellationToken cancellationToken = default);
    }
}
