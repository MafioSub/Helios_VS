using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Helios.Models
{
    public class HeliosRepository : IDisposable
    {
        private HeliosEntities db = new HeliosEntities();

        #region ROOMS_SEATS

        public IEnumerable<SALA> GetAllRooms()
        {
            return db.SALA;
        }

        public SALA GetRoomById(int id)
        {
            return db.SALA.FirstOrDefault(s => s.IdSali == id);
        }

        public SALA AddRoom(SALA s)
        {
            db.SALA.Add(s);
            return s;
        }

        public SALA UpdateRoom(SALA s)
        {
            db.Entry(s).State = EntityState.Modified;
            return s;
        }

        public void DeleteRoom(int id)
        {
            SALA toDel = db.SALA.Find(id);
            db.SALA.Remove(toDel);
        }

        public IEnumerable<string> GetRoomNames()
        {
            var retList = new List<string>();
            foreach (SALA r in db.SALA)
            {
                retList.Add(r.NazwaSali);
            }
            return retList;
        }

        public RoomSeats GetRoomSeats(int roomId)
        {
            SALA room = db.SALA.Find(roomId);
            RoomSeats roomSeats = new RoomSeats();
            roomSeats.Room = room;
            var seats = from m in db.MIEJSCE
                        where m.FK_IdSali == roomId
                        select m;
            var query2 = from s in seats
                         orderby s.Rzad
                         select s.Rzad;
            List<int> rowNumbers = query2.Distinct().ToList();

            roomSeats.Rows = new List<SeatsRow>();

            foreach (int n in rowNumbers)
            {
                SeatsRow row = new SeatsRow();
                row.RowNumber = n;
                row.Seats = seats.Where(s => s.Rzad == n).ToList();
                row.SeatsCount = seats.Where(s => s.Rzad == n).Count();
                roomSeats.Rows.Add(row);
            }

            roomSeats.RowsCount = roomSeats.Rows.Count();
            roomSeats.SeatsCount = seats.Count();
            return roomSeats;
        }

        public SeatsRow GetSeatsRow(int roomId, int rowNr)
        {
            RoomSeats rs = GetRoomSeats(roomId);
            SeatsRow row = rs.Rows.FirstOrDefault(r => r.RowNumber == rowNr);
            row.RoomId = roomId;
            return row;
        }

        public SeatsRow UpdateSeatsRow(SeatsRow sr)
        {
            var seats = from m in db.MIEJSCE
                        where m.FK_IdSali == sr.RoomId && m.Rzad == sr.RowNumber
                        orderby m.Miejsce1
                        select m;
            List<MIEJSCE> backup = seats.ToList();
            if (sr.SeatsCount == seats.Count())
            {
                return sr;
            }
            else if (sr.SeatsCount < seats.Count())
            {
                for (int i = seats.Count() - 1; i > sr.SeatsCount - 1; i--)
                {
                    db.MIEJSCE.Remove(seats.ToList().ElementAt(i));
                }
            }
            else if (sr.SeatsCount > seats.Count())
            {
                for (int i = seats.Count() + 1; i <= sr.SeatsCount; i++)
                {
                    db.MIEJSCE.Add(new MIEJSCE { FK_IdSali = sr.RoomId, Miejsce1 = i, Rzad = sr.RowNumber });
                }
            }
            return sr;
        }

        public bool SeatsRowExists(int roomId, int rowNr)
        {
            return db.MIEJSCE.Any(m => m.FK_IdSali == roomId && m.Rzad == rowNr);
        }

        public SeatsRow AddSeatsRow(SeatsRow sr)
        {
            for (int i = 1; i <= sr.SeatsCount; i++)
            {
                db.MIEJSCE.Add(new MIEJSCE { FK_IdSali = sr.RoomId, Rzad = sr.RowNumber, Miejsce1 = i });
            }
            return sr;
        }

        public void DeleteSeatsRow(int roomId, int rowNr)
        {
            var query = from m in db.MIEJSCE
                        where m.Rzad == rowNr && m.FK_IdSali == roomId
                        select m;
            foreach (MIEJSCE m in query.ToList())
            {
                db.MIEJSCE.Remove(m);
            }
        }

        public Dictionary<int, string> GetRoomNamesAndIds()
        {
            var retDict = new Dictionary<int, string>();
            foreach (SALA s in db.SALA)
            {
                retDict.Add(s.IdSali, s.NazwaSali + ", klima:" + s.Klimatyzacja + ", miejsca: " + s.MIEJSCE.Count());
            }
            return retDict;
        }

        #endregion ROOMS_SEATS

        #region MOVIES

        public IQueryable<FILM> GetMovies()
        {
            return db.FILM;
        }

        public FILM GetMovieById(int id)
        {
            return db.FILM.Find(id);
        }

        public FILM AddMovie(FILM m)
        {
            db.FILM.Add(m);
            return m;
        }

        public FILM UpdateMovie(FILM m)
        {
            db.Entry(m).State = EntityState.Modified;
            return m;
        }

        public void DeleteMovie(int id)
        {
            FILM m = db.FILM.Find(id);
            db.FILM.Remove(m);
        }

        public ICollection<string> GetMovieTypes()
        {
            var types = new List<string>();

            var typesQuery = from m in db.FILM
                             orderby m.RodzajFilmu
                             select m.RodzajFilmu;
            types.AddRange(typesQuery.Distinct());
            return types;
        }

        public Dictionary<int, string> GetMovieTitlesAndIds()
        {
            var retDict = new Dictionary<int, string>();
            foreach (FILM m in db.FILM)
            {
                retDict.Add(m.IdFilmu, m.NazwaFilmuPL + " (" + m.RodzajFilmu + ") " + m.CzasTrwania);
            }
            return retDict;
        }

        #endregion MOVIES

        #region TICKETS

        public IEnumerable<BILET> GetTickets()
        {
            return db.BILET;
        }

        public BILET GetTicketById(int id)
        {
            return db.BILET.Find(id);
        }

        public BILET AddTicket(BILET t)
        {
            db.BILET.Add(t);
            return t;
        }

        public BILET UpdateTicket(BILET t)
        {
            db.Entry(t).State = EntityState.Modified;
            return t;
        }

        public void DeleteTicket(int id)
        {
            BILET t = db.BILET.Find(id);
            db.BILET.Remove(t);
        }

        public Dictionary<int, string> GetTicketNamesAndIds()
        {
            var retDict = new Dictionary<int, string>();
            foreach (BILET t in db.BILET)
            {
                retDict.Add(t.IdBiletu, t.NazwaBiletu + " " + t.Cena);
            }
            return retDict;
        }

        #endregion TICKETS

        #region SEANCES

        public IQueryable<SEANS> GetSeances()
        {
            return db.SEANS;
        }

        public SEANS GetSeanceById(int id)
        {
            return db.SEANS.Find(id);
        }

        public SEANS AddSeance(SEANS s)
        {
            db.SEANS.Add(s);
            return s;
        }

        public SEANS UpdateSeance(SEANS s)
        {
            db.Entry(s).State = EntityState.Modified;
            return s;
        }

        public void DeleteSeance(SEANS s)
        {
            db.SEANS.Remove(s);
        }

        #endregion SEANCES

        #region BOOKINGS

        public IQueryable<WYKUP_BILET> GetBookings()
        {
            return db.WYKUP_BILET;
        }



        public WYKUP_BILET GetBookingById(int id)
        {
            return db.WYKUP_BILET.Find(id);
        }

        public WYKUP_BILET GetFullBookingById(int id)
        {
            return db.WYKUP_BILET.Include("SEANS").Include("SEANS.SALA").Include("SEANS.FILM").Include("BILET").Include("MIEJSCE").FirstOrDefault(b => b.IdRezerwacji == id);
        }


        public WYKUP_BILET AddBooking(WYKUP_BILET b)
        {
            db.WYKUP_BILET.Add(b);
            return b;
        }

        public WYKUP_BILET UpdateBooking(WYKUP_BILET b)
        {
            db.Entry(b).State = EntityState.Modified;
            return b;
        }

        public void DeleteBooking(int id)
        {
            WYKUP_BILET b = db.WYKUP_BILET.Find(id);
            db.WYKUP_BILET.Remove(b);
        }

        #endregion BOOKINGS

        #region EMPLOYEES

        public IEnumerable<ETAT> GetEmployeeTypes()
        {
            return db.ETAT;
        }

        public ETAT GetEmployeeTypeById(int id)
        {
            return db.ETAT.Find(id);
        }

        public ETAT AddEmployeeType(ETAT et)
        {
            db.ETAT.Add(et);
            return et;
        }

        public ETAT UpdateEmployeeType(ETAT et)
        {
            db.Entry(et).State = EntityState.Modified;
            return et;
        }

        public void DeleteEmployeeType(int id)
        {
            ETAT et = db.ETAT.Find(id);
            db.ETAT.Remove(et);
        }

        public IQueryable<PRACOWNIK> GetEmployees()
        {
            return db.PRACOWNIK;
        }

        public PRACOWNIK AddEmployee(PRACOWNIK e)
        {
            db.PRACOWNIK.Add(e);
            return e;
        }

        public PRACOWNIK UpdateEmployee(PRACOWNIK e)
        {
            db.Entry(e).State = EntityState.Modified;
            return e;
        }

        public PRACOWNIK GetEmployeeById(int id)
        {
            return db.PRACOWNIK.Find(id);
        }

        public void DeleteEmployee(int id)
        {
            PRACOWNIK e = db.PRACOWNIK.Find(id);
            db.PRACOWNIK.Remove(e);
        }

        public Dictionary<int, string> GetEmployeeTypeIdsAndNames()
        {
            Dictionary<int, string> retDict = new Dictionary<int, string>();
            foreach (ETAT et in db.ETAT)
            {
                retDict.Add(et.IdEtatu, et.NazwaEtatu);
            }
            return retDict;
        }

        #endregion EMPLOYEES

        #region CINEMAS

        public Dictionary<int, string> GetCinemaNamesAndIds()
        {
            var retDict = new Dictionary<int, string>();
            foreach (KINO c in db.KINO)
            {
                retDict.Add(c.IdKina, c.NazwaKina);
            }
            return retDict;
        }

        #endregion CINEMAS

        #region ROOMLOADS

        public RoomLoadViewModel GetRoomLoad(int seanceId)
        {
            RoomLoadViewModel rl = new RoomLoadViewModel();
            SEANS seance = GetSeanceById(seanceId);
            SALA r = GetRoomById(seance.FK_IdSali);            
            rl.RoomId = r.IdSali;
            rl.AirConditioner = r.Klimatyzacja;
            rl.RoomName = r.NazwaSali;
            rl.SeatsCount = r.MIEJSCE.Count();

            var seats = r.MIEJSCE.OrderBy(rd => rd.Rzad).ThenBy(rd => rd.Miejsce1);
            var busySeats = from s in db.MIEJSCE
                            join b in db.WYKUP_BILET on s.IdMiejsca equals b.FK_IdMiejsca
                            join se in db.SEANS on b.FK_IdSeansu equals se.IdSeansu
                            where se.IdSeansu == seanceId
                            select s;

            var emptySeats = seats.Except(busySeats);
            rl.BusySeatsCount = busySeats.Count();
            rl.EmptySeatsCount = emptySeats.Count();
            var rows = from s in seats
                       select s.Rzad;
            var rowMaxNr = rows.Max();
            rl.RowsCount = rowMaxNr;
            var srList = new List<SeatsRow>();
            for(int i = 0; i < rowMaxNr; i++)
            {
                srList.Add(new SeatsRow());
                srList.ElementAt(i).Seats = new List<MIEJSCE>();
                srList.ElementAt(i).RowNumber = i + 1;
            }
            rl.SeatStatuses = new Dictionary<int, bool>();
            foreach( MIEJSCE s in seats)
            {
                srList.ElementAt(s.Rzad - 1).Seats.Add(s);               
            }
            foreach(MIEJSCE s in emptySeats)
            {
                rl.SeatStatuses.Add(s.IdMiejsca, false);
            }
            foreach(MIEJSCE s in busySeats)
            {
                rl.SeatStatuses.Add(s.IdMiejsca, true);
            }
            rl.Rows = srList;

            rl.MovieDuration = (System.TimeSpan) seance.FILM.CzasTrwania;
            rl.MovieTitle = seance.FILM.NazwaFilmuPL;
            rl.SeanceDate = seance.SeansData;
            rl.SeanceTime = seance.SeansGodzina;
            rl.MovieType = seance.FILM.RodzajFilmu;
            rl.SeanceId = seance.IdSeansu;

            return rl;


        }

        #endregion ROOMLOADS

        #region BOOK

        public bool IsSeatFree(int seanceId, int seatId)
        {
            return !db.WYKUP_BILET.Any(b => b.FK_IdMiejsca == seatId && b.FK_IdSeansu == seanceId);
        }

        #endregion

        #region Repository

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion Repository
    }
}