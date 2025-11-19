using Social;

namespace social.Tests;

public class SocialNetworkTests
{
    private const int MAX_USERS = 100;
    private const int MAX_POSTS = 10000;
    private const int MAX_FOLLOWS = 5050;

    // Pomocná metoda pro inicializaci dat
    private (string[] users, int userCount, string[] posts, string[] postAuthors, int postCount, string[] followers, string[] followees, int followCount) InitializeData()
    {
        return (
            new string[MAX_USERS],
            0,
            new string[MAX_POSTS],
            new string[MAX_POSTS],
            0,
            new string[MAX_FOLLOWS],
            new string[MAX_FOLLOWS],
            0
        );
    }

    #region AddValue Tests
    [Fact]
    public void AddValue_AddsValueSuccessfully()
    {
        var data = new string[5];
        var result = SocialNetwork.AddValue("test", data, 0);
        
        Assert.True(result);
        Assert.Equal("test", data[0]);
    }

    [Fact]
    public void AddValue_ReturnsFalseWhenArrayIsFull()
    {
        var data = new string[2];
        SocialNetwork.AddValue("first", data, 0);
        SocialNetwork.AddValue("second", data, 1);
        var result = SocialNetwork.AddValue("third", data, 2);
        
        Assert.False(result);
    }
    #endregion

    #region RemoveValue Tests
    [Fact]
    public void RemoveValue_RemovesValueSuccessfully()
    {
        var data = new string[] { "a", "b", "c", "d", "e" };
        var result = SocialNetwork.RemoveValue(data, 2, 5);
        
        Assert.True(result);
        Assert.Equal("d", data[2]);
        Assert.Equal("e", data[3]);
        Assert.Equal("", data[4]);
    }

    [Fact]
    public void RemoveValue_ReturnsFalseForInvalidIndex()
    {
        var data = new string[] { "a", "b", "c" };
        var result = SocialNetwork.RemoveValue(data, -1, 3);
        
        Assert.False(result);
    }

    [Fact]
    public void RemoveValue_ReturnsFalseForIndexOutOfRange()
    {
        var data = new string[] { "a", "b", "c" };
        var result = SocialNetwork.RemoveValue(data, 5, 3);
        
        Assert.False(result);
    }
    #endregion

    #region AddUser Tests
    [Fact]
    public void AddUser_AddsNewUserSuccessfully()
    {
        var (users, userCount, _, _, _, _, _, _) = InitializeData();
        
        SocialNetwork.AddUser("alice", users, ref userCount);
        
        Assert.Equal(1, userCount);
        Assert.Equal("alice", users[0]);
    }

    [Fact]
    public void AddUser_AddsMultipleUsers()
    {
        var (users, userCount, _, _, _, _, _, _) = InitializeData();
        
        SocialNetwork.AddUser("alice", users, ref userCount);
        SocialNetwork.AddUser("bob", users, ref userCount);
        SocialNetwork.AddUser("charlie", users, ref userCount);
        
        Assert.Equal(3, userCount);
        Assert.Equal("alice", users[0]);
        Assert.Equal("bob", users[1]);
        Assert.Equal("charlie", users[2]);
    }

    [Fact]
    public void AddUser_DoesNotAddDuplicateUser()
    {
        var (users, userCount, _, _, _, _, _, _) = InitializeData();
        
        SocialNetwork.AddUser("alice", users, ref userCount);
        var countBefore = userCount;
        SocialNetwork.AddUser("alice", users, ref userCount);
        
        Assert.Equal(countBefore, userCount);
    }
    #endregion

    #region RemoveUser Tests
    [Fact]
    public void RemoveUser_RemovesExistingUser()
    {
        var (users, userCount, _, _, _, _, _, _) = InitializeData();
        
        SocialNetwork.AddUser("alice", users, ref userCount);
        SocialNetwork.AddUser("bob", users, ref userCount);
        SocialNetwork.AddUser("charlie", users, ref userCount);
        
        SocialNetwork.RemoveUser("bob", users, ref userCount);
        
        Assert.Equal(2, userCount);
        Assert.Equal(-1, Array.IndexOf(users, "bob", 0, userCount));
        Assert.Contains("alice", users.Take(userCount));
        Assert.Contains("charlie", users.Take(userCount));
    }

