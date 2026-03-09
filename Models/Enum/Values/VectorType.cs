namespace BHSDK.Models.Enum.Values
{
    public enum VectorType : byte
    {
        /// <summary>
        /// Just vector, X as X, Y as Y
        /// </summary>
        Value = 0,
        
        /// <summary>
        /// Random point in rect, X from (X, X2), Y from (Y, Y2)
        /// </summary>
        RandomRect = 1,
        
        /// <summary>
        /// Random point in rect with grid, X from (X, X2, Step), Y from (Y, Y2, Step)
        /// </summary>
        RandomRectStep = 2,
        
        /// <summary>
        /// Random point in circle, use 
        /// </summary>
        RandomCircle = 3,
    }
}