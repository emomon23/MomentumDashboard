using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Test.IEmosoft.com.ORMModel
{
    [Table("LaunchCommands")]
    public class LaunchCommand
    {
        [Key]
        public int LuanchCommandId { get; set; }

        public int TestId { get; set; }

        public virtual Test Test { get; set; }

        public string LaunchedByUserName { get; set; }

        public Guid LaunchedByUserGuuid { get; set; }

        public string CommandText { get; set; }

        public string Parameters { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime ? DateScheduled { get; set; }

        public string PickupKey { get; set; }

        public DateTime? DatePickedup { get; set; }

        public DateTime? DateExecuted { get; set; }
     
    }
}