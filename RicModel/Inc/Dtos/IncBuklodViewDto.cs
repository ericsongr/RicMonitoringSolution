using System;

namespace RicModel.Inc.Dtos
{
    public class IncBuklodViewDto : IncBuklod
    {
        public string Name { get; set; }
        public string AnniversaryString { get; set; }
        public string BirthdayString { get; set; }
        public string PurokGrupo {
            get
            {
                return Purok == 0 && Grupo == 0 ? "" : $"*{Purok}-{Grupo}*";
            }
        }

    }
}
