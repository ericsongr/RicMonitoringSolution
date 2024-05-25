using System;

namespace RicModel.Inc
{
    public class IncBuklod
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int Purok { get; set; }
        public int Grupo { get; set; }
        public string Mobile { get; set; }
        public DateTime? Anniversary { get; set; }
        public DateTime? Birthday { get; set; }
        public bool IsDeleted { get; set; }

    }
}
