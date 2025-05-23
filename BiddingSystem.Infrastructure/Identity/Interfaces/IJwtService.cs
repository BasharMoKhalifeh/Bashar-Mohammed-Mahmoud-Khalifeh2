﻿using BiddingSystem.Application.DTOs.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Domain.Entities;

namespace BiddingSystem.Infrastructure.Identity.Interfaces
{
    public interface IJwtService
    {
        Task<JwtToken> GenerateTokenAsync(User user);
    }
}
