public interface IPremiumCalculator {
    decimal CalculateMonthlyPremium(PolicyHolder holder, DateTime now);
}

public class StandardCalculator : IPremiumCalculator {
    public decimal CalculateMonthlyPremium(PolicyHolder holder, DateTime now) {
        return holder.GetActivePolicies(now).Sum((p) => p.MonthlyPremium);
    }
}

public class MultiPolicyDiscountCalculator : IPremiumCalculator {
    public decimal CalculateMonthlyPremium(PolicyHolder holder, DateTime now) {
        IReadOnlyList<Policy> policies = holder.GetActivePolicies(now);
        bool hasTwoOrMorePolicies = policies.Count() >= 2;
        // 10% discount for 2 or more policies
        return hasTwoOrMorePolicies ? policies.Sum((p) => p.MonthlyPremium) * (1 - 0.1m) : policies.Sum((p) => p.MonthlyPremium);
    
    }
}