    [Fact]
    public void RemoveUser_DoesNothingForNonexistentUser()
    {
        var (users, userCount, _, _, _, _, _, _) = InitializeData();
        
        SocialNetwork.AddUser("alice", users, ref userCount);
        var countBefore = userCount;
        
        SocialNetwork.RemoveUser("nonexistent", users, ref userCount);
        
        Assert.Equal(countBefore, userCount);
    }
    #endregion

    #region AddPost Tests
    [Fact]
    public void AddPost_AddsNewPost()
    {
        var (_, _, posts, postAuthors, postCount, _, _, _) = InitializeData();
        
        SocialNetwork.AddPost("Hello World!", "alice", posts, postAuthors, ref postCount);
        
        Assert.Equal(1, postCount);
        Assert.Equal("Hello World!", posts[0]);
        Assert.Equal("alice", postAuthors[0]);
    }

    [Fact]
    public void AddPost_AddsMultiplePosts()
    {
        var (_, _, posts, postAuthors, postCount, _, _, _) = InitializeData();
        
        SocialNetwork.AddPost("Hello World!", "alice", posts, postAuthors, ref postCount);
        SocialNetwork.AddPost("C# is awesome!", "charlie", posts, postAuthors, ref postCount);
        SocialNetwork.AddPost("I love coding!", "alice", posts, postAuthors, ref postCount);
        
        Assert.Equal(3, postCount);
        Assert.Equal("Hello World!", posts[0]);
        Assert.Equal("C# is awesome!", posts[1]);
        Assert.Equal("I love coding!", posts[2]);
    }

    [Fact]
    public void AddPost_DoesNotAddDuplicatePost()
    {
        var (_, _, posts, postAuthors, postCount, _, _, _) = InitializeData();
        
        SocialNetwork.AddPost("Hello World!", "alice", posts, postAuthors, ref postCount);
        var countBefore = postCount;
        SocialNetwork.AddPost("Hello World!", "alice", posts, postAuthors, ref postCount);
        
        Assert.Equal(countBefore, postCount);
    }
    #endregion

    #region GetUserPosts Tests
    [Fact]
    public void GetUserPosts_ReturnsAllUserPosts()
    {
        var (_, _, posts, postAuthors, postCount, _, _, _) = InitializeData();
        
        SocialNetwork.AddPost("Hello World!", "alice", posts, postAuthors, ref postCount);
        SocialNetwork.AddPost("C# is awesome!", "charlie", posts, postAuthors, ref postCount);
        SocialNetwork.AddPost("I love coding!", "alice", posts, postAuthors, ref postCount);
        
        var alicePosts = SocialNetwork.GetUserPosts("alice", posts, postAuthors, postCount);
        
        Assert.Equal(2, alicePosts.Length);
        Assert.Contains("Hello World!", alicePosts);
        Assert.Contains("I love coding!", alicePosts);
    }

    [Fact]
    public void GetUserPosts_ReturnsSinglePost()
    {
        var (_, _, posts, postAuthors, postCount, _, _, _) = InitializeData();
        
        SocialNetwork.AddPost("Hello World!", "alice", posts, postAuthors, ref postCount);
        SocialNetwork.AddPost("C# is awesome!", "charlie", posts, postAuthors, ref postCount);
        
        var charliePosts = SocialNetwork.GetUserPosts("charlie", posts, postAuthors, postCount);
        
        Assert.Single(charliePosts);
        Assert.Equal("C# is awesome!", charliePosts[0]);
    }

    [Fact]
    public void GetUserPosts_ReturnsEmptyArrayForUserWithNoPosts()
    {
        var (_, _, posts, postAuthors, postCount, _, _, _) = InitializeData();
        
        SocialNetwork.AddPost("Hello World!", "alice", posts, postAuthors, ref postCount);
        
        var bobPosts = SocialNetwork.GetUserPosts("bob", posts, postAuthors, postCount);
        
        Assert.Empty(bobPosts);
    }
    #endregion

