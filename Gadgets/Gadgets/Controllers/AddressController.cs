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
    /// Address-related interfaces
    /// </summary>
    [Route("[controller]/[action]")]
    public class AddressController : Controller
    {
        private readonly long _uid;
        private readonly string _role;
        private readonly AddressService _bll;

        /// <summary>
        /// constructor
        /// </summary>
        public AddressController()
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

            _bll = new AddressService();
        }


        /// <summary>
        /// sub-Page interface
        /// </summary>
        /// <param name="page">Current Page</param>
        /// <param name="limit">Length of each Page record</param>
        /// <param name="sort">Sort Fields</param>
        /// <param name="order">Ascending order (Defaultasc)</param>
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
        /// <param name="page">Current Page</param>
        /// <param name="limit">Length of each Page record</param>
        /// <param name="sort">Sort Fields</param>
        /// <param name="order">Ascending order (Defaultasc)</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult List(int page = 1, int limit = 10, string sort = "id", string order = "asc")
        {
            try
            {
                List<IConditionalModel> conModels = new List<IConditionalModel>();
                return Json(_bll.GetPageList(page, limit, sort, order, conModels));
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, Msg = ex.Message });
            }
        }

        /// <summary>
        /// Preservation Interface
        /// </summary>
        /// <param name="entity">physical object</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin,Client")]
        public JsonResult Save([FromBody] AddressDbModel entity)
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
        /// Preservation Interface
        /// </summary>
        /// <param name="entity">physical object</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Add([FromBody] AddressDbModel entity)
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
        /// Updating the interface
        /// </summary>
        /// <param name="entity">Updating Entity Objects</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin,Client")]
        public JsonResult Update([FromBody] AddressDbModel entity)
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
        /// Get DefaultAddress
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Client")]
        public JsonResult Default()
        {
            try
            {
                return Json(new { Code = 0, Data = _bll.GetDefault(CacheHelper.TokenModel.Uid) });
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, Msg = ex.Message });
            }
        }

        /// <summary>
        /// Get Number of Records to Remind Interface
        /// </summary>
        /// <param name="columnName">listings</param>
        /// <param name="type">Type (1 for numeric comparison reminders, 2 for date comparison reminders)</param>
        /// <param name="remindStart">remindStart is less than or equal to columnName to meet the Result pieces remind, when comparing the date, the Value represents the number of days</param>
        /// <param name="remindEnd">columnName is less than or equal to remindEnd meet Result pieces remind, when comparing the date, the Value represents the number of days</param>
        /// <returns></returns>
        [HttpGet("{columnName}/{type}")]
        public JsonResult Remind(string columnName, int type, int remindStart, int remindEnd)
        {
            try
            {
                return Json(new { Code = 0, Count = _bll.Common("address", columnName, "", type, "remind", remindStart, remindEnd) });
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, Msg = ex.Message });
            }
        }



    }
}
