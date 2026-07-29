using AutoMapper;
using LaborStats.Application.ProfessionGroups;
using LaborStats.Application.Professions;
using LaborStats.Domain.Entities;

namespace LaborStats.Infrastructure.Mappings;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateProfessionRequest, Professions>();
        CreateMap<UpdateProfessionRequest, Professions>()
            .ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember is not null));
        CreateMap<Professions, ProfessionResponse>()
            .ForMember(dest => dest.ProfessionGroupName, opt => opt.MapFrom(src => src.ProfessionGroup.Name));
        CreateMap<Professions, ProfessionDetailResponse>()
            .ForMember(dest => dest.ProfessionGroupName, opt => opt.MapFrom(src => src.ProfessionGroup.Name));

        CreateMap<CreateProfessionGroupRequest, ProfessionGroups>();
        CreateMap<UpdateProfessionGroupRequest, ProfessionGroups>()
            .ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember is not null));
        CreateMap<ProfessionGroups, ProfessionGroupResponse>();
        CreateMap<ProfessionGroups, ProfessionGroupDetailResponse>()
            .ForMember(dest => dest.Professions, opt => opt.MapFrom(src => src.Professions.OrderBy(p => p.KzisCode)));
        CreateMap<Professions, ProfessionGroupProfessionResponse>();
    }
}
