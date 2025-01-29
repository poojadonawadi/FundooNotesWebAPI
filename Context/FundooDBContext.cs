using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Context
{
    public class FundooDBContext:DbContext
    {
        public FundooDBContext(DbContextOptions options) : base(options) { }

        public DbSet<Users> Users { get; set; }
    }
}
