namespace BuildingBlocks.SharedKernel.Both;

public partial record Both<TLeft, TRight> : IBoth<TLeft, TRight>
{
    #region Factory Methods

    /// <summary>
    /// Creates a Both with a left value.
    /// </summary>
    /// <param name="value">The left value.</param>
    /// <returns>A Both containing the left value.</returns>
    public static Both<TLeft, TRight> FromLeft(TLeft value)
    {
        return new Both<TLeft, TRight>(value, default, isLeft: true);
    }

    /// <summary>
    /// Creates a Both with a right value.
    /// </summary>
    /// <param name="value">The right value.</param>
    /// <returns>A Both containing the right value.</returns>
    public static Both<TLeft, TRight> FromRight(TRight value)
    {
        return new Both<TLeft, TRight>(default, value, isLeft: false);
    }

    #endregion
}
