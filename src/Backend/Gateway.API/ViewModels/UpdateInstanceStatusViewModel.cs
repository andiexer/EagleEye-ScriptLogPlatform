﻿using EESLP.Backend.Gateway.API.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Backend.Gateway.API.ViewModel
{
    public class UpdateInstanceStatusViewModel
    {
        public ScriptInstanceStatus Status { get; set; }
    }
}
