using Microsoft.EntityFrameworkCore;
using System.IO;
using Xamarin.Essentials;

namespace SimplePomodoro.DAL.Model
{
    public class PomodoroContext : DbContext
    {
        public PomodoroContext()
        {
            SQLitePCL.Batteries_V2.Init();
            Database.EnsureCreated();
        }

        public DbSet<Schedule> Schedules { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "pomodoro.db3");

            optionsBuilder.UseSqlite($"Filename={dbPath}");
        }
    }
}
