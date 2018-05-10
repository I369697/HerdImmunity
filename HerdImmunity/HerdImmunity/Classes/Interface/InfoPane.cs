using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Immunization.Classes;
using Immunization.Classes.Actors;
using SFML.Graphics;
using SFML.System;

namespace HerdImmunity.Classes
{
    class InfoPane : Drawable
    {
        private readonly Scene watchedScene;

        private readonly RectangleShape background;
        private readonly Text sceneInfo;

        readonly Vertex[] separator =
        {
            new Vertex(new Vector2f(0, Constants.SimulationGFXHeight), Color.Blue),
            new Vertex(new Vector2f(Constants.SimulationGFXWidth, Constants.SimulationGFXHeight), Constants.PaneColor)
        };

        public Person WatchedPerson;
        private Text watchedPersonInfo;
        private Sprite watchedPersonSprite;
        private RectangleShape watchedPersonHealth;
        private RectangleShape watchedPersonHealthOutline;

        public InfoPane(Scene watchedScene)
        {
            this.watchedScene = watchedScene;
            background = new RectangleShape(new Vector2f(Constants.SimulationGFXWidth, Constants.SimulationInfoPaneGFXHeight));
            background.Position = new Vector2f(0, Constants.SimulationGFXHeight);
            background.FillColor = Constants.PaneColor;

            sceneInfo = new Text("...", Constants.Font);
            sceneInfo.Color = Color.Black;
            sceneInfo.CharacterSize = 10;
            sceneInfo.Position = new Vector2f(10, Constants.SimulationGFXHeight+10);

            watchedPersonInfo = new Text("...", Constants.Font);
            watchedPersonInfo.Color = Color.Black;
            watchedPersonInfo.CharacterSize = 10;
            watchedPersonInfo.Position = new Vector2f(10 + (Constants.SimulationGFXWidth / 2), Constants.SimulationGFXHeight + 10);
            watchedPersonSprite = new Sprite();

            watchedPersonHealth = new RectangleShape(new Vector2f(100, 10));
            watchedPersonHealth.FillColor = Color.Green;
            watchedPersonHealth.Position  = new Vector2f(10 + (Constants.SimulationGFXWidth / 2) + 282, Constants.SimulationGFXHeight + 80);

            watchedPersonHealthOutline = new RectangleShape(new Vector2f(100, 10));
            watchedPersonHealthOutline.OutlineColor = Color.Black;
            watchedPersonHealthOutline.OutlineThickness = 2;
            watchedPersonHealthOutline.FillColor = Color.Transparent;
            watchedPersonHealthOutline.Position = new Vector2f(10 + (Constants.SimulationGFXWidth / 2) + 282, Constants.SimulationGFXHeight + 80);
        }

        public void Update()
        {
            sceneInfo.DisplayedString = watchedScene.InfoString();
            if(WatchedPerson!=null)
            {
                watchedPersonInfo.DisplayedString = WatchedPerson.InfoString();
                watchedPersonSprite = new Sprite(WatchedPerson.Sprite.Texture);
                watchedPersonSprite.Position = new Vector2f(10 + (Constants.SimulationGFXWidth / 2) + 300, Constants.SimulationGFXHeight + 10);
                watchedPersonSprite.Scale = new Vector2f(4, 4);

                watchedPersonHealth.Size = new Vector2f((float)WatchedPerson.Health, 10);
            }
            else
            {
                watchedPersonInfo.DisplayedString = "No person selected";
            }
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(separator, PrimitiveType.Lines);
            target.Draw(background);
            target.Draw(sceneInfo);
            target.Draw(watchedPersonInfo);

            if (WatchedPerson!=null)
            { 
              target.Draw(watchedPersonSprite);
              target.Draw(watchedPersonHealth);
              target.Draw(watchedPersonHealthOutline);
            }
        }
    }
}
