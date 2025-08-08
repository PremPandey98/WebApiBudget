using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using WebApiBudget.DomainOrCore.Entities;
using WebApiBudget.DomainOrCore.Interfaces;
using WebApiBudget.DomainOrCore.Models;

namespace WebApiBudget.Infrastucture.Services
{
    public class CacheBasedOtpService : IOtpService
    {
        private readonly IMemoryCache _cache;
        private readonly EmailSettings _emailSettings;

        public CacheBasedOtpService(IMemoryCache cache, IOptions<EmailSettings> emailSettings)
        {
            _cache = cache;
            _emailSettings = emailSettings.Value;
        }

        public async Task<string> GenerateOtpAsync(string email, OtpType type)
        {
            // Generate new OTP
            var otpCode = GenerateRandomOtp();
            var cacheKey = GetCacheKey(email, type);
            
            // Store OTP in cache with expiration
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_emailSettings.OtpExpirationMinutes),
                Priority = CacheItemPriority.Normal
            };
            
            var otpData = new CachedOtpData
            {
                OtpCode = otpCode,
                Email = email,
                Type = type,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_emailSettings.OtpExpirationMinutes),
                IsUsed = false
            };
            
            _cache.Set(cacheKey, otpData, cacheOptions);
            
            return await Task.FromResult(otpCode);
        }

        public async Task<bool> ValidateOtpAsync(string email, string otpCode, OtpType type)
        {
            var cacheKey = GetCacheKey(email, type);
            
            if (_cache.TryGetValue(cacheKey, out CachedOtpData? cachedOtp))
            {
                if (cachedOtp != null && 
                    cachedOtp.OtpCode == otpCode && 
                    !cachedOtp.IsUsed && 
                    cachedOtp.ExpiresAt > DateTime.UtcNow)
                {
                    // Mark as used
                    cachedOtp.IsUsed = true;
                    cachedOtp.UsedAt = DateTime.UtcNow;
                    _cache.Set(cacheKey, cachedOtp);
                    
                    return await Task.FromResult(true);
                }
            }
            
            return await Task.FromResult(false);
        }

        public async Task<bool> InvalidateOtpAsync(string email, OtpType type)
        {
            var cacheKey = GetCacheKey(email, type);
            _cache.Remove(cacheKey);
            return await Task.FromResult(true);
        }

        public string GenerateRandomOtp(int length = 6)
        {
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[4];
            rng.GetBytes(bytes);
            var random = BitConverter.ToUInt32(bytes, 0);
            
            var otp = (random % (int)Math.Pow(10, length)).ToString().PadLeft(length, '0');
            return otp;
        }

        private static string GetCacheKey(string email, OtpType type)
        {
            return $"otp_{email.ToLower()}_{type}";
        }
    }

    public class CachedOtpData
    {
        public string OtpCode { get; set; } = null!;
        public string Email { get; set; } = null!;
        public OtpType Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; }
        public DateTime? UsedAt { get; set; }
    }
}