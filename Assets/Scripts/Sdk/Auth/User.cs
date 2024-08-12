using System;
using System.Threading.Tasks;
using Nakama;
using System.Linq;

class User
{
    private CustomNakamaConnection nakamaInstance;

    public User()
    {
        this.nakamaInstance = CustomNakamaConnection.Instance;
    }

    public class SimplifedUser
    {
        public string userName;
        public string displayName;
        public int avatarUrl;

        public SimplifedUser(string userName)
        {
            this.userName = userName;
            this.displayName = userName;
            this.avatarUrl = 0;
        }
    }

    public async Task<bool> updateUser(string userName, string newDisplayName , string newAvatarUrl)
    {
        try
        {
            await nakamaInstance.client.UpdateAccountAsync(nakamaInstance.nakamaSession, userName, newDisplayName, newAvatarUrl);

            // here just populate the user with new avatar and display name

        }catch(Exception ex)
        {
            throw ex;
        }
        return true;
    }

    public async Task<SimplifedUser> fetchUserAccount(string userId)
    {
        try
        {
            var ids = new[] { userId};
            var users = await nakamaInstance.client.GetUsersAsync(nakamaInstance.nakamaSession, ids);
            var currentUser = users.Users.FirstOrDefault();

            // now get a simplified version of IApiUser
            SimplifedUser simplifedUser = new SimplifedUser(currentUser.Username);

            if (currentUser.DisplayName != null)
                simplifedUser.displayName = currentUser.DisplayName;

            if (currentUser.AvatarUrl != null)
            {
                bool canParse = Int32.TryParse(currentUser.AvatarUrl, out simplifedUser.avatarUrl);
                simplifedUser.avatarUrl = Math.Min(5, simplifedUser.avatarUrl);
            }

            return simplifedUser;

        }
        catch(Exception ex)
        {
            throw ex;
        }
    }
}