namespace RicMonitoringAPI.Common.Model
{
    public class AppSelectListItem
    {
        public string Value { get; set; }
        public string Text { get; set; }
        public string Label => Text;
    }
}
