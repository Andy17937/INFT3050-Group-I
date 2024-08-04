using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Gadgets.Business.Services;
using Gadgets.Common.Helpers;
using Gadgets.Models;
using Gadgets.Models.DbModel;
using Newtonsoft.Json.Linq;

namespace Gadgets.Controllers
{
    /// <summary>
    /// Returns Management相关接口
    /// </summary>
    [Route("[controller]/[action]")]
    public class TuihuoguanliController : Controller
    {
        private readonly long _uid;
        private readonly string _role;
        private readonly TuihuoguanliService _bll;

        /// <summary>
        /// constructor
        /// </summary>
        public TuihuoguanliController()
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

            _bll = new TuihuoguanliService();
        }


        /// <summary>
        /// sub-Page interface
        /// </summary>
        /// <param name="page">当前Page</param>
        /// <param name="limit">每Page记录的长度</param>
        /// <param name="sort">排序字段</param>
        /// <param name="order">升序（Defaultasc）</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin,Client")]
        public JsonResult Page(int page = 1, int limit = 10, string sort = "id", string order = "asc")
        {
            try
            {
            	List<IConditionalModel> conModels = new List<IConditionalModel>();
                if(CacheHelper.TokenModel != null && "huiyuanming" == CacheHelper.TokenModel.UNickname.ToString()) {
                    conModels.Add(new ConditionalModel() { FieldName = "huiyuanming", ConditionalType = ConditionalType.Equal, FieldValue = CacheHelper.TokenModel.Uname.ToString() });
                }
				TuihuoguanliDbModel entityObj = new TuihuoguanliDbModel();
				var hasProperty = entityObj.GetType().GetProperty(System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(CacheHelper.TokenModel.UNickname));
				if (_role != "Admin" && hasProperty != null)
				{
					conModels.Add(new ConditionalModel() { FieldName = CacheHelper.TokenModel.UNickname, ConditionalType = ConditionalType.Equal, FieldValue = CacheHelper.TokenModel.Uname });
				}
                var shijian = HttpContext.Request.Query["shijian"].ToString();
                if (!string.IsNullOrEmpty(shijian))
                {
                    if (shijian.Contains("%"))
                    {
                        conModels.Add(new ConditionalModel() { FieldName = "shijian", ConditionalType = ConditionalType.Like, FieldValue = shijian });
                    }
                    else
                    {
                        conModels.Add(new ConditionalModel() { FieldName = "shijian", ConditionalType = ConditionalType.Equal, FieldValue = shijian });
                    }
                }
                return Json(_bll.GetPageList(page, limit, sort, order, conModels));
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, Msg = ex.Message });
            }
        }

        /// <summary>
        /// sub-Page interface
        /// </summary>
        /// <param name="page">当前Page</param>
        /// <param name="limit">每Page记录的长度</param>
        /// <param name="sort">排序字段</param>
        /// <param name="order">升序（Defaultasc）</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult List(int page = 1, int limit = 10, string sort = "id", string order = "asc")
        {
            try
            {
                List<IConditionalModel> conModels = new List<IConditionalModel>();
                var shijian = HttpContext.Request.Query["shijian"].ToString();
				if (!string.IsNullOrEmpty(shijian))
                {
                    if (shijian.Contains("%"))
                    {
                        conModels.Add(new ConditionalModel() { FieldName = "shijian", ConditionalType = ConditionalType.Like, FieldValue = shijian });
                    }
                    else
                    {
                        conModels.Add(new ConditionalModel() { FieldName = "shijian", ConditionalType = ConditionalType.Equal, FieldValue = shijian });
                    }
                }
                return Json(_bll.GetPageList(page, limit, sort, order, conModels));
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, Msg = ex.Message });
            }
        }

        /// <summary>
        /// 保存接口
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin,Client")]
        public JsonResult Save([FromBody] TuihuoguanliDbModel entity)
        {
            try
            {
                entity.Id = DateTime.Now.Ticks / 100000;
                if (_bll.BaseInsert(entity) > 0)
                {
                    return Json(new { Code = 0, Msg = "Add Succeeded！" });
                }

                return Json(new { Code = -1, Msg = "Add Failed！" });
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, Msg = ex.Message });
            }
        }

        /// <summary>
        /// 保存接口
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Add([FromBody] TuihuoguanliDbModel entity)
        {
            try
            {
                entity.Id = DateTime.Now.Ticks / 100000;

                if (_bll.BaseInsert(entity) > 0)
                {
                    return Json(new { Code = 0, Msg = "Add Succeeded！" });
                }

                return Json(new { Code = -1, Msg = "Add Failed！" });
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, Msg = ex.Message });
            }
        }

        /// <summary>
        /// Updating the interface
        /// </summary>
        /// <param name="entity">Updating Entity Objects</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin,Client")]
        public JsonResult Update([FromBody] TuihuoguanliDbModel entity)
        {

            try
            {
                if (_bll.BaseUpdate(entity))
                {
                    return Json(new { Code = 0, Msg = "Modify Succeeded" });
                }

                return Json(new { Code = -1, Msg = "Modify Failed" });
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, Msg = ex.Message });
            }
        }

        /// <summary>
        /// Delete interface
        /// </summary>
        /// <param name="ids">Primary key int[]</param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize(Roles = "Admin,Client")]
        public JsonResult Delete([FromBody] dynamic[] ids)
        {
            try
            {
                if (_bll.BaseDels(ids))
                {
                    return Json(new { Code = 0, Msg = "Delete Succeeded" });
                }

                return Json(new { Code = -1, Msg = "Delete Failed" });
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, Msg = ex.Message });
            }
        }

        /// <summary>
        /// Detailed Interface
        /// </summary>
        /// <param name="id">Primary key id</param>
        /// <returns></returns>
        [HttpPost("{id}")]
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Client")]
        public JsonResult Info(long id)
        {
            try
            {
                return Json(new { Code = 0, Data = _bll.BaseGetById(id) });
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, Msg = ex.Message });
            }
        }

        /// <summary>
        /// Detailed Interface
        /// </summary>
        /// <param name="id">Primary key id</param>
        /// <returns></returns>
        [HttpPost("{id}")]
        [HttpGet("{id}")]
        public JsonResult Detail(long id)
        {
            try
            {
                return Json(new { Code = 0, Data = _bll.BaseGetById(id) });
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, Msg = ex.Message });
            }
        }

        


		/// <summary>
        /// 获取需要提醒的记录数接口
        /// </summary>
        /// <param name="columnName">listings</param>
        /// <param name="type">类型（1表示数字比较提醒，2表示日期比较提醒）</param>
        /// <param name="remindStart">remindStart小于等于columnName满足Result件提醒,当比较日期时，该Value表示天数</param>
        /// <param name="remindEnd">columnName小于等于remindEnd 满足Result件提醒,当比较日期时，该Value表示天数</param>
        /// <returns></returns>
        [HttpGet("{columnName}/{type}")]
        public JsonResult Remind(string columnName, int type, int remindStart, int remindEnd)
        {
            try
            {
                return Json(new { Code = 0, Count = _bll.Common("tuihuoguanli", columnName, "", type, "remind", remindStart, remindEnd) });
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, Msg = ex.Message });
            }
        }



    }
}
