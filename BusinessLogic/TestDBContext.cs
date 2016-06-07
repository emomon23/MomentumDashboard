using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Test.IEmosoft.com.ORMModel;
using Test.IEmosoft.com.ORMModel.DTOs;

namespace Test.IEmosoft.com.BusinessLogic
{
    public class TestDBContext : DbContext
    {
        public TestDBContext() : base("TestAutomation")
        {
            //disable initializer
            Database.SetInitializer<TestDBContext>(null);
        }

        public DbSet<Application> Applications { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Test.IEmosoft.com.ORMModel.Test> Tests { get; set; }

        public DbSet<TestRun> TestRuns { get; set; }

        public DbSet<Run> Runs { get; set; }

        public DbSet<Administrator> Admins { get; set; }

        public DbSet<LaunchCommand> LaunchCommands { get; set; }

    }
}