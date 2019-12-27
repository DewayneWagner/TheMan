using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheManXS.Model.Main;
using TheManXS.Model.Services.EntityFrameWork;
using TheManXS.ViewModel.Services;
using EFCore.BulkExtensions;

namespace TheManXS.Services.EntityFrameWork
{
    public class WriteMapToDB
    {
        private List<SQ> _mapList;
        private PageService _pageService;
        private const int MaxMethods = 3;

        private DateTime _startTime;

        public WriteMapToDB(List<SQ> mapList)
        {
            _mapList = mapList;
            _pageService = new PageService();

            RunScenarios();
            DisplayResults();
        }
        private List<TimeSpan> methodTimer { get; set; } = new List<TimeSpan>();

        private void RunScenarios()
        {
            for (int i = 0; i < MaxMethods; i++)
            {
                using (DBContext db = new DBContext())
                {
                    var _existingSQs = db.SQ.ToList();
                    if (_existingSQs != null) { db.SQ.RemoveRange(_existingSQs); }
                    db.SaveChanges();
                }
                _startTime = DateTime.Now;

                switch (i)
                {
                    case 0:
                        WriteMethod1();
                        break;
                    case 1:
                        WriteMethod2();
                        break;
                    case 2:
                        WriteMethod3();
                        break;
                    case 3:
                        WriteMethod4();
                        break;
                    default:
                        break;
                }
                EndTimer();
            }
        }

        private void WriteMethod1()
        {
            using (DBContext db = new DBContext())
            {
                db.SQ.AddRange(_mapList);
                db.SaveChanges();
            }
        }
        private void WriteMethod2()
        {
            using (DBContext db = new DBContext())
            {
                DbContextBulkExtensions.BulkInsert<SQ>(db, _mapList);
                db.SaveChanges();
            }
        }
        private void WriteMethod3()
        {
            using (DBContext db = new DBContext())
            {
                db.BulkInsert<SQ>(_mapList);
                db.SaveChanges();
            }
        }
        private void WriteMethod4()
        {
            using (DBContext db = new DBContext())
            {
                for (int i = 0; i < _mapList.Count; i++)
                {
                    db.Add(_mapList[i]);
                    db.SaveChanges();
                }
            }
        }
        private async void DisplayResults()
        {
            string message = null;
            TimeSpan interval;

            for (int i = 0; i < MaxMethods; i++)
            {
                interval = TimeSpan.ParseExact(Convert.ToString(methodTimer[i]), "c", null);
                message += "Method " + Convert.ToString(i + 1) + ": " + interval + "\n";
            }
            await _pageService.DisplayAlert("Timing", message);
        }
        private void EndTimer() => methodTimer.Add(DateTime.Now - _startTime); 
    }
}
