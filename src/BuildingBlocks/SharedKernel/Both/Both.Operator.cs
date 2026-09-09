namespace BuildingBlocks.SharedKernel.Both;

public partial record Both<TLeft, TRight> : IBoth<TLeft, TRight>
{
    #region Implicit Operators

    /// <summary>
    /// Implicitly converts a left value to a Both.
    /// </summary>
    /// <param name="value">The left value to convert.</param>
    public static implicit operator Both<TLeft, TRight>(TLeft value)
    {
        return FromLeft(value);
    }

    /// <summary>
    /// Implicitly converts a right value to a Both.
    /// </summary>
    /// <param name="value">The right value to convert.</param>
    public static implicit operator Both<TLeft, TRight>(TRight value)
    {
        return FromRight(value);
    }

    #endregion
}