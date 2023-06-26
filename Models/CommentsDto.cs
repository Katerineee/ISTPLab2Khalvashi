namespace LAB2ISTPP.Models
{
    public class CommentsDto
    {
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int NewsId { get; set; }
        public int UserId { get; set; }
    }

}
