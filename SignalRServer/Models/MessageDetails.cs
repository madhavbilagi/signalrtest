﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRSever.Models
{
    [Serializable]
    public class MessageDetails
    {
        public string Id { get; set; }
        public string Message { get; set; }
    }
}