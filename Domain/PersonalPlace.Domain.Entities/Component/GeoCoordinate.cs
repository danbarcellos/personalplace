using System;

namespace PersonalPlace.Domain.Entities.Component
{
    public class GeoCoordinate
    {
        protected GeoCoordinate() { }

        public GeoCoordinate(double latitude, double longitude)
        {
            if (!(longitude > -180) || !(longitude < 180) || !(latitude > -90) || !(latitude < 90))
                throw new ArgumentOutOfRangeException();

            Latitude = latitude;
            Longitude = longitude;
        }

        public virtual double Latitude { get; protected set; }

        public virtual double Longitude { get; protected set; }

        public override string ToString()
        {
            return $"{Latitude},{Longitude}";
        }
    }
}