# Code Citations

## License: unknown

https://github.com/hdduong/Qed.Webhook.Api/blob/5fed0deb6cab3741a1b3b59ff0e2d4b49e66eb25/Qed.Webhook.RedisCache.Api/Middlewares/GlobalExceptionMiddleware.cs

````
אני לא יכול לעדכן קבצים ישירות כעת, אבל אדפיס לך בדיוק מה לבנות בכל קובץ. תעתיק ויצור:

**1. `src/BeatsStoreYt.API/Common/ApiResponse.cs`**
```csharp
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
````

**2. `src/BeatsStoreYt.API/Common/PagedResult.cs`**

```csharp
using System.Text.Json.Serialization;

namespace BeatsStoreYt.API.Common;

public class PagedResult<T>
{
    [JsonPropertyName("data")]
    public List<T> Data { get; set; } = new();

    [JsonPropertyName("pagination")]
    public PaginationMetadata Pagination { get; set; } = new();

    public static PagedResult<T> Create(List<T> items, int currentPage, int pageSize, int totalItems)
    {
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        return new PagedResult<T>
        {
            Data = items,
            Pagination = new PaginationMetadata
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            }
        };
    }
}

public class PaginationMetadata
{
    [JsonPropertyName("currentPage")]
    public int CurrentPage { get; set; }

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    [JsonPropertyName("totalItems")]
    public int TotalItems { get; set; }

    [JsonPropertyName("totalPages")]
    public int TotalPages { get; set; }
}
```

**3. `src/BeatsStoreYt.API/Common/PaginationParams.cs`**

```csharp
namespace BeatsStoreYt.API.Common;

public class PaginationParams
{
    private const int MaxPageSize = 100;
    private int _pageSize = 20;

    public int Page { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    public void Validate()
    {
        if (Page < 1)
            throw new ArgumentException("Page must be >= 1");
        if (PageSize < 1)
            throw new ArgumentException("PageSize must be >= 1");
    }
}
```

**4. `src/BeatsStoreYt.API/Middleware/GlobalExceptionMiddleware.cs`**

```csharp
using System.Net;
using System.Text.Json;
using BeatsStoreYt.API.Common;

namespace BeatsStoreYt.API.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWar
```

## License: unknown

https://github.com/hdduong/Qed.Webhook.Api/blob/5fed0deb6cab3741a1b3b59ff0e2d4b49e66eb25/Qed.Webhook.RedisCache.Api/Middlewares/GlobalExceptionMiddleware.cs

````
אני לא יכול לעדכן קבצים ישירות כעת, אבל אדפיס לך בדיוק מה לבנות בכל קובץ. תעתיק ויצור:

**1. `src/BeatsStoreYt.API/Common/ApiResponse.cs`**
```csharp
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
````

**2. `src/BeatsStoreYt.API/Common/PagedResult.cs`**

```csharp
using System.Text.Json.Serialization;

namespace BeatsStoreYt.API.Common;

public class PagedResult<T>
{
    [JsonPropertyName("data")]
    public List<T> Data { get; set; } = new();

    [JsonPropertyName("pagination")]
    public PaginationMetadata Pagination { get; set; } = new();

    public static PagedResult<T> Create(List<T> items, int currentPage, int pageSize, int totalItems)
    {
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        return new PagedResult<T>
        {
            Data = items,
            Pagination = new PaginationMetadata
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            }
        };
    }
}

public class PaginationMetadata
{
    [JsonPropertyName("currentPage")]
    public int CurrentPage { get; set; }

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    [JsonPropertyName("totalItems")]
    public int TotalItems { get; set; }

    [JsonPropertyName("totalPages")]
    public int TotalPages { get; set; }
}
```

**3. `src/BeatsStoreYt.API/Common/PaginationParams.cs`**

```csharp
namespace BeatsStoreYt.API.Common;

public class PaginationParams
{
    private const int MaxPageSize = 100;
    private int _pageSize = 20;

    public int Page { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    public void Validate()
    {
        if (Page < 1)
            throw new ArgumentException("Page must be >= 1");
        if (PageSize < 1)
            throw new ArgumentException("PageSize must be >= 1");
    }
}
```

**4. `src/BeatsStoreYt.API/Middleware/GlobalExceptionMiddleware.cs`**

```csharp
using System.Net;
using System.Text.Json;
using BeatsStoreYt.API.Common;

namespace BeatsStoreYt.API.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWar
```

## License: unknown

https://github.com/hdduong/Qed.Webhook.Api/blob/5fed0deb6cab3741a1b3b59ff0e2d4b49e66eb25/Qed.Webhook.RedisCache.Api/Middlewares/GlobalExceptionMiddleware.cs

````
אני לא יכול לעדכן קבצים ישירות כעת, אבל אדפיס לך בדיוק מה לבנות בכל קובץ. תעתיק ויצור:

**1. `src/BeatsStoreYt.API/Common/ApiResponse.cs`**
```csharp
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
````

**2. `src/BeatsStoreYt.API/Common/PagedResult.cs`**

```csharp
using System.Text.Json.Serialization;

namespace BeatsStoreYt.API.Common;

public class PagedResult<T>
{
    [JsonPropertyName("data")]
    public List<T> Data { get; set; } = new();

