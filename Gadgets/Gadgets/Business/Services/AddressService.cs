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
    public class AddressService : BaseService<AddressDbModel>
    {
        private readonly long _uid;
        private readonly string _role;

        public AddressService()
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






        public PageModel<AddressDbModel> GetPageList(int page, int limit, string sort, string order, List<IConditionalModel> conModels)
        {
            PageModel pageModel = new PageModel() { PageIndex = page, PageSize = limit };

            int totalNumber = 0;
            int totalPage = 0;

            string dbColumnName = Db.EntityMaintenance.GetDbColumnName<AddressDbModel>(sort);
            order = order.ToLower() == "asc" ? "ASC" : "DESC";

            if (_role != "Admin")
            {
                conModels.Add(new ConditionalModel() { FieldName = "userid", ConditionalType = ConditionalType.Equal, FieldValue = _uid.ToString() });
            }


            List<AddressDbModel> ts = Db.Queryable<AddressDbModel>().Where(conModels).OrderBy(dbColumnName + " " + order).ToPageList(page, limit, ref totalNumber, ref totalPage);


            PageModel<AddressDbModel> t = new PageModel<AddressDbModel>()
            {
                Code = ResponseCodeEnum.Success,
                Data = new Page<AddressDbModel>()
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

        public dynamic GetDefault(long userId)
        {
            return CurrentDb.GetSingle(it => it.Userid == userId && it.Isdefault == "Yes");
        }




    }
}
