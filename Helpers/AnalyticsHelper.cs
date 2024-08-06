using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Helpers
{
    public class AnalyticsHelper
    {
        public static bool Insert(string BookName, string BookDescription, string Comment, int? Rating)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Analytic analytic = new Analytic();
                analytic.BookName = BookName;
                analytic.BookDescription = BookDescription;
                analytic.Comment = Comment;
                analytic.Rating = Rating;
                analytic.SoldDateTime = DateTime.Now;
                db.Analytics.Add(analytic);
                return db.SaveChanges() > 0;
            }
        }

        public static List<Feedback> getfeedback(int? bookid)
        {
            using (ApplicationDbContext core = new ApplicationDbContext())
            {
                var feed = (from f in core.Feedbacks
                            where f.SaleId == bookid
                            select f).ToList();
                return feed;
            }
        }

        public static List<Feedback> getSpecificfeedback(int? bookid)
        {
            using (ApplicationDbContext core = new ApplicationDbContext())
            {
                var feed = (from f in core.Feedbacks
                            where f.BookId == bookid
                            select f).ToList();
                return feed;
            }
        }
    }
}