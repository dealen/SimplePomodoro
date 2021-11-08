namespace SimplePomodoro.DAL.Model
{
    public class Schedule
    {
        public int ID{ get; set; }
        public string Name { get; set; }
        public int TimeForWork { get; set; }
        public int TimeForBreak { get; set; }
    }
}
