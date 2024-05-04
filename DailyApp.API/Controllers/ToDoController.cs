using DailyApp.API.ApiRepinses;
using DailyApp.API.DataModel;
using DailyApp.API.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DailyApp.API.Controllers
{
    /// <summary>
    /// 待办事项接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly DailyDbContext _db; // 数据库上下文

        public ToDoController(DailyDbContext db) // 构造函数
        {
            _db = db;
        }

        /// <summary>
        /// 统计待办事项
        /// </summary>
        /// <returns>1：统计成功；-99：异常</returns>
        [HttpGet]
        public IActionResult StatWait()
        {
            ApiResponse res = new ApiResponse();
            try
            {
                var list = _db.ToDoInfo.ToList();
                var finishList = list.Where(t => t.status == 1).ToList();

                StatWaitDTO statDto = new StatWaitDTO { TotalCount = list.Count, FinishedCount = finishList.Count };

                res.ResultCode = 1; // 统计成功
                res.Msg = "统计待办事项成功";
                res.ResultData = statDto;
            }
            catch (Exception)
            {
                res.ResultCode = -99;
                res.Msg = "服务器忙，请稍后...";
            }
            return Ok(res);
        }
    }
}
