using System.Text.Json.Serialization;

namespace SocialNetwork.Server.DTOs;

public sealed record ErrorResponse
(
    [property: JsonPropertyName("success")]
    bool Success,

    [property: JsonPropertyName("status")]
    int Status, 
    
    [property: JsonPropertyName("message")]
    string Message,

    [property: JsonPropertyName("code")]
    string? Code
);