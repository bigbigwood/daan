using System;
using System.Collections;
using IBatisNet.Common.Exceptions;
using IBatisNet.Common.Pagination;
using IBatisNet.DataMapper;
using IBatisNet.DataMapper.MappedStatements;
using IBatisNet.DataMapper.Scope;
using System.Data;
using IBatisNet.Common;
//using Oracle.DataAccess.Client;
//using Oracle.DataAccess;
using System.Data.OracleClient;
namespace daan.dao.common
{
    /// <summary>
    /// Summary description for BaseSqlMapDao.
    /// </summary>
    public class BaseDAO
    {
        protected const int PAGE_SIZE = 4;
        public ISqlMapper mapper = MapperDAO.Get();

        public ISqlMapper getMapper()
        {
            return MapperDAO.Get();
        }


        public string GetSql(string statementName, object paramObject)
        {
            string sql = "";
            try
            {
                IMappedStatement statement = mapper.GetMappedStatement(statementName);
                if (!mapper.IsSessionStarted)
                {
                    mapper.OpenConnection();
                }
                RequestScope scope = statement.Statement.Sql.GetRequestScope(statement, paramObject, mapper.LocalSession);
                sql = scope.PreparedStatement.PreparedSql;
            }
            catch (Exception e1)
            { 
                
            }
            finally
            {
                try
                {
                    mapper.CloseConnection();
                }
                catch (Exception)
                {

                }
            }
            return sql;
        }
        public DataSet SelectDS(string statementName, object paramObject, object _sqlmapper)
        {
            DataSet ds = new DataSet();
            ISqlMapper _mapper = _sqlmapper as ISqlMapper;
            try
            {

                IMappedStatement statement = _mapper.GetMappedStatement(statementName);
                if (!_mapper.IsSessionStarted)
                {
                    _mapper.OpenConnection();
                }
                RequestScope scope = statement.Statement.Sql.GetRequestScope(statement, paramObject, _mapper.LocalSession);
                statement.PreparedCommand.Create(scope, _mapper.LocalSession, statement.Statement, paramObject);

                IDbCommand command = _mapper.LocalSession.CreateCommand(CommandType.Text);
                command.CommandText = scope.IDbCommand.CommandText;

                foreach (IDataParameter pa in scope.IDbCommand.Parameters)
                {
                    command.Parameters.Add(new OracleParameter(pa.ParameterName, pa.Value));
                }
                _mapper.LocalSession.CreateDataAdapter(command).Fill(ds);
            }
            finally
            {
                _mapper.CloseConnection();                
            }
            return ds;
        }


        public DataSet SelectDS(string statementName, object paramObject)
        {
            DataSet ds = new DataSet();
            try
            {
                IMappedStatement statement = mapper.GetMappedStatement(statementName);
                if (!mapper.IsSessionStarted)
                {
                    mapper.OpenConnection();
                }
                RequestScope scope = statement.Statement.Sql.GetRequestScope(statement, paramObject, mapper.LocalSession);
                statement.PreparedCommand.Create(scope, mapper.LocalSession, statement.Statement, paramObject);

                IDbCommand command = mapper.LocalSession.CreateCommand(CommandType.Text);
                command.CommandText = scope.IDbCommand.CommandText;

                foreach (IDataParameter pa in scope.IDbCommand.Parameters)
                {
                    command.Parameters.Add(new OracleParameter(pa.ParameterName, pa.Value));
                }
                mapper.LocalSession.CreateDataAdapter(command).Fill(ds);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                try
                {
                    mapper.CloseConnection();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }               
            }
            return ds;
        }
    }
}
