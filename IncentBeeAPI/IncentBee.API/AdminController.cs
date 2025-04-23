using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IncentBee.API.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IncentBee.API.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly IncentBeeDbContext _context;
        private readonly ILogger<AdminController> _logger;
        
        public AdminController(IncentBeeDbContext context, ILogger<AdminController> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        #region Users
        
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers(int page = 1, int pageSize = 20)
        {
            try
            {
                var users = await _context.Users
                    .OrderByDescending(u => u.CreatedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(u => new {
                        u.UserId,
                        u.Username,
                        u.Email,
                        u.CreatedAt,
                        u.LastLogin,
                        u.PointsBalance,
                        u.AccountStatus,
                        u.Country,
                        ReferrerId = u.ReferrerId,
                        ReferrerName = u.Referrer != null ? u.Referrer.Username : null
                    })
                    .ToListAsync();
                
                var totalCount = await _context.Users.CountAsync();
                
                return Ok(new { 
                    users,
                    totalCount,
                    page,
                    pageSize,
                    totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting users");
                return StatusCode(500, "Error retrieving users");
            }
        }
        
        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                var user = await _context.Users
                    .Where(u => u.UserId == id)
                    .Select(u => new {
                        u.UserId,
                        u.Username,
                        u.Email,
                        u.CreatedAt,
                        u.LastLogin,
                        u.PointsBalance,
                        u.AccountStatus,
                        u.Country,
                        u.ExternalId,
                        u.IpAddress,
                        u.DeviceInfo,
                        ReferrerId = u.ReferrerId,
                        ReferrerName = u.Referrer != null ? u.Referrer.Username : null
                    })
                    .FirstOrDefaultAsync();
                
                if (user == null)
                {
                    return NotFound();
                }
                
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting user {id}");
                return StatusCode(500, "Error retrieving user");
            }
        }
        
        [HttpGet("users/{id}/completions")]
        public async Task<IActionResult> GetUserCompletions(int id, int page = 1, int pageSize = 20)
        {
            try
            {
                var completions = await _context.Completions
                    .Where(c => c.UserId == id)
                    .OrderByDescending(c => c.CompletedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(c => new {
                        c.CompletionId,
                        c.CompletedAt,
                        c.PointsAwarded,
                        c.UsdValue,
                        c.Status,
                        c.TransactionId,
                        c.AffiliateNetwork,
                        TaskName = c.Task != null ? c.Task.Title : null
                    })
                    .ToListAsync();
                
                var totalCount = await _context.Completions.CountAsync(c => c.UserId == id);
                
                return Ok(new {
                    completions,
                    totalCount,
                    page,
                    pageSize,
                    totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting completions for user {id}");
                return StatusCode(500, "Error retrieving completions");
            }
        }
        
        [HttpGet("users/{id}/referrals")]
        public async Task<IActionResult> GetUserReferrals(int id)
        {
            try
            {
                var referrals = await _context.Users
                    .Where(u => u.ReferrerId == id)
                    .Select(u => new {
                        u.UserId,
                        u.Username,
                        u.Email,
                        u.CreatedAt,
                        u.PointsBalance,
                        u.AccountStatus,
                        EarningsGenerated = _context.ReferralCommissions
                            .Where(rc => rc.Referral.ReferredId == u.UserId)
                            .Sum(rc => rc.Amount)
                    })
                    .ToListAsync();
                
                return Ok(referrals);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting referrals for user {id}");
                return StatusCode(500, "Error retrieving referrals");
            }
        }
        
        #endregion
        
        #region Completions
        
        [HttpGet("completions")]
        public async Task<IActionResult> GetCompletions(int page = 1, int pageSize = 20)
        {
            try
            {
                var completions = await _context.Completions
                    .OrderByDescending(c => c.CompletedAt)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(c => new {
                        c.CompletionId,
                        c.UserId,
                        UserName = c.User.Username,
                        c.CompletedAt,
                        c.PointsAwarded,
                        c.UsdValue,
                        c.Status,
                        c.TransactionId,
                        c.AffiliateNetwork,
                        TaskName = c.Task != null ? c.Task.Title : null
                    })
                    .ToListAsync();
                
                var totalCount = await _context.Completions.CountAsync();
                
                return Ok(new {
                    completions,
                    totalCount,
                    page,
                    pageSize,
                    totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting completions");
                return StatusCode(500, "Error retrieving completions");
            }
        }
        
        #endregion
        
        #region Rewards
        
        [HttpGet("rewards")]
        public async Task<IActionResult> GetRewards()
        {
            try
            {
                var rewards = await _context.Rewards
                    .OrderBy(r => r.PointsCost)
                    .Select(r => new {
                        r.RewardId,
                        r.Title,
                        r.Description,
                        r.PointsCost,
                        r.StockQuantity,
                        r.Status,
                        r.Category,
                        r.Featured,
                        RedemptionCount = _context.Redemptions
                            .Count(red => red.RewardId == r.RewardId)
                    })
                    .ToListAsync();
                
                return Ok(rewards);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting rewards");
                return StatusCode(500, "Error retrieving rewards");
            }
        }
        
        #endregion
        
        #region Redemptions
        
        [HttpGet("redemptions")]
        public async Task<IActionResult> GetRedemptions(int page = 1, int pageSize = 20)
        {
            try
            {
                var redemptions = await _context.Redemptions
                    .OrderByDescending(r => r.RedemptionDate)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(r => new {
                        r.RedemptionId,
                        r.UserId,
                        UserName = r.User.Username,
                        r.RewardId,
                        RewardTitle = r.Reward.Title,
                        r.RedemptionDate,
                        r.PointsSpent,
                        r.Status
                    })
                    .ToListAsync();
                
                var totalCount = await _context.Redemptions.CountAsync();
                
                return Ok(new {
                    redemptions,
                    totalCount,
                    page,
                    pageSize,
                    totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting redemptions");
                return StatusCode(500, "Error retrieving redemptions");
            }
        }
        
        #endregion
        
        #region Statistics
        
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            try
            {
                var userCount = await _context.Users.CountAsync();
                var totalPoints = await _context.Users.SumAsync(u => u.PointsBalance);
                var completionCount = await _context.Completions.CountAsync();
                var redeemedPoints = await _context.Redemptions.SumAsync(r => r.PointsSpent);
                var pendingRedemptions = await _context.Redemptions.CountAsync(r => r.Status == "pending");
                
                return Ok(new {
                    userCount,
                    totalPoints,
                    completionCount,
                    redeemedPoints,
                    pendingRedemptions,
                    referralCount = await _context.Referrals.CountAsync(),
                    lastDayCompletions = await _context.Completions
                        .CountAsync(c => c.CompletedAt >= DateTime.UtcNow.AddDays(-1)),
                    lastDayUsers = await _context.Users
                        .CountAsync(u => u.CreatedAt >= DateTime.UtcNow.AddDays(-1))
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting statistics");
                return StatusCode(500, "Error retrieving statistics");
            }
        }
        
        #endregion
    }
}