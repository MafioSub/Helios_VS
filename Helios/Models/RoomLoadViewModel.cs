using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Helios.Models
{
    public class RoomLoadViewModel
    {
        [Display(Name="Id sali")]
        public int RoomId { get; set; }

        [Display(Name="Nazwa sali")]
        public string RoomName { get; set; }

        [Display(Name="Klimatyzacja")]
        public Nullable<bool> AirConditioner { get; set; }

        public List<SeatsRow> Rows { get; set; }

        [Display(Name="Całkowita liczba miejsc")]
        public int SeatsCount { get; set; }

        [Display(Name="Ilość rzędów")]
        public int RowsCount { get; set; }

        [Display(Name="Ilośc wolnych miejsc")]
        public int EmptySeatsCount { get; set; }

        [Display(Name="Ilość zajętych miejsc")]
        public int BusySeatsCount { get; set; }

        public Dictionary<int, bool> SeatStatuses { get; set; }

        [Display(Name="Data seansu")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime SeanceDate { get; set; }
        [Display(Name = "Godzina seansu")]
        public System.TimeSpan SeanceTime { get; set; }
        [Display(Name = "Tytuł filmu")]
        public string MovieTitle { get; set; }
        [Display(Name = "2D/3D")]
        public string MovieType { get; set; }
        [Display(Name = "Czas trwania")]
        public System.TimeSpan MovieDuration { get; set; }

        public int SeanceId { get; set; }


    }
}