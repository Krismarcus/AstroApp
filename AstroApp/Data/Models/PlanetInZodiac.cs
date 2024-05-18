using AstroApp.Data.Enums;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AstroApp.Data.Models
{
    public partial class PlanetInZodiac : ObservableObject
    {
        [ObservableProperty]
        private Planet planet;

        private ZodiacSign newZodiacSign;
        private ZodiacSign previousZodiacSign;

        public ZodiacSign NewZodiacSign
        {
            get => newZodiacSign;
            set
            {
                newZodiacSign = value;
                UpdatePreviousZodiacSign();
            }
        }

        public ZodiacSign PreviousZodiacSign
        {
            get => previousZodiacSign;
            private set => previousZodiacSign = value;
        }

        public void UpdatePreviousZodiacSign()
        {
            if (isRetrograde)
            {
                if (newZodiacSign == ZodiacSign.Pisces)
                {
                    previousZodiacSign = ZodiacSign.Aries;
                }
                else
                {
                    previousZodiacSign = (ZodiacSign)(newZodiacSign + 1);
                }
            }
            else
            {
                if (newZodiacSign == ZodiacSign.Aries)
                {
                    previousZodiacSign = ZodiacSign.Pisces;
                }
                else
                {
                    previousZodiacSign = (ZodiacSign)(newZodiacSign - 1);
                }
            }
        }

        [ObservableProperty]
        private bool isZodiacTransitioning;

        [ObservableProperty]
        private DateTime transitionTime;

        [ObservableProperty]
        private string planetInZodiacInfo;

        [ObservableProperty]
        private bool isRetrograde;
    }
}
