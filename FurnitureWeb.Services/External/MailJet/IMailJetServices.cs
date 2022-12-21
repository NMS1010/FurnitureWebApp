using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FurnitureWeb.Services.External.MailJet
{
    public interface IMailJetServices
    {
        Task<bool> SendMail(string name, string email, string content, string title);
    }
}