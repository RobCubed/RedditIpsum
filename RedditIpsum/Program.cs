using System;
using System.Collections.Generic;
using System.Linq;
using RedditSharp;
using RedditSharp.Things;

namespace RedditIpsum
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("- Welcome to Reddit Ipsum. THIS WILL REMOVE ALL CONTENT THAT YOU HAVE POSTED.");
            Console.WriteLine("- Do not use this lightly.");
            Console.WriteLine("Enter your username: ");
            string username = Console.ReadLine();
            while (string.IsNullOrEmpty(username))
            {
                Console.WriteLine("Enter your username: ");
                username = Console.ReadLine();
            }
            Console.WriteLine("Enter your password: ");
            string password = Console.ReadLine();
            while (string.IsNullOrEmpty(password))
            {
                Console.WriteLine("Enter your password: ");
                password = Console.ReadLine();
            }
            Console.WriteLine();
            Console.WriteLine("- Are you 100% sure you want to convert all of your posts to Lorem Ipsum?");
            Console.WriteLine("- Type \"I understand\" and press enter if you understand.");
            string iUnderstand = Console.ReadLine();
            if (iUnderstand != null && iUnderstand.ToLowerInvariant().Equals("i understand"))
            {
                LoremIpsum(username, password);

                Console.WriteLine("");
                Console.WriteLine("=== All Done ===");
                Console.WriteLine("=== Press 'enter' to exit ===");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Whew, that was close! Press any key to exit.");
                Console.ReadKey();
            }
        }

        public static void LoremIpsum(string username, string password)
        {
            var reddit = new Reddit();
            var user = reddit.LogIn(username, password);

            Console.WriteLine("Starting comments.");
            IEnumerable<Comment> comments = user.Comments.GetListing(1000, 250);

            int commentCount = 0;
            foreach (var comment in comments)
            {
                int length = 20;
                if (comment.Body.Length <= 20) length = comment.Body.Length;
                Console.WriteLine("Old Comment: " + comment.Body.Substring(0, length));
                comment.EditText("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.");
                Console.WriteLine("New Comment: Lorem ipsum dolor sit amet, consectetur...");
                commentCount++;
            }
            Console.WriteLine(commentCount + " comments edited.");
            Console.WriteLine("Starting self posts.");
            IEnumerable<Post> posts = user.Posts.GetListing(1000, 250);

            int postCount = 0;
            foreach (var post in posts)
            {
                if (post.IsSelfPost)
                {
                    int length = 20;
                    if (post.SelfText.Length <= 20) length = post.SelfText.Length;
                    Console.WriteLine("Old Post: " + post.SelfText.Substring(0, length));
                    post.EditText("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.");
                    Console.WriteLine("New Post: Lorem ipsum dolor sit amet, consectetur...");
                    postCount++;
                }
            }
            Console.WriteLine(postCount + " posts edited.");
            Console.WriteLine("Delete content/link/image posts? (Type YES or NO and press enter)");
            string deleteContent = Console.ReadLine();

            while (deleteContent == null || (!deleteContent.ToLowerInvariant().Equals("yes") && !deleteContent.ToLowerInvariant().Equals("no")))
            {
                Console.WriteLine("Type YES or NO");
                deleteContent = Console.ReadLine();
            }

            if (deleteContent.ToLowerInvariant().Equals("no")) return;

            posts = user.Posts.GetListing(1000, 250);
            postCount = 0;
            foreach (var post in posts)
            {
                if (!post.IsSelfPost)
                {
                    int length = 20;
                    if (post.Title.Length <= 20) length = post.Title.Length;
                    post.Del();
                    Console.WriteLine("Old Post: " + post.Title.Substring(0, length) + "... deleted");
                    postCount++;
                }
            }

            Console.WriteLine(postCount + " posts deleted.");
        }
    }

    public static class Utils
    {
        public static bool IsAny<T>(this IEnumerable<T> data)
        {
            return data != null && data.Any();
        }
    }
}
