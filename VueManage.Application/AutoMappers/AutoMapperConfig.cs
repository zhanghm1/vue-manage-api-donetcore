using AutoMapper;
using AutoMapper.EquivalencyExpression;
using System;
using System.Collections.Generic;
using System.Text;

namespace VueManage.Application
{
    public class AutoMapperConfig
    {
        public static Action<IMapperConfigurationExpression> RegisterMappings()
        {

            return cfg =>
            {
                cfg.AddCollectionMappers();
                cfg.AddProfile(new PermissionsMappingProfile());
                cfg.AddProfile(new UserMappingProfile());
                
            };
        }
    }
}
