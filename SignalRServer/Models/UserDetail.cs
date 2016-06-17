using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRSever.Models
{
    [Serializable]
    public class UserDetail
    {
        public string connectionId { get; set; }
        public string sessionId { get; set; }
        public int userId { get; set; }
        public int teamId { get; set; }
        public int groupId{  get; set;}

    }
}