using AutoMapper;
using MilkMatrix.Api.Models.Request.Member;
using MilkMatrix.Api.Models.Request.Member.MemberAddress;
using MilkMatrix.Api.Models.Request.Member.MemberBankDetails;
using MilkMatrix.Api.Models.Request.Member.MemberDocuments;
using MilkMatrix.Api.Models.Request.Member.MemberMilkProfile;
using MilkMatrix.Milk.Models.Request.Member;
using MilkMatrix.Milk.Models.Request.Member.MemberAddress;
using MilkMatrix.Milk.Models.Request.Member.MemberBankDetails;
using MilkMatrix.Milk.Models.Request.Member.MemberDocuments;
using MilkMatrix.Milk.Models.Request.Member.MemberMilkProfile;

namespace MilkMatrix.Api.Models.AutomapperProfiles
{
    public class MemberMappingProfile : Profile
    {
        public MemberMappingProfile()
        {
            CreateMap<MemberInsertRequestModel, MemberInsertRequest>()
                .ForMember(dest => dest.MemberCode, opt => opt.MapFrom(src => src.MemberCode))
                .ForMember(dest => dest.FarmerName, opt => opt.MapFrom(src => src.FarmerName))
                .ForMember(dest => dest.LocalName, opt => opt.MapFrom(src => src.LocalName))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.MobileNo, opt => opt.MapFrom(src => src.MobileNo))
                .ForMember(dest => dest.AlternateNo, opt => opt.MapFrom(src => src.AlternateNo))
                .ForMember(dest => dest.EmailID, opt => opt.MapFrom(src => src.EmailID))
                .ForMember(dest => dest.AadharNo, opt => opt.MapFrom(src => src.AadharNo))
                .ForMember(dest => dest.SocietyID, opt => opt.MapFrom(src => src.SocietyID))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom((src, dest, member, context) => context.Items["CreatedBy"]));

