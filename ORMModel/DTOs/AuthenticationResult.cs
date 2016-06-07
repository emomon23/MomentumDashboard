using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.IEmosoft.com.ORMModel.DTOs
{
    public class AuthenticationResult
    {
        public enum AuthenticationResponseEnumeration
        {
            AccountLocked = 0,
            IsAdminUser = 1,
            IsClientUser = 2,
            InvalidCredentials = 3,
        }

        public AuthenticationResponseEnumeration AuthentiationStatus {  get; set; }
        public Guid ClientId { get; set; }

        public Guid UserId { get; set; }

        public bool MustChangePassword { get; set; }
    }
}