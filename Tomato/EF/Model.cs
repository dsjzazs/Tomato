using System;
using System.Data.Entity;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Tomato.EF
{
    public class Model : DbContext
    {
        public Model() : base("name=Model") { }
        public virtual DbSet<UserInfo> UserDB { get; set; }
        public virtual DbSet<Session> SessionDB { get; set; }

    }
}