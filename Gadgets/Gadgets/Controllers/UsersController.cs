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
    /// Dashboard员相关接口
    /// </summary>
    [Route("[controller]/[action]")]
    public class UsersController : Controller
    {
        private readonly UsersService _bll;

        /// <summary>
        /// constructor
        /// </summary>
        public UsersController()
        {
            _bll = new UsersService();
        }

        /// <summary>
        /// 用户Login接口
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Login(string username, string password)
        {
            try
            {
                var userInfo = _bll.Login(username, password);
                if (userInfo == null)
                {
                    return Json(new { Code = -1, Msg = "Incorrect account number or password!" });
                }

                TokenModel tokenModel = new TokenModel()
                {
                    Uid = userInfo.Id,
                    Uname = userInfo.Username,
                    Role = "Admin",
                    Project = "Gadgets",
                    TokenType = "Web",
                    UNickname = "",
                };
                CacheHelper.TokenModel = tokenModel;
                return Json(new { Code = 0, Token = JwtHelper.IssueJWT(tokenModel) });
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, Msg = ex.Message });
            }
        }

        /// <summary>
        /// 用户登出接口
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResult Logout()
        {
            return Json(new { Code = 0, Msg = "退出成功！" });
        }

        /// <summary>
        /// Register接口
        /// </summary>
        /// <param name="entity">Register实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResult Register([FromBody] UsersDbModel entity)
        {
            try
            {
                if (_bll.BaseInsert(entity) > 0)
                {
                    return Json(new { Code = 0, Msg = "Register Successed！" });
                }

                return Json(new { Code = -1, Msg = "Register失败！" });
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, Msg = ex.Message });
            }
        }

        /// <summary>
        /// 获取session的接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public JsonResult Session()
        {
            try
            {
                if (CacheHelper.TokenModel != null)
                {
                    TokenModel tm = CacheHelper.TokenModel;

                    return Json(new { Code = 0, Data = _bll.BaseGetById(tm.Uid) });
                }
                else return Json(new { Code = 500, Msg = "TokenMessage Not Found" });
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
        [Authorize(Roles = "Admin")]
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
        /// 保存接口
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResult Save([FromBody] UsersDbModel entity)
        {
            try
            {
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
        [Authorize(Roles = "Admin")]
        public JsonResult Update([FromBody] UsersDbModel entity)
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
        [Authorize(Roles = "Admin")]
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
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
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
    }
}
