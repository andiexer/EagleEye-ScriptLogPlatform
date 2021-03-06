﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EESLP.Backend.Gateway.API.ViewModels
{
    public class HostAddModel
    {
        [Required]
        public string Hostname { get; set; }
        [Required]
        public string FQDN { get; set; }
        [Required]
        public string ApiKey { get; set; }
    }
}
