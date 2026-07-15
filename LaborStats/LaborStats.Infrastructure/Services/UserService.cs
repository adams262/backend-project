// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Text;
using LaborStats.Application.Abstractions;
using LaborStats.Application.Users;
using LaborStats.Domain.Entities;
using LaborStats.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LaborStats.Infrastructure.Services
{
    public sealed class UserService(LaborStatsDbContext context) : IUserService
    {
        private static readonly PasswordHasher<Users> Hasher = new();

        public async Task<UserResponse> CreateAsync(
            CreateUserRequest request,
            CancellationToken cancellationToken = default)
        {
            bool roleExists = await context.Role
                .AnyAsync(r => r.Id == request.RoleId, cancellationToken);

            if (!roleExists)
                throw new InvalidOperationException("The specified role does not exist.");

            bool loginTaken = await context.User
                .AnyAsync(u => u.Login == request.Login, cancellationToken);

            if (loginTaken)
                throw new InvalidOperationException("This login is already taken.");

            bool emailTaken = await context.User
                .AnyAsync(u => u.Email == request.Email, cancellationToken);

            if (emailTaken)
                throw new InvalidOperationException("This email is already taken.");

            var user = new Users
            {
                Name = request.Name,
                Login = request.Login,
                Email = request.Email,
                RoleId = request.RoleId
            };

            user.PasswordHash = Hasher.HashPassword(user, request.Password);

            context.User.Add(user);
            await context.SaveChangesAsync(cancellationToken);

            string roleName = await context.Role
                .Where(r => r.Id == request.RoleId)
                .Select(r => r.Name)
                .FirstAsync(cancellationToken);

            return new UserResponse(user.Id, user.Name, user.Login, user.Email, roleName);
        }

        public async Task<IReadOnlyList<UserResponse>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await context.User
                .Include(u => u.Role)
                .OrderBy(u => u.Name)
                .Select(u => new UserResponse(u.Id, u.Name, u.Login, u.Email, u.Role.Name))
                .ToListAsync(cancellationToken);
        }

        public async Task<UserDetailResponse?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return await context.User
                .Include(u => u.Role)
                .Where(u => u.Id == id)
                .Select(u => new UserDetailResponse(u.Id, u.Name, u.Login, u.Email, u.RoleId, u.Role.Name))
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> AssignRoleAsync(
            Guid userId,
            AssignRoleRequest request,
            CancellationToken cancellationToken = default)
        {
            Users? user = await context.User
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user is null)
                return false;

            bool roleExists = await context.Role
                .AnyAsync(r => r.Id == request.RoleId, cancellationToken);

            if (!roleExists)
                throw new InvalidOperationException("The specified role does not exist.");

            user.RoleId = request.RoleId;
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
