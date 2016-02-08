using Helios.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Helios.Controllers
{
    public class HomeController : Controller
    {

        private HeliosRepository repository;
        public HomeController()
        {
            this.repository = new HeliosRepository();
        }


        public ActionResult Index()
        {
            var movieTypes = repository.GetMovieTypes();
            ViewBag.movieGenre = new SelectList(movieTypes);
            var seances = repository.GetSeances(); 
            DateTime fromDate = DateTime.Now;
            seances = seances.Where(se => se.SeansData >= fromDate );
            seances = seances.OrderBy(s => s.SeansData).ThenBy(s => s.SeansGodzina);
            return View(seances);
        }

        [HttpPost]
        public ActionResult Index(string movieGenre,string movie, string FromDate, string ToDate)
        {
            var seances = repository.GetSeances();
            if (!String.IsNullOrEmpty(movieGenre))
            {
                seances = seances.Where(s => s.FILM.RodzajFilmu == movieGenre);
            }
            if (!String.IsNullOrEmpty(movie))
            {
                seances = seances.Where(s => s.FILM.NazwaFilmuPL.Contains(movie));
            }
            if (!String.IsNullOrEmpty(FromDate))
            {
                DateTime from = DateTime.ParseExact(FromDate, "MM/dd/yyyy", new CultureInfo("en-US"));
                seances = seances.Where(s => s.SeansData >= from);
            }
            if (!String.IsNullOrEmpty(ToDate))
            {
                DateTime to = DateTime.ParseExact(ToDate, "MM/dd/yyyy", new CultureInfo("en-US"));
                to.AddDays(1);
                seances = seances.Where(s => s.SeansData <= to);
            }
            var movieTypes = repository.GetMovieTypes();
            ViewBag.movieGenre = new SelectList(movieTypes);
            return View(seances);
        }

        public ActionResult Book(int id)
        {
            RoomLoadViewModel vm = repository.GetRoomLoad(id);
            var Ticket = new List<SelectListItem>();
            var tickets = repository.GetTicketNamesAndIds();
            foreach (KeyValuePair<int, string> el in tickets)
            {
                Ticket.Add(new SelectListItem { Value = el.Key.ToString(), Text = el.Value });
            }
            ViewBag.Ticket = Ticket;
            return View(vm);
        }

        [HttpPost]
        public JsonResult IsSeatAvailable(int seanceId, int seatId)
        {
            bool result = repository.IsSeatFree(seanceId, seatId);
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Book()
        {
            
            int seatId = Int32.Parse(Request.Form["SeatId"]);
            int seanceId = Int32.Parse(Request.Form["SeanceId"]);
            int ticketId = Int32.Parse(Request.Form["Ticket"]);
            if(repository.IsSeatFree(seanceId,seatId))
            {             
                WYKUP_BILET b = new WYKUP_BILET();
                b.FK_IdMiejsca = seatId;
                b.FK_IdSeansu = seanceId;
                b.FK_IdBiletu = ticketId;
                b = repository.AddBooking(b);
                repository.Save();
                b = repository.GetFullBookingById(b.IdRezerwacji);
                return View("BookingSummary",b);
            }
            else
            {
                return View("BookingProblem");
            }
           
        }

    }
}
