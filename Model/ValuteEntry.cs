namespace ValuteConverter
{
    class ValuteEntry
    {
        public string ValuteImage { get; set; }
        public string ValuteName { get; set; }
        public string ValuteCode { get; set; }
        public double ValuteCourse { get; set; }
        public ValuteEntry(string vname, string vcode, double vcurs)
        {
            ValuteName = vname;
            ValuteCode = vcode;
            ValuteCourse = vcurs;
        }
    }
}
