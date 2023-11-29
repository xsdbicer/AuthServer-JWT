using AuthServerJWT.Core.Dtos;
using AuthServerJWT.Core.models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServerJWT.Service
{
    public class DtoMapper:Profile
    {
        public DtoMapper()
        {
            CreateMap<ProductDto,Product>().ReverseMap();
            CreateMap<UserAppDto,UserApp>().ReverseMap();
        }
    } 
}
