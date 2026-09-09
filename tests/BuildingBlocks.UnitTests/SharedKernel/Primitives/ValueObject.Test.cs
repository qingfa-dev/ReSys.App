using BuildingBlocks.SharedKernel.Domain.Primitives.ValueObject;

namespace BuildingBlocks.UnitTests.SharedKernel.Primitives;

public class ValueObjectTest
{
    private class Money(string currency, decimal amount) : ValueObject
    {
        public string Currency { get; } = currency;
        public decimal Amount { get; } = amount;

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Currency;
            yield return Amount;
        }
    }

    [Fact]
    public void Equals_SameValues_ShouldReturnTrue()
    {
        // Arrange
        var money1 = new Money("USD", 100);
        var money2 = new Money("USD", 100);

        // Act & Assert
        money1.Equals(money2).Should().BeTrue();
        (money1 == money2).Should().BeTrue();
        (money1 != money2).Should().BeFalse();
    }

    [Fact]
    public void Equals_DifferentValues_ShouldReturnFalse()
    {
        // Arrange
        var money1 = new Money("USD", 100);
        var money2 = new Money("EUR", 100);

        // Act & Assert
        money1.Equals(money2).Should().BeFalse();
        (money1 == money2).Should().BeFalse();
        (money1 != money2).Should().BeTrue();
    }

    [Fact]
    public void Equals_Null_ShouldReturnFalse()
    {
        // Arrange
        var money = new Money("USD", 100);

        // Act & Assert
        money.Equals(null).Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_SameValues_ShouldReturnSameHash()
    {
        // Arrange
        var money1 = new Money("USD", 100);
        var money2 = new Money("USD", 100);

        // Act & Assert
        money1.GetHashCode().Should().Be(money2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_DifferentValues_ShouldReturnDifferentHash()
    {
        // Arrange
        var money1 = new Money("USD", 100);
        var money2 = new Money("EUR", 100);

        // Act & Assert
        money1.GetHashCode().Should().NotBe(money2.GetHashCode());
    }
}
