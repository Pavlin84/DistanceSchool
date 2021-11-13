using DistanceSchool.Web.Infrastructure.CustomAuthorizeAttribute;
using DistanceSchool.Web.ViewModels.Chats;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DistanceSchool.Web.Hubs
{
    public class TeamChatHub : Hub
    {
        [TeamPassportAcsses]
        public async Task Send(string message)
        {
            await this.Clients.All.SendAsync(
                "NewMessage",
                new ChatMessageViewModel { Sender = this.Context.User.Identity.Name, Text = message, });
        }
    }
}
