using System;
using System.Collections.Generic;

using Microsoft.Practices.Unity;

namespace Scheduler.Core.Configurations
{
    public static class GlobalUnity
    {
        static GlobalUnity()
        {
            Container = new UnityContainer();
        }

        public static IUnityContainer Container { get; set; }

        public static T Resolve<T>(Type type)
        {
            return (T)Container.Resolve(type);
        }

        public static T Resolve<T>(Type type, string name, params KeyValuePair<string, object>[] parameters)
        {
            var overrides = new ParameterOverrides();

            foreach (var parameter in parameters)
            {
                overrides.Add(parameter.Key, parameter.Value);
            }

            return (T)Container.Resolve(type, name, overrides);
        }

        public static T Resolve<T>(Type type, params KeyValuePair<string, object>[] parameters)
        {
            var overrides = new ParameterOverrides();

            foreach (var parameter in parameters)
            {
                overrides.Add(parameter.Key, parameter.Value);
            }

            return (T)Container.Resolve(type, overrides);
        }

        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }

        public static T Resolve<T>(string name, params KeyValuePair<string, object>[] parameters)
        {
            var overrides = new ParameterOverrides();

            foreach (var parameter in parameters)
            {
                overrides.Add(parameter.Key, parameter.Value);
            }

            return Container.Resolve<T>(name, overrides);
        }

        public static T Resolve<T>(params KeyValuePair<string, object>[] parameters)
        {
            var overrides = new ParameterOverrides();

            foreach(var parameter in parameters)
            {
                overrides.Add(parameter.Key, parameter.Value);
            }

            return Container.Resolve<T>(overrides);
        }
    }
}
