﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TLAS.ViewModel
{
    public class AccessCardViewModel
    {
        public int CardID { get; set; }
        public int BayID { get; set; }
        public bool Active { get; set; }
        public bool IsAssigned { get; set; }
        public string Remarks { get; set; }
    }
}