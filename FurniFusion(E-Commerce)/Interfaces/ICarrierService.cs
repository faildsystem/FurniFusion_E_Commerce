using FurniFusion.Dtos.SuperAdmin.Carrier;
using FurniFusion.Models;

namespace FurniFusion.Interfaces
{
    public interface ICarrierService
    {
        Task<ServiceResult<List<Carrier>>> GetAllCarriersAsync();
        Task<ServiceResult<Carrier>> GetCarrierByIdAsync(int id);
        Task<ServiceResult<Carrier>> CreateCarrierAsync(CreateCarrierDto carrierDto, string creatorId);
        Task<ServiceResult<Carrier>> UpdateCarrierAsync(UpdateCarrierDto carrierDto);
        Task<ServiceResult<bool>> DeleteCarrierAsync(int id);
        Task<ServiceResult<Carrier>> ChangeCarrierStatusAsync(int id);
        Task<ServiceResult<List<Carrier>>> GetActiveCarriersAsync();
    }
}
