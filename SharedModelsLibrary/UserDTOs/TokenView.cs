﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModelsLibrary.UserDTOs
{
    public class TokenView
    {
        public string Token { get; set; }
        public DateTime? Expiration { get; set; }
    }
}
