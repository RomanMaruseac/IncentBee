using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

namespace IncentBee.API.Controllers
{
    [ApiController]
    [Route("api/callbacks")]
    public class CallbacksController : ControllerBase
    {
        private readonly ILogger<CallbacksController> _logger;
        private readonly string _secretKey = "QSnG5W3X2vxX6Orm432zkrFxc7F2p76s"; // BitLabs Secret Key
        private static readonly List<CallbackDetails> _callbackHistory = new List<CallbackDetails>();
        
        public CallbacksController(ILogger<CallbacksController> logger)
        {
            _logger = logger;
        }
        
        [HttpGet("bitlabs")]
        public IActionResult HandleBitLabsCallback(
            [FromQuery] string user_id,
            [FromQuery] decimal reward_amount,
            [FromQuery] decimal usd_value,
            [FromQuery] string transaction_id,
            [FromQuery] string hash)
        {
            try
            {
                // Create callback details
                var callbackDetails = new CallbackDetails
                {
                    Timestamp = DateTime.UtcNow,
                    Network = "BitLabs",
                    UserId = user_id,
                    RewardAmount = reward_amount,
                    UsdValue = usd_value,
                    TransactionId = transaction_id,
                    Hash = hash,
                    IsValid = VerifyHash(user_id, reward_amount, transaction_id, hash)
                };
                
                // Add to in-memory history
                _callbackHistory.Add(callbackDetails);
                
                // Keep only the last 100 callbacks
                if (_callbackHistory.Count > 100)
                {
                    _callbackHistory.RemoveAt(0);
                }
                
                // Log all callback parameters
                _logger.LogInformation($"BitLabs callback received: {System.Text.Json.JsonSerializer.Serialize(callbackDetails)}");
                
                // Return the callback information with status
                if (callbackDetails.IsValid)
                {
                    return Ok(callbackDetails);
                }
                else
                {
                    return BadRequest(callbackDetails);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing BitLabs callback");
                return StatusCode(500, new { Error = "Error processing callback", Message = ex.Message });
            }
        }
        
        [HttpGet("view")]
        public IActionResult ViewCallbacks()
        {
            return Ok(_callbackHistory);
        }
        
        private bool VerifyHash(string userId, decimal rewardAmount, string transactionId, string receivedHash)
        {
            // BitLabs hashing algorithm - adjust according to their documentation
            string dataToHash = $"{userId}{rewardAmount}{transactionId}{_secretKey}";
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(dataToHash));
                string computedHash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                
                return computedHash == receivedHash.ToLower();
            }
        }
    }
    
    public class CallbackDetails
    {
        public DateTime Timestamp { get; set; }
        public string Network { get; set; }
        public string UserId { get; set; }
        public decimal RewardAmount { get; set; }
        public decimal UsdValue { get; set; }
        public string TransactionId { get; set; }
        public string Hash { get; set; }
        public bool IsValid { get; set; }
    }
}