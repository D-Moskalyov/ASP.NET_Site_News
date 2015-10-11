using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using SiteNews.Models;

namespace SiteNews.Controllers
{
    public class HomeController : Controller
    {

        //static SqlConnection sql;

        public ActionResult Index()
        {
            ListNewsModel listNewsModel = new ListNewsModel();
            if (DataBase.GetSqlConnection().State == System.Data.ConnectionState.Open)
            {
                string query = string.Format("select * from News");
                SqlCommand command = new SqlCommand(query, DataBase.Sql);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    NewsModel newsModel = new NewsModel();

                    newsModel.Author = reader["UserName"].ToString();
                    newsModel.ID = (int)reader["NewsID"];
                    newsModel.Title = reader["Title"].ToString();
                    newsModel.Raiting = (int)reader["Raiting"];
                    newsModel.Text = reader["Contect"].ToString();
                    newsModel.CreateDate = (DateTime)reader["CreateDate"];
                    
                    listNewsModel.listNews.Add(newsModel);
                }
                reader.Close();
            }
            else
            {
                ViewBag.Message = "Непредвиденные проблемы с ДБ.";
            }
            return View(listNewsModel);
        }

        [HttpPost]
        //public ActionResult Index(NewsModel newsModel)
        //{
            
        //    return View(listNewsModel);
        //}

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Test1()
        {
            Test1Model test1Ex = new Test1Model();
            test1Ex.t = 10;
            //NewsModel nM = new NewsModel();
            //nM.CreateDate = DateTime.Now;
            //nM.Author = "MY";
            //nM.ID = 5;
            //nM.Raiting = 6;
            //nM.Text = "someText";
            //nM.Title = "title";
            //test1Ex.someNM = nM;
            //test1Ex.someSrt = "hi!";
            return View(test1Ex);
        }

        [HttpPost, ValidateInput(false)]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Test1(Test1Model test)
        {

            return View(test);
        }


    }
}
