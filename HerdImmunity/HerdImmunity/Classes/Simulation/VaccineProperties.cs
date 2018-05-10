using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Immunization.Classes
{
    class VaccineProperties
    {
        public double EfficacyRateMin;
        public double EfficacyRateMax;
        public double MortalityRateMin;
        public double MortalityRateMax;

        public VaccineProperties(double efficacyRateMin, double efficacyRateMax, double mortalityRateMin, double mortalityRateMax)
        {
            EfficacyRateMin = efficacyRateMin;
            EfficacyRateMax = efficacyRateMax;
            MortalityRateMin = mortalityRateMin;
            MortalityRateMax = mortalityRateMax;
        }
    }
}
