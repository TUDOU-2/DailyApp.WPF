using AutoMapper;
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
        private readonly IMapper _mapper;

        public ToDoController(DailyDbContext db, IMapper mapper) // 构造函数
        {
            _db = db;
            _mapper = mapper;
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

        /// <summary>
        /// 添加待办事项
        /// </summary>
        /// <param name="infoDTO">待办事项信息</param>
        /// <returns>1:添加成功; -1:添加失败; -99:异常</returns>
        [HttpPost]
        public IActionResult AddWait(ToDoDTO infoDTO)
        {
            ApiResponse response = new ApiResponse();

            try
            {
                ToDoInfo accountInfo = _mapper.Map<ToDoInfo>(infoDTO);
                _db.ToDoInfo.Add(accountInfo);
                int result = _db.SaveChanges();

                if (result == 1)
                {
                    response.ResultCode = 1;
                    response.Msg = "添加待办事项成功";
                }
                else
                {
                    response.ResultCode = -99;
                    response.Msg = "添加待办事项失败";
                }
            }
            catch (Exception)
            {
                response.ResultCode = -99;
                response.Msg = "服务器忙，请稍后...";
            }
            return Ok(response);
        }

        /// <summary>
        /// 获取待办状态的所有待办事项
        /// </summary>
        /// <returns>1：获取成功；-99：异常</returns>
        [HttpGet]
        public IActionResult GetToDoList()
        {
            ApiResponse res = new ApiResponse();
            try
            {
                var list = from t in _db.ToDoInfo
                           where t.status == 0
                           select new
                           {
                               t.waitID,
                               t.title,
                               t.content,
                               t.status
                           };
                res.ResultCode = 1;
                res.Msg = "获取待办事项成功";
                res.ResultData = list;
            }
            catch (Exception)
            {
                res.ResultCode = -99;
                res.Msg = "服务器忙，请稍后...";
            }
            return Ok(res);
        }

        /// <summary>
        /// 更新待办事项状态
        /// </summary>
        /// <param name="toDoDTO"></param>
        /// <returns>1:修改成功; -1:状态ID错误; -99:异常</returns>
        [HttpPut]
        public IActionResult UpdateStatus(ToDoDTO toDoDTO)
        {
            ApiResponse res = new ApiResponse();
            try
            {
                var dbInfo = _db.ToDoInfo.Find(toDoDTO.WaitID);
                if (dbInfo != null)
                {
                    dbInfo.status = toDoDTO.Status;
                    _db.ToDoInfo.Update(dbInfo);
                    int result = _db.SaveChanges();
                    if (result == 1)
                    {
                        res.ResultCode = 1;
                        res.Msg = (toDoDTO.Status == 0 ? "状态成功设置为待办" : "状态成功设置为已完成");
                    }
                    else
                    {
                        res.ResultCode = -99;
                        res.Msg = "设置失败";
                    }
                }
                else
                {

                    res.ResultCode = -1;
                    res.Msg = "请确认待办事项ID是否正确";
                }
            }
            catch (Exception)
            {
                res.ResultCode = -99;
                res.Msg = "服务器忙，请稍后...";
            }
            return Ok(res);
        }

        /// <summary>
        /// 修改待办事项
        /// </summary>
        /// <param name="toDoDTO"></param>
        /// <returns>1:修改成功; -1:状态ID错误; -99:异常</returns>
        [HttpPut]
        public IActionResult EditToDo(ToDoDTO toDoDTO)
        {
            ApiResponse res = new ApiResponse();
            try
            {
                var dbInfo = _db.ToDoInfo.Find(toDoDTO.WaitID);
                if (dbInfo != null)
                {
                    dbInfo.status = toDoDTO.Status;
                    dbInfo.title = toDoDTO.Title;
                    dbInfo.content = toDoDTO.Content;
                    _db.ToDoInfo.Update(dbInfo);
                    int result = _db.SaveChanges();
                    if (result == 1)
                    {
                        res.ResultCode = 1;
                        res.Msg = "编辑成功";
                    }
                    else
                    {
                        res.ResultCode = -99;
                        res.Msg = "编辑失败";
                    }
                }
                else
                {

                    res.ResultCode = -1;
                    res.Msg = "请确认待办事项ID是否正确";
                }
            }
            catch (Exception)
            {
                res.ResultCode = -99;
                res.Msg = "服务器忙，请稍后...";
            }
            return Ok(res);
        }

        /// <summary>
        /// 查询待办事项
        /// </summary>
        /// <param name="title">标题(模糊查询)</param>
        /// <param name="status">状态(等值查询)</param>
        /// <returns>1:查询成功</returns>
        [HttpGet]
        public IActionResult QueryToDo(string? title, int? status)
        {
            ApiResponse res = new ApiResponse();
            try
            {
                var query = from A in _db.ToDoInfo
                            select new
                            {
                                WaitID = A.waitID,
                                Title = A.title,
                                Content = A.content,
                                Status = A.status
                            };
                if (!string.IsNullOrEmpty(title))
                {
                    query = query.Where(t => t.Title.Contains(title));
                }
                if (status != null)
                {
                    query = query.Where(t => t.Status == status);
                }

                res.ResultCode = 1;
                res.Msg = "查询成功";
                res.ResultData = query;
            }
            catch (Exception)
            {
                res.ResultCode = -99;
                res.Msg = "服务器忙，请稍后...";
            }
            return Ok(res);
        }

        /// <summary>
        /// 删除待办事项
        /// </summary>
        /// <param name="toDoID">待办事项ID</param>
        /// <returns>1:删除成功 -2:ID异常 -99:异常</returns>
        [HttpDelete]
        public IActionResult DelToDo(int toDoID)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var dbInfo = _db.ToDoInfo.Find(toDoID);
                if (dbInfo == null)
                {
                    response.ResultCode = -2;
                    response.Msg = "待办事项不存在";
                    return Ok(response);
                }

                _db.ToDoInfo.Remove(dbInfo);
                int result = _db.SaveChanges();
                if (result == 1)
                {
                    response.ResultCode = 1;
                    response.Msg = "删除成功";
                }
                else
                {
                    response.ResultCode = -1;
                    response.Msg = "删除失败";
                }
            }
            catch
            {
                response.ResultCode = -99;
                response.Msg = "服务器忙,请稍等...";
            }
            return Ok(response);
        }
    }
}
