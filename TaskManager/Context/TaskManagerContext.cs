using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TaskManager.Models;

namespace TaskManager.Context
{
    public class TaskManagerContext : DbContext
    {
        public TaskManagerContext()
        {
        }

        public TaskManagerContext(DbContextOptions<TaskManagerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Channels> Channels { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Projects> Projects { get; set; }
        public virtual DbSet<Tasks> Tasks { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Settings> Settings { get; set; }
    }
}
