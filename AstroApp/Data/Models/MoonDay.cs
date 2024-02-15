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
        private int middleMoonDay;
        private int previousMoonDay;
        public bool IsTripleMoonDay
        {
            get;
            set;
        }

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
                
                AdjustDaysForTripleMoonDay();
            }
        }

        public int MiddleMoonDay
        {
            get => middleMoonDay;
            private set => middleMoonDay = value;
        }

        public int PreviousMoonDay
        {
            get => previousMoonDay;
            private set => previousMoonDay = value;
        }

        public string PreviousMoonDayInfo { get; set; }
        public string MiddleMoonDayInfo { get; set; }
        public string NewMoonDayInfo { get; set; }

        public DateTime TransitionTime { get; set; }
        public DateTime MiddleMoonDayTransitionTime { get; set; }

        private void AdjustDaysForTripleMoonDay()
        {
            // Check if it is a Triple Moon Day to adjust the moon days accordingly
            if (IsTripleMoonDay)
            {
                // Set middleMoonDay as the current previousMoonDay
                MiddleMoonDay = PreviousMoonDay;

                // Adjust previousMoonDay to be newMoonDay - 2, considering edge cases
                if (NewMoonDay == 1)
                {
                    PreviousMoonDay = 29; // Assuming a 30-day cycle
                }
                else if (NewMoonDay == 2)
                {
                    PreviousMoonDay = 30; // Assuming a 30-day cycle
                }
                else
                {
                    PreviousMoonDay = NewMoonDay - 2;
                }
            }
        }
    }
}

