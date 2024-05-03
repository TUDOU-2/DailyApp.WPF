using AutoMapper;
using DailyApp.API.ApiRepinses;
using DailyApp.API.DataModel;
using DailyApp.API.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DailyApp.API.Controllers
{
    /// <summary>
    /// 账号接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly DailyDbContext _db; // 数据库上下文
        private readonly IMapper _mapper;

        public AccountController(DailyDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="accountInfoDTO">注册信息</param>
        /// <returns>-1:账户已存在; 1:注册成功; -99:未知错误</returns>
        [HttpPost]
        public IActionResult Reg(AccountInfoDTO accountInfoDTO)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var dbAccount = _db.AccountInfo.Where(t => t.Account == accountInfoDTO.Account).FirstOrDefault();
                if (dbAccount != null)
                {
                    response.ResultCode = -1;
                    response.Msg = "账号已存在";
                    return Ok(response);
                }
                else
                {
                    AccountInfo accountInfo = _mapper.Map<AccountInfo>(accountInfoDTO);
                    _db.AccountInfo.Add(accountInfo);
                    int result = _db.SaveChanges();
                    if (result == 1)
                    {
                        response.ResultCode = 1;
                        response.Msg = "注册成功";
                    }
                    else
                    {
                        response.ResultCode = -99;
                        response.Msg = "服务器忙,请稍等...";
                    }
                }
            }
            catch (Exception)
            {
                response.ResultCode = -99;
                response.Msg = "服务器忙,请稍等...";
            }
            return Ok(response);
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="account">账户</param>
        /// <param name="pwd">密码</param>
        /// <returns>1:登录成功; -1:登录失败; -99:未知错误</returns>
        [HttpGet]
        public IActionResult Login(string account, string pwd)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var dbAccount = _db.AccountInfo.Where(t => t.Account == account && t.Pwd == pwd).FirstOrDefault();
                if (dbAccount != null)
                {
                    response.ResultCode = 1;
                    response.Msg = "登录成功";
                    response.ResultData = dbAccount;
                }
                else
                {
                    response.ResultCode = -1;
                    response.Msg = "账号或密码错误";
                }
            }
            catch (Exception)
            {
                response.ResultCode = -99;
                response.Msg = "服务器忙,请稍等...";
            }
            return Ok(response);
        }
    }
}
