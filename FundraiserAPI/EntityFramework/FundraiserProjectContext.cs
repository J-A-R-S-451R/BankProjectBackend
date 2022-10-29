using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FundraiserAPI.EntityFramework
{
    public partial class FundraiserProjectContext : FundraiserProjectContextRaw
    {
        public FundraiserProjectContext()
            : base()
        {
        }

        public FundraiserProjectContext(DbContextOptions<FundraiserProjectContextRaw> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connString = "";


                try
                {
                    IConfigurationRoot configuration = new ConfigurationBuilder()
                       .AddUserSecrets(Assembly.GetExecutingAssembly(), false)
                       .Build();

                    connString = configuration.GetConnectionString("FundraiserDB");

                } catch (Exception ex)
                {
                }

                if (String.IsNullOrEmpty(connString))
                {
                    connString = Environment.GetEnvironmentVariable("SQLAZURECONNSTR_FundraiserDB");
                }

                optionsBuilder.UseSqlServer(connString);
            }
        }
    }
}
