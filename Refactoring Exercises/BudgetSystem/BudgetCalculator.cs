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

            var startAmount = GetMonthAmount(start, budgets);
            return interval * startAmount / DateTime.DaysInMonth(end.Year, end.Month);
        }

        // Use Shift + F6 (IDEA Setting)
        // 將 totalInterval 重新命名為 middleAmount，並將 tempDate 改為 currentDate，
        // 明確表達中間月份的累計結果與實際迭代日期的角色，提升跨月份計算的可讀性。
        // 與其依賴流程結構理解邏輯，不如透過命名直接表達資料角色。
        // Rename totalInterval to middleAmount and tempDate to currentDate
        // to clearly express the accumulated middle-month amount and the iterated date role.
        // Prefer expressing intent through data naming rather than relying on control flow.
        decimal totalInterval = 0;
        var tempDate = start.AddMonths(1);
        var middleEnd = new DateTime(end.Year, end.Month, 1);
        while (tempDate > start && tempDate < middleEnd)
        {
            totalInterval += GetMonthAmount(tempDate, budgets);
            tempDate = tempDate.AddMonths(1);
        }

        return StartAmount(start, budgets) + totalInterval + EndAmount(end, budgets);
    }


    private static decimal EndAmount(DateTime end, List<Budget> budgets)
    {
        var endMonth_TotalDays = DateTime.DaysInMonth(end.Year, end.Month);

        var endAmount = GetMonthAmount(end, budgets);
        var endDays = end.Day;

        return endDays * endAmount / endMonth_TotalDays;
    }


    private static decimal GetMonthAmount(DateTime start, List<Budget> budgets)
    {
        var startMonthData = budgets.FirstOrDefault(a => a.YearMonth == start.ToString("yyyyMM"));

        return startMonthData?.Amount ?? 0;
    }


    private static decimal StartAmount(DateTime start, List<Budget> budgets)
    {
        var startAmount = GetMonthAmount(start, budgets);
        var startMonth_TotalDays = DateTime.DaysInMonth(start.Year, start.Month);
        var strDays = startMonth_TotalDays - start.Day + 1;

        return strDays * startAmount / startMonth_TotalDays;
    }
}