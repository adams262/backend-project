// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using LaborStats.Application.Abstractions;
using LaborStats.Application.Auth;
using LaborStats.Domain.Entities;
using LaborStats.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using LaborStats.Infrastructure.Options;
using Microsoft.EntityFrameworkCore;

namespace LaborStats.Infrastructure.Services
{
    public sealed class AuthService(LaborStatsDbContext context, IOptions<JwtOptions> jwtOptions) : IAuthService
    {
        private static readonly PasswordHasher<Users> Hasher = new();
        private readonly JwtOptions _jwtOptions = jwtOptions.Value;

        public async Task<LoginResponse?> LoginAsync(
            LoginRequest request,
            CancellationToken cancellationToken = default)
        {
            Users? user = await context.User
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Login == request.Login, cancellationToken);

            if (user is null)
                return null;

            var verification = Hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (verification == PasswordVerificationResult.Failed)
                return null;

            var expiresAt = DateTime.UtcNow.AddHours(_jwtOptions.ExpiryHours);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Login),
            new Claim(ClaimTypes.Role, user.Role.Name)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials);

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new LoginResponse(tokenString, expiresAt);
        }
    }
}
