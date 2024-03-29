﻿using System.ComponentModel.DataAnnotations;

namespace Plonks.Cards.Models
{
    public class EditCommentRequest
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
