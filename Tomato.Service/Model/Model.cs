using System;
using System.Data.Entity;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Tomato.Service.Model
{
    public class EntityModel : DbContext
    {
        public EntityModel() : base("name=Model1") { }
        public virtual DbSet<UserInfo> UserDB { get; set; }
        public virtual DbSet<Session> SessionDB { get; set; }

        public virtual DbSet<AuthorityEntity> AuthorityEntities { get; set; }
        public virtual DbSet<DepartmentEntity> DepartmentEntities { get; set; }
        public virtual DbSet<ModularEntity> ModularEntities { get; set; }
        public virtual DbSet<PositionEntity> PositionEntities { get; set; }
        public virtual DbSet<RoleEntity> RoleEntities { get; set; }
        public virtual DbSet<UserGrantEntity> UserGrantEntities { get; set; }

        public static void InitializeDb(bool NewDB)
        {
            if (NewDB)
            {
                System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseAlways<EntityModel>());
                using (var db = new EntityModel())
                {
                    db.UserDB.Add(new UserInfo()
                    {
                        UserName = "admin",
                        Password = "admin",
                        NickName = "管理员",
                        Email = "dsjzazs@live.cn"
                    });
                    db.UserDB.Add(new UserInfo()
                    {
                        UserName = "Guest",
                        Password = "Guest",
                        NickName = "游客",
                    });
                    db.SaveChanges();
                }
            }
            else
                System.Data.Entity.Database.SetInitializer(new System.Data.Entity.DropCreateDatabaseIfModelChanges<EntityModel>());
        }
    }
}