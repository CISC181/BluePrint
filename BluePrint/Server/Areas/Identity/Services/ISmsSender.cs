using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BluePrint.Server.Areas.Identity.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
