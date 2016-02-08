using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Helios.Models
{
    public class SeatsRow
    {
        [Display(Name="id sali")]
        public int RoomId { get; set; }

        [Display(Name="numer rzędu")]
        [Range(1,Int32.MaxValue,ErrorMessage="Nieprawidłowy numer rzędu")]
        public int RowNumber { get; set; }

        [Display(Name="liczba miejsc")]
        [Range(1,Int32.MaxValue,ErrorMessage="Nieprawidłowa liczba miejsc")]
        public int SeatsCount { get; set; }

        public ICollection<MIEJSCE> Seats {get; set; }
    }
}