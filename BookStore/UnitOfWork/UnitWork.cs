using BookStore.Models;
using BookStore.Repos;

namespace BookStore.UnitOfWork
{
    public class UnitWork
    {
        GenericRepo<Book> generic_BookRepo;
        GenericRepo<Author> generic_AuthorRepo;
        GenericRepo<Order> generic_OrderRepo;
        GenericRepo<Catalog> generic_CatalogRepo;
        

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

        public GenericRepo<Order> Generic_OrderRepo
        {
            get
            {
                if (generic_OrderRepo == null)
                {

                    generic_OrderRepo = new GenericRepo<Order>(db);
                }
                return generic_OrderRepo;
            }
        }


        public GenericRepo<Catalog> Generic_CatalogRepo
        {
            get
            {
                if (generic_CatalogRepo == null)
                {

                    generic_CatalogRepo = new GenericRepo<Catalog>(db);
                }
                return generic_CatalogRepo;
            }
        }
        public void Save()
        {

            db.SaveChanges();
        }

    }
}
