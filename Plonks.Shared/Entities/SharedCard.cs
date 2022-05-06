using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plonks.Shared.Entities
{
    public class SharedCard
    {
        public Guid Id { get; set; } = Guid.Empty;

        public string? Title { get; set; } = null;

        public Guid ListId { get; set; } = Guid.Empty;

        public int Order { get; set; } = -1;

        public bool Archived { get; set; } = false;

        public bool HasDescription { get; set; } = false;

        public int? CommentAmount { get; set; } = -1;

        public int? ChecklistItems { get; set; } = -1;

        public int? CompletedChecklistItems { get; set; } = -1;

        public DateTime? CreatedAt { get; set; }

        public List<SharedUser>? Users { get; set; } = null;
    }
}
