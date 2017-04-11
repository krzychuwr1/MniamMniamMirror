using kucharskaApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace kucharskaApi.DB
{
    public class CookBookContext : DbContext
    {
        public CookBookContext(): base()
        {

        }
    }
}