using BuildingBlocks.SharedKernel.Maybes;

namespace BuildingBlocks.UnitTests.SharedKernel.Maybes;

public class MaybeTest
{
    [Fact]
    public void From_ShouldCreateMaybeWithValue()
    {
        // Act
        var maybe = Maybe<int>.From(42);

        // Assert
        maybe.HasValue.Should().BeTrue();
        maybe.HasNoValue.Should().BeFalse();
        maybe.Value.Should().Be(42);
    }

    [Fact]
    public void Empty_ShouldCreateMaybeWithoutValue()
    {
        // Act
        var maybe = Maybe<int>.Empty();

        // Assert
        maybe.HasValue.Should().BeFalse();
        maybe.HasNoValue.Should().BeTrue();
    }

    [Fact]
    public void Value_WhenHasNoValue_ShouldThrowException()
    {
        // Arrange
        var maybe = Maybe<int>.Empty();

        // Act & Assert
        Action act = () => _ = maybe.Value;
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GetValueOrDefault_WhenHasValue_ShouldReturnValue()
    {
        // Arrange
        var maybe = Maybe<int>.From(42);

        // Act & Assert
        maybe.GetValueOrDefault(0).Should().Be(42);
    }

    [Fact]
    public void GetValueOrDefault_WhenHasNoValue_ShouldReturnDefault()
    {
        // Arrange
        var maybe = Maybe<int>.Empty();

        // Act & Assert
        maybe.GetValueOrDefault(0).Should().Be(0);
    }

    [Fact]
    public void GetValueOrThrow_WhenHasValue_ShouldReturnValue()
    {
        // Arrange
        var maybe = Maybe<int>.From(42);

        // Act & Assert
        maybe.GetValueOrThrow(new Exception("error")).Should().Be(42);
    }

    [Fact]
    public void GetValueOrThrow_WhenHasNoValue_ShouldThrowException()
    {
        // Arrange
        var maybe = Maybe<int>.Empty();
        var exception = new InvalidOperationException("No value");

        // Act & Assert
        Action act = () => maybe.GetValueOrThrow(exception);
        act.Should().Throw<InvalidOperationException>().WithMessage("No value");
    }

    [Fact]
    public void Map_WhenHasValue_ShouldTransform()
    {
        // Arrange
        var maybe = Maybe<int>.From(42);

        // Act
        var result = maybe.Map(x => x.ToString());

        // Assert
        result.HasValue.Should().BeTrue();
        result.Value.Should().Be("42");
    }

    [Fact]
    public void Map_WhenHasNoValue_ShouldReturnEmpty()
    {
        // Arrange
        var maybe = Maybe<int>.Empty();

        // Act
        var result = maybe.Map(x => x.ToString());

        // Assert
        result.HasNoValue.Should().BeTrue();
    }

    [Fact]
    public void Bind_WhenHasValue_ShouldChainOperations()
    {
        // Arrange
        var maybe = Maybe<int>.From(42);

        // Act
        var result = maybe.Bind(x => Maybe<string>.From(x.ToString()));

        // Assert
        result.HasValue.Should().BeTrue();
        result.Value.Should().Be("42");
    }

    [Fact]
    public void Bind_WhenHasNoValue_ShouldReturnEmpty()
    {
        // Arrange
        var maybe = Maybe<int>.Empty();

        // Act
        var result = maybe.Bind(x => Maybe<string>.From(x.ToString()));

        // Assert
        result.HasNoValue.Should().BeTrue();
    }

    [Fact]
    public void Tap_WhenHasValue_ShouldExecuteAction()
    {
        // Arrange
        var maybe = Maybe<int>.From(42);
        int captured = 0;

        // Act
        var result = maybe.Tap(x => captured = x);

        // Assert
        captured.Should().Be(42);
        result.Should().Be(maybe);
    }

    [Fact]
    public void Tap_WhenHasNoValue_ShouldNotExecuteAction()
    {
        // Arrange
        var maybe = Maybe<int>.Empty();
        bool executed = false;

        // Act
        var result = maybe.Tap(x => executed = true);

        // Assert
        executed.Should().BeFalse();
        result.Should().Be(maybe);
    }

    [Fact]
    public void TapNoValue_WhenHasNoValue_ShouldExecuteAction()
    {
        // Arrange
        var maybe = Maybe<int>.Empty();
        bool executed = false;

        // Act
        var result = maybe.TapNoValue(() => executed = true);

        // Assert
        executed.Should().BeTrue();
        result.Should().Be(maybe);
    }

    [Fact]
    public void TapNoValue_WhenHasValue_ShouldNotExecuteAction()
    {
        // Arrange
        var maybe = Maybe<int>.From(42);
        bool executed = false;

        // Act
        var result = maybe.TapNoValue(() => executed = true);

        // Assert
        executed.Should().BeFalse();
        result.Should().Be(maybe);
    }

    [Fact]
    public void Match_WhenHasValue_ShouldCallSome()
    {
        // Arrange
        var maybe = Maybe<int>.From(42);

        // Act
        var result = maybe.Match(
            some: x => $"Value: {x}",
            none: () => "No value");

        // Assert
        result.Should().Be("Value: 42");
    }

    [Fact]
    public void Match_WhenHasNoValue_ShouldCallNone()
    {
        // Arrange
        var maybe = Maybe<int>.Empty();

        // Act
        var result = maybe.Match(
            some: x => $"Value: {x}",
            none: () => "No value");

        // Assert
        result.Should().Be("No value");
    }

    [Fact]
    public void ImplicitConversion_WithValue_ShouldCreateMaybe()
    {
        // Act
        Maybe<int> maybe = 42;

        // Assert
        maybe.HasValue.Should().BeTrue();
        maybe.Value.Should().Be(42);
    }

    [Fact]
    public void ImplicitConversion_Null_ShouldCreateEmptyMaybe()
    {
        // Act
        string? nullValue = null;
        Maybe<string> maybe = nullValue!;

        // Assert
        maybe.HasNoValue.Should().BeTrue();
    }

    [Fact]
    public void Equals_SameValue_ShouldReturnTrue()
    {
        // Arrange
        var maybe1 = Maybe<int>.From(42);
        var maybe2 = Maybe<int>.From(42);

        // Act & Assert
        maybe1.Equals(maybe2).Should().BeTrue();
    }

    [Fact]
    public void Equals_DifferentValue_ShouldReturnFalse()
    {
        // Arrange
        var maybe1 = Maybe<int>.From(42);
        var maybe2 = Maybe<int>.From(43);

        // Act & Assert
        maybe1.Equals(maybe2).Should().BeFalse();
    }

    [Fact]
    public void Equals_DifferentHasValue_ShouldReturnFalse()
    {
        // Arrange
        var maybe1 = Maybe<int>.From(42);
        var maybe2 = Maybe<int>.Empty();

        // Act & Assert
        maybe1.Equals(maybe2).Should().BeFalse();
    }
}
