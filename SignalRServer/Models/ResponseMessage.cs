using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRSever.Models
{
    [Serializable]
    public class ResponseMessage
    {
        public int code { get; set; }
        public string desc { get; set; }
    }
}