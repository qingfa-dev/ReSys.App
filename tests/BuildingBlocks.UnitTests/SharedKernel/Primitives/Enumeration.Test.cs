using BuildingBlocks.SharedKernel.Primitives.Enumerations;

namespace BuildingBlocks.UnitTests.SharedKernel.Primitives;

public class EnumerationTest
{
    private class TestEnumeration : Enumeration<int, string>
    {
        public static readonly TestEnumeration First = new(1, "First");
        public static readonly TestEnumeration Second = new(2, "Second");
        public static readonly TestEnumeration Third = new(3, "Third");

        private TestEnumeration(int id, string value)
            : base(id, value)
        {
        }
    }

    [Fact]
    public void Constructor_ShouldSetProperties()
    {
        // Act
        var enumeration = TestEnumeration.First;

        // Assert
        enumeration.Id.Should().Be(1);
        enumeration.Value.Should().Be("First");
        enumeration.Name.Should().Be("First");
    }

    [Fact]
    public void Equals_SameId_ShouldReturnTrue()
    {
        // Arrange
        var enum1 = TestEnumeration.First;
        var enum2 = TestEnumeration.First;

        // Act & Assert
        enum1.Equals(enum2).Should().BeTrue();
    }

    [Fact]
    public void Equals_DifferentId_ShouldReturnFalse()
    {
        // Arrange
        var enum1 = TestEnumeration.First;
        var enum2 = TestEnumeration.Second;

        // Act & Assert
        enum1.Equals(enum2).Should().BeFalse();
    }

    [Fact]
    public void Equals_Null_ShouldReturnFalse()
    {
        // Arrange
        var enumeration = TestEnumeration.First;

        // Act & Assert
        enumeration.Equals(null).Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_SameId_ShouldReturnSameHash()
    {
        // Arrange
        var enum1 = TestEnumeration.First;
        var enum2 = TestEnumeration.First;

        // Act & Assert
        enum1.GetHashCode().Should().Be(enum2.GetHashCode());
    }

    [Fact]
    public void ToString_ShouldReturnName()
    {
        // Arrange & Act & Assert
        TestEnumeration.First.ToString().Should().Be("First");
    }

    [Fact]
    public void FromValue_ById_ShouldFindEnumeration()
    {
        // Act
        var result = Enumeration<int, string>.FromValue<TestEnumeration>(2);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(TestEnumeration.Second);
    }

    [Fact]
    public void FromValue_ByValue_ShouldFindEnumeration()
    {
        // Act
        var result = Enumeration<int, string>.FromValue<TestEnumeration>("First");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(TestEnumeration.First);
    }

    [Fact]
    public void FromValue_InvalidId_ShouldReturnFailure()
    {
        // Act
        var result = Enumeration<int, string>.FromValue<TestEnumeration>(999);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle();
        result.Errors.First().Code.Should().Be("Enumeration.NotFound");
    }

    [Fact]
    public void FromValue_InvalidValue_ShouldReturnFailure()
    {
        // Act
        var result = Enumeration<int, string>.FromValue<TestEnumeration>("Invalid");

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle();
        result.Errors.First().Code.Should().Be("Enumeration.NotFound");
    }
}