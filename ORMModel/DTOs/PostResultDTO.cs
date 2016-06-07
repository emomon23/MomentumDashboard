using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.IEmosoft.com.ORMModel.DTOs
{
    public class PostResultDTO
    {
        public bool Successful { get; set; }
        public object Payload { get; set; }
        public string Message { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorType { get; set; }
    }
}