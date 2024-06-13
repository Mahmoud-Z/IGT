using IGT.Core.Dtos;
using IGT.Core.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IGT.Service.Helpers.Exceptions
{
    public class BussinessException : Exception
    {
        public BaseDTO<string> ErrorObject { get; }
        public BussinessException(string message) : base(message)
        {
            ErrorObject = new BaseDTO<string>
            {
                IsSuccess = false,
                Status = ResponseStatusEnum.Failed.ToString(),
                Error = new Error
                {
                    Message = message,
                    ErrorCode = (int)ErrorCodesEnum.GeneralError
                }
            };
        }

        public BussinessException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
