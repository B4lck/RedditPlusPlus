namespace Entities;

public class Post
{
    public int PostId { get; set; }
    public string? Title { get; set; } // Kommentarer har ikke titler
    public string Content { get; set; }
    public int AuthorId { get; set; }
    public int SubforumId { get; set; }
    public int? CommentedOnPostId { get; set; } // null hvis det er et standard post, eller id'et på postet som kommentaren ligger på.
}