namespace PolicyDesk.Tests;

public class PolicyTests
{
    // Fixed dates so the tests never depend on today's date
    private static readonly DateTime Start = new(2026, 1, 1);
    private static readonly DateTime End = new(2026, 12, 31);

    private static Policy CreatePolicy() =>
        new("PH-001", PolicyType.Auto, 100m, Start, End);

    [Theory]
    [InlineData(0)]
    [InlineData(-50)]
    public void Constructor_RejectsPremiumOfZeroOrLess(decimal premium)
    {
        var ex = Assert.Throws<ArgumentException>(
            () => new Policy("PH-001", PolicyType.Auto, premium, Start, End));

        Assert.Equal("Premium must be greater than zero.", ex.Message);
    }

    [Fact]
    public void Cancel_ThrowsWhenPolicyIsAlreadyCanceled()
    {
        var policy = CreatePolicy();
        policy.Cancel();

        Assert.Throws<InvalidOperationException>(() => policy.Cancel());
        Assert.True(policy.IsCanceled);
    }

    [Fact]
    public void Renew_ThrowsWhenPolicyIsCanceled()
    {
        var policy = CreatePolicy();
        policy.Cancel();

        Assert.Throws<InvalidOperationException>(() => policy.Renew(6));
        Assert.Equal(End, policy.EndDate); // end date unchanged
    }
}
