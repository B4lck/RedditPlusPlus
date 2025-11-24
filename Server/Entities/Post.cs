namespace Entities;

public class Post
{
    public int PostId { get; set; }
    public string? Title { get; set; } // Kommentarer har ikke titler
    public string Content { get; set; }
    public User Author { get; set; }
    public Subforum InSubforum { get; set; }
    public Post? CommentedOnPost { get; set; } // null hvis det er et standard post, ellers reference til postet som kommentaren ligger på.
}