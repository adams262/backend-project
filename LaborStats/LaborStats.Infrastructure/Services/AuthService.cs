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
using System.Security.Cryptography;

namespace LaborStats.Infrastructure.Services;

public sealed class AuthService(LaborStatsDbContext context, IOptions<JwtOptions> jwtOptions, TimeProvider timeProvider, IPasswordHasher<Users> passwordHasher) : IAuthService
{

    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    public async Task<AuthResponse?> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        Users? user = await context.User
            .AsNoTracking()
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Login == request.Login, cancellationToken);

        if (user is null)
            return null;

        var verification = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

        if (verification == PasswordVerificationResult.Failed)
            return null;

        var now = timeProvider.GetUtcNow();

        var accessTokenExpiresAt = now.AddMinutes(_jwtOptions.AccessTokenExpirationMinutes);
        string accessToken = GenerateJwtToken(user, accessTokenExpiresAt);

        string rawRefreshToken = GenerateRefreshToken();
        string tokenHash = HashToken(rawRefreshToken);
        var refreshTokenExpiresAt = now.AddDays(_jwtOptions.RefreshTokenExpirationDays);

        var refreshTokenEntity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            TokenHash = tokenHash,
            CreatedAt = now,
            ExpiresAt = refreshTokenExpiresAt
        };

        context.RefreshTokens.Add(refreshTokenEntity);
        await context.SaveChangesAsync(cancellationToken);

        return new AuthResponse(accessToken, accessTokenExpiresAt, rawRefreshToken, refreshTokenExpiresAt);
    }

    public async Task<RefreshTokenResponse?> RefreshTokenAsync(
    RefreshTokenRequest request,
    CancellationToken cancellationToken = default)
    {
        string hashedToken = HashToken(request.RefreshToken);
        var now = timeProvider.GetUtcNow();

        var tokenEntity = await context.RefreshTokens
            .Include(t => t.User)
            .ThenInclude(u => u.Role)
            .FirstOrDefaultAsync(t => t.TokenHash == hashedToken, cancellationToken);

        var isTokenInvalid =
            tokenEntity is null ||
            tokenEntity.ExpiresAt <= now ||
            tokenEntity.RevokedAt is not null;

        if (isTokenInvalid)
        {
            return null;
        }

        tokenEntity.LastUsedAt = now;
        await context.SaveChangesAsync(cancellationToken);

        var newAccessTokenExpiresAt = now.AddMinutes(_jwtOptions.AccessTokenExpirationMinutes);
        string newAccessToken = GenerateJwtToken(tokenEntity.User, newAccessTokenExpiresAt);

        return new RefreshTokenResponse(newAccessToken, newAccessTokenExpiresAt);
    }

    public async Task<bool> TryRevokeTokenAsync(
        RevokeTokenRequest request,
        CancellationToken cancellationToken = default)
    {
        string tokenHash = HashToken(request.RefreshToken);
        DateTimeOffset now = timeProvider.GetUtcNow();

        int affectedRows = await context.RefreshTokens
    .Where(token =>
        token.TokenHash == tokenHash &&
        token.RevokedAt == null)
    .ExecuteUpdateAsync(
        setters => setters.SetProperty(
            token => token.RevokedAt,
            now),
        cancellationToken);

        return affectedRows == 1;
    }

    private static string GenerateRefreshToken()
    {
        byte[] bytes = RandomNumberGenerator.GetBytes(64);
        return Base64UrlEncoder.Encode(bytes);
    }

    private static string HashToken(string token)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(token);
        byte[] hash = SHA256.HashData(bytes);

        return Convert.ToHexString(hash);
    }

    private string GenerateJwtToken(Users user, DateTimeOffset expiresAt)
    {
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
            expires: expiresAt.UtcDateTime,
            signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
