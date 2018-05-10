using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Immunization.Classes
{
    class PopulationProperties
    {
        public int SampleSize;
        public double VaccinationRate;
        public double Immunodeficiency;

        public PopulationProperties(int sampleSize, double vaccinationRate, double immunodeficiency)
        {
            SampleSize = sampleSize;
            VaccinationRate = vaccinationRate;
            Immunodeficiency = immunodeficiency;
        }
    }
}
