using AutoMapper;
using MilkMatrix.Milk.Models;
using MilkMatrix.Api.Models.Request.Accounts.Accountgroups;
using MilkMatrix.Milk.Models.Request.Accounts.AccountGroups;
using MilkMatrix.Milk.Implementations;

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

            CreateMap<AccountHeadsInsertRequestModel, AccountHeadsInsertRequest>()
                       .ForMember(x => x.BusinessId, opt => opt.MapFrom(src => src.BusinessId))
                        .ForMember(x => x.Code, opt => opt.MapFrom(src => src.Code))
                        .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name))
                        .ForMember(x => x.GroupId, opt => opt.MapFrom(src => src.GroupId))
                        .ForMember(x => x.LedgerType, opt => opt.MapFrom(src => src.LedgerType))
                        .ForMember(x => x.CashBankType, opt => opt.MapFrom(src => src.CashBankType))
                        .ForMember(x => x.CityId, opt => opt.MapFrom(src => src.CityId))
                        .ForMember(x => x.CityText, opt => opt.MapFrom(src => src.CityText))
                        .ForMember(x => x.AlternateCode, opt => opt.MapFrom(src => src.AlternateCode))
                        .ForMember(x => x.BudgetApplicable, opt => opt.MapFrom(src => src.BudgetApplicable))
                        .ForMember(x => x.CostCenterApplicable, opt => opt.MapFrom(src => src.CostCenterApplicable))
                        .ForMember(x => x.TdsApplicable, opt => opt.MapFrom(src => src.TdsApplicable))
                        .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.IsActive))
                        .ForMember(x => x.BranchId, opt => opt.MapFrom(src => src.BranchId))
                        .ForMember(x => x.Notes, opt => opt.MapFrom(src => src.Notes))
                        .ForMember(x => x.CreatedBy, opt => opt.MapFrom((src, dest, destMember, context) =>
                            context.Items[Constants.AutoMapper.CreatedBy]));

                    CreateMap<AccountHeadsUpdateRequestModel , AccountHeadsUpdateRequest >()
                        .ForMember(x => x.BusinessId, opt => opt.MapFrom(src => src.BusinessId))
                        .ForMember(x => x.Code, opt => opt.MapFrom(src => src.Code))
                        .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name))
                        .ForMember(x => x.GroupId, opt => opt.MapFrom(src => src.GroupId))
                        .ForMember(x => x.LedgerType, opt => opt.MapFrom(src => src.LedgerType))
                        .ForMember(x => x.CashBankType, opt => opt.MapFrom(src => src.CashBankType))
                        .ForMember(x => x.CityId, opt => opt.MapFrom(src => src.CityId))
                        .ForMember(x => x.CityText, opt => opt.MapFrom(src => src.CityText))
                        .ForMember(x => x.AlternateCode, opt => opt.MapFrom(src => src.AlternateCode))
                        .ForMember(x => x.BudgetApplicable, opt => opt.MapFrom(src => src.BudgetApplicable))
                        .ForMember(x => x.CostCenterApplicable, opt => opt.MapFrom(src => src.CostCenterApplicable))
                        .ForMember(x => x.TdsApplicable, opt => opt.MapFrom(src => src.TdsApplicable))
                        .ForMember(x => x.IsActive, opt => opt.MapFrom(src => src.IsActive))
                        .ForMember(x => x.BranchId, opt => opt.MapFrom(src => src.BranchId))
                        .ForMember(x => x.Notes, opt => opt.MapFrom(src => src.Notes))
                        .ForMember(x => x.ModifyBy, opt => opt.MapFrom((src, dest, destMember, context) =>
                            context.Items[Constants.AutoMapper.ModifiedBy]));


        }
    }
}
