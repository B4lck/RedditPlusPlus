using CLI.UI;
using InMemoryRepositories;
using RepositoryContracts;

Console.WriteLine("Starting CLI app!");
IPostRepository posts = new PostInMemoryRepository();
IUserRepository users = new UserInMemoryRepository();
ISubforumRepository subforums = new SubforumInMemoryContract();
IReactionRepository reactions = new ReactionInMemoryRepository();

CLIApp app = new CLIApp(posts, users, subforums, reactions);
await app.StartAsync();