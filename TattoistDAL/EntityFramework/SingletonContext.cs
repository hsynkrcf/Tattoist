using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TattoistDAL.EntityFramework
{
    public class SingletonContext<TContext> 
           where TContext : DbContext, new()
    {
        private static TContext context;

        public static TContext Instance
        {
            get
            {
                if (context == null)
                {
                    context = new TContext();
                }
                return context;
            }
        }
    }
}