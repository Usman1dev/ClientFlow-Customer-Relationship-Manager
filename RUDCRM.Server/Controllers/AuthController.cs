using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
<<<<<<< HEAD
using RUDCRM.Server.DTOs;
using RUDCRM.Server.Models;
=======
using RUDCRM.Shared.DTOs;
using RUDCRM.Shared.Models;
>>>>>>> f1f16b05775f1962e046e7a92be03b9421eef765
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RUDCRM.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<ActionResult<LoginResponseDto>> Register([FromBody] RegisterDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new LoginResponseDto { Success = false, Message = "Invalid registration data." });

        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
            return BadRequest(new LoginResponseDto { Success = false, Message = "An account with this email already exists." });

        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            FullName = dto.FullName,
            Role = "Employee",
            EmailConfirmed = true,
            IsActive = true
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            return BadRequest(new LoginResponseDto { Success = false, Message = errors });
        }

        await _userManager.AddToRoleAsync(user, "Employee");
        var token = GenerateJwtToken(user);

        return Ok(new LoginResponseDto
        {
            Success = true,
            Token = token,
            Message = "Registration successful!",
            User = MapToUserDto(user)
        });
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new LoginResponseDto { Success = false, Message = "Invalid login data." });

        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null || !user.IsActive)
            return Unauthorized(new LoginResponseDto { Success = false, Message = "Invalid email or password." });

        var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
        if (!result.Succeeded)
            return Unauthorized(new LoginResponseDto { Success = false, Message = "Invalid email or password." });

        var token = GenerateJwtToken(user);

        return Ok(new LoginResponseDto
        {
            Success = true,
            Token = token,
            Message = "Login successful!",
            User = MapToUserDto(user)
        });
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<ActionResult<UserDto>> GetProfile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return NotFound();

        return Ok(MapToUserDto(user));
    }

    [Authorize]
    [HttpPut("profile")]
    public async Task<ActionResult<ApiResponse<UserDto>>> UpdateProfile([FromBody] UpdateProfileDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId!);
        if (user == null) return NotFound();

        user.FullName = dto.FullName;
        user.ProfileImageUrl = dto.ProfileImageUrl;

        await _userManager.UpdateAsync(user);

        return Ok(new ApiResponse<UserDto>
        {
            Success = true,
            Message = "Profile updated successfully.",
            Data = MapToUserDto(user)
        });
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<ActionResult<ApiResponse<string>>> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId!);
        if (user == null) return NotFound();

        var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            return BadRequest(new ApiResponse<string> { Success = false, Message = errors });
        }

        return Ok(new ApiResponse<string> { Success = true, Message = "Password changed successfully." });
    }

    private string GenerateJwtToken(ApplicationUser user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.Role, user.Role),
            new("sub", user.Id)
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(int.Parse(jwtSettings["ExpiryInDays"]!)),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static UserDto MapToUserDto(ApplicationUser user)
    {
        return new UserDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email!,
            Role = user.Role,
            ProfileImageUrl = user.ProfileImageUrl,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt
        };
    }
}
