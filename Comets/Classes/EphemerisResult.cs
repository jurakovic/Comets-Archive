﻿
namespace Comets.Classes
{
    public class EphemerisResult
    {
        public double LocalJD { get; set; }
        public double UtcJD { get; set; }
        public double RA { get; set; }
        public double Dec { get; set; }
        public double Alt { get; set; }
        public double Az { get; set; }
        public double EcLon { get; set; }
        public double EcLat { get; set; }
        public double Elongation { get; set; }
        public double PositionAngle { get; set; }
        public double HelioDist { get; set; }
        public double GeoDist { get; set; }
        public double Magnitude { get; set; }

        public EphemerisResult()
        {

        }
    }
}
