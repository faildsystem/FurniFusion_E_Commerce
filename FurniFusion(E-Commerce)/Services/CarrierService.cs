using FurniFusion.Data;
using FurniFusion.Dtos.SuperAdmin.Carrier;
using FurniFusion.Interfaces;
using FurniFusion.Models;
using Microsoft.EntityFrameworkCore;

namespace FurniFusion.Services
{
    public class CarrierService: ICarrierService
    {
        private readonly FurniFusionDbContext _context;

        public CarrierService(FurniFusionDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<Carrier>> ChangeCarrierStatusAsync(int id)
        {
            var result = await GetCarrierByIdAsync(id);

            if (result == null || result.Data == null)
            {
                return ServiceResult<Carrier>.ErrorResult("Carrier not found", StatusCodes.Status404NotFound);
            }

            result.Data.IsActive = !result.Data.IsActive;
            result.Data.UpdatedAt = DateTime.Now;

            _context.Carriers.Update(result.Data);
            await _context.SaveChangesAsync();

            return ServiceResult<Carrier>.SuccessResult(result.Data);
        }

        public async Task<ServiceResult<Carrier>> CreateCarrierAsync(CreateCarrierDto carrierDto, string creatorId)
        {
            var existingEmail = await _context.Carriers.FirstOrDefaultAsync(c => c.Email == carrierDto.Email);
            if (existingEmail != null) {
                return ServiceResult<Carrier>.ErrorResult("Carrier with this email already exists", StatusCodes.Status400BadRequest);
            }

            var existingPhone = await _context.Carriers.FirstOrDefaultAsync(c => c.Phone == carrierDto.Phone);
            if (existingPhone != null) {
                return ServiceResult<Carrier>.ErrorResult("Carrier with this phone already exists", StatusCodes.Status400BadRequest);
            }

            var createdCarrier = new Carrier
            {
                CarrierName = carrierDto.CarrierName,
                Email = carrierDto.Email,
                Phone = carrierDto.Phone,
                Website = carrierDto.Website,
                Address = carrierDto.Address,
                ShippingApi = carrierDto.ShippingApi,
                IsActive = carrierDto.IsActive,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _context.Carriers.AddAsync(createdCarrier);
            await _context.SaveChangesAsync();

            return ServiceResult<Carrier>.SuccessResult(createdCarrier, "Carrier created successfully");

        }

        public async Task<ServiceResult<bool>> DeleteCarrierAsync(int id)
        {
            var result = await GetCarrierByIdAsync(id);

            if (result == null || result.Data == null)
            {
                return ServiceResult<bool>.ErrorResult("Carrier not found", StatusCodes.Status404NotFound);
            }

            _context.Carriers.Remove(result.Data);
            await _context.SaveChangesAsync();

            return ServiceResult<bool>.SuccessResult(true, "Carrier deleted successfully");
        }

        public async Task<ServiceResult<List<Carrier>>> GetActiveCarriersAsync()
        {
            var carriers = await _context.Carriers.Where(c => c.IsActive == true).ToListAsync();

            return ServiceResult<List<Carrier>>.SuccessResult(carriers);
        }

        public async Task<ServiceResult<List<Carrier>>> GetAllCarriersAsync()
        {
            var carriers = await _context.Carriers.ToListAsync();

            return ServiceResult<List<Carrier>>.SuccessResult(carriers);
        }

        public async Task<ServiceResult<Carrier>> GetCarrierByIdAsync(int id)
        {
            var carrier = await _context.Carriers.FirstOrDefaultAsync(c => c.CarrierId == id);

            if (carrier == null)
            {
                return ServiceResult<Carrier>.ErrorResult("Carrier not found", StatusCodes.Status404NotFound);
            }

            return ServiceResult<Carrier>.SuccessResult(carrier);
        }

        public async Task<ServiceResult<Carrier>> UpdateCarrierAsync(UpdateCarrierDto carrierDto)
        {
            var result = await GetCarrierByIdAsync(carrierDto.CarrierId);

            if (result == null || result.Data == null)
            {
                return ServiceResult<Carrier>.ErrorResult("Carrier not found", StatusCodes.Status404NotFound);
            }

            var existingEmail = await _context.Carriers.FirstOrDefaultAsync(c => c.Email == carrierDto.Email);
            if (existingEmail != null)
            {
                return ServiceResult<Carrier>.ErrorResult("Carrier with this email already exists", StatusCodes.Status400BadRequest);
            }

            var existingPhone = await _context.Carriers.FirstOrDefaultAsync(c => c.Phone == carrierDto.Phone);
            if (existingPhone != null)
            {
                return ServiceResult<Carrier>.ErrorResult("Carrier with this phone already exists", StatusCodes.Status400BadRequest);
            }

            result.Data.CarrierName = carrierDto.CarrierName ?? result.Data.CarrierName;
            result.Data.Email = carrierDto.Email ?? result.Data.Email;
            result.Data.Phone = carrierDto.Phone ?? result.Data.Phone;
            result.Data.Website = carrierDto.Website ?? result.Data.Website;
            result.Data.Address = carrierDto.Address ?? result.Data.Address;
            result.Data.ShippingApi = carrierDto.ShippingApi ?? result.Data.ShippingApi;
            result.Data.IsActive = carrierDto.IsActive ?? result.Data.IsActive;
            result.Data.UpdatedAt = DateTime.Now;

            _context.Carriers.Update(result.Data);
            await _context.SaveChangesAsync();

            return ServiceResult<Carrier>.SuccessResult(result.Data, "Carrier updated successfully");
        }
    }
}
