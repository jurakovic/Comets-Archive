﻿
namespace Comets.Classes
{
    public class Location
    {
        #region Properties

        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Altitude { get; set; }
        public int Timezone { get; set; }
        public bool DST { get; set; }

        #endregion

        #region Constructor

        public Location()
        {
            Name = "Home";
            Latitude = 0.0;
            Longitude = 0.0;
            Altitude = 0;
            Timezone = 0;
            DST = false;
        }

        #endregion
    }
}