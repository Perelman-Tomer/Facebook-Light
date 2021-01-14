using System.Collections.Generic;
using System.Linq;
using FacebookWrapper.ObjectModel;

namespace C20_Ex20_Tomer_204389605_Binyamin_308051903
{
	public enum eSortType
	{
		SortByFirstName = 0,
		SortByLastName = 1
	}

	public abstract class FriendListSorter
	{
		public static FriendListSorter CreateSorter(eSortType i_SortType)
		{
			FriendListSorter result = null;

			switch (i_SortType)
			{
				case eSortType.SortByFirstName:
					{
						result = new SorterByFirstName();
					}

					break;
				case eSortType.SortByLastName:
					{
						result = new SorterByLastName();
					}

					break;
			}

			return result;
		}

		public void Sort(ref List<User> i_UserFriendList)
		{
			User[] UserFriendListAsArr = i_UserFriendList.ToArray();
			for (int i = 0; i < UserFriendListAsArr.Length; i++)
			{
				for (int j = 0; j < UserFriendListAsArr.Length - 1; j++)
				{
					if (needSwap(UserFriendListAsArr[j], UserFriendListAsArr[j + 1]))
					{
						swap(ref UserFriendListAsArr[j], ref UserFriendListAsArr[j + 1]);
					}
				}
			}

			i_UserFriendList = UserFriendListAsArr.ToList();
		}

		private void swap(ref User user1, ref User user2)
		{
			User temp = new User();
			temp = user1;
			user1 = user2;
			user2 = temp;
		}

		public abstract bool needSwap(User i_FirstUser, User i_SecondUser);

		private class SorterByFirstName : FriendListSorter
		{
			public override bool needSwap(User i_FirstUser, User i_SecondUser)
			{
				return string.Compare(i_FirstUser.FirstName, i_SecondUser.FirstName) > 0;
			}
		}

		private class SorterByLastName : FriendListSorter
		{
			public override bool needSwap(User i_FirstUser, User i_SecondUser)
			{
				return string.Compare(i_FirstUser.LastName, i_SecondUser.LastName) > 0;
			}
		}
	}
}
