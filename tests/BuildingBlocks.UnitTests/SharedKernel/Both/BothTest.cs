using BuildingBlocks.SharedKernel.Both;

namespace BuildingBlocks.UnitTests.SharedKernel.Both;

public class BothTest
{
    [Fact]
    public void FromLeft_ShouldCreateBothWithLeftValue()
    {
        // Act
        var both = Both<int, string>.FromLeft(42);

        // Assert
        both.IsLeft.Should().BeTrue();
        both.IsRight.Should().BeFalse();
        both.Left.Should().Be(42);
    }

    [Fact]
    public void FromRight_ShouldCreateBothWithRightValue()
    {
        // Act
        var both = Both<int, string>.FromRight("hello");

        // Assert
        both.IsLeft.Should().BeFalse();
        both.IsRight.Should().BeTrue();
        both.Right.Should().Be("hello");
    }

    [Fact]
    public void Left_WhenHasRight_ShouldThrowException()
    {
        // Arrange
        var both = Both<int, string>.FromRight("hello");

        // Act & Assert
        Action act = () => _ = both.Left;
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Right_WhenHasLeft_ShouldThrowException()
    {
        // Arrange
        var both = Both<int, string>.FromLeft(42);

        // Act & Assert
        Action act = () => _ = both.Right;
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void MapLeft_WhenHasLeft_ShouldTransform()
    {
        // Arrange
        var both = Both<int, string>.FromLeft(42);

        // Act
        var result = both.MapLeft(x => x.ToString());

        // Assert
        result.IsLeft.Should().BeTrue();
        result.Left.Should().Be("42");
    }

    [Fact]
    public void MapLeft_WhenHasRight_ShouldPropagateRight()
    {
        // Arrange
        var both = Both<int, string>.FromRight("hello");

        // Act
        var result = both.MapLeft(x => x.ToString());

        // Assert
        result.IsRight.Should().BeTrue();
        result.Right.Should().Be("hello");
    }

    [Fact]
    public void MapRight_WhenHasRight_ShouldTransform()
    {
        // Arrange
        var both = Both<int, string>.FromRight("hello");

        // Act
        var result = both.MapRight(x => x.Length);

        // Assert
        result.IsRight.Should().BeTrue();
        result.Right.Should().Be(5);
    }

    [Fact]
    public void MapRight_WhenHasLeft_ShouldPropagateLeft()
    {
        // Arrange
        var both = Both<int, string>.FromLeft(42);

        // Act
        var result = both.MapRight(x => x.Length);

        // Assert
        result.IsLeft.Should().BeTrue();
        result.Left.Should().Be(42);
    }

    [Fact]
    public void BindLeft_WhenHasLeft_ShouldChainOperations()
    {
        // Arrange
        var both = Both<int, string>.FromLeft(42);

        // Act
        var result = both.BindLeft(x => Both<string, string>.FromLeft(x.ToString()));

        // Assert
        result.IsLeft.Should().BeTrue();
        result.Left.Should().Be("42");
    }

    [Fact]
    public void BindLeft_WhenHasRight_ShouldPropagateRight()
    {
        // Arrange
        var both = Both<int, string>.FromRight("hello");

        // Act
        var result = both.BindLeft(x => Both<string, string>.FromLeft(x.ToString()));

        // Assert
        result.IsRight.Should().BeTrue();
        result.Right.Should().Be("hello");
    }

    [Fact]
    public void BindRight_WhenHasRight_ShouldChainOperations()
    {
        // Arrange
        var both = Both<int, string>.FromRight("hello");

        // Act
        var result = both.BindRight(x => Both<int, int>.FromRight(x.Length));

        // Assert
        result.IsRight.Should().BeTrue();
        result.Right.Should().Be(5);
    }

    [Fact]
    public void BindRight_WhenHasLeft_ShouldPropagateLeft()
    {
        // Arrange
        var both = Both<int, string>.FromLeft(42);

        // Act
        var result = both.BindRight(x => Both<int, int>.FromRight(x.Length));

        // Assert
        result.IsLeft.Should().BeTrue();
        result.Left.Should().Be(42);
    }

    [Fact]
    public void Match_WhenHasLeft_ShouldCallOnLeft()
    {
        // Arrange
        var both = Both<int, string>.FromLeft(42);

        // Act
        var result = both.Match(
            onLeft: x => $"Value: {x}",
            onRight: s => $"String: {s}");

        // Assert
        result.Should().Be("Value: 42");
    }

    [Fact]
    public void Match_WhenHasRight_ShouldCallOnRight()
    {
        // Arrange
        var both = Both<int, string>.FromRight("hello");

        // Act
        var result = both.Match(
            onLeft: x => $"Value: {x}",
            onRight: s => $"String: {s}");

        // Assert
        result.Should().Be("String: hello");
    }

    [Fact]
    public void TapLeft_WhenHasLeft_ShouldExecuteAction()
    {
        // Arrange
        var both = Both<int, string>.FromLeft(42);
        int captured = 0;

        // Act
        var result = both.TapLeft(x => captured = x);

        // Assert
        captured.Should().Be(42);
        result.Should().Be(both);
    }

    [Fact]
    public void TapLeft_WhenHasRight_ShouldNotExecuteAction()
    {
        // Arrange
        var both = Both<int, string>.FromRight("hello");
        bool executed = false;

        // Act
        var result = both.TapLeft(x => executed = true);

        // Assert
        executed.Should().BeFalse();
        result.Should().Be(both);
    }

    [Fact]
    public void TapRight_WhenHasRight_ShouldExecuteAction()
    {
        // Arrange
        var both = Both<int, string>.FromRight("hello");
        string captured = "";

        // Act
        var result = both.TapRight(s => captured = s);

        // Assert
        captured.Should().Be("hello");
        result.Should().Be(both);
    }

    [Fact]
    public void TapRight_WhenHasLeft_ShouldNotExecuteAction()
    {
        // Arrange
        var both = Both<int, string>.FromLeft(42);
        bool executed = false;

        // Act
        var result = both.TapRight(s => executed = true);

        // Assert
        executed.Should().BeFalse();
        result.Should().Be(both);
    }

    [Fact]
    public void GetLeftOrDefault_WhenHasLeft_ShouldReturnValue()
    {
        // Arrange
        var both = Both<int, string>.FromLeft(42);

        // Act & Assert
        both.GetLeftOrDefault(0).Should().Be(42);
    }

    [Fact]
    public void GetLeftOrDefault_WhenHasRight_ShouldReturnDefault()
    {
        // Arrange
        var both = Both<int, string>.FromRight("hello");

        // Act & Assert
        both.GetLeftOrDefault(0).Should().Be(0);
    }

    [Fact]
    public void GetRightOrDefault_WhenHasRight_ShouldReturnValue()
    {
        // Arrange
        var both = Both<int, string>.FromRight("hello");

        // Act & Assert
        both.GetRightOrDefault("").Should().Be("hello");
    }

    [Fact]
    public void GetRightOrDefault_WhenHasLeft_ShouldReturnDefault()
    {
        // Arrange
        var both = Both<int, string>.FromLeft(42);

        // Act & Assert
        both.GetRightOrDefault("").Should().Be("");
    }

    [Fact]
    public void GetLeftOrThrow_WhenHasLeft_ShouldReturnValue()
    {
        // Arrange
        var both = Both<int, string>.FromLeft(42);

        // Act & Assert
        both.GetLeftOrThrow().Should().Be(42);
    }

    [Fact]
    public void GetLeftOrThrow_WhenHasRight_ShouldThrowException()
    {
        // Arrange
        var both = Both<int, string>.FromRight("hello");

        // Act & Assert
        Action act = () => both.GetLeftOrThrow();
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GetRightOrThrow_WhenHasRight_ShouldReturnValue()
    {
        // Arrange
        var both = Both<int, string>.FromRight("hello");

        // Act & Assert
        both.GetRightOrThrow().Should().Be("hello");
    }

    [Fact]
    public void GetRightOrThrow_WhenHasLeft_ShouldThrowException()
    {
        // Arrange
        var both = Both<int, string>.FromLeft(42);

        // Act & Assert
        Action act = () => both.GetRightOrThrow();
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Swap_WhenHasLeft_ShouldReturnRight()
    {
        // Arrange
        var both = Both<int, string>.FromLeft(42);

        // Act
        var result = both.Swap();

        // Assert
        result.IsRight.Should().BeTrue();
        result.Right.Should().Be(42);
    }

    [Fact]
    public void Swap_WhenHasRight_ShouldReturnLeft()
    {
        // Arrange
        var both = Both<int, string>.FromRight("hello");

        // Act
        var result = both.Swap();

        // Assert
        result.IsLeft.Should().BeTrue();
        result.Left.Should().Be("hello");
    }

    [Fact]
    public void Switch_WhenHasLeft_ShouldExecuteOnLeft()
    {
        // Arrange
        var both = Both<int, string>.FromLeft(42);
        int captured = 0;

        // Act
        both.Switch(
            onLeft: x => captured = x,
            onRight: s => { });

        // Assert
        captured.Should().Be(42);
    }

    [Fact]
    public void Switch_WhenHasRight_ShouldExecuteOnRight()
    {
        // Arrange
        var both = Both<int, string>.FromRight("hello");
        string captured = "";

        // Act
        both.Switch(
            onLeft: x => { },
            onRight: s => captured = s);

        // Assert
        captured.Should().Be("hello");
    }

    [Fact]
    public void ImplicitConversion_Left_ShouldCreateBoth()
    {
        // Act
        Both<int, string> both = 42;

        // Assert
        both.IsLeft.Should().BeTrue();
        both.Left.Should().Be(42);
    }

    [Fact]
    public void ImplicitConversion_Right_ShouldCreateBoth()
    {
        // Act
        Both<int, string> both = "hello";

        // Assert
        both.IsRight.Should().BeTrue();
        both.Right.Should().Be("hello");
    }

    [Fact]
    public void ToString_WhenHasLeft_ShouldReturnLeftFormat()
    {
        // Arrange
        var both = Both<int, string>.FromLeft(42);

        // Act & Assert
        both.ToString().Should().Be("Left(42)");
    }

    [Fact]
    public void ToString_WhenHasRight_ShouldReturnRightFormat()
    {
        // Arrange
        var both = Both<int, string>.FromRight("hello");

        // Act & Assert
        both.ToString().Should().Be("Right(hello)");
    }
}