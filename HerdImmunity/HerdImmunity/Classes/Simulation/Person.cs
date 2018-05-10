using SFML.Graphics;
using System;
using HerdImmunity.Classes.Misc;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Immunization.Classes.Actors
{
    class Person : Drawable
    {
        public string Name;
        public double Age;
        public bool IsMale;
        public bool IsVaccinated;
        public bool IsImmunodeficient;
        public double Susceptibility;
        public double Health = 100.0;
        public Disease Disease;
        public bool IsSick = false;
        public int SickDays = 0;
        public bool IsAnEvilOrc;

        public double X;
        public double Y;

        public Sprite Sprite;
        public Text Label;

        public Person(string name, double age, bool isMale, bool isVaccinated, bool isImmunodeficient, double susceptibility, double x, double y, Disease assignedDisease, bool isAnEvilOrc)
        {
            Name = name;
            Age = age;
            IsMale = isMale;
            IsVaccinated = isVaccinated;
            IsImmunodeficient = isImmunodeficient;
            Susceptibility = susceptibility;
            X = x;
            Y = y;
            Disease = assignedDisease;
            IsAnEvilOrc = isAnEvilOrc;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            if (this.Health > 0)
            {
                Sprite.Color = Utility.GetHealthColor(this.Health);
            }
            else
            {
                Sprite.Color = Color.White;
            }
            target.Draw(Sprite, states);
            target.Draw(Label, states);

        }

        public string Tick()
        {
            string report = "";
            if (IsSick)
            {
                SickDays++;

                if (this.Health > 0)
                {
                    double damage = Math.Round(Utility.GetSusceptibilityWeightedValue(Disease.MortalityRateMin, Disease.MortalityRateMax, Susceptibility)/
                                               Utility.GetSusceptibilityWeightedValue(Disease.TerminationPeriodMin, Disease.TerminationPeriodMax, Susceptibility), 2);
                    this.Health -= damage;
                    if (damage < 0.0)
                    {
                        report += Name + " healed " + Convert.ToString(damage * (-1)) + " damage. \n";
                    }
                    else
                    {
                        report += Name + " took " + Convert.ToString(damage) + " damage. \n";
                    }

                    if (this.Health <= 1)
                    {
                        Health = 0;
                        this.onDeath();
                        report += Name + " died! \n";

                        if (IsAnEvilOrc)
                        {
                            Statistics.GetInstance().deadOrcs++;
                        } else
                        {
                            Statistics.GetInstance().deadPeople++;
                        }
                        if (IsImmunodeficient)
                            Statistics.GetInstance().deadImmunodeficient++;
                    }
                }
            }

            return report;
        }

        public string InfectionAttempt()
        {
            string report = Name + " is being infected";

            if(this.IsVaccinated)
            {
                report += ", but they're vaccinated.\r\n";
            }
            else if(IsImmunodeficient)
            {
                IsSick = true;
                Statistics.GetInstance().numberOfInfections++;
                report += " and they got sick and became infectious!\r\n";
            }
            else if (Utility.RollWithSusceptibility(Disease.InfectionRateMin, Disease.InfectionRateMax, Susceptibility))
            {
                report += ", but they resisted getting sick and becoming infectious.\r\n";
            }
            else
            {
                IsSick = true;
                Statistics.GetInstance().numberOfInfections++;
                report += " and they got sick and became infectious!\r\n";
            }

            return report;
        }

        public void Kill()
        {
            this.Health = 0;
            onDeath();
        }

        private void onDeath()
        {
            var newSprite = new Sprite(Constants.PersonSimulationDeadTexture)
            {
                Position = Sprite.Position,
                Origin = Sprite.Origin
            };

            Sprite = newSprite;
        }

        public string InfoString()
        {
            return $"Name: {Name}\n\nHealth: {Health}%\nAlive: {(Health == 0 ? "No" : "Yes")}\nInfected: {(Disease == null ? "No" : "Yes")}" +
                   $"\nVaccinated: {(IsVaccinated ? "Yes" : "No")}\nImmunodeficient: {(IsImmunodeficient ? "Yes" : "No")}";
        }
    }
}
