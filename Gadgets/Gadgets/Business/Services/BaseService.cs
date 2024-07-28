using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Gadgets.Business.DB;
using Gadgets.Models;

namespace Gadgets.Business.Services
{
    public class BaseService<T> : DbContext<T> where T : class, new()
    {
        public PageModel<T> BaseGetPageList(int page, int limit, string sort, string order)
        {
            PageModel pageModel = new PageModel() { PageIndex = page, PageSize = limit };

            int totalNumber = 0;
            int totalPage = 0;

            string dbColumnName = Db.EntityMaintenance.GetDbColumnName<T>(sort);
            order = order.ToLower() == "asc" ? "ASC" : "DESC";

            List<IConditionalModel> conModels = new List<IConditionalModel>();

            List<T> ts = Db.Queryable<T>().Where(conModels).OrderBy(dbColumnName + " " + order).ToPageList(page, limit, ref totalNumber, ref totalPage);

            PageModel<T> t = new PageModel<T>()
            {
                Code = ResponseCodeEnum.Success,
                Data = new Page<T>()
                {
                    Total = totalNumber,
                    PageSize = limit,
                    TotalPage = totalPage,
                    CurrPage = page,
                    List = ts
                }
            };

            return t;
        }

        public int BaseInsert(T entity)
        {
            //return CurrentDb.InsertReturnIdentity(entity);
            return Db.Insertable(entity).IgnoreColumns(true).ExecuteCommand();
        }

        public bool BaseUpdate(T entity)
        {
            return Db.Updateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommand() > 0 ? true : false;
        }

        public bool BaseDels(dynamic[] ids)
        {
            return CurrentDb.DeleteByIds(ids);
        }

        public T BaseGetById(long id)
        {
            return CurrentDb.GetById(id);
        }
    }
}
