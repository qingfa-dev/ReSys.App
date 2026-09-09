namespace BuildingBlocks.SharedKernel.Both;

public partial record Both<TLeft, TRight> : IBoth<TLeft, TRight>
{
    #region Functional Methods

    /// <summary>
    /// Maps the left value using the specified mapper function.
    /// If this instance contains a right value, it is returned unchanged.
    /// </summary>
    /// <typeparam name="TNewLeft">The type of the new left value.</typeparam>
    /// <param name="mapper">The function to transform the left value.</param>
    /// <returns>A new Both with the mapped left value or propagated right value.</returns>
    public Both<TNewLeft, TRight> MapLeft<TNewLeft>(Func<TLeft, TNewLeft> mapper)
    {
        ArgumentNullException.ThrowIfNull(mapper);

        return IsLeft
            ? Both<TNewLeft, TRight>.FromLeft(mapper(Left))
            : Both<TNewLeft, TRight>.FromRight(Right);
    }

    /// <summary>
    /// Maps the right value using the specified mapper function.
    /// If this instance contains a left value, it is returned unchanged.
    /// </summary>
    /// <typeparam name="TNewRight">The type of the new right value.</typeparam>
    /// <param name="mapper">The function to transform the right value.</param>
    /// <returns>A new Both with the mapped right value or propagated left value.</returns>
    public Both<TLeft, TNewRight> MapRight<TNewRight>(Func<TRight, TNewRight> mapper)
    {
        ArgumentNullException.ThrowIfNull(mapper);

        return IsRight
            ? Both<TLeft, TNewRight>.FromRight(mapper(Right))
            : Both<TLeft, TNewRight>.FromLeft(Left);
    }

    /// <summary>
    /// Chains the left value with a function that produces a new Both.
    /// If this instance contains a right value, it is returned unchanged.
    /// </summary>
    /// <typeparam name="TNewLeft">The type of the new left value.</typeparam>
    /// <param name="binder">The function to produce a new Both from the left value.</param>
    /// <returns>The Both produced by the binder, or the original right value.</returns>
    public Both<TNewLeft, TRight> BindLeft<TNewLeft>(Func<TLeft, Both<TNewLeft, TRight>> binder)
    {
        ArgumentNullException.ThrowIfNull(binder);

        return IsLeft ? binder(Left) : Both<TNewLeft, TRight>.FromRight(Right);
    }

    /// <summary>
    /// Chains the right value with a function that produces a new Both.
    /// If this instance contains a left value, it is returned unchanged.
    /// </summary>
    /// <typeparam name="TNewRight">The type of the new right value.</typeparam>
    /// <param name="binder">The function to produce a new Both from the right value.</param>
    /// <returns>The Both produced by the binder, or the original left value.</returns>
    public Both<TLeft, TNewRight> BindRight<TNewRight>(Func<TRight, Both<TLeft, TNewRight>> binder)
    {
        ArgumentNullException.ThrowIfNull(binder);

        return IsRight ? binder(Right) : Both<TLeft, TNewRight>.FromLeft(Left);
    }

    /// <summary>
    /// Matches the Both to one of two functions: one for left and one for right.
    /// </summary>
    /// <typeparam name="TResult">The return type of the match functions.</typeparam>
    /// <param name="onLeft">The function to invoke when this instance contains a left value.</param>
    /// <param name="onRight">The function to invoke when this instance contains a right value.</param>
    /// <returns>The value produced by the invoked function.</returns>
    public TResult Match<TResult>(Func<TLeft, TResult> onLeft, Func<TRight, TResult> onRight)
    {
        ArgumentNullException.ThrowIfNull(onLeft);
        ArgumentNullException.ThrowIfNull(onRight);

        return IsLeft ? onLeft(Left) : onRight(Right);
    }

    /// <summary>
    /// Executes the specified action if this instance contains a left value.
    /// Returns the original Both to enable fluent chaining.
    /// </summary>
    /// <param name="onLeft">The action to execute with the left value.</param>
    /// <returns>The original <see cref="Both{TLeft, TRight}"/>.</returns>
    public Both<TLeft, TRight> TapLeft(Action<TLeft> onLeft)
    {
        ArgumentNullException.ThrowIfNull(onLeft);

        if (IsLeft)
        {
            onLeft(Left);
        }

        return this;
    }

    /// <summary>
    /// Executes the specified action if this instance contains a right value.
    /// Returns the original Both to enable fluent chaining.
    /// </summary>
    /// <param name="onRight">The action to execute with the right value.</param>
    /// <returns>The original <see cref="Both{TLeft, TRight}"/>.</returns>
    public Both<TLeft, TRight> TapRight(Action<TRight> onRight)
    {
        ArgumentNullException.ThrowIfNull(onRight);

        if (IsRight)
        {
            onRight(Right);
        }

        return this;
    }

    /// <summary>
    /// Returns the left value if this instance contains one, otherwise the specified default value.
    /// </summary>
    /// <param name="defaultValue">The default value to return if this instance contains a right value.</param>
    /// <returns>The left value or the default value.</returns>
    public TLeft GetLeftOrDefault(TLeft defaultValue = default!)
    {
        return IsLeft ? Left : defaultValue;
    }

    /// <summary>
    /// Returns the right value if this instance contains one, otherwise the specified default value.
    /// </summary>
    /// <param name="defaultValue">The default value to return if this instance contains a left value.</param>
    /// <returns>The right value or the default value.</returns>
    public TRight GetRightOrDefault(TRight defaultValue = default!)
    {
        return IsRight ? Right : defaultValue;
    }

    /// <summary>
    /// Returns the left value if this instance contains one, otherwise throws an exception.
    /// </summary>
    /// <returns>The left value.</returns>
    /// <exception cref="InvalidOperationException">Thrown when this instance contains a right value.</exception>
    public TLeft GetLeftOrThrow()
    {
        if (IsRight)
        {
            throw new InvalidOperationException("Both contains a right value, not a left value.");
        }

        return Left;
    }

    /// <summary>
    /// Returns the right value if this instance contains one, otherwise throws an exception.
    /// </summary>
    /// <returns>The right value.</returns>
    /// <exception cref="InvalidOperationException">Thrown when this instance contains a left value.</exception>
    public TRight GetRightOrThrow()
    {
        if (IsLeft)
        {
            throw new InvalidOperationException("Both contains a left value, not a right value.");
        }

        return Right;
    }

    /// <summary>
    /// Swaps the left and right values.
    /// </summary>
    /// <returns>A new Both with swapped values.</returns>
    public Both<TRight, TLeft> Swap()
    {
        return IsLeft
            ? Both<TRight, TLeft>.FromRight(Left)
            : Both<TRight, TLeft>.FromLeft(Right);
    }

    /// <summary>
    /// Executes the appropriate action based on the value type.
    /// Unlike <see cref="Match{TResult}"/>, this method does not return a value.
    /// </summary>
    /// <param name="onLeft">The action to execute with the left value.</param>
    /// <param name="onRight">The action to execute with the right value.</param>
    public void Switch(Action<TLeft> onLeft, Action<TRight> onRight)
    {
        ArgumentNullException.ThrowIfNull(onLeft);
        ArgumentNullException.ThrowIfNull(onRight);

        if (IsLeft)
        {
            onLeft(Left);
        }
        else
        {
            onRight(Right);
        }
    }

    #endregion
}