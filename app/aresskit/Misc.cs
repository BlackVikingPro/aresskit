using System;
using System.IO;
using System.Linq;
using System.Text;

namespace aresskit
{
    public class Misc
    {
        public static byte[] byteCode(string contents)
        {
            return Encoding.ASCII.GetBytes(contents);
        }

        public static string ShowMethods(Type type)
        {
            string helpMenu = default(string);
            if (type.Name != "Core" && type.Name != "Misc")
            {
                foreach (var method in type.GetMethods())
                {
                    var parameters = method.GetParameters();
                    var parameterDescriptions = string.Join
                        (", ", method.GetParameters()
                        .Select(x => x.ParameterType + " " + x.Name).ToArray());

                    if (parameterDescriptions.ToString().Contains("System.Object") == false &&
                            method.Name.ToString() != "ToString" &&
                            method.Name.ToString() != "GetHashCode" &&
                            method.Name.ToString() != "GetType")
                    {
                        if (parameterDescriptions == "")
                            helpMenu += type.Name + "::" + method.Name + "\n";
                        else
                            helpMenu += type.Name + "::" + method.Name + " (" + parameterDescriptions + ")\n";
                    }
                }
            }
            return helpMenu;
        }

        // Thanks to: https://stackoverflow.com/a/1344242/8280922
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // Thanks to: http://stackoverflow.com/a/6413615/5925502
        public static string GetLast(string source, int tail_length)
        {
            if (tail_length >= source.Length)
                return source;
            return source.Substring(source.Length - tail_length);
        }

        public static string GetFirst(string str, int maxLength)
        { return str.Substring(0, Math.Min(str.Length, maxLength)); }

        public static void Log(string logMessage, TextWriter w)
        { w.Write(logMessage); }
    }
}