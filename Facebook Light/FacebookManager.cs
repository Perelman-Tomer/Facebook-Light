using System;
using System.Collections.Generic;
using FacebookWrapper;
using FacebookWrapper.ObjectModel;
using System.Windows.Forms;

namespace C20_Ex20_Tomer_204389605_Binyamin_308051903
{

    // $G$ CSS-999 (-5) access modifiers are missing
    class FacebookManager
    {
		public User LoggedInUser { get; set; }

		public LoginResult LoginResult { get; set; }

		public AppSettings AppSettingsInstance { get; set; }

		private static FacebookManager m_FacebookManager = null;

		public FinderFeature FinderFeature { get; set; }

		public FacebookServiceFacade m_FaceBookServiceFacade { get; set; }

		public FriendListSorter FriendListSorter { get; set; }

		private List<User> m_LoggedInUserFriends = new List<User>();

		public List<User> LoggedInUserFriends
		{
			get
			{
				return m_LoggedInUserFriends;
			}

			set
			{
				m_LoggedInUserFriends = value;
			}
		}

		public static FacebookManager GetInstance()
		{
			if (m_FacebookManager == null)
			{
				m_FacebookManager = new FacebookManager();
			}

			return m_FacebookManager;
		}

		private FacebookManager()
		{
			AppSettingsInstance = AppSettings.LoadFromFile();
			m_FaceBookServiceFacade = new FacebookServiceFacade();
		}

		public void Login()
		{
			LoginResult = m_FaceBookServiceFacade.Login();
			LoggedInUser = LoginResult.LoggedInUser;

			if (m_LoggedInUserFriends.Count != 0)
			{
				for (int i = m_LoggedInUserFriends.Count; i > 0; i--)
					m_LoggedInUserFriends.RemoveAt(i-1);
			}


			foreach (User friend in LoggedInUser.Friends)
			{
				m_LoggedInUserFriends.Add(friend);
			}
		}

		public void Logout()
		{
			m_FaceBookServiceFacade.Logout();
			LoggedInUser = null;
			LoginResult = null;
			AppSettingsInstance.RememberUser = false;
		}

		public void Connect()
		{
			LoginResult = FacebookService.Connect(this.AppSettingsInstance.AccessToken);
			LoggedInUser = LoginResult.LoggedInUser;
		}

		internal List<string> FetchUserFriendsMale()
		{
			List<string> maleGenderList = new List<string>();

			foreach (User friendGenderMale in LoggedInUser.Friends)
			{
				if(friendGenderMale.Gender.ToString() == "male")
                {
					maleGenderList.Add(friendGenderMale.Name);
				}
			}

			return maleGenderList;
		}



        // $G$ CSS-999 (-5) You should have used constants instead of spesific text\numbers in the code.

        internal List<string> FetchUserFriendsFemale()
		{
			List<string> femaleGenderList = new List<string>();

			foreach (User friendGenderFemale in LoggedInUser.Friends)
			{
				if (friendGenderFemale.Gender.ToString() == "female")
				{
					femaleGenderList.Add(friendGenderFemale.Name);
				}
			}

			return femaleGenderList;
		}

		public void SortFriendByFirstName()
		{
			FriendListSorter = FriendListSorter.CreateSorter(eSortType.SortByFirstName);
			FriendListSorter.Sort(ref m_LoggedInUserFriends);
		}

		public void SortFriendByLastName()
		{
			FriendListSorter = FriendListSorter.CreateSorter(eSortType.SortByLastName);
			FriendListSorter.Sort(ref m_LoggedInUserFriends);
		}

		private void startFinderFeature()
		{
			FinderFeature.FindFinder(LoggedInUser);
		}

		public void startFindByLikes()
		{
			FinderFeature = new FinderFeature(new FinderByLikes());
			startFinderFeature();
		}

		public void startFindByComments()
		{
			FinderFeature = new FinderFeature(new FinderByComments());
			startFinderFeature();
		}
	}
}
