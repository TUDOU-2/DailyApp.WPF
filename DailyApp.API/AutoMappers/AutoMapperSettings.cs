using AutoMapper;
using DailyApp.API.DataModel;
using DailyApp.API.DTOs;

namespace DailyApp.API.AutoMappers
{
    public class AutoMapperSettings : Profile
    {
        /// <summary>
        /// model和DTO映射
        /// </summary>
        public AutoMapperSettings()
        {
            CreateMap<AccountInfoDTO, AccountInfo>().ReverseMap();
            CreateMap<ToDoDTO, ToDoInfo>().ReverseMap();
            CreateMap<MemoDTO, MemoInfo>().ReverseMap();
        }
    }
}
