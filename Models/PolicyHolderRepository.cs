interface IPolicyHolderRepository {
    void Add(PolicyHolder holder);
    PolicyHolder GetById(string id);
    IReadOnlyList<PolicyHolder> GetAll();
}

public class PolicyHolderRepository : IPolicyHolderRepository {
    private readonly Dictionary<string, PolicyHolder> _policyHolders = new Dictionary<string, PolicyHolder>();

    public void Add(PolicyHolder holder)
    {
        if (_policyHolders.ContainsKey(holder.Id)) {
            throw new Exception("Policy holder already exists");
        }
        _policyHolders.Add(holder.Id, holder);
    }

    public PolicyHolder GetById(string id)
    {
        return _policyHolders[id];
    }

    public IReadOnlyList<PolicyHolder> GetAll()
    {
        return _policyHolders.Values.ToList();
    }
}