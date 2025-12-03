namespace Discussion.Core.Infrastructure.Models.Common;

public class PaginationModel
{
    private const int MaxPageSize = 150;
    private int _pageSize = 10;
    private int _offset;

    public int Offset
    {
        get => this._offset;
        set
        {
            if (value < 0)
                value = 0;
            this._offset = value;
        }
    }

    public int PageSize
    {
        get => this._pageSize;
        set
        {
            if (value < 0)
                value = 0;
            this._pageSize = value > 150 ? 150 : value;
        }
    }
}