using Immunization.Classes.Actors;
using RandomNameGeneratorLibrary;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HerdImmunity.Classes.Misc;

namespace Immunization.Classes
{
    class Scene
    {
        public List<Person> People;
        private static PersonNameGenerator personGenerator;
        public PopulationProperties SimulatedPopulation;
        public Disease SimulatedDisease;
        public VaccineProperties SimulatedVaccine;
        public String ReportFilename;

        private static int numberOfDays = 30;
        private int passedDays;
        public Scene(PopulationProperties simulatedPopulation, Disease simulatedDisease, VaccineProperties simulatedVaccine)
        {
            SimulatedPopulation = simulatedPopulation;
            SimulatedDisease = simulatedDisease;
            SimulatedVaccine = simulatedVaccine;

            People = new List<Person>();

            personGenerator = new PersonNameGenerator();
            Utility.Repeat(simulatedPopulation.SampleSize, () => GeneratePerson());
            AdministerVaccine();
            InfectFirstPerson();
            passedDays = 0;

            ReportFilename = $"{Constants.ReportFilename}-{DateTime.Now:yyMMddhhmmss}.txt";
        }

        private void GeneratePerson()
        {
            bool isMale = Convert.ToBoolean(Utility.Random(0, 1));
            String name = (isMale ? personGenerator.GenerateRandomMaleFirstAndLastName() : personGenerator.GenerateRandomFemaleFirstAndLastName());
            Double age = Utility.Random(Constants.PersonGenerationMinAge, Constants.PersonGenerationMaxAge);
            bool isVaccinated = Convert.ToBoolean(Utility.Random(0, 1));
            bool isImmunodeficient = Utility.Roll(SimulatedPopulation.Immunodeficiency);
            Double susceptibility = Utility.Random(Constants.PersonGenerationMinSusceptibility, Constants.PersonGenerationMaxSusceptibility);
            List<VaccineProperties> vaccinations = new List<VaccineProperties>();
            var coordinates = GetNewCoordinates();

            var isAnEvilOrc = !Utility.Roll(SimulatedPopulation.VaccinationRate);

            Person p = new Person(name, age, isMale, false, isImmunodeficient, susceptibility, coordinates.Item1, coordinates.Item2, SimulatedDisease, isAnEvilOrc);
            p.Sprite = GenerateAppearance(isMale, isAnEvilOrc);
            p.Label = GenerateLabel(name);

            if (isAnEvilOrc)
            {
                Statistics.GetInstance().numberOfOrcs++;
            } else
            {
                Statistics.GetInstance().numberOfPeople++;
            }
            if (isImmunodeficient)
                Statistics.GetInstance().immunodeficient++;

            People.Add(p);
        }

        private Sprite GenerateAppearance(bool isMale, bool isAnEvilOrc)
        {
            Image spritesheet;

            if (isMale)
            {
                spritesheet = new Image(Constants.PersonGenerationSpritesheetMalePath);
            }
            else
            {
                spritesheet = new Image(Constants.PersonGenerationSpritesheetFemalePath);
            }

            // Body
            int x = 0;
            int y;
            IntRect bodySpritePos;
            if (isAnEvilOrc)
                bodySpritePos = Utility.GetSpriteRect(x, 3);
            else
            {
                y = Utility.Random(0, 2);
                bodySpritePos = Utility.GetSpriteRect(x, y);
            }

            // Chest
            x = Utility.Random(2, 13);
            y = Utility.Random(0, 2);
            var chestSpritePos = Utility.GetSpriteRect(x, y);

            // Legs
            x = 1;
            y = Utility.Random(0, 7);
            var legsSpritePos = Utility.GetSpriteRect(x, y);

            // Head
            x = Utility.Random(2, 5);
            y = Utility.Random(3, 7);
            var headSpritePos = Utility.GetSpriteRect(x, y);

            // Accessory
            x = Utility.Random(6, 13);
            y = Utility.Random(3, 4);

            var accessorySpritePos = Utility.GetSpriteRect(x, y);

            var paperdoll = new Image((uint)Constants.PersonGenerationSpritesheetSpriteSize,
                (uint)Constants.PersonGenerationSpritesheetSpriteSize, Color.Transparent);

            var body = new Texture(spritesheet, bodySpritePos);
            var chest = new Texture(spritesheet, chestSpritePos);
            var legs = new Texture(spritesheet, legsSpritePos);
            var head = new Texture(spritesheet, headSpritePos);
            var accessory = new Texture(spritesheet, accessorySpritePos);

            // Order matters
            paperdoll.Copy(body.CopyToImage(), 0, 0, Constants.SpriteSizeIntRect, true);
            paperdoll.Copy(legs.CopyToImage(), 0, 0, Constants.SpriteSizeIntRect, true);
            paperdoll.Copy(chest.CopyToImage(), 0, 0, Constants.SpriteSizeIntRect, true);
            paperdoll.Copy(head.CopyToImage(), 0, 0, Constants.SpriteSizeIntRect, true);
            paperdoll.Copy(accessory.CopyToImage(), 0, 0, Constants.SpriteSizeIntRect, true);

            var texture = new Texture(paperdoll);
            Sprite sprite = new Sprite(texture);

            return sprite;
        }

