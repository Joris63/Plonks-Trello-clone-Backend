﻿using System.ComponentModel.DataAnnotations;

namespace Plonks.Lists.Entities
{
    public class BoardList
    {
        [Key]
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public Guid BoardId { get; set; }

        public Board? Board { get; set; }

        public bool Archived { get; set; }

        public List<Card> Cards { get; set; } = new List<Card>();


        public BoardList(string title, Guid boardId)
        {
            Id = Guid.NewGuid();
            Title = title;
            BoardId = boardId;
            Archived = false;
        }
    }
}
