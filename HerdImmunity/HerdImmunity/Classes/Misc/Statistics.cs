using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HerdImmunity.Classes.Misc
{
    //Statistics is Singleton
    public sealed class Statistics
    {
        private static readonly Statistics instance = null;
        public long numberOfOrcs { get; set; }
        public long numberOfPeople { get; set; }
        public long deadOrcs { get; set; }
        public long deadPeople { get; set; }
        public long immunodeficient { get; set; }
        public long deadImmunodeficient { get; set; }
        public long numberOfInfections { get; set; }

        private Statistics()
        {
            numberOfOrcs = 0;
            numberOfPeople = 0;
            deadOrcs = 0;
            deadPeople = 0;
            immunodeficient = 0;
            deadImmunodeficient = 0;
            numberOfInfections = 0;
        }

    static Statistics()
        {
            instance = new Statistics();
        }
        public static Statistics GetInstance()
        {
            return instance;
        }
    }
}
