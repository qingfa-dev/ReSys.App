namespace BuildingBlocks.UnitTests.SharedKernel.ValueResults;

public class ValueResultFunctionalTest
{
    #region Map

    [Fact]
    public void Map_Success_ShouldTransformValue()
    {
        var result = Result<int, Error>.Ok(5);

        var mapped = result.Map(x => x * 2);

        mapped.IsSuccess.Should().BeTrue();
        mapped.Value.Should().Be(10);
    }

    [Fact]
    public void Map_Failure_ShouldPropagateErrors()
    {
        var error = Error.BadRequest("Code", "Desc");
        var result = Result<int, Error>.Failure(error);

        var mapped = result.Map(x => x * 2);

        mapped.IsSuccess.Should().BeFalse();
        mapped.Errors.Should().BeEquivalentTo(new[] { error });
    }

    [Fact]
    public void Map_WithMetadata_ShouldPreserveMetadata()
    {
        var metadata = new Dictionary<string, object?> { ["key"] = "value" };
        var result = Result<int, Error>.Ok(5).WithResultMeta(metadata);

        var mapped = result.Map(x => x * 2);

        mapped.Metadata.Should().ContainKey("key");
    }

    [Fact]
    public void Map_FailureWithMetadata_ShouldPreserveMetadata()
    {
        var error = Error.BadRequest("Code", "Desc");
        var metadata = new Dictionary<string, object?> { ["key"] = "value" };
        var result = Result<int, Error>.Failure(error).WithResultMeta(metadata);

        var mapped = result.Map(x => x * 2);

        mapped.Metadata.Should().ContainKey("key");
    }

