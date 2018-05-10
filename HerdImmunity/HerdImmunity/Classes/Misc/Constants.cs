using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SFML.System;

namespace Immunization.Classes
{
    public static class Constants
    {
        public static NumberFormatInfo NFI { get; } = new NumberFormatInfo();

        public static uint SimulationGFXWidth => 800;
        public static uint SimulationGFXHeight => 600;
        public static uint SimulationInfoPaneGFXHeight => 100;
        public static IntRect SimulationGFXIntRect { get; } = new IntRect(new Vector2i(0, 0), new Vector2i((int)Constants.SimulationGFXWidth, (int)Constants.SimulationGFXHeight));
        public static String SimulationGFXFontPath => "Resources/FiraSans.otf";

        public static int SimulationTickInterval => 500;

        public static double SimulationSceneMinX => 0;
        public static double SimulationSceneMinY => 0;
        public static double SimulationSceneMaxX => 100;
        public static double SimulationSceneMaxY => 100;

        public static double PersonGenerationMinAge => 6;
        public static double PersonGenerationMaxAge => 60;
        public static double PersonGenerationMinSusceptibility => 0;
        public static double PersonGenerationMaxSusceptibility => 100;
        public static double PersonGenerationMinDistance => 2;
        public static double PersonGenerationMaxCoordinateSearchIterations => 10;

        public static String PersonGenerationSpritesheetFemalePath => "Resources/female.png";
        public static String PersonGenerationSpritesheetMalePath => "Resources/male.png";
        public static int PersonGenerationSpritesheetSpriteSize => 16;
        public static int PersonGenerationSpritesheetSpriteMargin => 1;

        public static IntRect SpriteSizeIntRect { get; } = new IntRect(0, 0, Constants.PersonGenerationSpritesheetSpriteSize, Constants.PersonGenerationSpritesheetSpriteSize);

        public static Font Font { get; } = new Font(SimulationGFXFontPath);

        public static String INIPopulationsPath => "Populations";
        public static String INIDiseasesPath => "Diseases";

        public static String MiscSpritesheetPath => "Resources/misc.png";
        private static Image MiscSpritesheetImage { get; } = new Image(Constants.MiscSpritesheetPath);

        public static Texture PersonSimulationDeadTexture { get; } = new Texture(MiscSpritesheetImage, Utility.GetSpriteRect(0, 0));
        public static Texture PersonSimulationShieldActiveTexture { get; } = new Texture(MiscSpritesheetImage, Utility.GetSpriteRect(1, 0));
        public static Texture PersonSimulationShieldInactiveTexture { get; } = new Texture(MiscSpritesheetImage, Utility.GetSpriteRect(2, 0));

        public static Color PaneColor { get; } = new Color(250, 240, 210);

        public static string ReportFilename => "SimulationReport";

        static Constants()
        {
            NFI.NumberDecimalSeparator = ".";
        }
    }
}
