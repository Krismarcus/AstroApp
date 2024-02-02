using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroApp.Data.Models
{
    public class MoonDay
    {
        private int newMoonDay;
        private int previousMoonDay;

        public int NewMoonDay
        {
            get => newMoonDay;
            set
            {
                newMoonDay = value;
                if (newMoonDay == 1)
                {
                    previousMoonDay = 30;
                }
                else
                {
                    previousMoonDay = newMoonDay - 1;
                }
            }
        }

        public int PreviousMoonDay
        {
            get => previousMoonDay;
            set => previousMoonDay = value;
        }

        public string MoonDayInfo { get; set; }

        public DateTime TransitionTime { get; set; }
    }
}

