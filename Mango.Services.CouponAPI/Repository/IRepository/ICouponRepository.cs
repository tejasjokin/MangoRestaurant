using Mango.Services.CouponAPI.Models.Dto;

namespace Mango.Services.CouponAPI.Repository.IRepository
{
    public interface ICouponRepository
    {
        Task<CouponDto> GetCouponByCode(string couponCode);
    }
}
