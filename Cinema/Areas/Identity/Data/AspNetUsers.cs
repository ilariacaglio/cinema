﻿using System;
using Microsoft.AspNetCore.Identity;

namespace Cinema.Areas.Identity.Data
{
	public class AspNetUsers : IdentityUser
    {
        [PersonalData]
        public string Cognome { get; set; } = null!;

        [PersonalData]
        public string Nome { get; set; } = null!;

        [PersonalData]
        public string Mail { get; set; } = null!;

        [PersonalData]
        public string Password { get; set; } = null!;

        [PersonalData]
        public string Sesso { get; set; } = null!;

        [PersonalData]
        public DateOnly Nascita { get; set; }

        [PersonalData]
        public string Residenza { get; set; } = null!;

    }
}

