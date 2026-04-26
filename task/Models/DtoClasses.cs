namespace task.Models
{

    public class Rootobject
    {
        public City[] city { get; set; }
    }

    public class City
    {
        public string id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public int? cityID { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string url { get; set; }
        public string timeshift { get; set; }
        public string requestEndTime { get; set; }
        public string sfrequestEndTime { get; set; }
        public string day2dayRequest { get; set; }
        public string day2daySFRequest { get; set; }
        public string preorderRequest { get; set; }
        public string freeStorageDays { get; set; }
        public Terminals terminals { get; set; }
    }

    public class Terminals
    {
        public Terminal[] terminal { get; set; }
    }

    public class Terminal
    {
        public string id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string fullAddress { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public bool isPVZ { get; set; }
        public bool cashOnDelivery { get; set; }
        public DtoPhone[] phones { get; set; }
        public bool storage { get; set; }
        public string mail { get; set; }
        public bool isOffice { get; set; }
        public bool receiveCargo { get; set; }
        public bool giveoutCargo { get; set; }
        public Maps maps { get; set; }
        public bool _default { get; set; }
        public Calcschedule calcSchedule { get; set; }
        public Addresscode addressCode { get; set; }
        public string mainPhone { get; set; }
        public float maxWeight { get; set; }
        public float maxLength { get; set; }
        public float maxWidth { get; set; }
        public float maxHeight { get; set; }
        public float maxVolume { get; set; }
        public float maxShippingWeight { get; set; }
        public float maxShippingVolume { get; set; }
        public Worktables worktables { get; set; }
    }

    public class Maps
    {
        public Width width { get; set; }
    }

    public class Width
    {
        public _640 _640 { get; set; }
        public _944 _944 { get; set; }
        public _960 _960 { get; set; }
    }

    public class _640
    {
        public Height height { get; set; }
    }

    public class Height
    {
        public _6401 _640 { get; set; }
    }

    public class _6401
    {
        public string url { get; set; }
    }

    public class _944
    {
        public Height1 height { get; set; }
    }

    public class Height1
    {
        public _352 _352 { get; set; }
    }

    public class _352
    {
        public string url { get; set; }
    }

    public class _960
    {
        public Height2 height { get; set; }
    }

    public class Height2
    {
        public _9601 _960 { get; set; }
    }

    public class _9601
    {
        public string url { get; set; }
    }

    public class Calcschedule
    {
        public string derival { get; set; }
        public string arrival { get; set; }
    }

    public class Addresscode
    {
        public string street_code { get; set; }
        public string place_code { get; set; }
    }

    public class Worktables
    {
        public Specialworktable specialWorktable { get; set; }
        public Worktable[] worktable { get; set; }
    }

    public class Specialworktable
    {
        public string[] receive { get; set; }
        public string[] giveout { get; set; }
    }

    public class Worktable
    {
        public string department { get; set; }
        public string monday { get; set; }
        public string tuesday { get; set; }
        public string wednesday { get; set; }
        public string thursday { get; set; }
        public string friday { get; set; }
        public string saturday { get; set; }
        public string sunday { get; set; }
        public string timetable { get; set; }
    }

    public class DtoPhone
    {
        public string number { get; set; }
        public string type { get; set; }
        public string comment { get; set; }
        public bool primary { get; set; }
    }

}
