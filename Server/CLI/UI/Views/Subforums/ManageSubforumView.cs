using Entities;
using RepositoryContracts;

namespace CLI.UI.Views.Forums;

public class ManageSubforumView : IView
{
    private readonly ViewHandler _viewHandler;
    private readonly ISubforumRepository _subforumRepository;
    
    public ManageSubforumView(ViewHandler viewHandler, ISubforumRepository subforumRepository)
    {
        _viewHandler = viewHandler;
        _subforumRepository = subforumRepository;
    }
    public void Display()
    {
        Console.WriteLine("-- Manage Subforum --");
        Console.WriteLine("To change a subforum name, write id of subforum, command name and the new name");
        Console.WriteLine("Example: [sf id] name [New Name]");
        Console.WriteLine("To change a subforum moderator, write the id of subforum, command mod and the id of the new moderator");
        Console.WriteLine("Example: [sf id] mod [new mod id]");
        Console.WriteLine("Type 'exit' to exit.");
        Console.WriteLine("---------------------");
    }

    public void HandleInput(string input)
    {
        try
        {
            switch (input.ToLower())
            {
                case "exit":
                    break;
                default:
                    var splitInput = input.Split(" ");
                    var id = int.Parse(splitInput[0]);
                    var command = splitInput[1].ToLower();
                    Subforum subforum;
                    if (command == "name")
                    {
                        int nameStartIndex = splitInput[0].Length + splitInput[1].Length + 2;
                        _subforumRepository.UpdateAsync(new Subforum()
                        {
                            SubforumId = id,
                            Name = input.Substring(nameStartIndex),
                            ModeratorId = _subforumRepository.GetSingleAsync(id).Result.ModeratorId
                        });
                    }
                    else if (command == "mod")
                    {
                        _subforumRepository.UpdateAsync(new Subforum()
                        {
                            SubforumId = id,
                            Name = _subforumRepository.GetSingleAsync(id).Result.Name,
                            ModeratorId = int.Parse(input.Split(" ")[3])
                        });
                    }
                    break;
            }
            _viewHandler.GoToMainMenu();
        }
        catch (Exception e)
        {
            _viewHandler.GoToView(Views.ManageSubforum);
        }
    }
}