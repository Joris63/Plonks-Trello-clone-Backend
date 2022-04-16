using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plonks.Shared.Entities
{
    public class SharedCard
    {
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public Guid ListId { get; set; }

        public int Order { get; set; }

        public bool HasDescription { get; set; }

        public int? CommentAmount { get; set; }

        public int? ChecklistItems { get; set; }

        public int? CompletedChecklistItems { get; set; }

        public DateTime? CreatedAt { get; set; }

        public List<SharedUser>? Users { get; set; } = new List<SharedUser>();
    }
}
