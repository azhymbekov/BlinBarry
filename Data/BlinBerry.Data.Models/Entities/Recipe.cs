using System;
using System.Collections.Generic;
using System.Text;
using BlinBerry.Data.Common.Models;
using BlinBerry.Data.Models.IdentityModels;

namespace BlinBerry.Data.Models.Entities
{
    public class Recipe : BaseProdusctModel<Guid, ApplicationUser>
    {
        public string Name { get; set; }
    }
}
