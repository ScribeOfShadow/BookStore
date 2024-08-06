using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookStore.Models;

namespace BookStore.Helpers
{
    public class ImageHelper
    {
        public static Evaluation GetImage(int? Id)
        {
            try
            {
                using (ApplicationDbContext core = new ApplicationDbContext())
                {
                    var FindSalesItem = core.Evaluations.FirstOrDefault(x => x.Id == Id) ?? null;
                  
                    return FindSalesItem;
                }
            }
            catch (Exception)
            {

                return null;
            }
        
        }
    }
}