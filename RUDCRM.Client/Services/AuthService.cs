using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using RUDCRM.Client.Auth;
using RUDCRM.Shared.DTOs;

namespace RUDCRM.Client.Services;

public class AuthService
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationStateProvider _authStateProvider;

    public AuthService(HttpClient http, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider)
    {
        _http = http;
        _localStorage = localStorage;
        _authStateProvider = authStateProvider;
    }

    public async Task<LoginResponseDto> RegisterAsync(RegisterDto dto)
    {
        var response = await _http.PostAsJsonAsync("api/auth/register", dto);
        var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
        if (result != null && result.Success)
        {
            await _localStorage.SetItemAsStringAsync("authToken", result.Token);
            _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result.Token);
            ((CustomAuthStateProvider)_authStateProvider).NotifyAuthenticationStateChanged();
        }
        return result ?? new LoginResponseDto { Success = false, Message = "Registration failed." };
    }

    public async Task<LoginResponseDto> LoginAsync(LoginDto dto)
    {
        var response = await _http.PostAsJsonAsync("api/auth/login", dto);
        var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
        if (result != null && result.Success)
        {
            await _localStorage.SetItemAsStringAsync("authToken", result.Token);
            _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result.Token);
            ((CustomAuthStateProvider)_authStateProvider).NotifyAuthenticationStateChanged();
        }
        return result ?? new LoginResponseDto { Success = false, Message = "Login failed." };
    }

    public async Task LogoutAsync()
    {
        await ((CustomAuthStateProvider)_authStateProvider).LogoutAsync();
    }

    public async Task<UserDto?> GetProfileAsync()
    {
        try { return await _http.GetFromJsonAsync<UserDto>("api/auth/profile"); }
        catch { return null; }
    }

    public async Task<bool> UpdateProfileAsync(UpdateProfileDto dto)
    {
        var response = await _http.PutAsJsonAsync("api/auth/profile", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<ApiResponse<string>> ChangePasswordAsync(ChangePasswordDto dto)
    {
        var response = await _http.PostAsJsonAsync("api/auth/change-password", dto);
        return await response.Content.ReadFromJsonAsync<ApiResponse<string>>() ?? new ApiResponse<string> { Success = false, Message = "Failed." };
    }
}
