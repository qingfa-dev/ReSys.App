namespace BuildingBlocks.UnitTests.SharedKernel.Results;

public class ResultValidatorTest
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(5)]
    public void ValidateErrors_ValidErrors_ShouldNotThrow(int count)
    {
        var errors = Enumerable.Range(0, count)
            .Select(i => Error.BadRequest($"Code{i}", $"Desc{i}"))
            .ToList().AsReadOnly();

        Action act = () => ResultValidator.ValidateErrors(errors);
        act.Should().NotThrow();
    }

    [Fact]
    public void ValidateErrors_EmptyList_ShouldThrowArgumentException()
    {
        var errors = new List<Error>().AsReadOnly();

        Action act = () => ResultValidator.ValidateErrors(errors);
        act.Should().Throw<ArgumentException>()
            .WithMessage($"{ResultConstant.Codes.EmptyErrors} : *");
    }

    [Fact]
    public void ValidateErrors_ExceedsMaxErrors_ShouldThrowArgumentException()
    {
        var errors = Enumerable.Repeat(
            Error.BadRequest("Code", "Desc"),
            ResultConstant.Constraints.MaxErrors + 1).ToList().AsReadOnly();

        Action act = () => ResultValidator.ValidateErrors(errors);
        act.Should().Throw<ArgumentException>()
            .WithMessage($"{ResultConstant.Codes.ExceedsMaxErrors} : *");
    }

    [Fact]
    public void ValidateErrors_MixedStatusCodes_ShouldThrowArgumentException()
    {
        var errors = new List<Error>
        {
            Error.BadRequest("Code1", "Desc1"),
            Error.NotFound("Code2", "Desc2")
        }.AsReadOnly();

        Action act = () => ResultValidator.ValidateErrors(errors);
        act.Should().Throw<ArgumentException>()
            .WithMessage($"{ResultConstant.Codes.MixedStatusCodes} : *");
    }

    [Fact]
    public void ValidateErrors_MaxErrors_ShouldNotThrow()
    {
        var errors = Enumerable.Repeat(
            Error.BadRequest("Code", "Desc"),
            ResultConstant.Constraints.MaxErrors).ToList().AsReadOnly();

        Action act = () => ResultValidator.ValidateErrors(errors);
        act.Should().NotThrow();
    }

    [Theory]
    [InlineData("Code1", "Desc1")]
    [InlineData("Test.Code", "Description")]
    public void ValidateErrors_SameStatusCodes_ShouldNotThrow(string code, string desc)
    {
        var errors = new List<Error>
        {
            Error.NotFound(code, desc),
            Error.NotFound("Code2", "Desc2"),
            Error.NotFound("Code3", "Desc3")
        }.AsReadOnly();

        Action act = () => ResultValidator.ValidateErrors(errors);
        act.Should().NotThrow();
    }
}
