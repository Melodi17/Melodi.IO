using System;
using System.Linq;
using System.Reflection;

namespace Melodi.IO
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = false)]
    public class BenchmarkAttribute : Attribute
    {
        public object[] Parameters;
        public BenchmarkAttribute()
        {
            Parameters = null;
        }
        public BenchmarkAttribute(params object[] parameters)
        {
            Parameters = parameters;
        }
        public static void Test()
        {
            TableBuilder table = new();
            table.AddRow("Method", "Parameters", "Duration");
            table.AddRow("------", "----------", "--------");

            Type[] types = Assembly.GetExecutingAssembly().GetTypes();

            foreach (var type in types)
            {
                MethodInfo[] methods = type.GetMethods();
                methods = methods.Where(x => x.GetCustomAttribute<BenchmarkAttribute>() != null).ToArray();

                foreach (var method in methods)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();

                    BenchmarkAttribute attribute = method.GetCustomAttribute<BenchmarkAttribute>();

                    DateTime startTime = DateTime.Now;
                    DateTime endTime = DateTime.Now;
                    bool success = true;
                    try
                    {
                        startTime = DateTime.Now;
                        method.Invoke(null, attribute.Parameters);
                        endTime = DateTime.Now;
                    }
                    catch (Exception)
                    {
                        success = false;
                    }

                    table.AddRow(method.Name, attribute.Parameters == null ? "<NULL>"
                        : string.Join(", ", attribute.Parameters.Select(x => x.ToString())),
                            endTime - startTime);
                }
            }

            Console.WriteLine(table.Output());
        }
    }
}
