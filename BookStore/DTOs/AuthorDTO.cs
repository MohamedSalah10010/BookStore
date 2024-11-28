namespace BookStore.DTOs
{
    public class AuthorDTO
    {

        public int Author_Id { get; set; } 
        public string AuthorFullName { get; set; }    
        public string AuthorBIO { get; set; } 
        public int Authors_NumberOfBooks { get; set; }  
        public int Author_Age { get; set; }

       
    }
}
