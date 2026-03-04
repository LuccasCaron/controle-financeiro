namespace ControleFinanceiro.Api.Helpers;

public static class PaginationHelper
{
    public static (int page, int pageSize) NormalizePagination(int page, int pageSize)
    {
        if (page < 1)
            page = 1;

        if (pageSize < 1)
            pageSize = 10;

        if (pageSize > 100)
            pageSize = 100;

        return (page, pageSize);
    }
}
