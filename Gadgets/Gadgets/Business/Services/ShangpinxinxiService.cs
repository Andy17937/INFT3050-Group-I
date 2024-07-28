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
    public class ShangpinxinxiService : BaseService<ShangpinxinxiDbModel>
    {
        private readonly long _uid;
        private readonly string _role;

        public ShangpinxinxiService()
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


		public int Thumbsup(long id, int type)
        {
            ShangpinxinxiDbModel updateObj = new ShangpinxinxiDbModel();
            int res = 0;
            if (type == 1)
            {
                res = Db.Updateable(updateObj).UpdateColumns(it => new { it.Thumbsupnum }).ReSetValue(it => it.Thumbsupnum = (it.Thumbsupnum + 1)).Where(it => it.Id == id).ExecuteCommand();
            }

            if (type == 2)
            {
                res = Db.Updateable(updateObj).UpdateColumns(it => new { it.Crazilynum }).ReSetValue(it => it.Crazilynum = (it.Crazilynum + 1)).Where(it => it.Id == id).ExecuteCommand();
            }
            return res;
        }

        public int IntelRecom(long id)
        {
            ShangpinxinxiDbModel updateObj = new ShangpinxinxiDbModel();
            //return Db.Updateable(updateObj).UpdateColumns(it => new { it.Clicktime }).ReSetValue(it => it.Clicktime == (it.Clicktime + 1)).Where(it => it.Id == id).ExecuteCommand();
            return Db.Updateable(updateObj).UpdateColumns(it => new { it.Clicktime }).ReSetValue(it => it.Clicktime = DateTime.Now).Where(it => it.Id == id).ExecuteCommand();
        }

		public int BrowseClick(long id)
        {
            ShangpinxinxiDbModel updateObj = new ShangpinxinxiDbModel();
            return Db.Updateable(updateObj).UpdateColumns(it => new { it.Clicknum }).ReSetValue(it => it.Clicknum = (it.Clicknum + 1)).Where(it => it.Id == id).ExecuteCommand();
        }


        public PageModel<ShangpinxinxiDbModel> GetPageList(int page, int limit, string sort, string order, List<IConditionalModel> conModels)
        {
            PageModel pageModel = new PageModel() { PageIndex = page, PageSize = limit };

            int totalNumber = 0;
            int totalPage = 0;

            string dbColumnName = Db.EntityMaintenance.GetDbColumnName<ShangpinxinxiDbModel>(sort);
            order = order.ToLower() == "asc" ? "ASC" : "DESC";


            List<ShangpinxinxiDbModel> ts = Db.Queryable<ShangpinxinxiDbModel>().Where(conModels).OrderBy(dbColumnName + " " + order).ToPageList(page, limit, ref totalNumber, ref totalPage);


            PageModel<ShangpinxinxiDbModel> t = new PageModel<ShangpinxinxiDbModel>()
            {
                Code = ResponseCodeEnum.Success,
                Data = new Page<ShangpinxinxiDbModel>()
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
