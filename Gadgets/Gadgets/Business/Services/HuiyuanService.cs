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
    public class HuiyuanService : BaseService<HuiyuanDbModel>
    {
        private readonly long _uid;
        private readonly string _role;

        public HuiyuanService()
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

		public dynamic Login(string username, string password)
        {
            return CurrentDb.GetSingle(it => it.Huiyuanming == username && it.Mima == password);
        }
        
        public bool ResetPass(string username)
        {
            return CurrentDb.Update(it => new HuiyuanDbModel() { Mima = "123456" }, it => it.Huiyuanming == username);
        }





        public PageModel<HuiyuanDbModel> GetPageList(int page, int limit, string sort, string order, List<IConditionalModel> conModels)
        {
            PageModel pageModel = new PageModel() { PageIndex = page, PageSize = limit };

            int totalNumber = 0;
            int totalPage = 0;

            string dbColumnName = Db.EntityMaintenance.GetDbColumnName<HuiyuanDbModel>(sort);
            order = order.ToLower() == "asc" ? "ASC" : "DESC";


            List<HuiyuanDbModel> ts = Db.Queryable<HuiyuanDbModel>().Where(conModels).OrderBy(dbColumnName + " " + order).ToPageList(page, limit, ref totalNumber, ref totalPage);


            PageModel<HuiyuanDbModel> t = new PageModel<HuiyuanDbModel>()
            {
                Code = ResponseCodeEnum.Success,
                Data = new Page<HuiyuanDbModel>()
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
