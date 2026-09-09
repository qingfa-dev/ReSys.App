using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace BuildingBlocks.SharedKernel.Both;

/// <summary>
/// Represents a value that is one of two possible types.
/// Provides a discriminated union for handling one of two possible outcomes.
/// </summary>
/// <typeparam name="TLeft">The type of the left value.</typeparam>
/// <typeparam name="TRight">The type of the right value.</typeparam>
public partial record Both<TLeft, TRight> : IBoth<TLeft, TRight>
{
    private readonly TLeft? _left;
    private readonly TRight? _right;

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="Both{TLeft, TRight}"/> record with a left value.
    /// </summary>
    /// <param name="left">The left value.</param>
    /// <param name="isLeft">Whether this instance contains a left value.</param>
    [JsonConstructor]
    internal Both(TLeft? left, TRight? right, bool isLeft)
    {
        _left = left;
        _right = right;
        IsLeft = isLeft;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets a value indicating whether this instance contains a left value.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Left))]
    [MemberNotNullWhen(false, nameof(Right))]
    public bool IsLeft { get; }

    /// <summary>
    /// Gets a value indicating whether this instance contains a right value.
    /// </summary>
    [JsonIgnore]
    [MemberNotNullWhen(false, nameof(Left))]
    [MemberNotNullWhen(true, nameof(Right))]
    public bool IsRight => !IsLeft;

    /// <summary>
    /// Gets the left value.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when this instance contains a right value.</exception>
    public TLeft Left => IsLeft
        ? _left!
        : throw new InvalidOperationException("Both contains a right value, not a left value.");

    /// <summary>
    /// Gets the right value.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when this instance contains a left value.</exception>
    public TRight Right => IsRight
        ? _right!
        : throw new InvalidOperationException("Both contains a left value, not a right value.");

    #endregion

    #region Overrides

    /// <inheritdoc />
    public override string ToString()
    {
        return IsLeft
            ? $"Left({Left})"
            : $"Right({Right})";
    }

    #endregion
}