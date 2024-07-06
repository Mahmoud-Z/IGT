using IGT.Service.Helpers.EmailConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGT.Service.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(Message message);
    }
}
