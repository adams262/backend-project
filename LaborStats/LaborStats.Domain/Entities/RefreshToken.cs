using System;
using System.Collections.Generic;
using System.Text;

namespace LaborStats.Domain.Entities
{
    public sealed class RefreshToken
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Users User { get; set; } = null!;
        public string TokenHash { get; set; } = null!;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset ExpiresAt { get; set; }
        public DateTimeOffset? LastUsedAt { get; set; }
        public DateTimeOffset? RevokedAt { get; set; }
    }

}
