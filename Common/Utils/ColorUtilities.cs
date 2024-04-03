using System;

namespace Common.Utils
{
    public static class ColorUtilities
    {
        /// <summary>
        /// Convert Hex to Revit Color
        /// </summary>
        public static Autodesk.Revit.DB.Color HexColorToRevitColor(string hexColor)
        {
            hexColor = hexColor.Replace("#", string.Empty);
            Byte red = System.Convert.ToByte(hexColor.Substring(0, 2), 16);
            Byte green = System.Convert.ToByte(hexColor.Substring(2, 2), 16);
            Byte blue = System.Convert.ToByte(hexColor.Substring(4, 2), 16);
            return new Autodesk.Revit.DB.Color(red, green, blue);
        }

        /// <summary>
        /// Convert Revit Color to Hex
        /// </summary>
        public static string RevitColorToHexColor(Autodesk.Revit.DB.Color revitColor)
        {
            return string.Format("#{0:X2}{1:X2}{2:X2}", revitColor.Red, revitColor.Green, revitColor.Blue);
        }
    }
}
