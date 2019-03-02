using System.Drawing;

namespace Elchwinkel.CLI
{
    /// <summary>
    /// A Collection of <see cref="Color"/>s used in the context of a CLI (<see cref="CliBase"/>).
    /// Defines e.g. Colors for the Terminal Background and Colors for Error Messages etc.
    /// Ready made Schemes like <see cref="Dark"/> and <see cref="Light"/> Schemes are provided, but feel free to define your own.
    /// </summary>
    public class ColorScheme
    {
        public static ColorScheme Dark => new ColorScheme()
        {
            Background = Color.Black,
            Color1 = Color.Azure,
            Color2 = Color.CadetBlue,
            Color3 = Color.LightGray,
            Error = Color.Red,
        };
        public static ColorScheme Light => new ColorScheme()
        {
            Background = Color.White,
            Color1 = Color.Black,
            Color2 = Color.CadetBlue,
            Color3 = Color.DarkBlue,
            Error = Color.Red,
        };

        public Color Background { get; set; }
        public Color Color1 { get; set; }
        public Color Color2 { get; set; }
        public Color Color3 { get; set; }
        public Color Error { get; set; }
    }
}