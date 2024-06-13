using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGT.Core.Dtos
{
    public class BaseDTO<T>
    {
        public bool? IsSuccess { get; set; }
        public string? Status { get; set; }
        public T? Data { get; set; }
        public Error? Error { get; set; }
    }
    public class Error
    {
        public string? Message { get; set; }
        public int? ErrorCode { get; set; }
    }
}
