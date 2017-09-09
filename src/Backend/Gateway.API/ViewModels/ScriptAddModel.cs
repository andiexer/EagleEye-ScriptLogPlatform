﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EESLP.Backend.Gateway.API.ViewModels
{
    public class ScriptAddModel
    {
        [Required]
        public string Scriptname { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
