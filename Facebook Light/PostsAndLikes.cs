using System.Collections;
using System.Collections.Generic;

namespace C20_Ex20_Tomer_204389605_Binyamin_308051903
{
    public class PostsAndLikes : IEnumerable
	{
		public int TotalLikes { get; set; }

		public int NumOfPosts { get; set; }

		public List<string> Posts;

		public PostsAndLikes(int i_NumOfPosts, int i_TotalLikes)
		{
			NumOfPosts = i_NumOfPosts;
			TotalLikes = i_TotalLikes;
			Posts = new List<string>();
		}

		public IEnumerator GetEnumerator()
		{
			return Posts.GetEnumerator();
		}
	}
}
