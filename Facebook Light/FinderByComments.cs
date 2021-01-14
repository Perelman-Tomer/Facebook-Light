using System.Collections.Generic;
using FacebookWrapper.ObjectModel;

namespace C20_Ex20_Tomer_204389605_Binyamin_308051903
{
	public class FinderByComments : IStrategyFinder
	{
		public int Time { get; set; }

		public string BestPost { get; set; }
		

		public void FindBestHour(User i_LoggedInUser)
		{
			FacebookObjectCollection<Post> allPosts;
			allPosts = i_LoggedInUser.Posts;
			List<PostsAndComments> listOfPostsAndCommentsCountByTime = new List<PostsAndComments>();

			float maxCommentsPerPost = 0;
			float commentsPerPost;
			int bestHourToNewPost = -1;
			int hour = 0;

			for (int i = 0; i < 24; i++)
			{
				listOfPostsAndCommentsCountByTime.Add(new PostsAndComments(0, 0));
			}

			foreach (Post post in allPosts)
			{
				listOfPostsAndCommentsCountByTime[post.CreatedTime.Value.Hour].NumOfPosts += 1;
				listOfPostsAndCommentsCountByTime[post.CreatedTime.Value.Hour].TotalComments += post.Comments.Count;
			}

			PostsAndComments bestHourPostsAndComments = new PostsAndComments(0, 0);


			foreach (PostsAndComments postsAndComments in listOfPostsAndCommentsCountByTime)
			{
				if (postsAndComments.NumOfPosts != 0)
				{
					commentsPerPost = postsAndComments.TotalComments / postsAndComments.NumOfPosts;
				}
				else
				{
					commentsPerPost = 0;
				}

				if (maxCommentsPerPost < commentsPerPost)
				{
					maxCommentsPerPost = commentsPerPost;
					bestHourToNewPost = hour;
				}

				hour += 1;
			}

            Post bestPostOnBestTime = new Post();

            foreach (Post post in bestHourPostsAndComments)
            {
                if (bestPostOnBestTime.Comments.Count < post.Comments.Count)
                {
                    bestPostOnBestTime = post;
                }
            }

			Time = bestHourToNewPost;
			BestPost = bestPostOnBestTime.ToString();


		}
	}
}