    [JsonPropertyName("pagination")]
    public PaginationMetadata Pagination { get; set; } = new();

    public static PagedResult<T> Create(List<T> items, int currentPage, int pageSize, int totalItems)
    {
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        return new PagedResult<T>
        {
            Data = items,
            Pagination = new PaginationMetadata
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            }
        };
    }
}

public class PaginationMetadata
{
    [JsonPropertyName("currentPage")]
    public int CurrentPage { get; set; }

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    [JsonPropertyName("totalItems")]
    public int TotalItems { get; set; }

    [JsonPropertyName("totalPages")]
    public int TotalPages { get; set; }
}
```

**3. `src/BeatsStoreYt.API/Common/PaginationParams.cs`**

```csharp
namespace BeatsStoreYt.API.Common;

public class PaginationParams
{
    private const int MaxPageSize = 100;
    private int _pageSize = 20;

    public int Page { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    public void Validate()
    {
        if (Page < 1)
            throw new ArgumentException("Page must be >= 1");
        if (PageSize < 1)
            throw new ArgumentException("PageSize must be >= 1");
    }
}
```

**4. `src/BeatsStoreYt.API/Middleware/GlobalExceptionMiddleware.cs`**

```csharp
using System.Net;
using System.Text.Json;
using BeatsStoreYt.API.Common;

namespace BeatsStoreYt.API.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWar
```

## License: unknown

https://github.com/hdduong/Qed.Webhook.Api/blob/5fed0deb6cab3741a1b3b59ff0e2d4b49e66eb25/Qed.Webhook.RedisCache.Api/Middlewares/GlobalExceptionMiddleware.cs

````
אני לא יכול לעדכן קבצים ישירות כעת, אבל אדפיס לך בדיוק מה לבנות בכל קובץ. תעתיק ויצור:

**1. `src/BeatsStoreYt.API/Common/ApiResponse.cs`**
```csharp
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
````

**2. `src/BeatsStoreYt.API/Common/PagedResult.cs`**

```csharp
using System.Text.Json.Serialization;

namespace BeatsStoreYt.API.Common;

public class PagedResult<T>
{
    [JsonPropertyName("data")]
    public List<T> Data { get; set; } = new();

    [JsonPropertyName("pagination")]
    public PaginationMetadata Pagination { get; set; } = new();

    public static PagedResult<T> Create(List<T> items, int currentPage, int pageSize, int totalItems)
    {
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        return new PagedResult<T>
        {
            Data = items,
            Pagination = new PaginationMetadata
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            }
        };
    }
}

public class PaginationMetadata
{
    [JsonPropertyName("currentPage")]
    public int CurrentPage { get; set; }

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    [JsonPropertyName("totalItems")]
    public int TotalItems { get; set; }

    [JsonPropertyName("totalPages")]
    public int TotalPages { get; set; }
}
```

**3. `src/BeatsStoreYt.API/Common/PaginationParams.cs`**

```csharp
namespace BeatsStoreYt.API.Common;

public class PaginationParams
{
    private const int MaxPageSize = 100;
    private int _pageSize = 20;

    public int Page { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    public void Validate()
    {
        if (Page < 1)
            throw new ArgumentException("Page must be >= 1");
        if (PageSize < 1)
            throw new ArgumentException("PageSize must be >= 1");
    }
}
```

**4. `src/BeatsStoreYt.API/Middleware/GlobalExceptionMiddleware.cs`**

```csharp
using System.Net;
using System.Text.Json;
using BeatsStoreYt.API.Common;

namespace BeatsStoreYt.API.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWar
```

## License: unknown

https://github.com/hdduong/Qed.Webhook.Api/blob/5fed0deb6cab3741a1b3b59ff0e2d4b49e66eb25/Qed.Webhook.RedisCache.Api/Middlewares/GlobalExceptionMiddleware.cs

````
אני לא יכול לעדכן קבצים ישירות כעת, אבל אדפיס לך בדיוק מה לבנות בכל קובץ. תעתיק ויצור:

**1. `src/BeatsStoreYt.API/Common/ApiResponse.cs`**
```csharp
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
````

**2. `src/BeatsStoreYt.API/Common/PagedResult.cs`**

```csharp
using System.Text.Json.Serialization;

namespace BeatsStoreYt.API.Common;

public class PagedResult<T>
{
    [JsonPropertyName("data")]
    public List<T> Data { get; set; } = new();

