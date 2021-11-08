namespace SimplePomodoro.DataAccess.Model
{
    public class Schedule
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int TimeOfWork { get; set; }
        public int TimeOfBreak { get; set; }
    }
}
