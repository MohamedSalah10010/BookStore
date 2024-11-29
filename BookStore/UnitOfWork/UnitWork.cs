using BookStore.Models;
using BookStore.Repos;

namespace BookStore.UnitOfWork
{
    public class UnitWork
    {
        GenericRepo<Book> generic_BookRepo;
        GenericRepo<Author> generic_AuthorRepo;

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
        public GenericRepo<Author> Generic_AuthorRepo
        {
            get
            {
                if (generic_AuthorRepo == null)
                {

                    generic_AuthorRepo = new GenericRepo<Author>(db);
                }
                return generic_AuthorRepo;
            }
        }
        public void Save()
        {

            db.SaveChanges();
        }

    }
}