    [JsonPropertyName("pagination")]
    public PaginationMetadata Pagination { get; set; } = new();

    public static PagedResult<T> Create(List<T> items, int currentPage, int pageSize, int totalItems)
    {
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        return new PagedResult<T>
        {
            Data = items,
            Pagination = new PaginationMetadata
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            }
        };
    }
}

public class PaginationMetadata
{
    [JsonPropertyName("currentPage")]
    public int CurrentPage { get; set; }

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    [JsonPropertyName("totalItems")]
    public int TotalItems { get; set; }

    [JsonPropertyName("totalPages")]
    public int TotalPages { get; set; }
}
```

**3. `src/BeatsStoreYt.API/Common/PaginationParams.cs`**

```csharp
namespace BeatsStoreYt.API.Common;

public class PaginationParams
{
    private const int MaxPageSize = 100;
    private int _pageSize = 20;

    public int Page { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    public void Validate()
    {
        if (Page < 1)
            throw new ArgumentException("Page must be >= 1");
        if (PageSize < 1)
            throw new ArgumentException("PageSize must be >= 1");
    }
}
```

**4. `src/BeatsStoreYt.API/Middleware/GlobalExceptionMiddleware.cs`**

```csharp
using System.Net;
using System.Text.Json;
using BeatsStoreYt.API.Common;

namespace BeatsStoreYt.API.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWar
```

## License: unknown

https://github.com/hdduong/Qed.Webhook.Api/blob/5fed0deb6cab3741a1b3b59ff0e2d4b49e66eb25/Qed.Webhook.RedisCache.Api/Middlewares/GlobalExceptionMiddleware.cs

````
אני לא יכול לעדכן קבצים ישירות כעת, אבל אדפיס לך בדיוק מה לבנות בכל קובץ. תעתיק ויצור:

**1. `src/BeatsStoreYt.API/Common/ApiResponse.cs`**
```csharp
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
````

**2. `src/BeatsStoreYt.API/Common/PagedResult.cs`**

```csharp
using System.Text.Json.Serialization;

namespace BeatsStoreYt.API.Common;

public class PagedResult<T>
{
    [JsonPropertyName("data")]
    public List<T> Data { get; set; } = new();

    [JsonPropertyName("pagination")]
    public PaginationMetadata Pagination { get; set; } = new();

    public static PagedResult<T> Create(List<T> items, int currentPage, int pageSize, int totalItems)
    {
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        return new PagedResult<T>
        {
            Data = items,
            Pagination = new PaginationMetadata
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            }
        };
    }
}

public class PaginationMetadata
{
    [JsonPropertyName("currentPage")]
    public int CurrentPage { get; set; }

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    [JsonPropertyName("totalItems")]
    public int TotalItems { get; set; }

    [JsonPropertyName("totalPages")]
    public int TotalPages { get; set; }
}
```

**3. `src/BeatsStoreYt.API/Common/PaginationParams.cs`**

```csharp
namespace BeatsStoreYt.API.Common;

public class PaginationParams
{
    private const int MaxPageSize = 100;
    private int _pageSize = 20;

    public int Page { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    public void Validate()
    {
        if (Page < 1)
            throw new ArgumentException("Page must be >= 1");
        if (PageSize < 1)
            throw new ArgumentException("PageSize must be >= 1");
    }
}
```

**4. `src/BeatsStoreYt.API/Middleware/GlobalExceptionMiddleware.cs`**

```csharp
using System.Net;
using System.Text.Json;
using BeatsStoreYt.API.Common;

namespace BeatsStoreYt.API.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWar
```

## License: unknown

https://github.com/hdduong/Qed.Webhook.Api/blob/5fed0deb6cab3741a1b3b59ff0e2d4b49e66eb25/Qed.Webhook.RedisCache.Api/Middlewares/GlobalExceptionMiddleware.cs

````
אני לא יכול לעדכן קבצים ישירות כעת, אבל אדפיס לך בדיוק מה לבנות בכל קובץ. תעתיק ויצור:

**1. `src/BeatsStoreYt.API/Common/ApiResponse.cs`**
```csharp
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
````

**2. `src/BeatsStoreYt.API/Common/PagedResult.cs`**

```csharp
using System.Text.Json.Serialization;

namespace BeatsStoreYt.API.Common;

public class PagedResult<T>
{
    [JsonPropertyName("data")]
    public List<T> Data { get; set; } = new();

