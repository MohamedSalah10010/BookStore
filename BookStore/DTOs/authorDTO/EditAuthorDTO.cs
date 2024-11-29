﻿using System.ComponentModel.DataAnnotations;

namespace BookStore.DTOs.authorDTO
{
    public class EditAuthorDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string AuthorFullName { get; set; }
        public string? AuthorBIO { get; set; }
        [Required]
        public int Authors_NumberOfBooks { get; set; }
        [Required]
        public int Author_Age { get; set; }

    }
}
