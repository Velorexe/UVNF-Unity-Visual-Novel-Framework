using UnityEngine;

public static class Color32Extensions
{
    #region UDSF Colors
    public static Color32 Audio(this Color32 color)
    {
        color.r = 0xC2;
        color.g = 0xEA;
        color.b = 0xB9;
        color.a = 0xFF;
        return color;
    }

    public static Color32 Character(this Color32 color)
    {
        color.r = 0xFE;
        color.g = 0xEC;
        color.b = 0xCE;
        color.a = 0xFF;
        return color;
    }

    public static Color32 Other(this Color32 color)
    {
        color.r = 0xB7;
        color.g = 0xB7;
        color.b = 0xB7;
        color.a = 0xFF;
        return color;
    }

    public static Color32 Scene(this Color32 color)
    {
        color.r = 0xFF;
        color.g = 0xF0;
        color.b = 0xAA;
        color.a = 0xFF;
        return color;
    }

    public static Color32 Story(this Color32 color)
    {
        color.r = 0xFE;
        color.g = 0xC4;
        color.b = 0xC4;
        color.a = 0xFF;
        return color;
    }

    public static Color32 Utility(this Color32 color)
    {
        color.r = 0xB3;
        color.g = 0xBD;
        color.b = 0xED;
        color.a = 0xFF;
        return color;
    }
    #endregion
}
