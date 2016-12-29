﻿using AutoMapper;
using Golf.Model;
using Golf.ServiceLayer.Dto.Implementations;

namespace Golf.RESTService.App_Start
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<Golfer, GolferDto>();
                config.CreateMap<GolferDto, Golfer>();
            });
        }
    }
}