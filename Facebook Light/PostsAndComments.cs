using System.Collections;
using System.Collections.Generic;
namespace C20_Ex20_Tomer_204389605_Binyamin_308051903
{
	public class PostsAndComments : IEnumerable
	{
		public int TotalComments { get; set; }

		public int NumOfPosts { get; set; }

		public List<string> Posts;

		public PostsAndComments(int i_NumOfPosts, int i_TotalComments)
		{
			NumOfPosts = i_NumOfPosts;
			TotalComments = i_TotalComments;
			Posts = new List<string>();
		}

		public IEnumerator GetEnumerator()
		{
			return Posts.GetEnumerator();
		}
	}
}