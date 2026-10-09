public enum PolicyType { Auto, Home, Renters }
public enum ClaimStatus { Pending, Approved, Rejected }
public class Policy
{
    public string PolicyholderId { get; }
    public PolicyType Type { get; }
    public decimal MonthlyPremium { get; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public bool IsCanceled { get; private set; }

    // New property to check if the policy has been canceled before, policies can be canceled only once
    public bool HasBeenCanceled { get; private set; }

    public Policy(string policyholderId, PolicyType type, decimal monthlyPremium, DateTime startDate, DateTime endDate)
    {
        if (monthlyPremium <= 0) throw new ArgumentException("Premium must be greater than zero.");
        if (endDate <= startDate) throw new ArgumentException("End date must be after start date.");

        PolicyholderId = policyholderId;
        Type = type;
        MonthlyPremium = monthlyPremium;
        StartDate = startDate;
        EndDate = endDate;
    }

    public bool IsActive(DateTime now) => !IsCanceled && now >= StartDate && now <= EndDate;

    public void Cancel()
    {
        if (HasBeenCanceled) throw new InvalidOperationException("Policy has already been canceled.");
        if (IsCanceled) throw new InvalidOperationException("Policy is already canceled.");
        IsCanceled = true;
    }

    public void Renew(int months)
    {
        if (months < 1 || months > 24) throw new ArgumentException("Months must be between 1 and 24.");
        if (IsCanceled) throw new InvalidOperationException("This policy is already canceled. It cannot be renewed.");
        EndDate = EndDate.AddMonths(months);
    }

}