﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBlog.Data.DTO
{
    public class User : Entity
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
    }
}
