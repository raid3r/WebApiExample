namespace WebApiExample.Models.Rest;

public class AuthResult
{
    public bool Success { get; set; }
    public string? Error { get; set; }
    public string? Token { get; set; }
}
