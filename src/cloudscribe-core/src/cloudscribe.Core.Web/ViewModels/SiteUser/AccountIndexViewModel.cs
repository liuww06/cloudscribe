﻿using Microsoft.AspNet.Identity;
using System.Collections.Generic;

namespace cloudscribe.Core.Web.ViewModels.SiteUser
{
    public class AccountIndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }
}