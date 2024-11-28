using BookStore.Models;
using BookStore.Repos;

namespace BookStore.UnitOfWork
{
    public class UnitWork
    {
        GenericRepo<Book> generic_BookRepo;

        BookShopDBContext db;
        public UnitWork(BookShopDBContext db) 
        { 
        
            this.db = db;  
        }

        public GenericRepo<Book> Generic_BookRepo
        {
            get
            {
                if (generic_BookRepo == null)
                {

                    generic_BookRepo = new GenericRepo<Book>(db);
                }
                return generic_BookRepo;
            }
        }
        public void Save()
        {

            db.SaveChanges();
        }

    }
}
