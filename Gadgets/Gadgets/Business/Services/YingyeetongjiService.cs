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
    public class YingyeetongjiService : BaseService<YingyeetongjiDbModel>
    {
        private readonly long _uid;
        private readonly string _role;

        public YingyeetongjiService()
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






        public PageModel<YingyeetongjiDbModel> GetPageList(int page, int limit, string sort, string order, List<IConditionalModel> conModels)
        {
            PageModel pageModel = new PageModel() { PageIndex = page, PageSize = limit };

            int totalNumber = 0;
            int totalPage = 0;

            string dbColumnName = Db.EntityMaintenance.GetDbColumnName<YingyeetongjiDbModel>(sort);
            order = order.ToLower() == "asc" ? "ASC" : "DESC";


            List<YingyeetongjiDbModel> ts = Db.Queryable<YingyeetongjiDbModel>().Where(conModels).OrderBy(dbColumnName + " " + order).ToPageList(page, limit, ref totalNumber, ref totalPage);

            //foreach (var item in ts)
            //{
                //item.Shijian = item.Shijian.ObjToString("yyyy-MM-dd");
            //}            

            PageModel<YingyeetongjiDbModel> t = new PageModel<YingyeetongjiDbModel>()
            {
                Code = ResponseCodeEnum.Success,
                Data = new Page<YingyeetongjiDbModel>()
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
