using AutoMapper;
using SuitAlterations.Core.Entities;
using SuitAlterations.Dto;

namespace SuitAlterations {
	public class SuitAlterationsMappingProfile : Profile {
		public SuitAlterationsMappingProfile() {
			CreateMap<Customer, CustomerDto>()
				.ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

			CreateMap<SuitAlteration, SuitAlterationDto>()
				.ForMember(dest => dest.AlterationTitle, opt => opt.MapFrom(src => $"Alt.#: {src.Id} | Status: {src.Status}"))
				.ReverseMap()
				.ForMember(dest => dest.Customer, opt => opt.Ignore());
		}
	}
}