using System.Collections.Generic;
using FacebookWrapper.ObjectModel;

namespace C20_Ex20_Tomer_204389605_Binyamin_308051903
{
    public class FinderByLikes : IStrategyFinder
    {
        public int Time { get; set; }

        public string BestPost { get; set; }

        public void FindBestHour(User i_LoggedInUser)
        {
            FacebookObjectCollection<Post> allPosts;
            allPosts = i_LoggedInUser.Posts;
            List<PostsAndLikes> listOfPostsAndLikesCountByTime = new List<PostsAndLikes>();
            
            float maxCommentsPerPost = 0;
            float commentsPerPost;
            int bestHourToNewPost = -1;
            int hour = 0;

            for (int i = 0; i < 24; i++)
            {
                listOfPostsAndLikesCountByTime.Add(new PostsAndLikes(0, 0));
            }

            foreach (Post post in allPosts)
            {
                listOfPostsAndLikesCountByTime[post.CreatedTime.Value.Hour].NumOfPosts += 1;
                listOfPostsAndLikesCountByTime[post.CreatedTime.Value.Hour].TotalLikes += post.LikedBy.Count;
            }

            PostsAndLikes bestHourPhotoAndLikes = new PostsAndLikes(0, 0);

            foreach (PostsAndLikes postsAndLikes in listOfPostsAndLikesCountByTime)
            {
                if (postsAndLikes.NumOfPosts != 0)
                {
                    commentsPerPost = postsAndLikes.TotalLikes / postsAndLikes.NumOfPosts;
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

            foreach (Post post in bestHourPhotoAndLikes)
            {
                if (bestPostOnBestTime.LikedBy.Count < post.LikedBy.Count)
                {
                    bestPostOnBestTime = post;
                }
            }

            Time = bestHourToNewPost;
            BestPost = bestHourToNewPost.ToString();

        }
    }
}


