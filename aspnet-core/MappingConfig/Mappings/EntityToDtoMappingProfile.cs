﻿using AutoMapper;
using TgDrive.Domain.Shared;
using TgDrive.DataAccess.EntityFrameworkCore.Entities;

namespace TgDrive.Infrastructure.AutoMapper;

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
