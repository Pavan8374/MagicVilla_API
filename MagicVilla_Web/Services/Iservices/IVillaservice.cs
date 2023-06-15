using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web.Services.Iservices
{
    public interface IVillaservice
    {
        Task<T> GetAll<T>();
        Task<T> GetT<T>(int id);
        Task<T> CreateVilla<T>(VillaCreateDTO dto);
        Task<T> UpdateVilla<T>(VillaUpdateDTO dto);
        Task<T> DeleteVilla<T>(int id);
    }
}