    [JsonPropertyName("pagination")]
    public PaginationMetadata Pagination { get; set; } = new();

    public static PagedResult<T> Create(List<T> items, int currentPage, int pageSize, int totalItems)
    {
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        return new PagedResult<T>
        {
            Data = items,
            Pagination = new PaginationMetadata
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            }
        };
    }
}

public class PaginationMetadata
{
    [JsonPropertyName("currentPage")]
    public int CurrentPage { get; set; }

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    [JsonPropertyName("totalItems")]
    public int TotalItems { get; set; }

    [JsonPropertyName("totalPages")]
    public int TotalPages { get; set; }
}
```

**3. `src/BeatsStoreYt.API/Common/PaginationParams.cs`**

```csharp
namespace BeatsStoreYt.API.Common;

public class PaginationParams
{
    private const int MaxPageSize = 100;
    private int _pageSize = 20;

    public int Page { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    public void Validate()
    {
        if (Page < 1)
            throw new ArgumentException("Page must be >= 1");
        if (PageSize < 1)
            throw new ArgumentException("PageSize must be >= 1");
    }
}
```

**4. `src/BeatsStoreYt.API/Middleware/GlobalExceptionMiddleware.cs`**

```csharp
using System.Net;
using System.Text.Json;
using BeatsStoreYt.API.Common;

namespace BeatsStoreYt.API.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWar
```

## License: unknown

https://github.com/hdduong/Qed.Webhook.Api/blob/5fed0deb6cab3741a1b3b59ff0e2d4b49e66eb25/Qed.Webhook.RedisCache.Api/Middlewares/GlobalExceptionMiddleware.cs

````
אני לא יכול לעדכן קבצים ישירות כעת, אבל אדפיס לך בדיוק מה לבנות בכל קובץ. תעתיק ויצור:

**1. `src/BeatsStoreYt.API/Common/ApiResponse.cs`**
```csharp
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
````

**2. `src/BeatsStoreYt.API/Common/PagedResult.cs`**

```csharp
using System.Text.Json.Serialization;

namespace BeatsStoreYt.API.Common;

public class PagedResult<T>
{
    [JsonPropertyName("data")]
    public List<T> Data { get; set; } = new();

    [JsonPropertyName("pagination")]
    public PaginationMetadata Pagination { get; set; } = new();

    public static PagedResult<T> Create(List<T> items, int currentPage, int pageSize, int totalItems)
    {
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        return new PagedResult<T>
        {
            Data = items,
            Pagination = new PaginationMetadata
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            }
        };
    }
}

public class PaginationMetadata
{
    [JsonPropertyName("currentPage")]
    public int CurrentPage { get; set; }

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    [JsonPropertyName("totalItems")]
    public int TotalItems { get; set; }

    [JsonPropertyName("totalPages")]
    public int TotalPages { get; set; }
}
```

**3. `src/BeatsStoreYt.API/Common/PaginationParams.cs`**

```csharp
namespace BeatsStoreYt.API.Common;

public class PaginationParams
{
    private const int MaxPageSize = 100;
    private int _pageSize = 20;

    public int Page { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    public void Validate()
    {
        if (Page < 1)
            throw new ArgumentException("Page must be >= 1");
        if (PageSize < 1)
            throw new ArgumentException("PageSize must be >= 1");
    }
}
```

**4. `src/BeatsStoreYt.API/Middleware/GlobalExceptionMiddleware.cs`**

```csharp
using System.Net;
using System.Text.Json;
using BeatsStoreYt.API.Common;

namespace BeatsStoreYt.API.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWar
```

## License: unknown

https://github.com/hdduong/Qed.Webhook.Api/blob/5fed0deb6cab3741a1b3b59ff0e2d4b49e66eb25/Qed.Webhook.RedisCache.Api/Middlewares/GlobalExceptionMiddleware.cs

````
אני לא יכול לעדכן קבצים ישירות כעת, אבל אדפיס לך בדיוק מה לבנות בכל קובץ. תעתיק ויצור:

**1. `src/BeatsStoreYt.API/Common/ApiResponse.cs`**
```csharp
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
````

**2. `src/BeatsStoreYt.API/Common/PagedResult.cs`**

```csharp
using System.Text.Json.Serialization;

namespace BeatsStoreYt.API.Common;

public class PagedResult<T>
{
    [JsonPropertyName("data")]
    public List<T> Data { get; set; } = new();

