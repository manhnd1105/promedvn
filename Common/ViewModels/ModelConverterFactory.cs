using Common.Contract.Records;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using AutoMapper;

namespace Common.ViewModels
{
    public abstract class ModelConverterBase
    {
        public ModelConverterBase()
        {
            InitProfiles();
        }

        protected abstract void InitProfiles();

        public TDestination Convert<TSource, TDestination>(TSource source)
            where TSource : class
            where TDestination : class
        {
            try
            {
                AutoMapper.Mapper.CreateMap<TSource, TDestination>();
                return AutoMapper.Mapper.Map<TDestination>(source);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public List<TDestination> Converts<TSource, TDestination>(List<TSource> sources)
            where TSource : class
            where TDestination : class
        {
            try
            {
                AutoMapper.Mapper.CreateMap<TSource, TDestination>();
                var result = new List<TDestination>();
                sources.ForEach(source =>
                {
                    result.Add(AutoMapper.Mapper.Map<TDestination>(source));
                });
                return result;
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}