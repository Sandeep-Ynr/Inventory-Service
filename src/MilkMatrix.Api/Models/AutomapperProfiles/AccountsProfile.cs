using AutoMapper;
using MilkMatrix.Milk.Models;
using MilkMatrix.Api.Models.Request.Accounts.Accountgroups;
using MilkMatrix.Milk.Models.Request.Accounts.AccountGroups;

namespace MilkMatrix.Api.Models.AutomapperProfiles
{
    public class AccountsProfile : Profile
    {
        public AccountsProfile() 
        {
            CreateMap<AccountGroupsUpdateRequestModel, AccountGroupsUpdateRequest>()
                        .ForMember(x => x.BusinessId, opt => opt.MapFrom(src => src.BusinessId))
                        .ForMember(x => x.Code, opt => opt.MapFrom(src => src.Code))
                        .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name))
                        .ForMember(x => x.ParentId, opt => opt.MapFrom(src => src.ParentId))
                        .ForMember(x => x.ScheduleId, opt => opt.MapFrom(src => src.ScheduleId))
                        .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.IsActive))
                        .ForMember(x => x.AllowPosting, opt => opt.MapFrom(src => src.AllowPosting))
                        .ForMember(x => x.Notes, opt => opt.MapFrom(src => src.Notes))
                        .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.IsActive))  // if request has IsActive
                        .ForMember(x => x.ModifyBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.ModifiedBy]));

            CreateMap<AccountGroupsAGInsertRequestModel, AccountGroupsInsertRequest>()
                        .ForMember(x => x.BusinessId, opt => opt.MapFrom(src => src.BusinessId))
                        .ForMember(x => x.Code, opt => opt.MapFrom(src => src.Code))
                        .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name))
                        .ForMember(x => x.ParentId, opt => opt.MapFrom(src => src.ParentId))
                        .ForMember(x => x.ScheduleId, opt => opt.MapFrom(src => src.ScheduleId))
                        .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.IsActive))
                        .ForMember(x => x.AllowPosting, opt => opt.MapFrom(src => src.AllowPosting))
                        .ForMember(x => x.Notes, opt => opt.MapFrom(src => src.Notes))
                        .ForMember(x => x.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) => context.Items[Constants.AutoMapper.CreatedBy]));
        }
    }
}
