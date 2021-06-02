using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectApi.Domain.SeedWork
{
    /// <summary>
    /// 下面这个泛型约束表示当前泛型参数T，必须是IAggregateRoot接口类型或者其子类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T:IAggregateRoot
    {
        /// <summary>
        /// 这个是用来做事务的
        /// </summary>
        IUnitOfWork UnitOfWork { get; set; }
    }
}
