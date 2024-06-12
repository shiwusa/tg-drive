﻿using AutoMapper;
using TgDrive.Domain.Shared;
using TgDrive.DataAccess.EntityFrameworkCore.Entities;
using Microsoft.EntityFrameworkCore;
using TgDrive.DataAccess.Shared;

namespace TgDrive.DataAccess.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly TgDriveContext _db;
    private readonly IMapper _mapper;

    public UserRepository(TgDriveContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<UserInfoDto?> GetUserInfo(long userId)
    {
        var userInfo = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
        return _mapper.Map<UserInfoDto>(userInfo);
    }

    public async Task<UserInfoDto> SetUserInfo(UserInfoDto userInfo)
    {
        var userInfoEntity =
            await _db.Users.FirstOrDefaultAsync(u => u.Id == userInfo.Id);
        if (userInfoEntity == null)
        {
            userInfoEntity = _mapper.Map<UserInfoEntity>(userInfo);
            _db.Users.Add(userInfoEntity);
        }
        else
        {
            userInfoEntity.StorageChannelId = userInfo.StorageChannelId;
        }

        await _db.SaveChangesAsync();
        return _mapper.Map<UserInfoDto>(userInfoEntity);
    }
}
