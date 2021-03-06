﻿using System;

namespace AirBench.Api.Models
{
    public class ReviewAddResponse
    {
        public bool Success { get; set; }

        public int Id { get; set; }

        public int BenchId { get; set; }

        public string Description { get; set; }

        public int Rating { get; set; }

        public DateTimeOffset Date { get; set; }

        public int UserId { get; set; }
    }
}