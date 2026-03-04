namespace ControleFinanceiro.Application.Common;

public class PagedRequest
{
    public int Page { get; private set; } = 1;
    public int PageSize { get; private set; } = 10;

    public PagedRequest(int page = 1, int pageSize = 10)
    {
        if (page < 1)
            page = 1;

        if (pageSize < 1)
            pageSize = 10;

        if (pageSize > 100)
            pageSize = 100;

        Page = page;
        PageSize = pageSize;
    }

    public int Skip => (Page - 1) * PageSize;
    public int Take => PageSize;
}
