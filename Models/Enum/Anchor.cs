namespace BHSDK.Models.Enum
{
    /// <summary>
    /// The location from which part of the screen the position is calculated.
    /// This is necessary for local and global positions, as well as since
    /// phone screens are different, and therefore a tool
    /// is needed to better customize the position of objects
    /// </summary>
    public enum Anchor
    {
        Undefined = 0,
        Left_Top = 1,    Center_Top = 2,    Right_Top = 3,
        Left_Middle = 4, Center_Middle = 5, Right_Middle = 6,
        Left_Bottom = 7, Center_Bottom = 8, Right_Bottom = 9,
    }
}