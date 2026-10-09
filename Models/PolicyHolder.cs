public class PolicyHolder
{
    public string Id { get; }
    public string Name { get; }
    public string Email { get; }
    private readonly List<Policy> policies;

    public IReadOnlyList<Policy> Policies => policies.AsReadOnly();

    public PolicyHolder(string id, string name, string email)
    {
        Id = id;
        Name = name;
        Email = email;
        policies = new List<Policy>();
    }

    public void AddPolicy(Policy policy)
    {
        if (policy.PolicyholderId != Id) throw new InvalidOperationException("Policy does not belong to this policyholder.");
        if (policies.Contains(policy)) throw new InvalidOperationException("Policy already added.");
        policies.Add(policy);
    }

    public IReadOnlyList<Policy> GetActivePolicies(DateTime now)
    {
        return policies.Where((p) => p.IsActive(now)).ToList();
    }
}