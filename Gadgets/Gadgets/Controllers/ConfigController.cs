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

namespace Gadgets.Controllers
{
    /// <summary>
    /// 系统配置相关接口
    /// </summary>
    [Route("[controller]/[action]")]
    public class ConfigController : Controller
    {
        private readonly ConfigService _bll;

        /// <summary>
        /// constructor
        /// </summary>
        public ConfigController()
        {
            _bll = new ConfigService();
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
                return Json(_bll.BaseGetPageList(page, limit, sort, order));
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
                return Json(_bll.BaseGetPageList(page, limit, sort, order));
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
        public JsonResult Update([FromBody] ConfigDbModel entity)
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
        /// 配置获取接口
        /// </summary>
        /// <param name="name">配置Name</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin,Client")]
        public JsonResult Info(string name)
        {
            try
            {
                return Json(new { Code = 0, Value = _bll.GetValueByName(name) });
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
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Client")]
        public JsonResult Info(int id)
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
        /// 配置获取接口
        /// </summary>
        /// <param name="name">配置Name</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin,Client")]
        public JsonResult Detail(string name)
        {
            try
            {
                return Json(new { Code = 0, Value = _bll.GetValueByName(name) });
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, Msg = ex.Message });
            }
        }
    }
}
