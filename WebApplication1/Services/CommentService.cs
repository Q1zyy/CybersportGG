using Microsoft.AspNetCore.Razor.Language.Intermediate;
using WebApplication1.Models;

namespace WebApplication1.Services
{
	public class CommentService : ICommentService
	{

		string path = @"D:\Forum\WebApplication1\WebApplication1\Comments.txt";

		public void AddComment(Comment model)
		{
			using (StreamWriter writer = new StreamWriter(path, true))
			{
				writer.WriteLine(model.Author + " " + model.NewsId);
				writer.WriteLine(model.Content);
			}

		}

		public IEnumerable<Comment> GetComments(int id)
		{
			List<Comment> comments = new List<Comment>();
			using (StreamReader reader = new StreamReader(path))
			{
				string text = "";
				while (text != null)
				{
					Comment comment = new Comment();
					text = reader.ReadLine();
					if (text == null) break;
					string newsContent = reader.ReadLine();
					var ss = text.Split(' ');
					comment.Content = newsContent;
					comment.Author = ss[0];
					comment.NewsId = int.Parse(ss[1]);
					comments.Add(comment);
				}
			}
			return comments;
		}
	}
}