    [JsonPropertyName("pagination")]
    public PaginationMetadata Pagination { get; set; } = new();

    public static PagedResult<T> Create(List<T> items, int currentPage, int pageSize, int totalItems)
    {
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        return new PagedResult<T>
        {
            Data = items,
            Pagination = new PaginationMetadata
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            }
        };
    }
}

public class PaginationMetadata
{
    [JsonPropertyName("currentPage")]
    public int CurrentPage { get; set; }

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    [JsonPropertyName("totalItems")]
    public int TotalItems { get; set; }

    [JsonPropertyName("totalPages")]
    public int TotalPages { get; set; }
}
```

**3. `src/BeatsStoreYt.API/Common/PaginationParams.cs`**

```csharp
namespace BeatsStoreYt.API.Common;

public class PaginationParams
{
    private const int MaxPageSize = 100;
    private int _pageSize = 20;

    public int Page { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    public void Validate()
    {
        if (Page < 1)
            throw new ArgumentException("Page must be >= 1");
        if (PageSize < 1)
            throw new ArgumentException("PageSize must be >= 1");
    }
}
```

**4. `src/BeatsStoreYt.API/Middleware/GlobalExceptionMiddleware.cs`**

```csharp
using System.Net;
using System.Text.Json;
using BeatsStoreYt.API.Common;

namespace BeatsStoreYt.API.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWar
```

## License: unknown

https://github.com/hdduong/Qed.Webhook.Api/blob/5fed0deb6cab3741a1b3b59ff0e2d4b49e66eb25/Qed.Webhook.RedisCache.Api/Middlewares/GlobalExceptionMiddleware.cs

````
אני לא יכול לעדכן קבצים ישירות כעת, אבל אדפיס לך בדיוק מה לבנות בכל קובץ. תעתיק ויצור:

**1. `src/BeatsStoreYt.API/Common/ApiResponse.cs`**
```csharp
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
````

**2. `src/BeatsStoreYt.API/Common/PagedResult.cs`**

```csharp
using System.Text.Json.Serialization;

namespace BeatsStoreYt.API.Common;

public class PagedResult<T>
{
    [JsonPropertyName("data")]
    public List<T> Data { get; set; } = new();

    [JsonPropertyName("pagination")]
    public PaginationMetadata Pagination { get; set; } = new();

    public static PagedResult<T> Create(List<T> items, int currentPage, int pageSize, int totalItems)
    {
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        return new PagedResult<T>
        {
            Data = items,
            Pagination = new PaginationMetadata
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            }
        };
    }
}

public class PaginationMetadata
{
    [JsonPropertyName("currentPage")]
    public int CurrentPage { get; set; }

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    [JsonPropertyName("totalItems")]
    public int TotalItems { get; set; }

    [JsonPropertyName("totalPages")]
    public int TotalPages { get; set; }
}
```

**3. `src/BeatsStoreYt.API/Common/PaginationParams.cs`**

```csharp
namespace BeatsStoreYt.API.Common;

public class PaginationParams
{
    private const int MaxPageSize = 100;
    private int _pageSize = 20;

    public int Page { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    public void Validate()
    {
        if (Page < 1)
            throw new ArgumentException("Page must be >= 1");
        if (PageSize < 1)
            throw new ArgumentException("PageSize must be >= 1");
    }
}
```

**4. `src/BeatsStoreYt.API/Middleware/GlobalExceptionMiddleware.cs`**

```csharp
using System.Net;
using System.Text.Json;
using BeatsStoreYt.API.Common;

namespace BeatsStoreYt.API.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWar
```

## License: unknown

https://github.com/hdduong/Qed.Webhook.Api/blob/5fed0deb6cab3741a1b3b59ff0e2d4b49e66eb25/Qed.Webhook.RedisCache.Api/Middlewares/GlobalExceptionMiddleware.cs

````
אני לא יכול לעדכן קבצים ישירות כעת, אבל אדפיס לך בדיוק מה לבנות בכל קובץ. תעתיק ויצור:

**1. `src/BeatsStoreYt.API/Common/ApiResponse.cs`**
```csharp
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
````

**2. `src/BeatsStoreYt.API/Common/PagedResult.cs`**

```csharp
using System.Text.Json.Serialization;

namespace BeatsStoreYt.API.Common;

public class PagedResult<T>
{
    [JsonPropertyName("data")]
    public List<T> Data { get; set; } = new();

