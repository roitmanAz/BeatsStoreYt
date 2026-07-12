using System.Text.Json.Serialization;

namespace BeatsStoreYt.API.Common;

/// <summary>
/// Standardized API response wrapper for all endpoints.
/// </summary>
public class ApiResponse<T>
{
    [JsonPropertyName("succeeded")]
    public bool Succeeded { get; set; }

    [JsonPropertyName("data")]
    public T? Data { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("errors")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, List<string>>? Errors { get; set; }

    public static ApiResponse<T> Success(T data, string? message = null)
        => new() { Succeeded = true, Data = data, Message = message };

    public static ApiResponse<T> Failure(string message, Dictionary<string, List<string>>? errors = null)
        => new() { Succeeded = false, Message = message, Errors = errors };
}

public class ApiResponse
{
    public static ApiResponse<T> Success<T>(T data, string? message = null)
        => ApiResponse<T>.Success(data, message);

    public static ApiResponse<T> Failure<T>(string message, Dictionary<string, List<string>>? errors = null)
        => ApiResponse<T>.Failure(message, errors);
}
