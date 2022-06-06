namespace Slav_Menu
{
    public class MusicEventStorage
    {
        public List<Mission> Missions { get; set; }
        public List<StrangerAndFreak> StrangersAndFreaks { get; set; }
        public List<RandomEvent> RandomEvents { get; set; }
        public List<Activity> Activities { get; set; }
        public List<Content> OnlineContent { get; set; }
        public List<Other> Miscellaneous { get; set; }
    }
    public class Mission
    {
        public string MissionName { get; set; }
        public List<Event> MissionEvents { get; set; }
    }
    public class StrangerAndFreak
    {
        public string StrangerAndFreakName { get; set; }
        public List<Event> StrangerAndFreakEvents { get; set; }
    }
    public class RandomEvent
    {
        public string RandomEventName { get; set; }
        public List<Event> RandomMusicEvents { get; set; }
    }
    public class Activity
    {
        public string ActivityName { get; set; }
        public List<Event> ActivityEvents { get; set; }
    }
    public class Content
    {
        public string ContentName { get; set; }
        public List<Event> ContentEvents { get; set; }
    }
    public class Other
    {
        public string OtherName { get; set; }
        public List<Event> OtherEvents { get; set; }
    }
    public class Event
    {
        public string EventName { get; set; }
        public string EventHash { get; set; }
    }
}
