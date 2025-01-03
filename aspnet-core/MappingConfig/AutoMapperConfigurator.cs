﻿using AutoMapper;

namespace TgDrive.Infrastructure.AutoMapper;

public static class AutoMapperConfigurator
{
    public static IMapper GetMapper()
    {
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new EntityToDtoMappingProfile());
        });

        var mapper = mapperConfig.CreateMapper();
        return mapper;
    }
}
