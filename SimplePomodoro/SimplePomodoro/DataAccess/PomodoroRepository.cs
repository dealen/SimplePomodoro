using SimplePomodoro.DataAccess.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplePomodoro.DataAccess
{
    public class PomodoroRepository
    {
        public PomodoroRepository()
        {

        }

        public List<Schedule> GetSchedules()
        {
            List<Schedule> list = null;
            using (var pomodoroContext = new PomodoroContext())
            {
                list = pomodoroContext.Schedules.ToList();
            }
            return list;
        }

        public async Task AddSchedule(string name, int timeOfWork, int timeOfBreak, int intervals, int timeUnit)
        {
            using (var pomodoroContext = new PomodoroContext())
            {
                await pomodoroContext.AddAsync(new Schedule()
                {
                    Intervals = intervals,
                    TimeOfBreak = timeOfBreak,
                    TimeOfWork = timeOfWork,
                    TimeUnit = timeUnit, 
                    Name = name
                });
                await pomodoroContext.SaveChangesAsync();
            }
        }

        public async Task Update(Schedule schedule)
        {
            using (var pomodoroContext = new PomodoroContext())
            {
                var item = await pomodoroContext.FindAsync<Schedule>(schedule.ID);

                item.Intervals = schedule.Intervals;
                item.TimeOfBreak = schedule.TimeOfBreak;
                item.TimeOfWork = schedule.TimeOfWork;
                item.TimeUnit = schedule.TimeUnit;
                item.Name = schedule.Name;

                pomodoroContext.Update(item);
                await pomodoroContext.SaveChangesAsync();
            }
        }

        public async Task DeleteSchedule(int id)
        {
            using (var pomodoroContext = new PomodoroContext())
            {
                var item = await pomodoroContext.Schedules.FindAsync(id);
                pomodoroContext.Schedules.Remove(item);
                await pomodoroContext.SaveChangesAsync();
            }
        }
    }
}
