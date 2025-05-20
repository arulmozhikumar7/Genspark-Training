public class Post
{
    public string Caption { get; set; }
    public int Likes { get; set; }

    public Post(string caption = "", int likes = 0)
    {
        Caption = caption ?? ""; 
        Likes = likes;
    }

    public void Display(int postNumber)
    {
        Console.WriteLine($"Post {postNumber} - Caption : {Caption} | Likes : {Likes}");
    }
}

public class InstagramApp
{
    private Post[][] _userPosts = null!;

    public void Run()
    {
        int userCount = GetNumberOfUsers();
        _userPosts = new Post[userCount][];

        for (int i = 0; i < userCount; i++)
        {
            _userPosts[i] = GetPostsForUser(i + 1);
        }

        DisplayAllPosts();
    }

    private int GetNumberOfUsers()
    {
        int userCount;
        while (true)
        {
            Console.Write("Enter number of users: ");
            string? input = Console.ReadLine()?.Trim();

            if (int.TryParse(input, out userCount) && userCount > 0)
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a positive integer number.");
            }
        }
        return userCount;
    }

    private Post[] GetPostsForUser(int userNumber)
    {
        int postCount;
        while (true)
        {
            Console.Write($"\nUser {userNumber}: How many posts? ");
            string? input = Console.ReadLine()?.Trim();

            if (int.TryParse(input, out postCount) && postCount >= 0)
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a non-negative integer number.");
            }
        }

        Post[] posts = new Post[postCount];

        for (int i = 0; i < postCount; i++)
        {
            posts[i] = CreatePost(i + 1);
        }

        return posts;
    }
    private int ReadNonNegativeInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(input))
                return 0;

            if (int.TryParse(input, out int value) && value >= 0)
                return value;

            Console.WriteLine("Invalid input. Please enter a non-negative integer or leave it empty.");
        }
    }


    private Post CreatePost(int postNumber)
    {
        Console.Write($"Enter caption for post {postNumber}: ");
        string caption = Console.ReadLine()?.Trim() ?? ""; 
        int likes = ReadNonNegativeInt("Enter likes: ");
        return new Post(caption, likes);
    }

    private void DisplayAllPosts()
    {
        Console.WriteLine("\n--- Displaying Instagram Posts ---");

        for (int i = 0; i < _userPosts.Length; i++)
        {
            Console.WriteLine($"User {i + 1}:");
            Post[] posts = _userPosts[i];

            for (int j = 0; j < posts.Length; j++)
            {
                posts[j].Display(j + 1);
            }

            Console.WriteLine();
        }
    }
}

class Program
{
    static void Main()
    {
        InstagramApp app = new InstagramApp();
        app.Run();
    }
}
