﻿using AirBench.Api.Models;
using AirBench.Api.Repositories;
using AirBench.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace AirBench.Api.Controllers
{
    public class BenchController : ApiController
    {
        private IBenchApiRepository _benchRepo;

        public BenchController(IBenchApiRepository benchRepo)
        {
            _benchRepo = benchRepo;
        }

        /// <summary>
        /// Attempts to add a bench.
        /// 
        /// ROUTE:
        /// "api/bench"
        /// 
        /// REQUEST BODY:
        /// {
        ///     "Description": `string`,
        ///     "Latitude": `float`,
        ///     "Longitude": `float`,
        ///     "NumberSeats": `int`,
        ///     "UserId": `int`
        /// }
        /// 
        /// RESPONSE BODY:
        /// {
        ///     "Success": `bool`,
        ///     "Id": `int`,
        ///     "Description": `string`,
        ///     "Latitude": `float`,
        ///     "Longitude": `float`,
        ///     "NumberSeats": `int`,
        ///     "UserId": `int`
        /// }
        /// </summary>
        /// <param name="request">The request body.</param>
        /// <returns>The response body.</returns>
        public async Task<BenchAddResponse> Post(BenchAddRequest request)
        {
            var response = new BenchAddResponse()
            {
                Success = false
            };

            if (ModelState.IsValid)
            {
                var bench = new Bench()
                {
                    Description = request.Description,
                    Latitude = request.Latitude.Value,
                    Longitude = request.Longitude.Value,
                    NumberSeats = request.NumberSeats.Value,
                    UserId = request.UserId.Value
                };
                var benchId = await _benchRepo.AddAsync(bench);
                if (benchId.HasValue)
                {
                    response.Success = true;
                    response.Id = benchId.Value;
                    response.Description = bench.Description;
                    response.Latitude = bench.Latitude;
                    response.Longitude = bench.Longitude;
                    response.NumberSeats = bench.NumberSeats;
                    response.UserId = bench.UserId;
                }
            }

            return response;

        }

        /// <summary>
        /// Attempts to retrieve the bench with the given ID.
        /// 
        /// ROUTE:
        /// "api/bench"
        /// 
        /// RESPONSE BODY:
        /// {
        ///     "Success": `bool`,
        ///     "Id": `int`,
        ///     "Description": `string`,
        ///     "Latitude": `float`,
        ///     "Longitude": `float`,
        ///     "NumberSeats": `int`,
        ///     "AverageRating": `double`
        ///     "Reviews": [
        ///         {
        ///             "description": `string`,
        ///             "rating": `int`,
        ///             "reviewer": `string`
        ///             "date": `datetimeoffset`
        ///         }
        ///         .
        ///         .
        ///         .
        ///     ]
        /// }
        /// </summary>
        /// <param name="id">The bench ID.</param>
        /// <returns>The response body.</returns>
        public async Task<BenchDetailsResponse> Get(int id)
        {
            var bench = await _benchRepo.GetAsync(id);
            var response = new BenchDetailsResponse();
            if (bench != null)
            {
                response.Success = true;
                response.Id = bench.Id;
                response.Description = bench.Description;
                response.Latitude = bench.Latitude;
                response.Longitude = bench.Longitude;
                response.NumberSeats = bench.NumberSeats;
                response.AverageRating = bench.AverageRating;
                response.AddedBy = bench.User.ShortName;

                bench.Reviews.ForEach(r =>
                {
                    var reviewInfo = new ShortReviewInfo();
                    reviewInfo.Description = r.Description;
                    reviewInfo.Rating = r.Rating;
                    reviewInfo.Reviewer = r.User.ShortName;
                    reviewInfo.Date = r.Date;
                    response.Reviews.Add(reviewInfo);
                });
            }
            else
            {
                response.Success = false;
            }

            return response;
        }

        /// <summary>
        /// Retrieves the list of all benches.
        /// 
        /// ROUTE:
        /// "api/bench"
        /// 
        /// RESPONSE BODY:
        /// {
        ///     "Benches": [
        ///         {
        ///             "Id": `int`,
        ///             "Description": `string',
        ///             "Latitude": `float`,
        ///             "Longitude": `float`,
        ///             "NumberSeats": `int`,
        ///             "AverageRating": `double`
        ///         }
        ///         .
        ///         .
        ///         .
        ///     ]
        /// }
        /// </summary>
        /// <returns>The response body.</returns>
        public async Task<BenchListResponse> Get()
        {
            var benches = await _benchRepo.ListAsync();
            var response = new BenchListResponse();
            benches.ForEach(b =>
            {
                var shortDescription = b.Description;
                var descriptionArray = b.Description.Split(' ');
                if (descriptionArray.Length > 10)
                {
                    shortDescription = string.Join(" ", descriptionArray.Take(10).ToArray()) + "...";
                }
                var benchInfo = new ShortBenchInfo()
                {
                    Id = b.Id,
                    Description = shortDescription,
                    Latitude = b.Latitude,
                    Longitude = b.Longitude,
                    NumberSeats = b.NumberSeats,
                    AverageRating = b.AverageRating,
                    AddedBy = b.User.ShortName
                };
                response.Benches.Add(benchInfo);
            });
            return response;
        }

    }
}