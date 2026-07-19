// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;

namespace LaborStats.Application.Users;

public sealed record CreateUserRequest(
    string Name,
    string Login,
    string Email,
    string Password,
    Guid RoleId);

public sealed record UserResponse(
    Guid Id,
    string Name,
    string Login,
    string Email,
    string RoleName);

public sealed record UserDetailResponse(
    Guid Id,
    string Name,
    string Login,
    string Email,
    Guid RoleId,
    string RoleName);

public sealed record AssignRoleRequest(Guid RoleId);

public sealed record ChangeOwnPasswordRequest(string CurrentPassword, string NewPassword);

public sealed record SetUserPasswordRequest(string NewPassword);