        private void InfectFirstPerson()
        {
            foreach (Person p in People)
            {
                if (p.IsAnEvilOrc)
                {
                    p.IsSick = true;
                    return;
                }
            }
        }

        private void AdministerVaccine()
        {
            foreach (var p in People)
            {
                Vaccinate(p);
            }
        }

        private void Vaccinate(Person p)
        {
            // Evil Orcs refuse vaccination!
            if (p.IsAnEvilOrc)
                return;

            // We know that this person is immunodeficient and we do not vaccinate them
            if (p.IsImmunodeficient)
                return;

            // Efficacy chance. This determines whether the vaccine has an effect.
            var efficacy = Utility.Random(SimulatedVaccine.EfficacyRateMin, SimulatedVaccine.EfficacyRateMax);
            if (Utility.Roll(efficacy))
            {
                p.IsVaccinated = true;

                // Add vaccination indication to the sprite
                p.Sprite = Utility.AddToSprite(p.Sprite, Constants.PersonSimulationShieldActiveTexture);
            }
            else
            {
                p.IsVaccinated = false;

                // Add failed vaccination indication to the sprite
                p.Sprite = Utility.AddToSprite(p.Sprite, Constants.PersonSimulationShieldInactiveTexture);
            }

            //Mortality chance. This determines whether the vaccine is fatal to the person.
            var mortality = Utility.GetSusceptibilityWeightedValue(SimulatedVaccine.MortalityRateMin, SimulatedVaccine.MortalityRateMax, p.Susceptibility);
            var isFatal = Utility.Roll(mortality);

            if (isFatal)
            {
                Statistics.GetInstance().deadPeople++;
                p.Kill();
            }
        }

        private Text GenerateLabel(String text)
        {
            Text nameLabel = new Text(text, Constants.Font);
            nameLabel.Color = Color.Black;
            nameLabel.CharacterSize = 10;

            return nameLabel;
        }

        public bool Simulate()
        {
            using (var tw = new StreamWriter(ReportFilename, true))
            {
                    tw.WriteLine("Day: " + Convert.ToString(passedDays + 1));
                foreach (Person p in People)
                {
                    String tickReport = p.Tick();

                    if (p.IsSick && p.SickDays >= SimulatedDisease.IncubationPeriodMin && p.SickDays <= SimulatedDisease.TerminationPeriodMin)
                    {
                        List<Person> closePeople = checkPersonForPeopleInRange(p, ((SimulatedDisease.InfectionRateMax+SimulatedDisease.InfectionRateMin) / 2 / 10) * 3.5 + 5);
                        foreach (Person p2 in closePeople)
                        {
                            String infectionReport = p2.InfectionAttempt();
                            tickReport += " " + infectionReport;
                        }
                    }

                    if (tickReport.Length != 0)
                    {
                        tw.WriteLine(tickReport);
                    }
                }
            }
            passedDays++;

            if (passedDays == numberOfDays)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private List<Person> checkPersonForPeopleInRange(Person center, double range)
        {
            List<Person> closePeople = new List<Person>();
            foreach (Person p in People)
            {
                if (p.X == center.X && p.Y == center.Y)
                {
                    continue;
                }
                else
                {
                    double distance = Utility.GetDistance(p.X, p.Y, center.X, center.Y);
                    if (distance <= range)
                    {
                        closePeople.Add(p);
                    }
                }
            }

            return closePeople;
        }

        private (Double, Double) GetNewCoordinates()
        {
            Double x, y;
            bool collision;
            int i = 0;

            do
            {
                collision = false;

                x = Utility.Random(Constants.SimulationSceneMinX, Constants.SimulationSceneMaxX);
                y = Utility.Random(Constants.SimulationSceneMinY, Constants.SimulationSceneMaxY);

                foreach (Person p in People)
                {
                    if (Utility.GetDistance(x, y, p.X, p.Y) <= Constants.PersonGenerationMinDistance)
                    {
                        collision = true;
                        break;
                    }
                }

                i++;
            } while (collision == true && i < Constants.PersonGenerationMaxCoordinateSearchIterations);

            return (x, y);
        }

        public String InfoString()
        {
            var averageHealth = Math.Round(People.Average(p => p.Health), 2);
            var alive = People.Count(p => p.Health > 0);
            var infected = People.Count(p => p.IsSick);
            var vaccinated = People.Count(p => p.IsVaccinated);
            var immunodeficient = People.Count(p => p.IsImmunodeficient);
            return $"Day {passedDays}\n\nHealth: {averageHealth}%\nAlive: {alive}\nInfected: {infected}\nVaccinated:{vaccinated}\nImmunodeficient:{immunodeficient}";
        }
    }
}
