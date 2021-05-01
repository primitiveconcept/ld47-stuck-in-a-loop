namespace Spineless
{
    using System.Text;
    using UnityEngine;


    public static class ColorExtensions
    {
        /// <summary>
        /// Convert hex string to a Color object.
        /// </summary>
        /// <param name="rgbaHex">RGB/RGBA hex string to conver to Color.</param>
        /// <returns>Color object from hex string.</returns>
        public static Color ToColor(this string rgbaHex)
        {
            rgbaHex = rgbaHex.Replace("0x", "").Replace("#", "");
            byte a = 255;
            byte r = byte.Parse(rgbaHex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(rgbaHex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(rgbaHex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            if (rgbaHex.Length > 7)
                a = byte.Parse(rgbaHex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
            return new Color32(r, g, b, a);
        }


        /// <summary>
        /// Convert Color to a hex string.
        /// </summary>
        /// <param name="color">Color to convert.</param>
        /// <returns>Color as an RGBA hex string.</returns>
        public static string ToHex(this Color color)
        {
            return new StringBuilder()
                .Append(color.r.ToString("X2"))
                .Append(color.g.ToString("X2"))
                .Append(color.b.ToString("X2"))
                .Append(color.a.ToString("X2"))
                .ToString();
        }
    }
}