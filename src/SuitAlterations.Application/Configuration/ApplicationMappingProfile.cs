using AutoMapper;
using SuitAlterations.Application.Customers;
using SuitAlterations.Application.SuitAlterations;
using SuitAlterations.Domain.Customers;
using SuitAlterations.Domain.SuitAlterations;

namespace SuitAlterations.Application.Configuration
{
	public class ApplicationMappingProfile : Profile
	{
		public ApplicationMappingProfile()
		{
			CreateMap<Customer, CustomerDto>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
				.ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

			CreateMap<SuitAlteration, SuitAlterationDto>(MemberList.Destination)
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.Value))
				.ForMember(dest => dest.AlterationTitle,
					opt => opt.MapFrom(src => $"Order#: {src.Id.Value} | Status: {src.Status}"))
				.ReverseMap();
		}
	}
}