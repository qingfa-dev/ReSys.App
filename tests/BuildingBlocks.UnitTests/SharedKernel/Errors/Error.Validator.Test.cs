namespace BuildingBlocks.UnitTests.SharedKernel.Errors;

public class ErrorValidatorTest
{
    #region ValidateStatus

    [Fact]
    public void ValidateStatus_ValidStatus_ShouldNotThrow()
    {
        Action act = () => ErrorValidator.ValidateStatus(200);
        act.Should().NotThrow();
    }

    [Theory]
    [InlineData(100)]
    [InlineData(200)]
    [InlineData(400)]
    [InlineData(404)]
    [InlineData(500)]
    [InlineData(503)]
    [InlineData(599)]
    public void ValidateStatus_BoundaryAndCommonCodes_ShouldNotThrow(int status)
    {
        Action act = () => ErrorValidator.ValidateStatus(status);
        act.Should().NotThrow();
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(99)]
    public void ValidateStatus_BelowRange_ShouldThrowOutOfRangeException(int status)
    {
        Action act = () => ErrorValidator.ValidateStatus(status);
        act.Should().Throw<ArgumentOutOfRangeException>()
            .And.ParamName.Should().Be("status");
    }

    [Theory]
    [InlineData(600)]
    [InlineData(999)]
    [InlineData(int.MaxValue)]
    public void ValidateStatus_AboveRange_ShouldThrowOutOfRangeException(int status)
    {
        Action act = () => ErrorValidator.ValidateStatus(status);
        act.Should().Throw<ArgumentOutOfRangeException>()
            .And.ParamName.Should().Be("status");
    }

    #endregion

    #region ValidateType

    [Fact]
    public void ValidateType_NonNullType_ShouldNotThrow()
    {
        Action act = () => ErrorValidator.ValidateType("some-type");
        act.Should().NotThrow();
    }

    [Fact]
    public void ValidateType_Null_ShouldThrowArgumentNullException()
    {
        Action act = () => ErrorValidator.ValidateType(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion

    #region ValidateCode

    [Fact]
    public void ValidateCode_ValidCode_ShouldNotThrow()
    {
        Action act = () => ErrorValidator.ValidateCode("Test.Code");
        act.Should().NotThrow();
    }

    [Fact]
    public void ValidateCode_MaxLength_ShouldNotThrow()
    {
        var code = new string('a', ErrorConstant.Constraints.MaxCodeLength);
        Action act = () => ErrorValidator.ValidateCode(code);
        act.Should().NotThrow();
    }

    [Fact]
    public void ValidateCode_Null_ShouldThrowArgumentException()
    {
        Action act = () => ErrorValidator.ValidateCode(null!);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ValidateCode_Empty_ShouldThrowArgumentException()
    {
        Action act = () => ErrorValidator.ValidateCode(string.Empty);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ValidateCode_Whitespace_ShouldThrowArgumentException()
    {
        Action act = () => ErrorValidator.ValidateCode("   ");
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ValidateCode_ExceedsMaxLength_ShouldThrowArgumentOutOfRangeException()
    {
        var code = new string('a', ErrorConstant.Constraints.MaxCodeLength + 1);
        Action act = () => ErrorValidator.ValidateCode(code);
        act.Should().Throw<ArgumentOutOfRangeException>()
            .And.ParamName.Should().Be("code");
    }

    #endregion

    #region ValidateDescription

    [Fact]
    public void ValidateDescription_ValidDescription_ShouldNotThrow()
    {
        Action act = () => ErrorValidator.ValidateDescription("A valid description.");
        act.Should().NotThrow();
    }

    [Fact]
    public void ValidateDescription_MaxLength_ShouldNotThrow()
    {
        var description = new string('b', ErrorConstant.Constraints.MaxDescriptionLength);
        Action act = () => ErrorValidator.ValidateDescription(description);
        act.Should().NotThrow();
    }

    [Fact]
    public void ValidateDescription_Null_ShouldThrowArgumentException()
    {
        Action act = () => ErrorValidator.ValidateDescription(null!);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ValidateDescription_Empty_ShouldThrowArgumentException()
    {
        Action act = () => ErrorValidator.ValidateDescription(string.Empty);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ValidateDescription_Whitespace_ShouldThrowArgumentException()
    {
        Action act = () => ErrorValidator.ValidateDescription("   ");
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ValidateDescription_ExceedsMaxLength_ShouldThrowArgumentOutOfRangeException()
    {
        var description = new string('b', ErrorConstant.Constraints.MaxDescriptionLength + 1);
        Action act = () => ErrorValidator.ValidateDescription(description);
        act.Should().Throw<ArgumentOutOfRangeException>()
            .And.ParamName.Should().Be("description");
    }

    #endregion
}