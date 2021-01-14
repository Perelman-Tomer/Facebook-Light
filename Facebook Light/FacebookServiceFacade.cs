using FacebookWrapper;

namespace C20_Ex20_Tomer_204389605_Binyamin_308051903
{
	public class FacebookServiceFacade
	{
		public LoginResult Login()
		{
			LoginResult result = null;
			AppSettings instance = AppSettings.GetInstance();

			if (!string.IsNullOrEmpty(instance.AccessToken))
			{
				try
				{
					result = FacebookService.Connect(instance.AccessToken);
				}
				catch
				{
					result = FacebookService.Login("720748892023578", "public_profile", "email", "publish_to_groups", "user_birthday", "user_age_range", "user_gender", "user_link", "user_tagged_places", "user_videos", "publish_to_groups", "groups_access_member_info", "user_friends", "user_events", "user_likes", "user_location", "user_photos", "user_posts", "user_hometown", "pages_read_engagement", "pages_manage_posts");
				}
			}
			else
			{
				result = FacebookService.Login("720748892023578", "public_profile", "email", "publish_to_groups", "user_birthday", "user_age_range", "user_gender", "user_link", "user_tagged_places", "user_videos", "publish_to_groups", "groups_access_member_info", "user_friends", "user_events", "user_likes", "user_location", "user_photos", "user_posts", "user_hometown", "pages_read_engagement", "pages_manage_posts");
			}

			return result;
		}

		public void Logout()
		{
			FacebookService.Logout(null);
		}
	}
}
