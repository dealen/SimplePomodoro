using SimplePomodoro.Helpers;

namespace SimplePomodoro.DataAccess.Model
{
    public class Schedule
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int TimeUnit { get; set; }
        public int TimeOfWork { get; set; }
        public int TimeOfBreak { get; set; }
        public int Intervals { get; set; }

        public override string ToString()
        {
            var timeUnit = (TimeUnits)TimeUnit;
            var timeUnitString = string.Empty;
            switch (timeUnit)
            {
                case TimeUnits.Second:
                    timeUnitString = "s";
                    break;
                case TimeUnits.Minutes:
                    timeUnitString = "m";
                    break;
                case TimeUnits.Hours:
                    timeUnitString = "h";
                    break;
                default:
                    break;
            }
            return $"Name={Name} Work={TimeOfWork}{timeUnitString} Break={TimeOfBreak}{timeUnitString} Intervals:{Intervals}";
        }
    }
}
