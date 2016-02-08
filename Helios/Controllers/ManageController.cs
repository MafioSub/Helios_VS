using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Helios.Models;
using System.Globalization;

namespace Helios.Controllers
{
    [Authorize(Roles="Admin")]
    public class ManageController : Controller
    {

        private HeliosRepository repository;
        public ManageController()
        {
            this.repository = new HeliosRepository();
        }
        //
        // GET: /Manage/

        public ActionResult Index()
        {
            return View();
        }

        #region ROOMS_SEATS
        public ActionResult Rooms()
        {
            List<SALA> model = new List<SALA>();
            model = repository.GetAllRooms().ToList() ;
            return View(model);
        }

        public ActionResult RoomDetails(int id)
        {
            return View(repository.GetRoomById(id));
        }

        public ActionResult CreateRoom()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateRoom(SALA model)
        {
            if (ModelState.IsValid)
            {
                repository.AddRoom(model);
                repository.Save();
                return RedirectToAction("Rooms");
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult EditRoom(int id)
        {
            SALA model = repository.GetRoomById(id);
            repository.Save();
            return View(model);
        }

        [HttpPost]
        public ActionResult EditRoom(SALA model)
        {
            if(ModelState.IsValid)
            {
                repository.UpdateRoom(model);
                repository.Save();
                return RedirectToAction("Rooms");
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult DeleteRoom(int id)
        {
            SALA model = repository.GetRoomById(id);
            return View(model);
        }

        [HttpPost,ActionName("DeleteRoom")]
        public ActionResult DeleteRoomConfirmed(int id)
        {
            repository.DeleteRoom(id);
            repository.Save();
            return RedirectToAction("Rooms");
        }

        public ActionResult RoomSeats(int id)
        {
            RoomSeats model = repository.GetRoomSeats(id);
            return View(model);
        }

        public ActionResult EditSeatsRow(int roomId, int rowNr)
        {
            SeatsRow sr = repository.GetSeatsRow(roomId, rowNr);
            return View(sr);
        }

        [HttpPost]
        public ActionResult EditSeatsRow(SeatsRow model)
        {
            if(ModelState.IsValid)
            {
                repository.UpdateSeatsRow(model);
                repository.Save();
                return RedirectToAction("RoomSeats", new { id = model.RoomId });
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult CreateSeatsRow(int roomId)
        {
            SeatsRow sr = new SeatsRow();
            sr.RoomId = roomId;
            return View(sr);
        }

        [HttpPost]
        public ActionResult CreateSeatsRow(SeatsRow model)
        {
            if(repository.SeatsRowExists(model.RoomId,model.RowNumber))
            {
                ModelState.AddModelError("RowNumber","Rząd o podanym numerze już istnieje");
            }
            if(ModelState.IsValid)
            {
                repository.AddSeatsRow(model);
                repository.Save();
                return RedirectToAction("RoomSeats",new { id = model.RoomId });
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult DeleteSeatsRow(int roomId, int rowNr)
        {
            SeatsRow sr = repository.GetSeatsRow(roomId, rowNr);
            return View(sr);
        }

        [HttpPost, ActionName("DeleteSeatsRow")]
        public ActionResult DeleteRoomConfirmed(int roomId, int rowNr)
        {
            repository.DeleteSeatsRow(roomId, rowNr);
            repository.Save();
            return RedirectToAction("RoomSeats",new {id = roomId});
        }

        #endregion

        #region MOVIES
        public ActionResult Movies()
        {
            var types = repository.GetMovieTypes();
            ViewBag.movieGenre = new SelectList(types);

            

            return View(repository.GetMovies());
        }

        [HttpPost]
        public ActionResult Movies(string SearchString, string movieGenre)
        {
            var movies = repository.GetMovies();

            var types = repository.GetMovieTypes();
            ViewBag.movieGenre = new SelectList(types);

            if(!String.IsNullOrEmpty(SearchString))
            {
                movies = movies.Where(m => m.NazwaFilmuPL.Contains(SearchString));
            }
            if(String.IsNullOrEmpty(movieGenre))
            {
                return View(movies);
            }
            else
            {
                return View(movies.Where(m => m.RodzajFilmu == movieGenre));
            }

            
        }

        public ActionResult CreateMovie()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateMovie(FILM m)
        {
            if(ModelState.IsValid)
            {
                repository.AddMovie(m);
                repository.Save();
                return RedirectToAction("Movies");
            }
            else
            {
                return View(m);
            }
        }

        public ActionResult EditMovie(int id)
        {
            FILM m = repository.GetMovieById(id);
            return View(m);
        }

        [HttpPost]
        public ActionResult EditMovie(FILM model)
        {
            if(ModelState.IsValid)
            {
                repository.UpdateMovie(model);
                repository.Save();
                return RedirectToAction("Movies");
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult DeleteMovie(int id)
        {
            FILM m = repository.GetMovieById(id);
            return View(m);
        }

        [HttpPost,ActionName("DeleteMovie")]
        public ActionResult DeleteMovieConfirmed(int id)
        {
            repository.DeleteMovie(id);
            repository.Save();
            return RedirectToAction("Movies");
        }

        public ActionResult MovieDetails(int id)
        {
            FILM m = repository.GetMovieById(id);
            return View(m);
        }
        #endregion

        #region TICKETS

        public ActionResult Tickets()
        {
            return View(repository.GetTickets());
        }


        public ActionResult CreateTicket()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateTicket(BILET t)
        {
            if (ModelState.IsValid)
            {
                repository.AddTicket(t);
                repository.Save();
                return RedirectToAction("Tickets");
            }
            else
            {
                return View(t);
            }
        }

        public ActionResult EditTicket(int id)
        {
            BILET t = repository.GetTicketById(id);
            return View(t);
        }

        [HttpPost]
        public ActionResult EditTicket(BILET model)
        {
            if (ModelState.IsValid)
            {
                repository.UpdateTicket(model);
                repository.Save();
                return RedirectToAction("Tickets");
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult DeleteTicket(int id)
        {
            BILET t = repository.GetTicketById(id);
            return View(t);
        }

        [HttpPost, ActionName("DeleteTicket")]
        public ActionResult DeleteTicketConfirmed(int id)
        {
            repository.DeleteTicket(id);
            repository.Save();
            return RedirectToAction("Tickets");
        }

        public ActionResult TicketDetails(int id)
        {
            BILET t = repository.GetTicketById(id);
            return View(t);
        }

        #endregion

        #region SEANCES

        public ActionResult Seances()
        {
            var roomNames = repository.GetRoomNames();
            var movieTypes = repository.GetMovieTypes();

            ViewBag.room = new SelectList(roomNames);
            ViewBag.movieGenre = new SelectList(movieTypes);
            return View(repository.GetSeances());
        }

        [HttpPost]
        public ActionResult Seances(string movieGenre, string room, string movie, string FromDate, string ToDate)
        {
            var seances = repository.GetSeances();

            if(!String.IsNullOrEmpty(movieGenre))
            {
                seances = seances.Where(s => s.FILM.RodzajFilmu == movieGenre);
            }
            if(!String.IsNullOrEmpty(room))
            {
                seances = seances.Where(s => s.SALA.NazwaSali == room);
            }
            if(!String.IsNullOrEmpty(movie))
            {
                seances = seances.Where(s => s.FILM.NazwaFilmuPL.Contains(movie));
            }
            if(!String.IsNullOrEmpty(FromDate))
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
            var roomNames = repository.GetRoomNames();
            var movieTypes = repository.GetMovieTypes();

            ViewBag.room = new SelectList(roomNames);
            ViewBag.movieGenre = new SelectList(movieTypes);
            return View(seances);
        }
        
        public ActionResult CreateSeance()
        {
            List<SelectListItem> roomSelectList = new List<SelectListItem>();
            var roomsDict = repository.GetRoomNamesAndIds();
            foreach(KeyValuePair<int, string> el in roomsDict)
            {
                roomSelectList.Add(new SelectListItem { Text = el.Value, Value = el.Key.ToString() });
            }

            List<SelectListItem> movieSelectList = new List<SelectListItem>();
            var moviesDict = repository.GetMovieTitlesAndIds();
            foreach (KeyValuePair<int, string> el in moviesDict)
            {
                movieSelectList.Add(new SelectListItem { Text = el.Value, Value = el.Key.ToString() });
            }
            ViewBag.Room = roomSelectList;
            ViewBag.Movie = movieSelectList;
            return View();
        }

        [HttpPost]
        public ActionResult CreateSeance(SEANS model)
        {
            if(ModelState.IsValid)
            {
                int roomId = Int32.Parse(Request.Form["Room"]);
                int movieId = Int32.Parse(Request.Form["Movie"]);
                model.FK_IdSali = roomId;
                model.FK_IdFilmu = movieId;
                repository.AddSeance(model);
                repository.Save();
                return RedirectToAction("Seances");
            }
            else
            {
                List<SelectListItem> roomSelectList = new List<SelectListItem>();
                var roomsDict = repository.GetRoomNamesAndIds();
                foreach (KeyValuePair<int, string> el in roomsDict)
                {
                    roomSelectList.Add(new SelectListItem { Text = el.Value, Value = el.Key.ToString() });
                }

                List<SelectListItem> movieSelectList = new List<SelectListItem>();
                var moviesDict = repository.GetMovieTitlesAndIds();
                foreach (KeyValuePair<int, string> el in moviesDict)
                {
                    movieSelectList.Add(new SelectListItem { Text = el.Value, Value = el.Key.ToString() });
                }
                ViewBag.Room = roomSelectList;
                ViewBag.Movie = movieSelectList;
                return View(model);
            }
            
        }

        public ActionResult EditSeance(int id)
        {
            SEANS s = repository.GetSeanceById(id);
            List<SelectListItem> roomSelectList = new List<SelectListItem>();
            var roomsDict = repository.GetRoomNamesAndIds();
            foreach (KeyValuePair<int, string> el in roomsDict)
            {

                roomSelectList.Add(new SelectListItem { Text = el.Value, Value = el.Key.ToString() });
            }

            List<SelectListItem> movieSelectList = new List<SelectListItem>();
            var moviesDict = repository.GetMovieTitlesAndIds();
            foreach (KeyValuePair<int, string> el in moviesDict)
            {
                movieSelectList.Add(new SelectListItem { Text = el.Value, Value = el.Key.ToString() });
            }
            roomSelectList.FirstOrDefault(rs => Int32.Parse(rs.Value) == s.FK_IdSali).Selected = true;
            movieSelectList.FirstOrDefault(ms => Int32.Parse(ms.Value) == s.FK_IdFilmu).Selected = true;
            ViewBag.Room = roomSelectList;
            ViewBag.Movie = movieSelectList;
            return View(s);

        }

        [HttpPost]
        public ActionResult EditSeance(SEANS model)
        {
            int roomId = Int32.Parse(Request.Form["Room"]);
            int movieId = Int32.Parse(Request.Form["Movie"]);
            if(ModelState.IsValid)
            {
                
                model.FK_IdSali = roomId;
                model.FK_IdFilmu = movieId;
                repository.UpdateSeance(model);
                repository.Save();
                return RedirectToAction("Seances");
            }
            else
            {
                List<SelectListItem> roomSelectList = new List<SelectListItem>();
                var roomsDict = repository.GetRoomNamesAndIds();
                foreach (KeyValuePair<int, string> el in roomsDict)
                {

                    roomSelectList.Add(new SelectListItem { Text = el.Value, Value = el.Key.ToString() });
                }

                List<SelectListItem> movieSelectList = new List<SelectListItem>();
                var moviesDict = repository.GetMovieTitlesAndIds();
                foreach (KeyValuePair<int, string> el in moviesDict)
                {
                    movieSelectList.Add(new SelectListItem { Text = el.Value, Value = el.Key.ToString() });
                }
                roomSelectList.FirstOrDefault(rs => Int32.Parse(rs.Value) == roomId).Selected = true;
                movieSelectList.FirstOrDefault(ms => Int32.Parse(ms.Value) == movieId).Selected = true;
                ViewBag.Room = roomSelectList;
                ViewBag.Movie = movieSelectList;
                return View(model);
            }
        }

        public ActionResult DeleteSeance(int id)
        {
            SEANS s = repository.GetSeanceById(id);
            List<SelectListItem> roomSelectList = new List<SelectListItem>();
            var roomsDict = repository.GetRoomNamesAndIds();
            foreach (KeyValuePair<int, string> el in roomsDict)
            {

                roomSelectList.Add(new SelectListItem { Text = el.Value, Value = el.Key.ToString() });
            }

            List<SelectListItem> movieSelectList = new List<SelectListItem>();
            var moviesDict = repository.GetMovieTitlesAndIds();
            foreach (KeyValuePair<int, string> el in moviesDict)
            {
                movieSelectList.Add(new SelectListItem { Text = el.Value, Value = el.Key.ToString() });
            }
            roomSelectList.FirstOrDefault(rs => Int32.Parse(rs.Value) == s.FK_IdSali).Selected = true;
            movieSelectList.FirstOrDefault(ms => Int32.Parse(ms.Value) == s.FK_IdFilmu).Selected = true;
            ViewBag.Room = roomSelectList;
            ViewBag.Movie = movieSelectList;
            return View(s);
        }

        [HttpPost,ActionName("DeleteSeance")]
        public ActionResult DeleteSeanceConfirmed(int id)
        {
            SEANS s = repository.GetSeanceById(id);
            repository.DeleteSeance(s);
            repository.Save();
            return RedirectToAction("Seances");
        }

        public ActionResult SeanceDetails(int id)
        {
            SEANS s = repository.GetSeanceById(id);
            return View(s);
        }

        #endregion

        #region BOOKINGS

        public ActionResult Bookings()
        {
            var bookings = repository.GetBookings();
            return View(bookings);
        }

        public ActionResult BookingDetails(int id)
        {
            WYKUP_BILET b = repository.GetBookingById(id);
            return View(b);
        }

        public ActionResult DeleteBooking(int id)
        {
            WYKUP_BILET b = repository.GetBookingById(id);
            return View(b);
        }

        [HttpPost,ActionName("DeleteBooking")]
        public ActionResult DeleteBookingConfirmed(int id)
        {
            repository.DeleteBooking(id);
            repository.Save();
            return RedirectToAction("Bookings");
        }

        #endregion

        #region EMPLOYEES

        public ActionResult EmployeeTypes()
        {
            return View(repository.GetEmployeeTypes());
        }

        public ActionResult EmployeeTypeDetails(int id)
        {
            ETAT et = repository.GetEmployeeTypeById(id);
            return View(et);
        }

        public ActionResult EditEmployeeType(int id)
        {
            ETAT et = repository.GetEmployeeTypeById(id);
            return View(et);
        }

        [HttpPost]
        public ActionResult EditEmployeeType(ETAT et)
        {
            if(ModelState.IsValid)
            {
                repository.UpdateEmployeeType(et);
                repository.Save();
                return RedirectToAction("EmployeeTypes");
            }
            else
            {
                return View(et);
            }
        }

        public ActionResult DeleteEmployeeType(int id)
        {
            ETAT et = repository.GetEmployeeTypeById(id);
            return View(et);
        }

        [HttpPost,ActionName("DeleteEmployeeType")]
        public ActionResult DeleteEmployeeTypeConfirmed(int id)
        {
            repository.DeleteEmployeeType(id);
            repository.Save();
            return RedirectToAction("EmployeeTypes");
        }

        public ActionResult CreateEmployeeType()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateEmployeeType(ETAT et)
        {
            if(ModelState.IsValid)
            {
                repository.AddEmployeeType(et);
                repository.Save();
                return RedirectToAction("EmployeeTypes");
            }
            else
            {
                return View(et);
            }
        }

        public ActionResult Employees()
        {
            return View(repository.GetEmployees());
        }

        public ActionResult EmployeeDetails(int id)
        {
            return View(repository.GetEmployeeById(id));
        }

        public ActionResult CreateEmployee()
        {
            var etList = repository.GetEmployeeTypeIdsAndNames();
            var cinList = repository.GetCinemaNamesAndIds();
            List<SelectListItem> etypes = new List<SelectListItem>();
            List<SelectListItem> cinemas = new List<SelectListItem>();
            foreach(KeyValuePair<int,string> el in etList)
            {
                etypes.Add( new SelectListItem {Text = el.Value, Value = el.Key.ToString() });
            }
            foreach( KeyValuePair<int,string> el in cinList)
            {
                cinemas.Add(new SelectListItem { Text = el.Value, Value = el.Key.ToString() } );
            }
            ViewBag.Cinema = cinemas;
            ViewBag.EmployeeType = etypes;
            return View();
        }

        [HttpPost]
        public ActionResult CreateEmployee(PRACOWNIK e)
        {
            if(ModelState.IsValid)
            {
                int cinemaId = Int32.Parse(Request.Form["Cinema"]);
                int etId = Int32.Parse(Request.Form["EmployeeType"]);
                e.FK_IdKina = cinemaId;
                e.FK_IdEtatu = etId;
                repository.AddEmployee(e);
                repository.Save();
                return RedirectToAction("Employees");
            }
            else
            {
                var etList = repository.GetEmployeeTypeIdsAndNames();
                var cinList = repository.GetCinemaNamesAndIds();
                List<SelectListItem> etypes = new List<SelectListItem>();
                List<SelectListItem> cinemas = new List<SelectListItem>();
                foreach (KeyValuePair<int, string> el in etList)
                {
                    etypes.Add(new SelectListItem { Text = el.Value, Value = el.Key.ToString() });
                }
                foreach (KeyValuePair<int, string> el in cinList)
                {
                    cinemas.Add(new SelectListItem { Text = el.Value, Value = el.Key.ToString() });
                }
                ViewBag.Cinema = cinemas;
                ViewBag.EmployeeType = etypes;
                return View();
            }
        }

        public ActionResult EditEmployee(int id)
        {
            PRACOWNIK e = repository.GetEmployeeById(id);
            var etList = repository.GetEmployeeTypeIdsAndNames();
            var cinList = repository.GetCinemaNamesAndIds();
            List<SelectListItem> etypes = new List<SelectListItem>();
            List<SelectListItem> cinemas = new List<SelectListItem>();
            foreach (KeyValuePair<int, string> el in etList)
            {
                etypes.Add(new SelectListItem { Text = el.Value, Value = el.Key.ToString() });
            }
            foreach (KeyValuePair<int, string> el in cinList)
            {
                cinemas.Add(new SelectListItem { Text = el.Value, Value = el.Key.ToString() });
            }
            etypes.FirstOrDefault(el => Int32.Parse(el.Value) == e.FK_IdEtatu).Selected = true;
            cinemas.FirstOrDefault(c => Int32.Parse(c.Value) == e.FK_IdKina).Selected = true;
            ViewBag.Cinema = cinemas;
            ViewBag.EmployeeType = etypes;
            return View(e);
        }

        [HttpPost]
        public ActionResult EditEmployee(PRACOWNIK e)
        {
            int cinemaId = Int32.Parse(Request.Form["Cinema"]);
            int etId = Int32.Parse(Request.Form["EmployeeType"]);
            if(ModelState.IsValid)
            {
                e.FK_IdEtatu = etId;
                e.FK_IdKina = cinemaId;
                repository.UpdateEmployee(e);
                repository.Save();
                return RedirectToAction("Employees");
            }
            else
            {
                var etList = repository.GetEmployeeTypeIdsAndNames();
                var cinList = repository.GetCinemaNamesAndIds();
                List<SelectListItem> etypes = new List<SelectListItem>();
                List<SelectListItem> cinemas = new List<SelectListItem>();
                foreach (KeyValuePair<int, string> el in etList)
                {
                    etypes.Add(new SelectListItem { Text = el.Value, Value = el.Key.ToString() });
                }
                foreach (KeyValuePair<int, string> el in cinList)
                {
                    cinemas.Add(new SelectListItem { Text = el.Value, Value = el.Key.ToString() });
                }
                etypes.FirstOrDefault(el => Int32.Parse(el.Value) == etId).Selected = true;
                cinemas.FirstOrDefault(c => Int32.Parse(c.Value) == cinemaId).Selected = true;
                ViewBag.Cinema = cinemas;
                ViewBag.EmployeeType = etypes;
                return View(e);
            }
        }

        public ActionResult DeleteEmployee(int id)
        {
            PRACOWNIK e = repository.GetEmployeeById(id);
            return View(e);
        }

        [HttpPost,ActionName("DeleteEmployee")]
        public ActionResult DeleteEmployeeConfirmed(int id)
        {
            repository.DeleteEmployee(id);
            repository.Save();
            return RedirectToAction("Employees");
        }

        #endregion

        #region ROOMLOAD

        public ActionResult RoomLoad(int id)
        {
            var vm = repository.GetRoomLoad(id);
            return View(vm);
        }

        #endregion
    }
}
