using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.Iservices;
using System.Globalization;

namespace MagicVilla_Web.Services
{
    public class VillaService : BaseService, IVillaservice
    {
        private readonly IHttpClientFactory clientFactory;
        private string villaUrl;
        public VillaService(IHttpClientFactory _clientFactory, IConfiguration configuration) : base(_clientFactory)
        {
            clientFactory = _clientFactory;
            villaUrl = configuration.GetValue<string>("ServiceUrls: VillaAPI");
        }
        public Task<T> CreateVilla<T>(VillaCreateDTO dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = villaUrl + "/api/villaAPI"
            });
        }

        public Task<T> DeleteVilla<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = villaUrl + "/api/villaAPI" + id
            });
        }

        public Task<T> GetAll<T>()
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = villaUrl + "/api/villaAPI"
            });
        }

        public Task<T> GetT<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = villaUrl + "/api/villaAPI" + id
            });
        }

        public Task<T> UpdateVilla<T>(VillaUpdateDTO dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = villaUrl + "/api/villaAPI/" + dto.Id
            });
        }
    }
}
