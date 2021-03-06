﻿namespace AirBench.Api.Models
{
    public class ShortBenchInfo
    {
        public int Id { get; set; }
        
        public string Description { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }

        public int NumberSeats { get; set; }

        public double? AverageRating { get; set; }

        public string AddedBy { get; set; }
    }
}