            CreateMap<MemberUpdateRequestModel, MemberUpdateRequest>()
                .ForMember(dest => dest.MemberID, opt => opt.MapFrom(src => src.MemberID))
                .ForMember(dest => dest.MemberCode, opt => opt.MapFrom(src => src.MemberCode))
                .ForMember(dest => dest.FarmerName, opt => opt.MapFrom(src => src.FarmerName))
                .ForMember(dest => dest.LocalName, opt => opt.MapFrom(src => src.LocalName))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.MobileNo, opt => opt.MapFrom(src => src.MobileNo))
                .ForMember(dest => dest.AlternateNo, opt => opt.MapFrom(src => src.AlternateNo))
                .ForMember(dest => dest.EmailID, opt => opt.MapFrom(src => src.EmailID))
                .ForMember(dest => dest.AadharNo, opt => opt.MapFrom(src => src.AadharNo)) 
                .ForMember(dest => dest.SocietyID, opt => opt.MapFrom(src => src.SocietyID))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsStatus))
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom((src, dest, member, context) => context.Items["ModifiedBy"]));


             CreateMap<MemberBankDetailsInsertRequestModel, MemberBankDetailsInsertRequest>()
                .ForMember(dest => dest.MemberID, opt => opt.MapFrom(src => src.MemberID))
                .ForMember(dest => dest.BankID, opt => opt.MapFrom(src => src.BankID))
                .ForMember(dest => dest.BranchID, opt => opt.MapFrom(src => src.BranchID))
                .ForMember(dest => dest.AccountHolderName, opt => opt.MapFrom(src => src.AccountHolderName))
                .ForMember(dest => dest.AccountNumber, opt => opt.MapFrom(src => src.AccountNumber))
                .ForMember(dest => dest.IFSCCode, opt => opt.MapFrom(src => src.IFSCCode))
                .ForMember(dest => dest.IsJointAccount, opt => opt.MapFrom(src => src.IsJointAccount))
                .ForMember(dest => dest.PassbookFilePath, opt => opt.MapFrom(src => src.PassbookFilePath))
                .ForMember(dest => dest.IsStatus, opt => opt.MapFrom(src => src.IsStatus))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom((src, dest, member, context) => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom((src, dest, member, context) => context.Items["CreatedBy"]));

            CreateMap<MemberBankDetailsUpdateRequestModel, MemberBankDetailsUpdateRequest>()
                .ForMember(dest => dest.BankDetailID, opt => opt.MapFrom(src => src.BankDetailID))
                .ForMember(dest => dest.MemberID, opt => opt.MapFrom(src => src.MemberID))
                .ForMember(dest => dest.BankID, opt => opt.MapFrom(src => src.BankID))
                .ForMember(dest => dest.BranchID, opt => opt.MapFrom(src => src.BranchID))
                .ForMember(dest => dest.AccountHolderName, opt => opt.MapFrom(src => src.AccountHolderName))
                .ForMember(dest => dest.AccountNumber, opt => opt.MapFrom(src => src.AccountNumber))
                .ForMember(dest => dest.IFSCCode, opt => opt.MapFrom(src => src.IFSCCode))
                .ForMember(dest => dest.IsJointAccount, opt => opt.MapFrom(src => src.IsJointAccount))
                .ForMember(dest => dest.PassbookFilePath, opt => opt.MapFrom(src => src.PassbookFilePath))
                .ForMember(dest => dest.IsStatus, opt => opt.MapFrom(src => (bool?)src.IsStatus))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                .ForMember(dest => dest.ModifiedOn, opt => opt.MapFrom((src, dest, member, context) => DateTime.UtcNow))
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom((src, dest, member, context) => context.Items["ModifiedBy"]));

            CreateMap<MemberMilkProfileInsertRequestModel, MemberMilkProfileInsertRequest>()
                .ForMember(dest => dest.MemberID, opt => opt.MapFrom(src => src.MemberID))
                .ForMember(dest => dest.AnimalType, opt => opt.MapFrom(src => src.AnimalType))
                .ForMember(dest => dest.NoOfMilchAnimals, opt => opt.MapFrom(src => src.NoOfMilchAnimals))
                .ForMember(dest => dest.AvgMilkYield, opt => opt.MapFrom(src => src.AvgMilkYield))
                .ForMember(dest => dest.PreferredShift, opt => opt.MapFrom(src => src.PreferredShift))
                .ForMember(dest => dest.PouringStartDate, opt => opt.MapFrom(src => src.PouringStartDate))
                .ForMember(dest => dest.IsStatus, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom((src, dest, member, context) => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom((src, dest, member, context) => context.Items["CreatedBy"]));

            CreateMap<MemberMilkProfileUpdateRequestModel, MemberMilkProfileUpdateRequest>()
                .ForMember(dest => dest.MilkProfileID, opt => opt.MapFrom(src => src.MilkProfileID))
                .ForMember(dest => dest.MemberID, opt => opt.MapFrom(src => src.MemberID))
                .ForMember(dest => dest.AnimalType, opt => opt.MapFrom(src => src.AnimalType))
                .ForMember(dest => dest.NoOfMilchAnimals, opt => opt.MapFrom(src => src.NoOfMilchAnimals))
                .ForMember(dest => dest.AvgMilkYield, opt => opt.MapFrom(src => src.AvgMilkYield))
                .ForMember(dest => dest.PreferredShift, opt => opt.MapFrom(src => src.PreferredShift))
                .ForMember(dest => dest.PouringStartDate, opt => opt.MapFrom(src => src.PouringStartDate))
                .ForMember(dest => dest.IsStatus, opt => opt.MapFrom(src => (bool?)src.IsStatus))
                .ForMember(dest => dest.ModifiedOn, opt => opt.MapFrom((src, dest, member, context) => DateTime.UtcNow))
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom((src, dest, member, context) => context.Items["ModifiedBy"]));
            CreateMap<MemberDocumentsInsertRequestModel, MemberDocumentsInsertRequest>()
         .ForMember(dest => dest.MemberID, opt => opt.MapFrom(src => src.MemberID))
         .ForMember(dest => dest.AadharFile, opt => opt.MapFrom(src => src.AadharFile))
         .ForMember(dest => dest.VoterOrRationCard, opt => opt.MapFrom(src => src.VoterOrRationCard))
         .ForMember(dest => dest.OtherDocument, opt => opt.MapFrom(src => src.OtherDocument))
         .ForMember(dest => dest.IsStatus, opt => opt.MapFrom(src => true))
         .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false))
         .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom((src, dest, member, context) => DateTime.UtcNow))
         .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom((src, dest, member, context) => context.Items["CreatedBy"]));

            CreateMap<MemberDocumentsUpdateRequestModel, MemberDocumentsUpdateRequest>()
                .ForMember(dest => dest.DocumentID, opt => opt.MapFrom(src => src.DocumentID))
                .ForMember(dest => dest.MemberID, opt => opt.MapFrom(src => src.MemberID))
                .ForMember(dest => dest.AadharFile, opt => opt.MapFrom(src => src.AadharFile))
                .ForMember(dest => dest.VoterOrRationCard, opt => opt.MapFrom(src => src.VoterOrRationCard))
                .ForMember(dest => dest.OtherDocument, opt => opt.MapFrom(src => src.OtherDocument))
                .ForMember(dest => dest.IsStatus, opt => opt.MapFrom(src => (bool?)src.IsStatus))
                .ForMember(dest => dest.ModifiedOn, opt => opt.MapFrom((src, dest, member, context) => DateTime.UtcNow))
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom((src, dest, member, context) => context.Items["ModifiedBy"]));

            CreateMap<MemberAddressInsertRequestModel, MemberAddressInsertRequest>()
                .ForMember(dest => dest.MemberID, opt => opt.MapFrom(src => src.MemberID))
                .ForMember(dest => dest.StateID, opt => opt.MapFrom(src => src.StateID))
                .ForMember(dest => dest.DistrictID, opt => opt.MapFrom(src => src.DistrictID))
                .ForMember(dest => dest.TehsilID, opt => opt.MapFrom(src => src.TehsilID))
                .ForMember(dest => dest.VillageID, opt => opt.MapFrom(src => src.VillageID))
                .ForMember(dest => dest.HamletID, opt => opt.MapFrom(src => src.HamletID))
                .ForMember(dest => dest.FullAddress, opt => opt.MapFrom(src => src.FullAddress))
                .ForMember(dest => dest.Pincode, opt => opt.MapFrom(src => src.Pincode))
                .ForMember(dest => dest.IsStatus, opt => opt.MapFrom(src => src.IsStatus))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom((src, dest, member, context) => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom((src, dest, member, context) => context.Items["CreatedBy"]));

            CreateMap<MemberAddressUpdateRequestModel, MemberAddressUpdateRequest>()
                .ForMember(dest => dest.AddressID, opt => opt.MapFrom(src => src.AddressID))
                .ForMember(dest => dest.MemberID, opt => opt.MapFrom(src => src.MemberID))
                .ForMember(dest => dest.StateID, opt => opt.MapFrom(src => src.StateID))
                .ForMember(dest => dest.DistrictID, opt => opt.MapFrom(src => src.DistrictID))
                .ForMember(dest => dest.TehsilID, opt => opt.MapFrom(src => src.TehsilID))
                .ForMember(dest => dest.VillageID, opt => opt.MapFrom(src => src.VillageID))
                .ForMember(dest => dest.HamletID, opt => opt.MapFrom(src => src.HamletID))
                .ForMember(dest => dest.FullAddress, opt => opt.MapFrom(src => src.FullAddress))
                .ForMember(dest => dest.Pincode, opt => opt.MapFrom(src => src.Pincode))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                .ForMember(dest => dest.IsStatus, opt => opt.MapFrom(src => (bool?)src.IsStatus))
                .ForMember(dest => dest.ModifiedOn, opt => opt.MapFrom((src, dest, member, context) => DateTime.UtcNow))
                .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom((src, dest, member, context) => context.Items["ModifiedBy"]));

        }
    }
}
