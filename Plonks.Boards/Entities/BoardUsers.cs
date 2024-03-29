﻿using System.ComponentModel.DataAnnotations;

namespace Plonks.Boards.Entities
{
    public class BoardUsers
    {

        [Required]
        public Guid BoardId { get; set; }

        public Board? Board { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public User? User { get; set; }

        public bool Favorited { get; set; } = false;
    }
}
