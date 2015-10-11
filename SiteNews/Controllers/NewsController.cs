using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiteNews.Models;
using System.Data.SqlClient;
using System.Net;

namespace SiteNews.Controllers
{
    public class NewsController : Controller
    {
        static ArticleModel currentArticle;
        //
        // GET: /News/

        public ActionResult Write()
        {
            //NewsModel model = new NewsModel();
            //model.userName = User.Identity.Name;
            return View();
        }

        [HttpPost]
        public ActionResult Write(NewsModel model)
        {
            if (ModelState.IsValid)
            {
                model.Author = User.Identity.Name;
                model.Raiting = 0;
                model.CreateDate = DateTime.Now;
                //ViewBag.Text = model.Text;

                string query = string.Format("insert into News (UserName, Title, Contect, Raiting, CreateDate) values('{0}', '{1}', '{2}', 0, '{3}')",
                    model.Author, model.Title, WebUtility.HtmlEncode(model.Text), model.CreateDate);
                SqlCommand command = new SqlCommand(query, DataBase.Sql);
                command.ExecuteNonQuery();

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(model);
            }
        }

        //[HttpPost]
        public ActionResult PrintNews(int model)
        {
            string query = string.Format("select * from News where NewsID = {0}", model);
            SqlCommand command = new SqlCommand(query, DataBase.Sql);
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();

            NewsModel newsModel = new NewsModel();

            newsModel.Author = reader["UserName"].ToString();
            newsModel.ID = (int)reader["NewsID"];
            newsModel.Title = reader["Title"].ToString();
            newsModel.Raiting = (int)reader["Raiting"];
            newsModel.Text = WebUtility.HtmlDecode(reader["Contect"].ToString());
            newsModel.CreateDate = (DateTime)reader["CreateDate"];

            reader.Close();

            ArticleModel article = new ArticleModel();
            article.News = newsModel;
            article.newsID = newsModel.ID;

            string query1 = string.Format("select * from Comments where NewsID = {0}", newsModel.ID);
            SqlCommand command1 = new SqlCommand(query1, DataBase.Sql);
            SqlDataReader reader1 = command1.ExecuteReader();

            if (reader1.HasRows)
            {
                while (reader1.Read())
                {
                    int userID = (int)reader1["UserID"];

                    string query2 = string.Format("select Name from Users where UserID = {0}", userID);
                    SqlCommand command2 = new SqlCommand(query2, DataBase.GetSqlConnection());
                    SqlDataReader reader2 = command2.ExecuteReader();
                    reader2.Read();
                    string name = reader2["Name"].ToString();
                    reader2.Close();

                    string comment = reader1["Comment"].ToString();
                    DateTime date = (DateTime)reader1["CreateDate"];
                    article.Comments.Add(new CommentModel(name, comment, date));
                }

            }

            reader1.Close();

            //int newID = _tasks.Create(item);
            currentArticle = article;
            return RedirectToAction("PrintArticle", "News");
            //new { model1 = article.News.ID }
            //return View(article);
        }

        public ActionResult PrintArticle()
        {
            return View(currentArticle);
        }

        [HttpPost, ValidateInput(false)]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult PrintArticle(ArticleModel model)
        {
            //string query2 = string.Format("select UserID from Users where Name = '{0}'", User.Identity.Name);
            //SqlCommand command2 = new SqlCommand(query2, DataBase.GetSqlConnection());
            //SqlDataReader reader2 = command2.ExecuteReader();
            //reader2.Read();
            //int id = (int)reader2["UserID"];
            //reader2.Close();

            //string query = string.Format("insert into Comments (NewsID, UserID, Comment, CreateDate) values({0}, {1}, '{2}', '{3}')",
            //    model.News.ID, id, model.newComment, DateTime.Now);
            //SqlCommand command = new SqlCommand(query, DataBase.Sql);
            //SqlDataReader reader = command.ExecuteReader();
            //reader.Read();

            //model.Comments.Add(new CommentModel(User.Identity.Name, model.newComment, DateTime.Now));

            string query = string.Format("select * from News where NewsID = {0}", model.newsID);
            SqlCommand command = new SqlCommand(query, DataBase.Sql);
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();

            NewsModel newsModel = new NewsModel();

            newsModel.Author = reader["UserName"].ToString();
            newsModel.ID = (int)reader["NewsID"];
            newsModel.Title = reader["Title"].ToString();
            newsModel.Raiting = (int)reader["Raiting"];
            newsModel.Text = WebUtility.HtmlDecode(reader["Contect"].ToString());
            newsModel.CreateDate = (DateTime)reader["CreateDate"];

            reader.Close();

            model.News = newsModel;

            //ArticleModel article = new ArticleModel();
            //article.News = newsModel;
            //article.newsID = newsModel.ID;

            string query1 = string.Format("select * from Comments where NewsID = {0}", newsModel.ID);
            SqlCommand command1 = new SqlCommand(query1, DataBase.Sql);
            SqlDataReader reader1 = command1.ExecuteReader();

            if (reader1.HasRows)
            {
                while (reader1.Read())
                {
                    int userID = (int)reader1["UserID"];

                    string query2 = string.Format("select Name from Users where UserID = {0}", userID);
                    SqlCommand command2 = new SqlCommand(query2, DataBase.GetSqlConnection());
                    SqlDataReader reader2 = command2.ExecuteReader();
                    reader2.Read();
                    string name = reader2["Name"].ToString();
                    reader2.Close();

                    string comment = reader1["Comment"].ToString();
                    DateTime date = (DateTime)reader1["CreateDate"];
                    model.Comments.Add(new CommentModel(name, comment, date));
                }

            }

            reader1.Close();

            string query4 = string.Format("select UserID from Users where Name = '{0}'", User.Identity.Name);
            SqlCommand command4 = new SqlCommand(query4, DataBase.GetSqlConnection());
            SqlDataReader reader4 = command4.ExecuteReader();
            reader4.Read();
            int id = (int)reader4["UserID"];
            reader4.Close();

            string query3 = string.Format("insert into Comments (NewsID, UserID, Comment, CreateDate) values({0}, {1}, '{2}', '{3}')",
                model.newsID, id, model.newComment, DateTime.Now);
            SqlCommand command3 = new SqlCommand(query3, DataBase.Sql);
            command3.ExecuteNonQuery();

            model.Comments.Add(new CommentModel(User.Identity.Name, model.newComment, DateTime.Now));

            model.newComment = string.Empty;

            return View(model);
        }

    }
}

