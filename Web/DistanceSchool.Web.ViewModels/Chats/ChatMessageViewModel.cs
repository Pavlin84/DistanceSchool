using System;
using System.Collections.Generic;
using System.Text;

namespace DistanceSchool.Web.ViewModels.Chats
{
    public class ChatMessageViewModel
    {
        public string Text { get; set; }

        public string Sender { get; set; }

        public bool IsTeacher { get; set; }
    }
}
