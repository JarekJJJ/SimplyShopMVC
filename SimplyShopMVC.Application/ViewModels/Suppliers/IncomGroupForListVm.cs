﻿using AutoMapper;
using SimplyShopMVC.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyShopMVC.Application.ViewModels.Suppliers
{
    public class IncomGroupForListVm : IMapFrom<SimplyShopMVC.Domain.Model.Suppliers.IncomGroup>
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int GroupIdHome { get; set; }
        public string Name { get; set; }
        public int? CategoryId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<IncomGroupForListVm, SimplyShopMVC.Domain.Model.Suppliers.IncomGroup>().ReverseMap();
        }
    }
}