    [Fact]
    public void Map_NullMapper_ShouldThrowArgumentNullException()
    {
        var result = Result<int, Error>.Ok(5);

        Action act = () => result.Map<int>(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion

    #region Bind

    [Fact]
    public void Bind_Success_ShouldChainOperation()
    {
        var result = Result<int, Error>.Ok(5);

        var bound = result.Bind(x => Result<string, Error>.Ok(x.ToString()));

        bound.IsSuccess.Should().BeTrue();
        bound.Value.Should().Be("5");
    }

    [Fact]
    public void Bind_Success_ShouldReturnFailureFromBinder()
    {
        var result = Result<int, Error>.Ok(5);
        var error = Error.BadRequest("Code", "Desc");

        var bound = result.Bind<int>(_ => Result<int, Error>.Failure(error));

        bound.IsSuccess.Should().BeFalse();
        bound.Errors.Should().BeEquivalentTo(new[] { error });
    }

    [Fact]
    public void Bind_Failure_ShouldPropagateErrors()
    {
        var error = Error.BadRequest("Code", "Desc");
        var result = Result<int, Error>.Failure(error);

        var bound = result.Bind(x => Result<string, Error>.Ok(x.ToString()));

        bound.IsSuccess.Should().BeFalse();
        bound.Errors.Should().BeEquivalentTo(new[] { error });
    }

    [Fact]
    public void Bind_FailureWithMetadata_ShouldPreserveMetadata()
    {
        var error = Error.BadRequest("Code", "Desc");
        var metadata = new Dictionary<string, object?> { ["key"] = "value" };
        var result = Result<int, Error>.Failure(error).WithResultMeta(metadata);

        var bound = result.Bind(x => Result<string, Error>.Ok(x.ToString()));

        bound.Metadata.Should().ContainKey("key");
    }

    [Fact]
    public void Bind_NullBinder_ShouldThrowArgumentNullException()
    {
        var result = Result<int, Error>.Ok(5);

        Action act = () => result.Bind<int>(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion

    #region Match

    [Theory]
    [InlineData(5, "5")]
    [InlineData(10, "10")]
    public void Match_Success_ShouldInvokeOnSuccess(int input, string expected)
    {
        var result = Result<int, Error>.Ok(input);

        var matched = result.Match(
            onSuccess: x => x.ToString(),
            onFailure: _ => "failure");

        matched.Should().Be(expected);
    }

    [Fact]
    public void Match_Failure_ShouldInvokeOnFailure()
    {
        var error = Error.BadRequest("Code", "Desc");
        var result = Result<int, Error>.Failure(error);

        var matched = result.Match(
            onSuccess: x => x,
            onFailure: errors => errors.Count);

        matched.Should().Be(1);
    }

    [Fact]
    public void Match_NullOnSuccess_ShouldThrowArgumentNullException()
    {
        var result = Result<int, Error>.Ok(5);

        Action act = () => result.Match<string>(
            onSuccess: null!,
            onFailure: _ => "failure");

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Match_NullOnFailure_ShouldThrowArgumentNullException()
    {
        var result = Result<int, Error>.Ok(5);

        Action act = () => result.Match<string>(
            onSuccess: x => x.ToString(),
            onFailure: null!);

        act.Should().Throw<ArgumentNullException>();
    }

    #endregion

    #region Tap

    [Fact]
    public void Tap_Success_ShouldExecuteActionWithValue()
    {
        var result = Result<int, Error>.Ok(5);
        int captured = 0;

        var returned = result.Tap(x => captured = x);

        captured.Should().Be(5);
        returned.Should().BeSameAs(result);
    }

    [Fact]
    public void Tap_Failure_ShouldNotExecuteAction()
    {
        var error = Error.BadRequest("Code", "Desc");
        var result = Result<int, Error>.Failure(error);
        var executed = false;

        var returned = result.Tap(_ => executed = true);

        executed.Should().BeFalse();
        returned.Should().BeSameAs(result);
    }

    [Fact]
    public void Tap_NullAction_ShouldThrowArgumentNullException()
    {
        var result = Result<int, Error>.Ok(5);

        Action act = () => result.Tap(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(3)]
    [InlineData(5)]
    public void Tap_FluentChaining_ShouldWork(int chainCount)
    {
        var tapCount = 0;
        var result = Result<int, Error>.Ok(5);

        var returned = result;
        for (int i = 0; i < chainCount; i++)
        {
            returned = returned.Tap(_ => tapCount++);
        }

        tapCount.Should().Be(chainCount);
        returned.Should().BeSameAs(result);
    }

    #endregion

    #region TapError

    [Fact]
    public void TapError_Success_ShouldNotExecuteAction()
    {
        var result = Result<int, Error>.Ok(5);
        var executed = false;

        var returned = result.TapErrors(_ => executed = true);

        executed.Should().BeFalse();
        returned.Should().BeSameAs(result);
    }

    [Fact]
    public void TapError_Failure_ShouldExecuteActionWithFirstError()
    {
        var error = Error.BadRequest("Code", "Desc");
        var result = Result<int, Error>.Failure(error);
        List<Error>? captured = null;

        var returned = result.TapErrors(e => captured = e);

        captured.Should().ContainSingle().Which.Should().Be(error);
        returned.Should().BeSameAs(result);
    }

    [Fact]
    public void TapError_NullAction_ShouldThrowArgumentNullException()
    {
        var error = Error.BadRequest("Code", "Desc");
        var result = Result<int, Error>.Failure(error);

        Action act = () => result.TapErrors(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion

    #region GetValueOrDefault

    [Fact]
    public void GetValueOrDefault_Success_ShouldReturnValue()
    {
        var result = Result<int, Error>.Ok(5);

        result.GetValueOrDefault().Should().Be(5);
    }

    [Fact]
    public void GetValueOrDefault_Failure_ShouldReturnDefault()
    {
        var error = Error.BadRequest("Code", "Desc");
        var result = Result<int, Error>.Failure(error);

        result.GetValueOrDefault().Should().Be(0);
    }

    [Fact]
    public void GetValueOrDefault_Failure_WithCustomDefault_ShouldReturnCustomDefault()
    {
        var error = Error.BadRequest("Code", "Desc");
        var result = Result<int, Error>.Failure(error);

        result.GetValueOrDefault(42).Should().Be(42);
    }

    #endregion

    #region GetValueOrThrow

    [Fact]
    public void GetValueOrThrow_Success_ShouldReturnValue()
    {
        var result = Result<int, Error>.Ok(5);

        result.GetValueOrThrow().Should().Be(5);
    }

    [Fact]
    public void GetValueOrThrow_Failure_ShouldThrowInvalidOperationException()
    {
        var error = Error.BadRequest("Test.Code", "Description");
        var result = Result<int, Error>.Failure(error);

        Action act = () => result.GetValueOrThrow();

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Errors(1): [Test.Code: Description]");
    }

    #endregion

    #region ThrowIfFailure

    [Fact]
    public void ThrowIfFailure_Success_ShouldReturnSameResult()
    {
        var result = Result<int, Error>.Ok(5);

        var returned = result.ThrowIfFailure();

        returned.Should().BeSameAs(result);
    }

    [Fact]
    public void ThrowIfFailure_Failure_ShouldThrowInvalidOperationException()
    {
        var error = Error.BadRequest("Test.Code", "Description");
        var result = Result<int, Error>.Failure(error);

        Action act = () => result.ThrowIfFailure();

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Errors(1): [Test.Code: Description]");
    }

    #endregion


    #region MapError

    [Fact]
    public void MapError_Success_ShouldPropagateValue()
    {
        var result = Result<int, Error>.Ok(5);

        var mapped = result.MapError(e => Error.NotFound(e.Code, e.Description));

        mapped.IsSuccess.Should().BeTrue();
        mapped.Value.Should().Be(5);
    }

    [Fact]
    public void MapError_Failure_ShouldTransformError()
    {
        var error = Error.BadRequest("Code", "Desc");
        var result = Result<int, Error>.Failure(error);

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
        var result = Result<int, Error>.Failure(error).WithResultMeta(metadata);

        var mapped = result.MapError(e => Error.NotFound(e.Code, e.Description));

        mapped.Metadata.Should().ContainKey("key");
    }

    [Fact]
    public void MapError_NullMapper_ShouldThrowArgumentNullException()
    {
        var result = Result<int, Error>.Ok(5);

        Action act = () => result.MapError<Error>(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    #endregion

    #region FailIf

    [Fact]
    public void FailIf_Success_PredicateTrue_ShouldReturnFailure()
    {
        var error = Error.BadRequest("Code", "Desc");
        var result = Result<int, Error>.Ok(5);

        var failed = result.FailIf(x => x > 3, error);

        failed.IsFailure.Should().BeTrue();
        failed.Errors.Should().ContainSingle().Which.Should().Be(error);
    }

    [Fact]
    public void FailIf_Success_PredicateFalse_ShouldReturnSuccess()
    {
        var error = Error.BadRequest("Code", "Desc");
        var result = Result<int, Error>.Ok(5);

        var failed = result.FailIf(x => x > 10, error);

        failed.IsSuccess.Should().BeTrue();
        failed.Value.Should().Be(5);
    }

    [Fact]
    public void FailIf_Failure_ShouldReturnOriginalFailure()
    {
        var originalError = Error.BadRequest("Orig", "Orig");
        var newError = Error.BadRequest("New", "New");
        var result = Result<int, Error>.Failure(originalError);

        var failed = result.FailIf(_ => true, newError);

        failed.IsFailure.Should().BeTrue();
        failed.Errors.Should().ContainSingle().Which.Should().Be(originalError);
    }

    [Fact]
    public void FailIf_NullPredicate_ShouldThrowArgumentNullException()
    {
        var error = Error.BadRequest("Code", "Desc");
        var result = Result<int, Error>.Ok(5);

        Action act = () => result.FailIf(null!, error);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void FailIf_NullError_ShouldThrowArgumentNullException()
    {
        var result = Result<int, Error>.Ok(5);

        Action act = () => result.FailIf(_ => true, (Error)null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void FailIf_WithFactory_PredicateTrue_ShouldReturnFailure()
    {
        var result = Result<int, Error>.Ok(5);

        var failed = result.FailIf(
            x => x > 3,
            x => Error.BadRequest("TooHigh", $"Value {x} is too high"));

        failed.IsFailure.Should().BeTrue();
        failed.Errors.First().Code.Should().Be("TooHigh");
    }

    [Fact]
    public void FailIf_WithFactory_NullFactory_ShouldThrowArgumentNullException()
    {
        var result = Result<int, Error>.Ok(5);

        Action act = () => result.FailIf(_ => true, (Func<int, Error>)null!);

        act.Should().Throw<ArgumentNullException>();
    }

    #endregion

    #region Ensure

    [Fact]
    public void Ensure_Success_PredicateTrue_ShouldReturnSuccess()
    {
        var result = Result<int, Error>.Ok(5);

        var ensured = result.Ensure(x => x > 0, Error.BadRequest("Invalid", "Must be positive"));

        ensured.IsSuccess.Should().BeTrue();
        ensured.Value.Should().Be(5);
    }

    [Fact]
    public void Ensure_Success_PredicateFalse_ShouldReturnFailure()
    {
        var error = Error.BadRequest("Invalid", "Must be positive");
        var result = Result<int, Error>.Ok(-1);

        var ensured = result.Ensure(x => x > 0, error);

        ensured.IsFailure.Should().BeTrue();
        ensured.Errors.Should().ContainSingle().Which.Should().Be(error);
    }

    [Fact]
    public void Ensure_Failure_ShouldReturnOriginalFailure()
    {
        var originalError = Error.BadRequest("Orig", "Orig");
        var result = Result<int, Error>.Failure(originalError);

        var ensured = result.Ensure(_ => true, Error.BadRequest("New", "New"));

        ensured.IsFailure.Should().BeTrue();
        ensured.Errors.Should().ContainSingle().Which.Should().Be(originalError);
    }

    [Fact]
    public void Ensure_WithValidator_Success_ShouldReturnSuccess()
    {
        var result = Result<int, Error>.Ok(5);

        var ensured = result.Ensure(x =>
            x > 0
                ? Result<int, Error>.Ok(x)
                : Result<int, Error>.Failure(Error.BadRequest("Invalid", "Must be positive")));

        ensured.IsSuccess.Should().BeTrue();
        ensured.Value.Should().Be(5);
    }

    [Fact]
    public void Ensure_WithValidator_Failure_ShouldReturnValidatorFailure()
    {
        var result = Result<int, Error>.Ok(-1);

        var ensured = result.Ensure(x =>
            x > 0
                ? Result<int, Error>.Ok(x)
                : Result<int, Error>.Failure(Error.BadRequest("Invalid", "Must be positive")));

        ensured.IsFailure.Should().BeTrue();
        ensured.Errors.First().Code.Should().Be("Invalid");
    }

    [Fact]
    public void Ensure_WithValidator_OriginalFailure_ShouldReturnOriginalFailure()
    {
        var originalError = Error.BadRequest("Orig", "Orig");
        var result = Result<int, Error>.Failure(originalError);

        var ensured = result.Ensure(x =>
            x > 0
                ? Result<int, Error>.Ok(x)
                : Result<int, Error>.Failure(Error.BadRequest("Invalid", "Must be positive")));

        ensured.IsFailure.Should().BeTrue();
        ensured.Errors.Should().ContainSingle().Which.Should().Be(originalError);
    }

    [Fact]
    public void Ensure_NullPredicate_ShouldThrowArgumentNullException()
    {
        var result = Result<int, Error>.Ok(5);

        Action act = () => result.Ensure(null!, Error.BadRequest("Code", "Desc"));

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Ensure_NullError_ShouldThrowArgumentNullException()
    {
        var result = Result<int, Error>.Ok(5);

        Action act = () => result.Ensure(_ => true, (Error)null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Ensure_NullValidator_ShouldThrowArgumentNullException()
    {
        var result = Result<int, Error>.Ok(5);

        Action act = () => result.Ensure(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    #endregion

    #region Switch

    [Fact]
    public void Switch_Success_ShouldExecuteOnSuccess()
    {
        var result = Result<int, Error>.Ok(5);
        int captured = 0;
        var failureExecuted = false;

        result.Switch(
            x => captured = x,
            _ => failureExecuted = true);

        captured.Should().Be(5);
        failureExecuted.Should().BeFalse();
    }

    [Fact]
    public void Switch_Failure_ShouldExecuteOnFailure()
    {
        var error = Error.BadRequest("Code", "Desc");
        var result = Result<int, Error>.Failure(error);
        var successExecuted = false;
        var capturedErrorCount = 0;

        result.Switch(
            _ => successExecuted = true,
            errors => capturedErrorCount = errors.Count);

        successExecuted.Should().BeFalse();
        capturedErrorCount.Should().Be(1);
    }

    [Fact]
    public void Switch_NullOnSuccess_ShouldThrowArgumentNullException()
    {
        var result = Result<int, Error>.Ok(5);

        Action act = () => result.Switch(null!, _ => { });

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Switch_NullOnFailure_ShouldThrowArgumentNullException()
    {
        var result = Result<int, Error>.Ok(5);

        Action act = () => result.Switch(_ => { }, null!);

        act.Should().Throw<ArgumentNullException>();
    }

    #endregion
}