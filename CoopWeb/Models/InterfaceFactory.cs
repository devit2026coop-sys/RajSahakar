using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Cooperative.Models
{
    interface IDatabase<TEntity> where TEntity : class
    {
        int Add(TEntity _Object);
        //int Update(TEntity _Object); wil be handled by int Add(TEntity _Object);
        int Delete(int _Object);
    }

    interface IDatabaseSelect<TEntity> where TEntity : class
    {
        DataTable GetAll(string WhereClause);
        // TEntity GetById(int Id); // will be handled by  DataTable GetAll();
    }

    public interface ILogger
    {
        void ErrorHandle(ErrorLogEntity errorLogModel, Exception ex);
    }
}
