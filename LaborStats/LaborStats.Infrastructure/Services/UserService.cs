// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;
using LaborStats.Application.Abstractions;
using LaborStats.Application.Users;
using LaborStats.Domain.Entities;
using LaborStats.Domain.Exceptions;
using LaborStats.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LaborStats.Infrastructure.Services;

public sealed class UserService(LaborStatsDbContext context, IPasswordHasher<Users> passwordHasher) : IUserService
{

    public async Task<UserResponse> CreateAsync(
        CreateUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var roleName = await context.Role
        .Where(r => r.Id == request.RoleId)
        .Select(r => r.Name)
        .FirstOrDefaultAsync(cancellationToken);

        if (roleName is null)
        {
            throw new NotFoundException(nameof(Roles), request.RoleId);
        }

        var existingUserData = await context.User
            .Where(u => u.Login == request.Login || u.Email == request.Email)
            .Select(u => new
            {
                LoginTaken = u.Login == request.Login,
                EmailTaken = u.Email == request.Email
            })
            .ToListAsync(cancellationToken);

        if (existingUserData.Any(x => x.LoginTaken))
        {
            throw new ConflictException("This login is already taken.");
        }

        if (existingUserData.Any(x => x.EmailTaken))
        {
            throw new ConflictException("This email is already taken.");
        }

        var user = new Users
        {
            Name = request.Name,
            Login = request.Login,
            Email = request.Email,
            RoleId = request.RoleId
        };

        user.PasswordHash = passwordHasher.HashPassword(user, request.Password);

        context.User.Add(user);
        await context.SaveChangesAsync(cancellationToken);

        

        return new UserResponse(user.Id, user.Name, user.Login, user.Email, roleName);
    }

    public async Task<IReadOnlyList<UserResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await context.User
            .OrderBy(u => u.Name)
            .Select(u => new UserResponse(u.Id, u.Name, u.Login, u.Email, u.Role.Name))
            .ToListAsync(cancellationToken);
    }

    public async Task<UserDetailResponse?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await context.User
            .Where(u => u.Id == id)
            .Select(u => new UserDetailResponse(u.Id, u.Name, u.Login, u.Email, u.RoleId, u.Role.Name))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AssignRoleAsync(
        Guid userId,
        AssignRoleRequest request,
        CancellationToken cancellationToken = default)
    {
        Users? user = await context.User
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user is null)
        {
            throw new NotFoundException(nameof(Users), userId);
        }    

        bool roleExists = await context.Role
            .AnyAsync(r => r.Id == request.RoleId, cancellationToken);

        if (!roleExists)
        {
            throw new NotFoundException(nameof(Roles), request.RoleId);
        }

        user.RoleId = request.RoleId;
        await context.SaveChangesAsync(cancellationToken);
        
    }
    public async Task ChangeOwnPasswordAsync(
    Guid userId,
    ChangeOwnPasswordRequest request,
    CancellationToken cancellationToken = default)
    {
        Users? user = await context.User
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user is null)
        {
            throw new NotFoundException(nameof(Users), userId);
        }

        var verification = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.CurrentPassword);

        if (verification == PasswordVerificationResult.Failed)
        {
            throw new ConflictException("Current password is incorrect.");
        }

        user.PasswordHash = passwordHasher.HashPassword(user, request.NewPassword);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task SetPasswordAsync(
        Guid userId,
        SetUserPasswordRequest request,
        CancellationToken cancellationToken = default)
    {
        Users? user = await context.User
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (user is null)
        {
            throw new NotFoundException(nameof(Users), userId);
        }

        user.PasswordHash = passwordHasher.HashPassword(user, request.NewPassword);
        await context.SaveChangesAsync(cancellationToken);
    }
}