    #region AddFollow Tests
    [Fact]
    public void AddFollow_AddsFollowRelationship()
    {
        var (_, _, _, _, _, followers, followees, followCount) = InitializeData();
        
        SocialNetwork.AddFollow("alice", "charlie", followers, followees, ref followCount);
        
        Assert.Equal(1, followCount);
        Assert.Equal("alice", followers[0]);
        Assert.Equal("charlie", followees[0]);
    }

    [Fact]
    public void AddFollow_AddsMultipleFollowRelationships()
    {
        var (_, _, _, _, _, followers, followees, followCount) = InitializeData();
        
        SocialNetwork.AddFollow("alice", "charlie", followers, followees, ref followCount);
        SocialNetwork.AddFollow("charlie", "alice", followers, followees, ref followCount);
        SocialNetwork.AddFollow("alice", "david", followers, followees, ref followCount);
        
        Assert.Equal(3, followCount);
    }

    [Fact]
    public void AddFollow_DoesNotAddDuplicateFollowRelationship()
    {
        var (_, _, _, _, _, followers, followees, followCount) = InitializeData();
        
        SocialNetwork.AddFollow("alice", "charlie", followers, followees, ref followCount);
        var countBefore = followCount;
        SocialNetwork.AddFollow("alice", "charlie", followers, followees, ref followCount);
        
        Assert.Equal(countBefore, followCount);
    }
    #endregion

    #region RemoveFollow Tests
    [Fact]
    public void RemoveFollow_RemovesFollowRelationship()
    {
        var (_, _, _, _, _, followers, followees, followCount) = InitializeData();
        
        SocialNetwork.AddFollow("alice", "charlie", followers, followees, ref followCount);
        SocialNetwork.AddFollow("alice", "david", followers, followees, ref followCount);
        
        SocialNetwork.RemoveFollow("alice", "david", followers, followees, ref followCount);
        
        Assert.Equal(1, followCount);
        var aliceFollows = SocialNetwork.GetUserFollows("alice", followers, followees, followCount);
        Assert.Single(aliceFollows);
        Assert.DoesNotContain("david", aliceFollows);
    }

    [Fact]
    public void RemoveFollow_DoesNothingForNonexistentRelationship()
    {
        var (_, _, _, _, _, followers, followees, followCount) = InitializeData();
        
        SocialNetwork.AddFollow("alice", "charlie", followers, followees, ref followCount);
        var countBefore = followCount;
        
        SocialNetwork.RemoveFollow("alice", "bob", followers, followees, ref followCount);
        
        Assert.Equal(countBefore, followCount);
    }
    #endregion

    #region GetUserFollows Tests
    [Fact]
    public void GetUserFollows_ReturnsAllFollowedUsers()
    {
        var (_, _, _, _, _, followers, followees, followCount) = InitializeData();
        
        SocialNetwork.AddFollow("alice", "charlie", followers, followees, ref followCount);
        SocialNetwork.AddFollow("alice", "david", followers, followees, ref followCount);
        SocialNetwork.AddFollow("charlie", "alice", followers, followees, ref followCount);
        
        var aliceFollows = SocialNetwork.GetUserFollows("alice", followers, followees, followCount);
        
        Assert.Equal(2, aliceFollows.Length);
        Assert.Contains("charlie", aliceFollows);
        Assert.Contains("david", aliceFollows);
    }

    [Fact]
    public void GetUserFollows_ReturnsSingleFollowedUser()
    {
        var (_, _, _, _, _, followers, followees, followCount) = InitializeData();
        
        SocialNetwork.AddFollow("charlie", "alice", followers, followees, ref followCount);
        
        var charlieFollows = SocialNetwork.GetUserFollows("charlie", followers, followees, followCount);
        
        Assert.Single(charlieFollows);
        Assert.Equal("alice", charlieFollows[0]);
    }

