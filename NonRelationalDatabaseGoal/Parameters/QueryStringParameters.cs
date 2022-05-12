using NonRelationalDatabaseGoal.Enums;

namespace NonRelationalDatabaseGoal.Parameters;

public class QueryStringParameters
{
    protected const int MaxPageSize = 50;
    private int _pageSize = 10;

    public int PageNumber { get; set; } = 1;
    public string? OrderBy { get; set; }
    public Order Order { get; set; }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }
}
