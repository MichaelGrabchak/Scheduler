using System;
using System.Linq;
using System.Collections.Generic;

using Scheduler.Core.Helpers;

namespace Scheduler.Core.Estensions
{
    public static class TypeExtensions
    {
        public static T Resolve<T>(this Type type)
        {
            if(type.IsInterface)
            {
                var impls = AssemblyLoaderManager.GetImplementorsOf(type);
                if (impls == null || impls.Count <= 0)
                {
                    throw new NotImplementedException($"Could not find implementor(s) for '{type.Name}'");
                }

                return Resolve<T>(impls.First());
            }

            var constructorParams = type.GetConstructorParameterTypes();

            // if no public constructors - assume it is singleton -> return property with name "Instance"
            if (constructorParams == null)
            {
                return (T)type.GetProperty("Instance").GetValue(null, null);
            }

            if (constructorParams.Length > 0)
            {
                var ctorParams = new List<object>();
                foreach(var ctorType in constructorParams)
                {
                    ctorParams.Add(Resolve<object>(ctorType));
                }

                return (T)Activator.CreateInstance(type, ctorParams.ToArray());
            }

            return (T)Activator.CreateInstance(type);
        }

        private static Type[] GetConstructorParameterTypes(this Type type)
        {
            var ctors = type.GetConstructors();

            if (ctors != null && ctors.Length > 0)
            {
                var ctrWithLessParams = ctors.First(ctor => ctor.GetParameters().Length == ctors.Min(_ => _.GetParameters().Length));
                return ctrWithLessParams.GetParameters().OrderBy(_ => _.Position).Select(_ => _.ParameterType).ToArray();
            }

            return null;
        }
    }
}
