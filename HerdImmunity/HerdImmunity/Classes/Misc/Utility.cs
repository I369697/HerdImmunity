using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace Immunization.Classes
{
    static class Utility
    {
        private static readonly Random randomNumberGenerator = new Random();

        public static Color GetHealthColor(double healthPercentage)
        {
            byte r, g, b;

            if (healthPercentage>=50)
            {
                r = 255;
                g = (byte)((healthPercentage - 50) * 5.1);
                b = g;
            }
            else
            {
                g = 0;
                b = 0;
                r = (byte)(128+healthPercentage * 2.55);
            }
            return new Color(r, g, b);
        }

        public static decimal ToNumericControlValue(string value)
        {
            return Convert.ToDecimal(value, Constants.NFI);
        }

        public static string FromNumericControlValue(decimal value)
        {
            return value.ToString(Constants.NFI);
        }

        public static Double Random(Double min, Double max)
        {
            return randomNumberGenerator.NextDouble() * (max - min) + min;
        }

        public static int Random(int min, int max)
        {
            return randomNumberGenerator.Next(min, max);
        }

        public static bool Roll(double chance)
        {
            var roll = randomNumberGenerator.Next(0, 100);
            return roll <= chance ? true : false;
        }

        public static double GetDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
        }

        public static void Repeat(int count, Action action)
        {
            for (int i = 0; i < count; i++)
            {
                action();
            }
        }

        public static double GetSusceptibilityWeightedValue(double min, double max, double susceptibility)
        {
            var increment = (max - min) / 100;
            return min + (increment * (100 - susceptibility));
        }

        public static IntRect GetSpriteRect(int x, int y)
        {
            var rect = new IntRect(x * Constants.PersonGenerationSpritesheetSpriteSize + x * Constants.PersonGenerationSpritesheetSpriteMargin,
                y * Constants.PersonGenerationSpritesheetSpriteSize + y * Constants.PersonGenerationSpritesheetSpriteMargin,
                Constants.PersonGenerationSpritesheetSpriteSize, Constants.PersonGenerationSpritesheetSpriteSize);
            return rect;
        }

        public static Sprite AddToSprite(Sprite sprite, Texture texture)
        {
            var spriteImage = sprite.Texture.CopyToImage();

            spriteImage.Copy(texture.CopyToImage(), 0, 0, Constants.SpriteSizeIntRect, true);

            Texture newTexture = new Texture(spriteImage);
            Sprite newSprite = new Sprite(newTexture);
            return newSprite;
        }

        public static bool RollWithSusceptibility(double min, double max, double susceptibility)
        {
            return Roll(GetSusceptibilityWeightedValue(min, max, susceptibility));
        }
    }
}
