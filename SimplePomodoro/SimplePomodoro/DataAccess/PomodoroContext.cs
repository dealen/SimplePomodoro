using Microsoft.EntityFrameworkCore;
using SimplePomodoro.DataAccess.Model;
using System.IO;
using Xamarin.Essentials;

namespace SimplePomodoro.DataAccess
{
    public class PomodoroContext : DbContext
    {
        public DbSet<Schedule> Schedules { get; set; }

        public PomodoroContext()
        {
            //The SQLitePCL.Batteries_V2.Init() is needed in the constructor to initiate SQLite on iOS.
            SQLitePCL.Batteries_V2.Init();

            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "pomodoro.db3");
            optionsBuilder.UseSqlite($"Filename={dbPath}");
        }
    }
}
