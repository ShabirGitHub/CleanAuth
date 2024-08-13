using Microsoft.AspNetCore.Mvc;
using CleanAuth.UseCases.DTO;
using CleanAuth.UseCases.Interfaces;
using UAParser;
using CleanAuth.CoreBusiness.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace CleanAuth.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IAuthenticateUseCase _authenticateUserCase;
        private readonly ISignupUseCase _signupUseCase;
        private readonly IBalanceUseCase _balanceUseCase;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IAuthenticateUseCase authenticateUserCase, ISignupUseCase signupUseCase, IBalanceUseCase balanceUseCase, ILogger<UsersController> logger)
        {
            _authenticateUserCase = authenticateUserCase;
            _signupUseCase = signupUseCase;
            _balanceUseCase = balanceUseCase;
            _logger = logger;
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(UserSignupRequest request)
        {
            try
            {
                var (remoteIpAddress, browser, device) = GetClientInfo();
                request.IpAddress = remoteIpAddress;
                request.Device = device;

                await _signupUseCase.ExecuteAsync(request);
                return Ok();
            }
            catch (DomainException ex)
            {
                _logger.LogWarning("Domain exception occurred during SignUp for user {Username}: {Message}", request.Username, ex.Message);
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during SignUp for user {Username}", request.Username);
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] UserLoginRequest request)
        {
            try
            {
                var (remoteIpAddress, browser, device) = GetClientInfo();

                request.IpAddress = remoteIpAddress;
                request.Device = device;
                request.Browser = browser;

                var response = await _authenticateUserCase.ExecuteAsync(request);
                return Ok(response);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Validation error occurred during authentication for user {Username}: {Message}", request.Username, ex.Message);
                return BadRequest(new { Message = ex.Message, Details = ex.InnerException?.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Unauthorized access during authentication for user {Username}: {Message}", request.Username, ex.Message);
                return Unauthorized(new { Message = ex.Message});
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "Application error during authentication for user {Username}", request.Username);
                return StatusCode(500, new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during authentication for user {Username}", request.Username);
                return StatusCode(500, new { Message = "An unexpected error occurred. Please try again later.", Details = ex.Message });
            }
        }

        [HttpPost("auth/balance")]
        public async Task<IActionResult> GetBalance([FromBody] BalanceRequest balanceRequest)
        {
            try
            {
                var balanceResponse = await _balanceUseCase.ExecuteAsync(balanceRequest.Token);
                return Ok(balanceResponse);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning("Unauthorized access while retrieving balance {Message}", ex.Message);
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving balance");
                return StatusCode(500, new { Message = "An unexpected error occurred. Please try again later.", Details = ex.Message });
            }
        }

        [HttpGet("default")]
        public IActionResult GetDefault()
        {
            // Create a response message
            var message = "Welcome! This application implements the following endpoints:\n" +
                          "- SignUp\n" +
                          "- Authenticate\n" +
                          "- auth/balance";

            // Return the message as plain text
            return Content(message, "text/plain");
        }

        public (string remoteIpAddress, string browser, string device) GetClientInfo()
        {
            // Get the remote IP address
            var remoteIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            // Get the User-Agent string
            var userAgentString = HttpContext.Request.Headers["User-Agent"].ToString();

            // Parse the User-Agent string using UAParser
            var uaParser = Parser.GetDefault();
            ClientInfo clientInfo = uaParser.Parse(userAgentString);

            // Extract Operating System, Browser, and Device information
            var browser = clientInfo.UA.ToString();
            var device = clientInfo.Device.ToString();

            // Return a tuple with all the information
            return (remoteIpAddress, browser, device);
        }
    }
}
