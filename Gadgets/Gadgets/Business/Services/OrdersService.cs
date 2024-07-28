using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gadgets.Common.Helpers;
using Gadgets.Models;
using Gadgets.Models.DbModel;

namespace Gadgets.Business.Services
{
    public class OrdersService : BaseService<OrdersDbModel>
    {
        private readonly long _uid;
        private readonly string _role;

        public OrdersService()
        {
            try
            {
                if (CacheHelper.TokenModel != null)
                {
                    _uid = CacheHelper.TokenModel.Uid;
                    _role = CacheHelper.TokenModel.Role;
                }
            }
            catch
            {
                _uid = 0;
                _role = "游客";
            }
        }






        public PageModel<OrdersDbModel> GetPageList(int page, int limit, string sort, string order, List<IConditionalModel> conModels)
        {
            PageModel pageModel = new PageModel() { PageIndex = page, PageSize = limit };

            int totalNumber = 0;
            int totalPage = 0;

            string dbColumnName = Db.EntityMaintenance.GetDbColumnName<OrdersDbModel>(sort);
            order = order.ToLower() == "asc" ? "ASC" : "DESC";

            //var sql = Db.Queryable<OrdersDbModel>().Where(conModels).OrderBy(dbColumnName + " " + order).ToSqlString();
            List<OrdersDbModel> ts = Db.Queryable<OrdersDbModel>().Where(conModels).OrderBy(dbColumnName + " " + order).ToPageList(page, limit, ref totalNumber, ref totalPage);


            PageModel<OrdersDbModel> t = new PageModel<OrdersDbModel>()
            {
                Code = ResponseCodeEnum.Success,
                Data = new Page<OrdersDbModel>()
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





    }
}
