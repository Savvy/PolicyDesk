# PolicyDesk

A small .NET 10 console app for managing insurance policyholders and their policies. It's a practice project covering domain rules, the Strategy pattern, a repository, and constructor-based dependency injection.

## Project layout

```
PolicyDesk/
├── Models/
│   ├── Policy.cs                    # Policy entity + PolicyType enum
│   ├── PolicyHolder.cs              # Policyholder with a read-only list of policies
│   ├── PolicyHolderRepository.cs    # IPolicyHolderRepository + in-memory implementation
│   └── PremiumCalculator.cs         # IPremiumCalculator + Standard and MultiPolicyDiscount
├── Services/
│   └── PolicyDeskService.cs         # Receives a repository and calculator via its constructor
├── PolicyDesk.Tests/                # xUnit test project
└── Program.cs                       # Seeds sample data and prints a report
```

## Business rules

- A policy's monthly premium must be greater than zero.
- A policy's end date must be after its start date.
- A policy is active when it isn't canceled and the given date falls between its start and end dates.
- A policy can't be canceled twice.
- A canceled policy can't be renewed, and renewals must be 1 to 24 months.
- A policyholder only accepts policies whose `PolicyholderId` matches their own `Id`.

## Premium calculators

Both implement `IPremiumCalculator`, so either can be swapped in without changing other code:

- **StandardCalculator** sums the monthly premiums of all active policies.
- **MultiPolicyDiscountCalculator** gives 10% off that sum when the holder has two or more active policies.

## Running

```bash
dotnet run --project PolicyDesk.csproj
```

The app seeds three policyholders with a mix of active, expired and canceled policies. It prints each holder's active policies and their totals under both calculators, then tries three invalid actions and prints the resulting error messages.

## Tests

```bash
dotnet test PolicyDesk.Tests
```

`PolicyTests.cs` covers three rules: a premium of zero or less is rejected, a policy can't be canceled twice, and a canceled policy can't be renewed.

Methods that depend on time, such as `IsActive` and `GetActivePolicies`, take the date as a parameter, so tests don't depend on today's date.
