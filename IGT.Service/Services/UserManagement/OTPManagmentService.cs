using IGT.Core.Dtos;
using IGT.Core.Enums;
using IGT.Core.Resources;
using IGT.Data.Models;
using IGT.Repository.UnitOfWork;
using IGT.Service.Helpers.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGT.Service.Services.UserManagement
{
    public class OTPManagmentService
    {
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly int length = 6;
        private readonly float OTPLifeTime = -1.5f;
        private readonly int OTPBlockTime = -15;
        public OTPManagmentService(UserManager<User> userManager, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<BaseDTO<string>> SendOTP(string phoneNumber)
        {
            try
            {
                await CanSendOTPAsync(phoneNumber);
                OTP oTP = new OTP()
                {
                    OTPCode = GenerateNumericOTP(),
                    ExpiresAt = DateTime.Now.AddMinutes(OTPLifeTime),
                    PhoneNumber = phoneNumber
                };
                await _unitOfWork.GetRepository<OTP>().AddAsync(oTP);
                return new BaseDTO<string>
                {
                    IsSuccess = true,
                    Data = "OTP was sent successfully",
                    Status = ResponseStatusEnum.Success.ToString(),
                };
            }
            catch (BussinessException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new BussinessException(AuthenticationResource.GeneralError);
            }
        }
        public async Task CanSendOTPAsync(string phoneNumber)
        {
            var otps = await _unitOfWork.GetRepository<OTP>()
                .FindAllAsync(o => o.PhoneNumber == phoneNumber, 3, null, o => o.OTPId, "DESC");

            if (otps.Count() == 3 && !otps.All(o => o.CreatedAt < DateTime.Now.AddMinutes(OTPBlockTime) || o.IsUsed))
            {
                throw new BussinessException("You have been suspende for 15 minutes");
            }
        }
        public async Task VerifyOTP(string phoneNumber, string otpCode)
        {
            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
                if (user == null)
                {
                    throw new BussinessException("Phone number not found");
                }

                var otp = await _unitOfWork.GetRepository<OTP>()
                    .FindAsync(o => o.PhoneNumber == phoneNumber && o.OTPCode == otpCode && !o.IsUsed && o.ExpiresAt >= DateTime.Now.AddMinutes(-1.5));

                if (otp == null)
                {
                    throw new BussinessException("Invalid or expired OTP");
                }

                otp.IsUsed = true;
                await _unitOfWork.GetRepository<OTP>().AddAsync(otp);
                await _unitOfWork.CompleteAsync();
            }
            catch (BussinessException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new BussinessException(AuthenticationResource.GeneralError);
            }
        }
        private string GenerateNumericOTP()
        {
            var random = new Random();
            var otp = new char[length];

            for (int i = 0; i < length; i++)
            {
                otp[i] = (char)('0' + random.Next(0, 10));
            }

            return new string(otp);
        }
    }

}
