using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.IEmosoft.com.ORMModel.DTOs
{
    public class ChangePasswordDTO
    {
        public Guid UserId { get; set; }
        public string NewPassword { get; set; }
    }
}