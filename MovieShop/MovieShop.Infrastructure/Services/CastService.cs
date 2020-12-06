using MovieShop.Core.Entities;
using MovieShop.Core.Models.Response;
using MovieShop.Core.RepositoryInterfaces;
using MovieShop.Core.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieShop.Infrastructure.Services
{
    public class CastService : ICastService
    {
        private readonly IAsyncRepository<Cast> _castRepository;
        public CastService(IAsyncRepository<Cast> castRepository)
        {
            _castRepository = castRepository;
        }
        public async Task<CastDetailsResponseModel> GetCastDetailsWithMovies(int id)
        {
            var dbcasts = await _castRepository.GetByIdAsync(id);
            if (dbcasts == null)
            {
                throw new Exception("No Casts Found");
            }
            var response = new CastDetailsResponseModel
            {
                Id = dbcasts.Id,
                Name = dbcasts.Name,
                Gender = dbcasts.Gender,
                TmdbUrl = dbcasts.TmdbUrl,
                ProfilePath = dbcasts.ProfilePath
            };
            return response;
        }
    }
}
