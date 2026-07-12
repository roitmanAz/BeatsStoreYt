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
