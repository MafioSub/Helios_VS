using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Helios.Models
{
    public class RoomSeats
    {
        public SALA Room { get; set; }

        [Display(Name="liczba miejsc")]
        public int SeatsCount { get; set; }

        [Display(Name="liczba rzędów")]
        public int RowsCount { get; set; }

        public ICollection<SeatsRow> Rows { get; set; }
    }
}