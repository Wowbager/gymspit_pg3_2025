namespace Social;

public static class SocialNetwork
{
    public static bool AddValue(string value, string[] data, int count)
    {
        if (count >= data.Length)
        {
            Console.WriteLine("I'm afraid I can't do that.");
            return false;
        }

        data[count] = value;
        return true;
    }

    public static bool RemoveValue(string[] data, int index, int count)
    {
        if (index < 0 || index >= count)
        {
            Console.WriteLine("I'm afraid I can't do that.");
            return false;
        }

        for (int i = index; i < count - 1; i += 1)
        {
            data[i] = data[i + 1];
        }
        data[count - 1] = "";
        return true;
    }

    public static void AddUser(string username, string[] users, ref int userCount)
    {
        int index = Array.IndexOf(users, username);
        if (index >= 0)
        {
            Console.WriteLine("User already exists.");
            return;
        }

        if (AddValue(username, users, userCount))
        {
            userCount += 1;
        }
    }

    public static void RemoveUser(string username, string[] users, ref int userCount)
    {
        int index = Array.IndexOf(users, username);
        if (index < 0)
        {
            Console.WriteLine("User does not exist.");
            return;
        }

        if (index >= 0 && RemoveValue(users, index, userCount))
        {
            userCount -= 1;
        }
    }

    public static void AddPost(string post, string author, string[] posts, string[] postAuthors, ref int postCount)
    {
        int index = Array.IndexOf(posts, post);
        if (index >= 0)
        {
            Console.WriteLine("Post already exists.");
            return;
        }
        if (AddValue(post, posts, postCount) && AddValue(author, postAuthors, postCount))
        {
            postCount += 1;
        }
    }

    public static string[] GetUserPosts(string user, string[] posts, string[] postAuthors, int postCount)
    {
        int[] userPosts = new int[postCount];
        int userPostCount = 0;
        for (int i = 0; i < postCount; i++)
        {
            if (postAuthors[i] == user)
            {
                userPosts[userPostCount] = i;
                userPostCount += 1;
            }
        }
        return userPosts.Take(userPostCount).Select(i => posts[i]).ToArray();
    }

    public static void AddFollow(string follower, string followee, string[] followers, string[] followees, ref int followCount)
    {
        // check if the follow relationship already exists
        for (int i = 0; i < followCount; i++)
        {
            if (followers[i] == follower && followees[i] == followee)
            {
                Console.WriteLine("Follow relationship already exists.");
                return;
            }
        }
        if (AddValue(follower, followers, followCount) && AddValue(followee, followees, followCount))
        {
            followCount += 1;
        }
    }

    public static void RemoveFollow(string follower, string followee, string[] followers, string[] followees, ref int followCount)
    {
        // find the follow relationship
        for (int i = 0; i < followCount; i++)
        {
            if (followers[i] == follower && followees[i] == followee)
            {
                if (RemoveValue(followers, i, followCount) && RemoveValue(followees, i, followCount))
                {
                    followCount -= 1;
                }
                return;
            }
        }
    }

    public static string[] GetUserFollows(string user, string[] followers, string[] followees, int followCount)
    {
        // chck if user follows anyone
        string[] userFollows = new string[followCount];
        int followIndex = 0;
        for (int i = 0; i < followCount; i++)
        {
            if (followers[i] == user)
            {
                userFollows[followIndex] = followees[i];
                followIndex += 1;
            }
        }
        return userFollows.Take(followIndex).ToArray();
    }

    public static string[] GetUserFollowers(string user, string[] followers, string[] followees, int followCount)
    {
        string[] userFollowers = new string[followCount];
        int followerIndex = 0;
        for (int i = 0; i < followCount; i++)
        {
            if (followees[i] == user)
            {
                userFollowers[followerIndex] = followers[i];
                followerIndex += 1;
            }
        }
        return userFollowers.Take(followerIndex).ToArray();
    }

    // Bonus
    public static string[] GetUserTimeline(string user, string[] posts, string[] postAuthors, int postCount, string[] followers, string[] followees, int followCount)
    {
        // TODO
        return new string[] { };
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Social Network Application");
        // Your application code here
    }
}