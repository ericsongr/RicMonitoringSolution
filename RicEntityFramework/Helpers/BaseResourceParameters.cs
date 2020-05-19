namespace RicEntityFramework.Helpers
{ 
    public class BaseResourceParameters
    {
        private const int maxPageSize = 20 * 10000;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }

        public string SearchQuery { get; set; }
        public string OrderBy { get; set; }
        public string Fields { get; set; }
    }
}
