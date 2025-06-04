using AutoMapper;
using MilkMatrix.Admin.Models;
using MilkMatrix.Admin.Models.Login.Requests;
using MilkMatrix.Api.Models.Request.Login;

namespace DetaServices.Models.Automapper
{
  public class AutomapperProfile : Profile
  {
    public AutomapperProfile()
    {
      CreateMap<LoginModel, LoginRequest>()
          .ForMember(x => x.HostName, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.HostName]))
          .ForMember(x => x.PrivateIP, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.PrivateIp]))
          .ForMember(x => x.PublicIP, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.PublicIp]))
          .ForMember(x => x.BrowserName, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.UserAgent]))
          .ForMember(x => x.IsLoginWithOtp, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.IsLoginWithOtp]));
    }
  }
}
