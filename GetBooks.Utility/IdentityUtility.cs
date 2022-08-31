using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace GetBooks.Utility
{
    public class IdentityUtility
    {
        public static string GetUserId(IIdentity userIdentity)
        {
            ClaimsIdentity claimsIdentity = (ClaimsIdentity)userIdentity;
            Claim claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null) 
            { 
                return null; 
            }

            return claim.Value;
        }
    }
}
