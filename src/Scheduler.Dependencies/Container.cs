using System;
using System.Collections.Generic;

using Microsoft.Practices.Unity;

namespace Scheduler.Dependencies
{
    public class Container
    {
        internal static IUnityContainer Unity;

        static Container()
        {
            Unity = new UnityContainer();
        }

        public static IUnityContainer GetInstance()
        {
            return Unity;
        }

        public static void RegisterSingleton<TFrom, TTo>() where TTo : TFrom
        {
            Unity.RegisterType<TFrom, TTo>(new ContainerControlledLifetimeManager());
        }

        public static void RegisterSingleton<TFrom, TTo>(params object[] constructorParams) where TTo : TFrom
        {
            Unity.RegisterType<TFrom, TTo>(new ContainerControlledLifetimeManager(), new InjectionConstructor(constructorParams));
        }

        public static void RegisterType<TFrom, TTo>() where TTo : TFrom
        {
            Unity.RegisterType<TFrom, TTo>();
        }

        public static void RegisterType<TFrom, TTo>(params object[] constructorParams) where TTo : TFrom
        {
            Unity.RegisterType<TFrom, TTo>(new InjectionConstructor(constructorParams));
        }

        public static T Resolve<T>()
        {
            return Unity.Resolve<T>();
        }

        public static T Resolve<T>(Type type)
        {
            return (T)Unity.Resolve(type);
        }

        public static T Resolve<T>(Type type, string name, params KeyValuePair<string, object>[] parameters)
        {
            var overrides = new ParameterOverrides();

            foreach (var parameter in parameters)
            {
                overrides.Add(parameter.Key, parameter.Value);
            }

            return (T)Unity.Resolve(type, name, overrides);
        }

        public static T Resolve<T>(Type type, params KeyValuePair<string, object>[] parameters)
        {
            var overrides = new ParameterOverrides();

            foreach (var parameter in parameters)
            {
                overrides.Add(parameter.Key, parameter.Value);
            }

            return (T)Unity.Resolve(type, overrides);
        }

        public static T Resolve<T>(string name, params KeyValuePair<string, object>[] parameters)
        {
            var overrides = new ParameterOverrides();

            foreach (var parameter in parameters)
            {
                overrides.Add(parameter.Key, parameter.Value);
            }

            return Unity.Resolve<T>(name, overrides);
        }

        public static T Resolve<T>(params KeyValuePair<string, object>[] parameters)
        {
            var overrides = new ParameterOverrides();

            foreach (var parameter in parameters)
            {
                overrides.Add(parameter.Key, parameter.Value);
            }

            return Unity.Resolve<T>(overrides);
        }
    }
}
