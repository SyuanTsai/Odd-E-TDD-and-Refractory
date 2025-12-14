namespace BudgetSystem;

public class BudgetCalculator
{
    private readonly IBudgetRepo _repo;

    public BudgetCalculator(IBudgetRepo repo)
    {
        _repo = repo;
    }

    public decimal Query(DateTime start, DateTime end)
    {
        if (start > end)
        {
            return 0;
        }

        var budgets = _repo.GetAll();
        if (start.ToString("yyyyMM") == end.ToString("yyyyMM"))
        {
            var interval = (end - start).Days + 1;

            // Use Shift + F6 (IDEA Setting)
            // 修改無意義的 startAmount1 命名，避免誤以為有實際區分用途
            // Rename meaningless startAmount1 to avoid implying a functional distinction
            var startAmount = GetMonthAmount(start, budgets);
            return interval * startAmount / DateTime.DaysInMonth(end.Year, end.Month);
        }

        if (Math.Abs(start.Month - end.Month) >= 2)
        {
            // Use Shift + F6 (IDEA Setting)
            // 修正變數拼字錯誤（toal → total），提升可讀性，不影響行為
            // Fix variable name typo (toal → total) to improve readability without behavior change
            decimal totalInterval = 0;
            var tempDate = start.AddMonths(1);
            while (tempDate > start && tempDate < end)
            {
                totalInterval += GetMonthAmount(tempDate, budgets);
                tempDate = tempDate.AddMonths(1);
            }

            return StartAmount(start, budgets) + totalInterval + EndAmount(end, budgets);
        }
        else
        {
            return StartAmount(start, budgets) + EndAmount(end, budgets);
        }
    }


    private static decimal EndAmount(DateTime end, List<Budget> budgets)
    {
        var endMonth_TotalDays = DateTime.DaysInMonth(end.Year, end.Month);

        var endAmount = GetMonthAmount(end, budgets);
        var endDays = end.Day;

        // Use Ctrl + Shift + N (IDEA Setting)
        // 移除僅使用一次的變數，讓回傳邏輯更直接
        // Remove single-use temporary variable to simplify return logic
        return endDays * endAmount / endMonth_TotalDays;
    }


    private static decimal GetMonthAmount(DateTime start, List<Budget> budgets)
    {
        var startMonthData = budgets.FirstOrDefault(a => a.YearMonth == start.ToString("yyyyMM"));

        // Step 1 - Use Alt + Enter => Convert to '?:' operator (IDEA Setting)
        // Step 2 - Use Alt + Enter => Use null-coalescing expression (IDEA Setting)
        // 簡化 null 判斷邏輯，降低控制流程複雜度，不影響行為
        // Simplify null-check logic to reduce control flow complexity without behavior change
        return startMonthData?.Amount ?? 0;
    }


    private static decimal StartAmount(DateTime start, List<Budget> budgets)
    {
        var startAmount = GetMonthAmount(start, budgets);
        var startMonth_TotalDays = DateTime.DaysInMonth(start.Year, start.Month);
        var strDays = startMonth_TotalDays - start.Day + 1;

        // Use Ctrl + Shift + N (IDEA Setting)
        // 移除僅使用一次的變數，讓回傳邏輯更直接
        // Remove single-use temporary variable to simplify return logic
        return strDays * startAmount / startMonth_TotalDays;
    }
}