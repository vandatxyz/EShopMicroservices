using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services
{
    public class DiscountService(DiscountContext dbContext, ILogger<DiscountService> logger) : DiscountProtoService.DiscountProtoServiceBase
    {
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            try
            {
                // get the discount from the database
                var counpon = await dbContext.Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName);
                if (counpon is null)
                {
                    return new CouponModel
                    {
                        ProductName = "No Discount",
                        Amount = 0,
                        Description = "No Discount Desc"
                    };
                }
                logger.LogInformation("Discount is retrieved for ProductName : {ProductName}, Amount : {Amount}", counpon.ProductName, counpon.Amount);
                var couponModel = counpon.Adapt<CouponModel>();
                return couponModel;
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();
            if (coupon is null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request"));
            }
            dbContext.Coupons.Add(coupon);
            await dbContext.SaveChangesAsync();

            logger.LogInformation("Discount is successfully created. ProductName : {ProductName}", coupon.ProductName);
            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = request.Coupon.Adapt<Coupon>();
            if (coupon is null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request"));
            }
            dbContext.Coupons.Update(coupon);
            await dbContext.SaveChangesAsync();
            logger.LogInformation("Discount is successfully updated. ProductName : {ProductName}", coupon.ProductName);
            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = await dbContext.Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName);
            if (coupon is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not found."));
            }
            dbContext.Coupons.Remove(coupon);
            await dbContext.SaveChangesAsync();
            logger.LogInformation("Discount is successfully deleted. ProductName : {ProductName}", coupon.ProductName);
            return new DeleteDiscountResponse
            {
                Success = true
            };
        }
    }
}
