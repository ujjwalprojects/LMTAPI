﻿using LMT.Application.DTOs;
using LMT.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LMT.Infrastructure.Data
{
    public class EFDBContext(DbContextOptions<EFDBContext> options) : DbContext(options)
    {

       
        public DbSet<M_BlockMunicipals> M_BlockMunicipals { get; set; }
        public DbSet<M_Districts> M_Districts { get; set; }
        public DbSet<M_JobRoles> M_JobRoles { get; set; }
        public DbSet<M_RegistrationActs> M_RegistrationActs { get; set; }
        public DbSet<M_States> M_States { get; set; }
        public DbSet<M_TaskPurposes> M_TaskPurposes { get; set; }
        public DbSet<M_WorkerTypes> M_WorkerTypes { get; set; }
        public DbSet<T_EstablishmentRegistrations> T_EstablishmentRegistrations { get; set; }
        public DbSet<T_TaskAllocationForms> T_TaskAllocationForms { get; set; }
        public DbSet<T_TaskAllocationSiteImages> T_TaskAllocationSiteImages { get; set; }
        public DbSet<T_WorkerRegistrations> T_WorkerRegistrations { get; set; }
        public DbSet<T_UniqueCodeRecords> T_UniqueCodeRecords { get; set; }

        public DbSet<GetT_WorkerRegistrationDTO> GetT_WorkerRegistrationDTO { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ignore any primary key mapping if DTO doesn't have a key
            modelBuilder.Entity<GetT_WorkerRegistrationDTO>().HasNoKey();

            // Other entity configurations
            base.OnModelCreating(modelBuilder);
        }
    }
}
