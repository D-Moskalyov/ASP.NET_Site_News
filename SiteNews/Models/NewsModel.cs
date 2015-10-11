using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SiteNews.Models
{
    public class NewsModel
    {
        [AllowHtml]
        [Required]
        public string Text { get; set; }
        [Required]
        public string Title { get; set; }
        public string Author { get; set; }
        public int Raiting { get; set; }
        public int ID { get; set; }
        public DateTime CreateDate { get; set; }
    }

    
    public class ListNewsModel
    {
        public List<NewsModel> listNews = new List<NewsModel>();
    }

    public class CommentModel
    {
        public CommentModel(string name, string comment, DateTime date)
        {
            Comment = comment;
            UserName = name;
            CreateDate = date;
        }
        public CommentModel()
        {
            
        }
        public string Comment { get; set; }
        public string UserName { get; set; }
        public DateTime CreateDate { get; set; }
    }

    [Serializable]
    public class ArticleModel
    {
        public ArticleModel()
        {
            Comments = new List<CommentModel>();
        }
        public ArticleModel(NewsModel nM, List<CommentModel> listCom)
        {
            News = new NewsModel();
            News = nM;
            Comments = new List<CommentModel>(listCom);
        }
        public NewsModel News { get; set; }
        public List<CommentModel> Comments { get; set; }
        public string newComment { get; set; }
        public int newsID { get; set; }
    }

}