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
    /// 订单相关接口
    /// </summary>
    [Route("[controller]/[action]")]
    public class OrdersController : Controller
    {
        private readonly long _uid;
        private readonly string _role;
        private readonly OrdersService _bll;

        /// <summary>
        /// 构造函数
        /// </summary>
        public OrdersController()
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

            _bll = new OrdersService();
        }


        /// <summary>
        /// 分Page接口
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
                if (_role != "Admin")
                {
                    conModels.Add(new ConditionalModel() { FieldName = "userid", ConditionalType = ConditionalType.Equal, FieldValue = _uid.ToString() });
                }
                var orderid = HttpContext.Request.Query["orderid"].ToString();
                if (!string.IsNullOrEmpty(orderid))
                {
                    if (orderid.Contains("%"))
                    {
                        conModels.Add(new ConditionalModel() { FieldName = "orderid", ConditionalType = ConditionalType.Like, FieldValue = orderid });
                    }
                    else
                    {
                        conModels.Add(new ConditionalModel() { FieldName = "orderid", ConditionalType = ConditionalType.Equal, FieldValue = orderid });
                    }
                }
                var goodname = HttpContext.Request.Query["goodname"].ToString();
                if (!string.IsNullOrEmpty(goodname))
                {
                    if (goodname.Contains("%"))
                    {
                        conModels.Add(new ConditionalModel() { FieldName = "goodname", ConditionalType = ConditionalType.Like, FieldValue = goodname });
                    }
                    else
                    {
                        conModels.Add(new ConditionalModel() { FieldName = "goodname", ConditionalType = ConditionalType.Equal, FieldValue = goodname });
                    }
                }
                string status = HttpContext.Request.Query["status"];
                if (!string.IsNullOrEmpty(status))
                {
                    conModels.Add(new ConditionalModel() { FieldName = "status", ConditionalType = ConditionalType.Equal, FieldValue = status });
                }
                int type = Convert.ToInt32(HttpContext.Request.Query["type"]);
                if (type != 0)
                {
                    conModels.Add(new ConditionalModel() { FieldName = "type", ConditionalType = ConditionalType.Equal, FieldValue = type.ToString() });
                }
                return Json(_bll.GetPageList(page, limit, sort, order, conModels));
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, Msg = ex.Message });
            }
        }

        /// <summary>
        /// 分Page接口
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
                var orderid = HttpContext.Request.Query["orderid"].ToString();
				if (!string.IsNullOrEmpty(orderid))
                {
                    if (orderid.Contains("%"))
                    {
                        conModels.Add(new ConditionalModel() { FieldName = "orderid", ConditionalType = ConditionalType.Like, FieldValue = orderid });
                    }
                    else
                    {
                        conModels.Add(new ConditionalModel() { FieldName = "orderid", ConditionalType = ConditionalType.Equal, FieldValue = orderid });
                    }
                }
                var goodname = HttpContext.Request.Query["goodname"].ToString();
				if (!string.IsNullOrEmpty(goodname))
                {
                    if (goodname.Contains("%"))
                    {
                        conModels.Add(new ConditionalModel() { FieldName = "goodname", ConditionalType = ConditionalType.Like, FieldValue = goodname });
                    }
                    else
                    {
                        conModels.Add(new ConditionalModel() { FieldName = "goodname", ConditionalType = ConditionalType.Equal, FieldValue = goodname });
                    }
                }
                string status = HttpContext.Request.Query["status"];
                if (!string.IsNullOrEmpty(status))
                {
                    conModels.Add(new ConditionalModel() { FieldName = "status", ConditionalType = ConditionalType.Equal, FieldValue = status });
                }
                int type = Convert.ToInt32(HttpContext.Request.Query["type"]);
                if (type != 0)
                {
                    conModels.Add(new ConditionalModel() { FieldName = "type", ConditionalType = ConditionalType.Equal, FieldValue = type.ToString() });
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
        public JsonResult Save([FromBody] OrdersDbModel entity)
        {
            try
            {
                entity.Id = DateTime.Now.Ticks / 100000;
				if (string.IsNullOrEmpty(entity.Userid.ToString()) || entity.Userid == 0) {
					entity.Userid = _uid;
				}
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
        public JsonResult Add([FromBody] OrdersDbModel entity)
        {
            try
            {
                entity.Id = DateTime.Now.Ticks / 100000;
                entity.Userid = _uid;

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
        /// 更新接口
        /// </summary>
        /// <param name="entity">更新实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin,Client")]
        public JsonResult Update([FromBody] OrdersDbModel entity)
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
        /// Delete接口
        /// </summary>
        /// <param name="ids">主键int[]</param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize(Roles = "Admin,Client")]
        public JsonResult Delete([FromBody] dynamic[] ids)
        {
            try
            {
                if (_bll.BaseDels(ids))
                {
                    return Json(new { Code = 0, Msg = "Delete成功！" });
                }

                return Json(new { Code = -1, Msg = "Delete Failed" });
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, Msg = ex.Message });
            }
        }

        /// <summary>
        /// 详情接口
        /// </summary>
        /// <param name="id">主键id</param>
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
        /// 详情接口
        /// </summary>
        /// <param name="id">主键id</param>
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
        /// <param name="columnName">列名</param>
        /// <param name="type">类型（1表示数字比较提醒，2表示日期比较提醒）</param>
        /// <param name="remindStart">remindStart小于等于columnName满足Result件提醒,当比较日期时，该Value表示天数</param>
        /// <param name="remindEnd">columnName小于等于remindEnd 满足Result件提醒,当比较日期时，该Value表示天数</param>
        /// <returns></returns>
        [HttpGet("{columnName}/{type}")]
        public JsonResult Remind(string columnName, int type, int remindStart, int remindEnd)
        {
            try
            {
                return Json(new { Code = 0, Count = _bll.Common("orders", columnName, "", type, "remind", remindStart, remindEnd) });
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, Msg = ex.Message });
            }
        }



    }
}