    [Fact]
    public void GetUserFollows_ReturnsEmptyArrayForUserNotFollowingAnyone()
    {
        var (_, _, _, _, _, followers, followees, followCount) = InitializeData();
        
        SocialNetwork.AddFollow("alice", "charlie", followers, followees, ref followCount);
        
        var bobFollows = SocialNetwork.GetUserFollows("bob", followers, followees, followCount);
        
        Assert.Empty(bobFollows);
    }
    #endregion

    #region GetUserFollowers Tests
    [Fact]
    public void GetUserFollowers_ReturnsAllFollowers()
    {
        var (_, _, _, _, _, followers, followees, followCount) = InitializeData();
        
        SocialNetwork.AddFollow("alice", "charlie", followers, followees, ref followCount);
        SocialNetwork.AddFollow("bob", "charlie", followers, followees, ref followCount);
        
        var charlieFollowers = SocialNetwork.GetUserFollowers("charlie", followers, followees, followCount);
        
        Assert.Equal(2, charlieFollowers.Length);
        Assert.Contains("alice", charlieFollowers);
        Assert.Contains("bob", charlieFollowers);
    }

    [Fact]
    public void GetUserFollowers_ReturnsSingleFollower()
    {
        var (_, _, _, _, _, followers, followees, followCount) = InitializeData();
        
        SocialNetwork.AddFollow("charlie", "alice", followers, followees, ref followCount);
        
        var aliceFollowers = SocialNetwork.GetUserFollowers("alice", followers, followees, followCount);
        
        Assert.Single(aliceFollowers);
        Assert.Equal("charlie", aliceFollowers[0]);
    }

    [Fact]
    public void GetUserFollowers_ReturnsEmptyArrayForUserWithNoFollowers()
    {
        var (_, _, _, _, _, followers, followees, followCount) = InitializeData();
        
        SocialNetwork.AddFollow("alice", "charlie", followers, followees, ref followCount);
        
        var davidFollowers = SocialNetwork.GetUserFollowers("david", followers, followees, followCount);
        
        Assert.Empty(davidFollowers);
    }
    #endregion

    #region Integration Tests
    [Fact]
    public void CompleteWorkflow_UsersPostsAndFollows()
    {
        var (users, userCount, posts, postAuthors, postCount, followers, followees, followCount) = InitializeData();
        
        // Přidání uživatelů
        SocialNetwork.AddUser("alice", users, ref userCount);
        SocialNetwork.AddUser("bob", users, ref userCount);
        SocialNetwork.AddUser("charlie", users, ref userCount);
        
        // Přidání postů
        SocialNetwork.AddPost("Hello World!", "alice", posts, postAuthors, ref postCount);
        SocialNetwork.AddPost("First post!", "bob", posts, postAuthors, ref postCount);
        SocialNetwork.AddPost("C# rocks!", "charlie", posts, postAuthors, ref postCount);
        SocialNetwork.AddPost("Second post!", "alice", posts, postAuthors, ref postCount);
        
        // Přidání follow vztahů
        SocialNetwork.AddFollow("alice", "bob", followers, followees, ref followCount);
        SocialNetwork.AddFollow("alice", "charlie", followers, followees, ref followCount);
        SocialNetwork.AddFollow("bob", "alice", followers, followees, ref followCount);
        
        // Ověření
        Assert.Equal(3, userCount);
        Assert.Equal(4, postCount);
        Assert.Equal(3, followCount);
        
        var alicePosts = SocialNetwork.GetUserPosts("alice", posts, postAuthors, postCount);
        Assert.Equal(2, alicePosts.Length);
        
        var aliceFollows = SocialNetwork.GetUserFollows("alice", followers, followees, followCount);
        Assert.Equal(2, aliceFollows.Length);
        
        var aliceFollowers = SocialNetwork.GetUserFollowers("alice", followers, followees, followCount);
        Assert.Single(aliceFollowers);
    }
    #endregion
}
