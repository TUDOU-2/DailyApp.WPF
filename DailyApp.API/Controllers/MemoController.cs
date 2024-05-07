using AutoMapper;
using DailyApp.API.ApiRepinses;
using DailyApp.API.DataModel;
using DailyApp.API.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DailyApp.API.Controllers
{
    /// <summary>
    /// 备忘录接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MemoController : ControllerBase
    {
        private readonly DailyDbContext _db; // 数据库上下文
        private readonly IMapper _mapper;

        public MemoController(DailyDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        /// <summary>
        /// 统计备忘录数据
        /// </summary>
        /// <returns>1:查询成功 -99:异常</returns>
        [HttpGet]
        public IActionResult SataMemo()
        {
            ApiResponse response = new ApiResponse();
            try
            {
                int count = _db.MemoInfo.Count();
                response.ResultCode = 1;
                response.Msg = "查询成功";
                response.ResultData = count;
            }
            catch
            {
                response.ResultCode = -99;
                response.Msg = "服务器忙,请稍等...";
            }
            return Ok(response);
        }

        /// <summary>
        /// 添加备忘录
        /// </summary>
        /// <param name="memoDTO">备忘录信息</param>
        /// <returns>1:添加成功 -1:失败 -99:异常</returns>
        [HttpPost]
        public IActionResult AddMemo(MemoDTO memoDTO)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                MemoInfo memoInfo = _mapper.Map<MemoInfo>(memoDTO); // 修改全部字段时可以直接转换，否则最好手动赋值
                _db.MemoInfo.Add(memoInfo);
                int result = _db.SaveChanges();
                if (result == 1)
                {
                    response.ResultCode = 1;
                    response.Msg = "添加成功";
                }
                else
                {
                    response.ResultCode = -1;
                    response.Msg = "添加失败";
                }
            }
            catch
            {
                response.ResultCode = -99;
                response.Msg = "服务器忙,请稍等...";
            }
            return Ok(response);
        }

        /// <summary>
        /// 备忘录查询
        /// </summary>
        /// <param name="title">标题(模糊查询)</param>
        /// <returns>1:添加成功 -1:失败 -2:ID错误 -99:异常</returns>
        [HttpGet]
        public IActionResult QueryMemo(string? title)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var query = from A in _db.MemoInfo
                            select new MemoDTO
                            {
                                MemoID = A.memoID,
                                Title = A.title,
                                Content = A.content,
                            };
                if (!string.IsNullOrEmpty(title))
                {
                    query = query.Where(m => m.Title.Contains(title));
                }
                response.ResultCode = 1;
                response.Msg = "查询成功";
                response.ResultData = query;
            }
            catch
            {
                response.ResultCode = -99;
                response.Msg = "服务器忙,请稍等...";
            }
            return Ok(response);
        }

        /// <summary>
        /// 编辑备忘录信息
        /// </summary>
        /// <param name="memoDTO">备忘录新信息</param>
        /// <returns>1:添加成功 -1:失败 -99:异常</returns>
        [HttpPut]
        public IActionResult EditMemo(MemoDTO memoDTO)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var dbInfo = _db.MemoInfo.Find(memoDTO.MemoID);
                if (dbInfo == null)
                {
                    response.ResultCode = -2;
                    response.Msg = "备忘录不存在";
                    return Ok(response);
                }

                dbInfo.title = memoDTO.Title;
                dbInfo.content = memoDTO.Content;

                int result = _db.SaveChanges();
                if (result == 1)
                {
                    response.ResultCode = 1;
                    response.Msg = "修改成功";
                }
                else
                {
                    response.ResultCode = -1;
                    response.Msg = "修改失败";
                }
            }
            catch
            {
                response.ResultCode = -99;
                response.Msg = "服务器忙,请稍等...";
            }
            return Ok(response);
        }

        /// <summary>
        /// 删除备忘录
        /// </summary>
        /// <param name="memoID">备忘录ID</param>
        /// <returns>1:删除成功 -2:ID异常 -99:异常</returns>
        [HttpDelete]
        public IActionResult DelMemo(int memoID)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var dbInfo = _db.MemoInfo.Find(memoID);
                if (dbInfo == null)
                {
                    response.ResultCode = -2;
                    response.Msg = "备忘录不存在";
                    return Ok(response);
                }

                _db.MemoInfo.Remove(dbInfo);
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
