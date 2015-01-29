using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Settings.Model
{
    public class SqlService : ISqlService
    {
        public SqlService(ISqlRepository repository)
        {
            this.Repository = repository;
        }

        private ISqlRepository Repository { get; set; }

        public bool InitDatabase()
        {
            return Repository.InitDatabase();
        }
    }
}
