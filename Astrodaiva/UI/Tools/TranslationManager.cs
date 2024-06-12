using Astrodaiva.Data.Enums;
using Astrodaiva.Data.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astrodaiva.UI.Tools
{
    public static class TranslationManager
    {
        public static string TranslateMoonDay(int moonDay)
        {            
            switch (moonDay)
            {
                case 1:
                    return "1 Mėnulio diena - Žibintas";
                case 2:
                    return "2 Mėnulio diena - Banginis";
                case 3:
                    return "3 Mėnulio diena - Leopardas";
                case 4:
                    return "4 Mėnulio diena - Medis";
                case 5:
                    return "5 Mėnulio diena - Vienaragis";
                case 6:
                    return "6 Mėnulio diena - Vaivorykštė";
                case 7:
                    return "7 Mėnulio diena - Gaidys";
                case 8:
                    return "8 Mėnulio diena - Feniksas";
                case 9:
                    return "9 Mėnulio diena - Šikšnosparnis";
                case 10:
                    return "10 Mėnulio diena - Fontanas";
                case 11:
                    return "11 Mėnulio diena - Karūna";
                case 12:
                    return "12 Mėnulio diena - Taurė";
                case 13:
                    return "13 Mėnulio diena - Ratas";
                case 14:
                    return "14 Mėnulio diena - Trimitas";
                case 15:
                    return "15 Mėnulio diena - Gyvatė";
                case 16:
                    return "16 Mėnulio diena - Balandis";
                case 17:
                    return "17 Mėnulio diena - Vynuogė";
                case 18:
                    return "18 Mėnulio diena - Bezdžionė";
                case 19:
                    return "19 Mėnulio diena - Voras";
                case 20:
                    return "20 Mėnulio diena - Erelis";
                case 21:
                    return "21 Mėnulio diena - Arklys";
                case 22:
                    return "22 Mėnulio diena - Dramblys";
                case 23:
                    return "23 Mėnulio diena - Krokodilas";
                case 24:
                    return "24 Mėnulio diena - Meška";
                case 25:
                    return "25 Mėnulio diena - Vėžlys";
                case 26:
                    return "26 Mėnulio diena - Varlė";
                case 27:
                    return "27 Mėnulio diena - Laivas";
                case 28:
                    return "28 Mėnulio diena - Lotosas";
                case 29:
                    return "29 Mėnulio diena - Aštunkojis";
                case 30:
                    return "30 Mėnulio diena - Gulbė";
                default:
                    return "nežinoma diena"; // Default case for unknown or uninitialized values
            }
        }

        public static string TranslatePlanetInZodiac(Planet planet, ZodiacSign zodiac)
        {
            string planetTranslation;
            switch (planet)
            {
                case Planet.Sun:
                    planetTranslation = "Saulė";
                    break;
                case Planet.Moon:
                    planetTranslation = "Mėnulis";
                    break;
                case Planet.Mercury:
                    planetTranslation = "Merkurijus";
                    break;
                case Planet.Venus:
                    planetTranslation = "Venera";
                    break;
                case Planet.Mars:
                    planetTranslation = "Marsas";
                    break;
                case Planet.Jupiter:
                    planetTranslation = "Jupiteris";
                    break;
                case Planet.Saturn:
                    planetTranslation = "Saturnas";
                    break;
                case Planet.Uranus:
                    planetTranslation = "Uranas";
                    break;
                case Planet.Neptune:
                    planetTranslation = "Neptūnas";
                    break;
                case Planet.Pluto:
                    planetTranslation = "Plutonas";
                    break;
                case Planet.Selena:
                    planetTranslation = "Selena";
                    break;
                case Planet.Lilith:
                    planetTranslation = "Lilit";
                    break;
                case Planet.Rahu:
                    planetTranslation = "Rahu";
                    break;
                case Planet.Ketu:
                    planetTranslation = "Ketu";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(planet), planet, null);
            }

            string zodiacTranslation;
            switch (zodiac)
            {
                case ZodiacSign.Aries:
                    zodiacTranslation = "Avine";
                    break;
                case ZodiacSign.Taurus:
                    zodiacTranslation = "Jautyje";
                    break;
                case ZodiacSign.Gemini:
                    zodiacTranslation = "Dvyniuose";
                    break;
                case ZodiacSign.Cancer:
                    zodiacTranslation = "Vėžyje";
                    break;
                case ZodiacSign.Leo:
                    zodiacTranslation = "Liūte";
                    break;
                case ZodiacSign.Virgo:
                    zodiacTranslation = "Mergelėje";
                    break;
                case ZodiacSign.Libra:
                    zodiacTranslation = "Svarstyklėse";
                    break;
                case ZodiacSign.Scorpio:
                    zodiacTranslation = "Skorpione";
                    break;
                case ZodiacSign.Sagittarius:
                    zodiacTranslation = "Šaulyje";
                    break;
                case ZodiacSign.Capricorn:
                    zodiacTranslation = "Ožiaragyje";
                    break;
                case ZodiacSign.Aquarius:
                    zodiacTranslation = "Vandenyje";
                    break;
                case ZodiacSign.Pisces:
                    zodiacTranslation = "Žuvyse";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(zodiac), zodiac, null);
            }

            return $"{planetTranslation} {zodiacTranslation}";
        }

    }
}
