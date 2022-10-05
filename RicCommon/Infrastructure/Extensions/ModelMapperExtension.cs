using System;
using AutoMapper;

namespace RicCommon.Infrastructure.Extensions
{
    public static class ModelMapperExtension
    {
        public static TU Convert<T, TU>(this T input)
        {
            var configuration = new MapperConfiguration(cfg => cfg.CreateMap<T, TU>());

            var mapper = configuration.CreateMapper();

            return mapper.Map<TU>(input);

        }

        public static TU Convert<T, TU>(this T input, Action<T, TU> customMapper)
        {
            TU output = Convert<T, TU>(input);
            if (customMapper != null)
            {
                customMapper(input, output);
            }
            return output;
        }
    }
}