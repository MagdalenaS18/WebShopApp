using Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UserWebApi.Services.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendMailAsync(EmailData emailData);
    }
}
