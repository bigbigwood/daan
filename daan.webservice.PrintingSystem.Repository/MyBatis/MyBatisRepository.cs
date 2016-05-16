using IBatisNet.DataMapper;

namespace daan.webservice.PrintingSystem.Repository.MyBatis
{
    public abstract class MyBatisRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class
    {
        protected ISqlMapper _sqlMapper;

        public MyBatisRepository()
        {
            SessionToPersistanceAdapter session = RepositoryManager.GetConnection() as SessionToPersistanceAdapter;
            _sqlMapper = session.GetUndelayingSession();
        }

        /// <summary>  
        /// 新增  
        /// </summary>  
        protected abstract string InsertStatement { get; }
        /// <summary>  
        /// 编辑  
        /// </summary>  
        protected abstract string UpdateStatement { get; }
        /// <summary>  
        /// 删除  
        /// </summary>  
        protected abstract string DeleteStatement { get; }
        /// <summary>  
        /// 单查  
        /// </summary>  
        protected abstract string QueryObjectStatement { get; }
        /// <summary>  
        /// Count  
        /// </summary>  
        protected abstract string QueryCountStatement { get; }
        /// <summary>  
        /// Full Query  
        /// </summary>  
        protected abstract string QueryAllStatement { get; }

        public virtual bool Insert(TEntity entity)
        {
            return _sqlMapper.Insert(this.InsertStatement, entity) != null;
        }

        public virtual bool Update(TEntity entity)
        {
            return _sqlMapper.Update(this.UpdateStatement, entity) > 0;
        }

        public virtual bool Delete(TKey key)
        {
            return _sqlMapper.Delete(this.DeleteStatement, key) > 0;
        }

        public virtual TEntity GetByKey(TKey key)
        {
            return _sqlMapper.QueryForObject<TEntity>(this.QueryObjectStatement, key);
        }
    }
}