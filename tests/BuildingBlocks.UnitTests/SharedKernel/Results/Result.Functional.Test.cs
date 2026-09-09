namespace BuildingBlocks.UnitTests.SharedKernel.Results;

public class ResultFunctionalTest
{
    #region Match

    [Theory]
    [InlineData("success")]
    [InlineData("other")]
    public void Match_Success_ShouldInvokeOnSuccess(string expected)
    {
        var result = Result<Error>.Ok();

        var matched = result.Match(
            onSuccess: () => expected,
            onFailure: _ => "failure");

        matched.Should().Be(expected);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(100)]
    public void Match_Failure_ShouldInvokeOnFailure(int errorCount)
    {
        var errors = Enumerable.Range(0, errorCount)
            .Select(i => Error.BadRequest($"Code{i}", $"Desc{i}"))
            .ToArray();
        var result = Result<Error>.Failure(errors);

        var matched = result.Match(
            onSuccess: () => 0,
            onFailure: errors => errors.Count);

        matched.Should().Be(errorCount);
    }

    [Fact]
    public void Match_NullOnSuccess_ShouldThrowArgumentNullException()
    {
        var result = Result<Error>.Ok();

        Action act = () => result.Match(
            onSuccess: null!,
            onFailure: _ => "failure");

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Match_NullOnFailure_ShouldThrowArgumentNullException()
    {
        var result = Result<Error>.Ok();

        Action act = () => result.Match<string>(
            onSuccess: () => "success",
            onFailure: null!);

        act.Should().Throw<ArgumentNullException>();
    }

    #endregion

    #region Tap

    [Fact]
    public void Tap_Success_ShouldExecuteAction()
    {
        var result = Result<Error>.Ok();
        var executed = false;

        var returned = result.Tap(() => executed = true);

        executed.Should().BeTrue();
        returned.Should().BeSameAs(result);
    }

    [Fact]
    public void Tap_Failure_ShouldNotExecuteAction()
    {
        var error = Error.BadRequest("Code", "Desc");
        var result = Result<Error>.Failure(error);
        var executed = false;

        var returned = result.Tap(() => executed = true);

        executed.Should().BeFalse();
        returned.Should().BeSameAs(result);
    }

    [Fact]
    public void Tap_NullAction_ShouldThrowArgumentNullException()
    {
        var result = Result<Error>.Ok();

        Action act = () => result.Tap(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(3)]
    [InlineData(5)]
    public void Tap_FluentChaining_ShouldWork(int chainCount)
    {
        var tapCount = 0;
        var result = Result<Error>.Ok();

        var returned = result;
        for (int i = 0; i < chainCount; i++)
        {
            returned = returned.Tap(() => tapCount++);
        }

        tapCount.Should().Be(chainCount);
        returned.Should().BeSameAs(result);
    }

    #endregion

    #region TapErrors

    [Fact]
    public void TapErrors_Success_ShouldNotExecuteAction()
    {
        var result = Result<Error>.Ok();
        var executed = false;

        var returned = result.TapErrors(_ => executed = true);

        executed.Should().BeFalse();
        returned.Should().BeSameAs(result);
    }

    [Theory]
    [InlineData("Code1", "Desc1")]
    [InlineData("Test.Code", "Description")]
    public void TapErrors_Failure_ShouldExecuteActionWithFirstError(string code, string desc)
    {
        var error = Error.BadRequest(code, desc);
        var result = Result<Error>.Failure(error);
        List<Error>? captured = null;

        var returned = result.TapErrors(e => captured = e);

        captured.Should().ContainSingle().Which.Should().Be(error);
        returned.Should().BeSameAs(result);
    }

    [Fact]
    public void TapErrors_FailureMultipleErrors_ShouldPassFirstError()
    {
        var error1 = Error.BadRequest("Code1", "Desc1");
        var error2 = Error.BadRequest("Code2", "Desc2");
        var result = Result<Error>.Failure(error1, error2);
        List<Error>? captured = null;

        result.TapErrors(e => captured = e);

        captured.Should().HaveCount(2);
        captured![0].Should().Be(error1);
    }

    [Fact]
    public void TapErrors_NullAction_ShouldThrowArgumentNullException()
    {
        var error = Error.BadRequest("Code", "Desc");
        var result = Result<Error>.Failure(error);

        Action act = () => result.TapErrors(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(2)]
    [InlineData(4)]
    public void TapErrors_FluentChaining_ShouldWork(int chainCount)
    {
        var tapCount = 0;
        var error = Error.BadRequest("Code", "Desc");
        var result = Result<Error>.Failure(error);

        var returned = result;
        for (int i = 0; i < chainCount; i++)
        {
            returned = returned.TapErrors(_ => tapCount++);
        }

        tapCount.Should().Be(chainCount);
        returned.Should().BeSameAs(result);
    }

    #endregion

    #region ThrowIfFailure

    [Fact]
    public void ThrowIfFailure_Success_ShouldReturnSameResult()
    {
        var result = Result<Error>.Ok();

        var returned = result.ThrowIfFailure();

        returned.Should().BeSameAs(result);
    }

    [Theory]
    [InlineData("Code1", "Desc1")]
    [InlineData("Test.Code", "Description")]
    public void ThrowIfFailure_Failure_ShouldThrowInvalidOperationException(string code, string desc)
    {
        var error = Error.BadRequest(code, desc);
        var result = Result<Error>.Failure(error);

        Action act = () => result.ThrowIfFailure();

        act.Should().Throw<InvalidOperationException>()
            .WithMessage($"Errors(1): [{code}: {desc}]");
    }

    [Fact]
    public void ThrowIfFailure_Failure500_ShouldThrowWithErrorCodeAndDescription()
    {
        var error = Error.Unexpected("Code", "Desc");
        var result = Result<Error>.Failure(error);

        Action act = () => result.ThrowIfFailure();

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Errors(1): [Code: Desc]");
    }

    #endregion

    #region MapError

    [Fact]
    public void MapError_Success_ShouldReturnSuccessWithNewErrorType()
    {
        var result = Result<Error>.Ok();

        var mapped = result.MapError(e => Error.NotFound(e.Code, e.Description));

        mapped.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void MapError_Failure_ShouldTransformErrors()
    {
        var error = Error.BadRequest("Code", "Desc");
        var result = Result<Error>.Failure(error);

        var mapped = result.MapError(e => Error.NotFound(e.Code, e.Description));

        mapped.IsFailure.Should().BeTrue();
        mapped.Errors.Should().ContainSingle();
        mapped.Errors.First().Code.Should().Be("Code");
        mapped.Errors.First().Status.Should().Be(404);
    }

    [Fact]
    public void MapError_FailureWithMetadata_ShouldPreserveMetadata()
    {
        var error = Error.BadRequest("Code", "Desc");
        var metadata = new Dictionary<string, object?> { ["key"] = "value" };
        var result = Result<Error>.Failure(error).WithResultMeta(metadata);

        var mapped = result.MapError(e => Error.NotFound(e.Code, e.Description));

        mapped.Metadata.Should().ContainKey("key");
    }

    [Fact]
    public void MapError_NullMapper_ShouldThrowArgumentNullException()
    {
        var result = Result<Error>.Ok();

        Action act = () => result.MapError<Error>(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    #endregion

    #region FailIf

    [Fact]
    public void FailIf_Success_PredicateTrue_ShouldReturnFailure()
    {
        var error = Error.BadRequest("Code", "Desc");
        var result = Result<Error>.Ok();

        var failed = result.FailIf(() => true, error);

        failed.IsFailure.Should().BeTrue();
        failed.Errors.Should().ContainSingle().Which.Should().Be(error);
    }

    [Fact]
    public void FailIf_Success_PredicateFalse_ShouldReturnSuccess()
    {
        var error = Error.BadRequest("Code", "Desc");
        var result = Result<Error>.Ok();

        var failed = result.FailIf(() => false, error);

        failed.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void FailIf_Failure_ShouldReturnOriginalFailure()
    {
        var originalError = Error.BadRequest("Orig", "Orig");
        var newError = Error.BadRequest("New", "New");
        var result = Result<Error>.Failure(originalError);

        var failed = result.FailIf(() => true, newError);

        failed.IsFailure.Should().BeTrue();
        failed.Errors.Should().ContainSingle().Which.Should().Be(originalError);
    }

    [Fact]
    public void FailIf_NullPredicate_ShouldThrowArgumentNullException()
    {
        var error = Error.BadRequest("Code", "Desc");
        var result = Result<Error>.Ok();

        Action act = () => result.FailIf(null!, error);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void FailIf_NullError_ShouldThrowArgumentNullException()
    {
        var result = Result<Error>.Ok();

        Action act = () => result.FailIf(() => true, (Error)null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void FailIf_WithFactory_PredicateTrue_ShouldReturnFailure()
    {
        var result = Result<Error>.Ok();

        var failed = result.FailIf(
            () => true,
            () => Error.BadRequest("TooHigh", "Value is too high"));

        failed.IsFailure.Should().BeTrue();
        failed.Errors.First().Code.Should().Be("TooHigh");
    }

    [Fact]
    public void FailIf_WithFactory_NullFactory_ShouldThrowArgumentNullException()
    {
        var result = Result<Error>.Ok();

        Action act = () => result.FailIf(() => true, (Func<Error>)null!);

        act.Should().Throw<ArgumentNullException>();
    }

    #endregion

    #region Ensure

    [Fact]
    public void Ensure_Success_PredicateTrue_ShouldReturnSuccess()
    {
        var result = Result<Error>.Ok();

        var ensured = result.Ensure(() => true, Error.BadRequest("Invalid", "Invalid state"));

        ensured.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Ensure_Success_PredicateFalse_ShouldReturnFailure()
    {
        var error = Error.BadRequest("Invalid", "Invalid state");
        var result = Result<Error>.Ok();

        var ensured = result.Ensure(() => false, error);

        ensured.IsFailure.Should().BeTrue();
        ensured.Errors.Should().ContainSingle().Which.Should().Be(error);
    }

    [Fact]
    public void Ensure_Failure_ShouldReturnOriginalFailure()
    {
        var originalError = Error.BadRequest("Orig", "Orig");
        var result = Result<Error>.Failure(originalError);

        var ensured = result.Ensure(() => true, Error.BadRequest("New", "New"));

        ensured.IsFailure.Should().BeTrue();
        ensured.Errors.Should().ContainSingle().Which.Should().Be(originalError);
    }

    [Fact]
    public void Ensure_WithValidator_Success_ShouldReturnSuccess()
    {
        var result = Result<Error>.Ok();

        var ensured = result.Ensure(() => Result<Error>.Ok());

        ensured.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Ensure_WithValidator_Failure_ShouldReturnValidatorFailure()
    {
        var result = Result<Error>.Ok();

        var ensured = result.Ensure(() => Result<Error>.Failure(Error.BadRequest("Invalid", "Invalid state")));

        ensured.IsFailure.Should().BeTrue();
        ensured.Errors.First().Code.Should().Be("Invalid");
    }

    [Fact]
    public void Ensure_WithValidator_OriginalFailure_ShouldReturnOriginalFailure()
    {
        var originalError = Error.BadRequest("Orig", "Orig");
        var result = Result<Error>.Failure(originalError);

        var ensured = result.Ensure(() => Result<Error>.Failure(Error.BadRequest("New", "New")));

        ensured.IsFailure.Should().BeTrue();
        ensured.Errors.Should().ContainSingle().Which.Should().Be(originalError);
    }

    [Fact]
    public void Ensure_NullPredicate_ShouldThrowArgumentNullException()
    {
        var result = Result<Error>.Ok();

        Action act = () => result.Ensure(null!, Error.BadRequest("Code", "Desc"));

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Ensure_NullError_ShouldThrowArgumentNullException()
    {
        var result = Result<Error>.Ok();

        Action act = () => result.Ensure(() => true, (Error)null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Ensure_NullValidator_ShouldThrowArgumentNullException()
    {
        var result = Result<Error>.Ok();

        Action act = () => result.Ensure(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    #endregion

    #region Switch

    [Fact]
    public void Switch_Success_ShouldExecuteOnSuccess()
    {
        var result = Result<Error>.Ok();
        var successExecuted = false;
        var failureExecuted = false;

        result.Switch(
            () => successExecuted = true,
            _ => failureExecuted = true);

        successExecuted.Should().BeTrue();
        failureExecuted.Should().BeFalse();
    }

    [Fact]
    public void Switch_Failure_ShouldExecuteOnFailure()
    {
        var error = Error.BadRequest("Code", "Desc");
        var result = Result<Error>.Failure(error);
        var successExecuted = false;
        var capturedErrorCount = 0;

        result.Switch(
            () => successExecuted = true,
            errors => capturedErrorCount = errors.Count);

        successExecuted.Should().BeFalse();
        capturedErrorCount.Should().Be(1);
    }

    [Fact]
    public void Switch_NullOnSuccess_ShouldThrowArgumentNullException()
    {
        var result = Result<Error>.Ok();

        Action act = () => result.Switch(null!, _ => { });

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Switch_NullOnFailure_ShouldThrowArgumentNullException()
    {
        var result = Result<Error>.Ok();

        Action act = () => result.Switch(() => { }, null!);

        act.Should().Throw<ArgumentNullException>();
    }

    #endregion

    #region TapError

    [Fact]
    public void TapError_Success_ShouldNotExecuteAction()
    {
        var result = Result<Error>.Ok();
        var executed = false;

        var returned = result.TapError(_ => executed = true);

        executed.Should().BeFalse();
        returned.Should().BeSameAs(result);
    }

    [Fact]
    public void TapError_Failure_ShouldExecuteActionWithFirstError()
    {
        var error = Error.BadRequest("Code", "Desc");
        var result = Result<Error>.Failure(error);
        Error? captured = null;

        var returned = result.TapError(e => captured = e);

        captured.Should().Be(error);
        returned.Should().BeSameAs(result);
    }

    [Fact]
    public void TapError_NullAction_ShouldThrowArgumentNullException()
    {
        var error = Error.BadRequest("Code", "Desc");
        var result = Result<Error>.Failure(error);

        Action act = () => result.TapError(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(2)]
    [InlineData(4)]
    public void TapError_FluentChaining_ShouldWork(int chainCount)
    {
        var tapCount = 0;
        var error = Error.BadRequest("Code", "Desc");
        var result = Result<Error>.Failure(error);

        var returned = result;
        for (int i = 0; i < chainCount; i++)
        {
            returned = returned.TapError(_ => tapCount++);
        }

        tapCount.Should().Be(chainCount);
        returned.Should().BeSameAs(result);
    }

    #endregion
}