    [JsonPropertyName("pagination")]
    public PaginationMetadata Pagination { get; set; } = new();

    public static PagedResult<T> Create(List<T> items, int currentPage, int pageSize, int totalItems)
    {
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        return new PagedResult<T>
        {
            Data = items,
            Pagination = new PaginationMetadata
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            }
        };
    }
}

public class PaginationMetadata
{
    [JsonPropertyName("currentPage")]
    public int CurrentPage { get; set; }

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    [JsonPropertyName("totalItems")]
    public int TotalItems { get; set; }

    [JsonPropertyName("totalPages")]
    public int TotalPages { get; set; }
}
```

**3. `src/BeatsStoreYt.API/Common/PaginationParams.cs`**

```csharp
namespace BeatsStoreYt.API.Common;

public class PaginationParams
{
    private const int MaxPageSize = 100;
    private int _pageSize = 20;

    public int Page { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    public void Validate()
    {
        if (Page < 1)
            throw new ArgumentException("Page must be >= 1");
        if (PageSize < 1)
            throw new ArgumentException("PageSize must be >= 1");
    }
}
```

**4. `src/BeatsStoreYt.API/Middleware/GlobalExceptionMiddleware.cs`**

```csharp
using System.Net;
using System.Text.Json;
using BeatsStoreYt.API.Common;

namespace BeatsStoreYt.API.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWar
```
