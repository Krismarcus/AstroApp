namespace Astrodaiva.Data.Models
{
    public class EclipseSegment
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsSolar { get; set; }     // SunEclipse
        public bool IsLunar { get; set; }     // MoonEclipse
        public int Duration => (EndDate - StartDate).Days + 1;
    }
}
