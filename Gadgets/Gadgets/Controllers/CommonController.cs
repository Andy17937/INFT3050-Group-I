using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Gadgets.Business.Services;
using Gadgets.Common.Helpers;
using Gadgets.Models.ViewModel;

namespace Gadgets.Controllers
{
    /// <summary>
    /// public interface
    /// </summary>
    [Route("[action]")]
    public class CommonController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly string _savePath;
        private readonly CommonService _bll;
        private readonly ConfigService _configBLL;

        /// <summary>
        /// constructor
        /// </summary>
        public CommonController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _savePath = _hostingEnvironment.WebRootPath + "\\upload\\";
            _bll = new CommonService();
            _configBLL = new ConfigService();
        }

        /// <summary>
        /// Get a field of a table List interface
        /// </summary>
        /// <param name="tableName">table name</param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        [HttpGet("{tableName}/{columnName}")]
        public JsonResult Option(string tableName, string columnName)
        {
            try
            {
                int level = Convert.ToInt32(HttpContext.Request.Query["level"]);
                string parent = HttpContext.Request.Query["parent"];
                if (level == 0)
                {
                    return Json(new { Code = 0, Data = _bll.Common(tableName, columnName, parent, level) });
                }
                else
                {
                    return Json(new { Code = 0, Data = _bll.Common(tableName, columnName) });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, Msg = ex.Message });
            }
        }

        /// <summary>
        /// Get a single row of a table based on the option field Value interface
        /// </summary>
        /// <param name="tableName">table name</param>
        /// <param name="columnName">listings</param>
        /// <param name="columnValue">列Value</param>
        /// <returns></returns>
        [HttpGet("{tableName}/{columnName}")]
        public JsonResult Follow(string tableName, string columnName, string columnValue)
        {
            try
            {
                return Json(new { Code = 0, Data = _bll.Common(tableName, columnName, columnValue, 0, "follow") });
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, Msg = ex.Message });
            }
        }

        /// <summary>
        /// Based on the sfshStatus interface of the primary key idModifytable table
        /// </summary>
        /// <param name="tableName">table name</param>
        /// <param name="id">Primary key id</param>
        /// <param name="sfsh">Current Audit Status（Yes/No）</param>
        /// <returns></returns>
        [HttpPost("{tableName}")]
        [Authorize(Roles = "Admin,Client")]
        public JsonResult Sh(string tableName, int id, string sfsh)
        {
            try
            {
                if (_bll.Common(tableName, id.ToString(), sfsh, 0, "sh") > 0)
                {
                    return Json(new { Code = 0, Msg = "更新成功！" });
                }

                return Json(new { Code = -1, Msg = "更新失败！" });
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, Msg = ex.Message });
            }
        }

        /// <summary>
        /// Get Number of Records to Remind Interface
        /// </summary>
        /// <param name="tableName">table name</param>
        /// <param name="columnName">listings</param>
        /// <param name="type">Type (1 for numeric comparison reminders, 2 for date comparison reminders)</param>
        /// <param name="remindStart">remindStart is less than or equal to columnName to meet the Result pieces remind, when comparing the date, the Value represents the number of days</param>
        /// <param name="remindEnd">columnName is less than or equal to remindEnd meet Result pieces remind, when comparing the date, the Value represents the number of days</param>
        /// <returns></returns>
        [HttpGet("{tableName}/{columnName}/{type}")]
        public JsonResult Remind(string tableName, string columnName, int type, int remindStart, int remindEnd)
        {
            try
            {
                return Json(new { Code = 0, Count = _bll.Common(tableName, columnName, "", type, "remind", remindStart, remindEnd) });
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, Msg = ex.Message });
            }
        }

        /// <summary>
        /// Computation Rules Interface
        /// </summary>
        /// <param name="tableName">table name</param>
        /// <param name="columnName">listings</param>
        /// <returns></returns>
        [HttpGet("{tableName}/{columnName}")]
        public JsonResult Cal(string tableName, string columnName)
        {
            try
            {
                return Json(new { Code = 0, Data = _bll.Common(tableName, columnName, "", 0, "cal") });
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, Msg = ex.Message });
            }
        }

        /// <summary>
        /// Face Comparison
        /// </summary>
        /// <param name="face1">Pic1Name</param>
        /// <param name="face2">Pic2Name</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult MatchFace(string face1, string face2)
        {
            try
            {
                BaiduAiHelper.clientId = _configBLL.GetValueByName("APIKey");
                BaiduAiHelper.clientSecret = _configBLL.GetValueByName("SecretKey");
                BaiduAiHelper.GetAccessToken();
                List<FaceMatchViewModel> matchInfo = new List<FaceMatchViewModel>
                {
                    new FaceMatchViewModel { image = FuncHelper.ImageToBase64(_savePath + face1) },
                    new FaceMatchViewModel { image = FuncHelper.ImageToBase64(_savePath + face2) }
                };
                string result = BaiduAiHelper.FaceMatch(JsonConvert.SerializeObject(matchInfo));
                dynamic resObj = JsonConvert.DeserializeObject(result);
                if (resObj.error_code == 0)
                {
                    return Json(new { Code = 0, Score = resObj.result.score, Msg = "匹配成功！" });
                }
                else
                {
                    return Json(new { Code = -1, Score = 0, Msg = "匹配失败！" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, Msg = ex.Message });
            }
        }

        /// <summary>
        /// Locate interface (based on latitude and longitude coordinates to get the province, city and county (district) Message)
        /// </summary>
        /// <param name="lat">longitudes</param>
        /// <param name="lng">longitude</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Location(double lat, double lng)
        {
            try
            {
                AddressViewModel addressViewModel = BaiduAiHelper.GetAddress(_configBLL.GetValueByName("baidu_ditu_ak"), lng, lat);
                if (addressViewModel == null)
                {
                    return Json(new { Code = -1, Msg = "Failed to get location information!" });
                }
                else
                {
                    return Json(new { Code = 0, Data = addressViewModel });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, Msg = ex.Message });
            }
        }

        /// <summary>
        /// Category Statistics Interface
        /// </summary>
        /// <param name="tableName">table name</param>
        /// <param name="columnName">listings</param>
        /// <returns></returns>
        [HttpGet("{tableName}/{columnName}")]
        public JsonResult Group(string tableName, string columnName)
        {
            try
            {
                return Json(new { Code = 0, Data = _bll.Common(tableName, columnName, "", 0, "group") });
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, Msg = ex.Message });
            }
        }

        /// <summary>
        /// Statistics by Value interface
        /// </summary>
        /// <param name="tableName">table name</param>
        /// <param name="xColumnName">listings</param>
        /// <param name="yColumnName">listings</param>
        /// <returns></returns>
        [HttpGet("{tableName}/{xColumnName}/{yColumnName}")]
        public JsonResult Value(string tableName, string xColumnName, string yColumnName)
        {
            try
            {
                return Json(new { Code = 0, Data = _bll.Common(tableName, xColumnName, yColumnName, 0, "value") });
            }
            catch (Exception ex)
            {
                return Json(new { Code = 500, Msg = ex.Message });
            }
        }
    }
}
