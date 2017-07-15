namespace BD_Project
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DBContext : DbContext
    {
        public DBContext()
            : base("name=DBContext")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
        //this.Configuration.LazyLoadingEnabled = false; 
        public virtual DbSet<Adresa> Adresa { get; set; }
        public virtual DbSet<Bicicleta> Bicicleta { get; set; }
        public virtual DbSet<Centru> Centru { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<Comenzi> Comenzi { get; set; }
        public virtual DbSet<Comenzi_Biciclete> Comenzi_Biciclete { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Adresa>()
                .HasMany(e => e.Centru)
                .WithRequired(e => e.Adresa)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Adresa>()
                .HasMany(e => e.Client)
                .WithRequired(e => e.Adresa)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Client>()
                .HasMany(e => e.Comenzi)
                .WithRequired(e => e.Client)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Centru>()
                .HasMany(e => e.Comenzi)
                .WithRequired(e => e.Centru)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Comenzi>()
                .HasMany(e => e.Comenzi_Biciclete)
                .WithRequired(e => e.Comenzi)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Bicicleta>()
               .HasMany(e => e.Comenzi_Biciclete)
               .WithRequired(e => e.Bicicleta)
               .WillCascadeOnDelete(false);
        }
    }
}
