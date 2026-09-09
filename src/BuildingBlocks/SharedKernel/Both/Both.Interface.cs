namespace BuildingBlocks.SharedKernel.Both;

/// <summary>
/// Represents a value that is one of two possible types.
/// Provides a discriminated union for handling one of two possible outcomes.
/// </summary>
/// <typeparam name="TLeft">The type of the left value.</typeparam>
/// <typeparam name="TRight">The type of the right value.</typeparam>
public interface IBoth<out TLeft, out TRight>
{
    /// <summary>
    /// Gets a value indicating whether this instance contains a left value.
    /// </summary>
    bool IsLeft { get; }

    /// <summary>
    /// Gets a value indicating whether this instance contains a right value.
    /// </summary>
    bool IsRight { get; }

    /// <summary>
    /// Gets the left value.
    /// </summary>
    TLeft? Left { get; }

    /// <summary>
    /// Gets the right value.
    /// </summary>
    TRight? Right { get; }
}
