using BuildingBlocks.SharedKernel.Primitives.StronglyTypedIds;

namespace BuildingBlocks.UnitTests.SharedKernel.Primitives;

public class StronglyTypedIdTest
{
    private class OrderId(Guid value) : StronglyTypedId<Guid>(value)
    {
    }

    private class ProductId(Guid value) : StronglyTypedId<Guid>(value)
    {
    }

    [Fact]
    public void Constructor_ShouldSetValue()
    {
        // Arrange
        var value = Guid.NewGuid();

        // Act
        var orderId = new OrderId(value);

        // Assert
        orderId.Value.Should().Be(value);
    }

    [Fact]
    public void Equals_SameTypeSameValue_ShouldReturnTrue()
    {
        // Arrange
        var value = Guid.NewGuid();
        var id1 = new OrderId(value);
        var id2 = new OrderId(value);

        // Act & Assert
        id1.Equals(id2).Should().BeTrue();
        (id1 == id2).Should().BeTrue();
        (id1 != id2).Should().BeFalse();
    }

    [Fact]
    public void Equals_SameValueDifferentType_ShouldReturnFalse()
    {
        // Arrange
        var value = Guid.NewGuid();
        var orderId = new OrderId(value);
        var productId = new ProductId(value);

        // Act & Assert
        orderId.Equals(productId).Should().BeFalse();
    }

    [Fact]
    public void Equals_Null_ShouldReturnFalse()
    {
        // Arrange
        var orderId = new OrderId(Guid.NewGuid());

        // Act & Assert
        orderId.Equals(null).Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_SameValue_ShouldReturnSameHash()
    {
        // Arrange
        var value = Guid.NewGuid();
        var id1 = new OrderId(value);
        var id2 = new OrderId(value);

        // Act & Assert
        id1.GetHashCode().Should().Be(id2.GetHashCode());
    }

    [Fact]
    public void ToString_ShouldReturnValueAsString()
    {
        // Arrange
        var value = Guid.NewGuid();
        var orderId = new OrderId(value);

        // Act & Assert
        orderId.ToString().Should().Be(value.ToString());
    }
}