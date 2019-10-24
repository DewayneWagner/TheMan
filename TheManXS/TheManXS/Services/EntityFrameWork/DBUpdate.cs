using System;
using System.Collections.Generic;
using System.Text;
using TheManXS.Model.Services.EntityFrameWork;

namespace TheManXS.Services.EntityFrameWork
{
    class DBUpdate
    {
        public DBUpdate()
        {
            new DBContext().DeleteDatabase();
        }

        private void UpdateDB()
        {
            using (DBContext db = new DBContext())
            {
                
            }
        }
        
    }
}
