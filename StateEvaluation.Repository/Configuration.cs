using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateEvaluation.Repository
{
    public class MyConfiguration : DbConfiguration
    {
        public MyConfiguration()
        {
            AddInterceptor(new StringTrimmerInterceptor());
        }
    }
}
