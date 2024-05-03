namespace DailyApp.API.DTOs
{
    /// <summary>
    /// 账户DTO(用来接收注册信息)
    /// </summary>
    public class AccountInfoDTO
    {
        public string Name { get; set; }
        public string Account { get; set; }
        public string Pwd { get; set; }
    }
}
