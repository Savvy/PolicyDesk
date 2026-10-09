class PolicyDeskService {
    private readonly IPolicyHolderRepository _policyHolderRepository;
    private readonly IPremiumCalculator _premiumCalculator;

    public PolicyDeskService(IPolicyHolderRepository policyHolderRepository, IPremiumCalculator premiumCalculator)
    {
        _policyHolderRepository = policyHolderRepository;
        _premiumCalculator = premiumCalculator;
    }

    public decimal CalculateMonthlyPremium(string policyHolderId)
    {
        var policyHolder = _policyHolderRepository.GetById(policyHolderId);
        return _premiumCalculator.CalculateMonthlyPremium(policyHolder, DateTime.Now);
    }
}