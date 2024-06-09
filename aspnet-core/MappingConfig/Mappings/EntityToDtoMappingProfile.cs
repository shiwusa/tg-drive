using AutoMapper;
using DataTransfer.Objects;
using EfRepositories.Entities;

namespace TgDrive.Config.AutoMapper;

public class EntityToDtoMappingProfile : Profile
{
    public EntityToDtoMappingProfile()
    {
        CreateMap<DirectoryEntity, DirectoryDto>()
            .ReverseMap();
        CreateMap<FileEntity, FileDto>()
            .ForMember(x => x.ChatId, opt => opt.MapFrom(x => x.ChannelId))
            .ReverseMap()
            .ForMember(x => x.ChannelId, opt => opt.MapFrom(x => x.ChatId));
        CreateMap<UserInfoEntity, UserInfoDto>()
            .ReverseMap();
    }
}
