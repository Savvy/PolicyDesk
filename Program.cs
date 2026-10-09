// One "now" for the whole run, so every check sees the same moment
var now = DateTime.Now;

var repository = new PolicyHolderRepository();
var standardCalculator = new StandardCalculator();
var discountCalculator = new MultiPolicyDiscountCalculator();

// --- Seed data ---

// John: two active policies + one expired -> qualifies for the multi-policy discount
var john = new PolicyHolder("PH-001", "John Doe", "john.doe@example.com");
john.AddPolicy(new Policy(john.Id, PolicyType.Auto, 120m, now.AddMonths(-2), now.AddMonths(10)));
john.AddPolicy(new Policy(john.Id, PolicyType.Home, 200m, now.AddMonths(-1), now.AddYears(1)));
john.AddPolicy(new Policy(john.Id, PolicyType.Renters, 90m, now.AddYears(-2), now.AddYears(-1))); // expired
repository.Add(john);

// Jane: one active policy + one canceled -> only one active, so no discount
var jane = new PolicyHolder("PH-002", "Jane Doe", "jane.doe@example.com");
jane.AddPolicy(new Policy(jane.Id, PolicyType.Renters, 599m, now.AddDays(-10), now.AddMonths(6)));
var janesAuto = new Policy(jane.Id, PolicyType.Auto, 100m, now.AddMonths(-3), now.AddMonths(9));
jane.AddPolicy(janesAuto);
janesAuto.Cancel();
repository.Add(jane);

// Jim: one active, one expired, one canceled
var jim = new PolicyHolder("PH-003", "Jim Doe", "jim.doe@example.com");
jim.AddPolicy(new Policy(jim.Id, PolicyType.Auto, 800m, now.AddMonths(-1), now.AddMonths(5)));
jim.AddPolicy(new Policy(jim.Id, PolicyType.Home, 200m, now.AddMonths(-8), now.AddDays(-30))); // expired
var jimsRenters = new Policy(jim.Id, PolicyType.Renters, 467m, now.AddDays(-5), now.AddDays(100));
jim.AddPolicy(jimsRenters);
jimsRenters.Cancel();
repository.Add(jim);

// --- Report ---

foreach (var holder in repository.GetAll())
{
    Console.WriteLine($"{holder.Name} ({holder.Email})");

    var active = holder.GetActivePolicies(now);
    Console.WriteLine($"  Active policies: {active.Count} of {holder.Policies.Count}");
    foreach (var policy in active)
    {
        Console.WriteLine($"    - {policy.Type,-8} {policy.MonthlyPremium,8:0.00}/mo  (ends {policy.EndDate:yyyy-MM-dd})");
    }

    Console.WriteLine($"  Standard total:       {standardCalculator.CalculateMonthlyPremium(holder, now),8:0.00}");
    Console.WriteLine($"  Multi-policy total:   {discountCalculator.CalculateMonthlyPremium(holder, now),8:0.00}");
    Console.WriteLine();
}

// --- Invalid actions ---

Console.WriteLine("Invalid actions:");
TryAction("Cancel a policy twice", () => janesAuto.Cancel());
TryAction("Renew a canceled policy", () => jimsRenters.Renew(6));
TryAction("Create a negative premium", () => new Policy(john.Id, PolicyType.Auto, -50m, now, now.AddYears(1)));

void TryAction(string description, Action action)
{
    try
    {
        action();
        Console.WriteLine($"  {description}: no error (unexpected!)");
    }
    catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
    {
        Console.WriteLine($"  {description}: {ex.Message}");
    }
}
