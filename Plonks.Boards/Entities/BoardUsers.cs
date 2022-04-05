namespace Plonks.Boards.Entities
{
    public class BoardUsers
    {
        public Guid UserId { get; set; }

        public Guid BoardId { get; set; }

        public Board Board { get; set; }


        public BoardUsers()
        {

        }

        public BoardUsers(Guid boardId, Guid userID)
        {
            BoardId = boardId;
            UserId = userID;
        }
    }
}
