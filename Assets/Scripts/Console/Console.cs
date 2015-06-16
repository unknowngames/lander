using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Console
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ConsoleCommand : Attribute
    {
    }

    public class Console
    {
        private static Console instance;
        private readonly Dictionary<string, MethodInfo> commandAndInfos = new Dictionary<string, MethodInfo>();
        private readonly Dictionary<string, List<Type>> commandAndTypes = new Dictionary<string, List<Type>>();

        private readonly List<string> log = new List<string>();
        private readonly Dictionary<Type, List<string>> typesAndCommands = new Dictionary<Type, List<string>>();

        private static Console ConsoleInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Console();
                    instance.Init();
                }
                return instance;
            }
        }

        public static List<string> LogStrings
        {
            get { return ConsoleInstance.log; }
        }

        private void Init()
        {
            Type[] types = Assembly.GetExecutingAssembly().GetTypes();

            for (int i = 0; i < types.Length; i++)
            {
                Type type = types[i];

                MethodInfo[] methods =
                    type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic |
                                    BindingFlags.Static);

                for (int j = 0; j < methods.Length; j++)
                {
                    MethodInfo minfo = methods[j];

                    object[] attrs = minfo.GetCustomAttributes(typeof (ConsoleCommand), true);

                    if (attrs.Length > 0)
                    {
                        if (commandAndTypes.ContainsKey(minfo.Name) == false)
                        {
                            commandAndTypes.Add(minfo.Name, new List<Type>());
                        }

                        commandAndTypes[minfo.Name].Add(type);


                        if (typesAndCommands.ContainsKey(type) == false)
                        {
                            typesAndCommands.Add(type, new List<string>());
                        }

                        typesAndCommands[type].Add(minfo.Name);

                        commandAndInfos.Add(minfo.Name, minfo);
                    }
                }
            }

            Application.logMessageReceived += LogCallback;
        }

        private void LogCallback(string condition, string stackTrace, LogType type)
        {
            log.Add(condition);
        }

        public static string[] GetAutocompleteResult(string methodName)
        {
            return ConsoleInstance.GetAutocompleteResultInternal(methodName).ToArray();
        }

        private List<string> GetAutocompleteResultInternal(string methodName)
        {
            List<string> result = new List<string>();

            // ищем все имена которые начинаются с данной строки
            foreach (string key in commandAndInfos.Keys)
            {
                if (key.ToLower().StartsWith(methodName.ToLower()))
                {
                    if (!result.Contains(key))
                    {
                        result.Add(key);
                    }
                }
            }

            // ищем все имена в которых содержится данная строка
            foreach (string key in commandAndInfos.Keys)
            {
                if (key.ToLower().Contains(methodName.ToLower()))
                {
                    if (!result.Contains(key))
                    {
                        result.Add(key);
                    }
                }
            }

            return result;
        }

        public static Dictionary<string, Type> GetCommandParameters(string command)
        {
            ParameterInfo[] parameters = ConsoleInstance.commandAndInfos[command].GetParameters();

            Dictionary<string, Type> result = new Dictionary<string, Type>();

            foreach (ParameterInfo par in parameters)
            {
                result.Add(par.Name, par.ParameterType);
            }
            return result;
        }

        public static void Execute(string command)
        {
            ConsoleInstance.ExecuteInternal(command);
        }

        private void ExecuteInternal(string command)
        {
            if (string.IsNullOrEmpty(command))
            {
                return;
            }

            try
            {
                string[] splitted = command.Split(' ');
                command = splitted[0];

                object[] args = new object[splitted.Length - 1];

                for (int i = 0; i < args.Length; i++)
                {
                    args[i] = splitted[i + 1];
                }


                if (commandAndTypes.ContainsKey(command))
                {
                    List<Type> types = commandAndTypes[command];
                    foreach (Type t in types)
                    {
                        MethodInfo minfo = commandAndInfos[command];

                        // GET PARAMS
                        ParameterInfo[] parameters = minfo.GetParameters();

                        if (parameters.Length != args.Length)
                        {
                            Debug.Log("Parameter count not match");
                            continue;
                        }

                        for (int i = 0; i < parameters.Length; i++)
                        {
                            ParameterInfo par = parameters[i];

                            if (par.ParameterType == typeof (string))
                            {
                                continue;
                            }

                            if (par.ParameterType == typeof (int))
                            {
                                args[i] = int.Parse(args[i].ToString());
                            }
                            else if (par.ParameterType == typeof (float))
                            {
                                args[i] = float.Parse(args[i].ToString());
                            }
                            else if (par.ParameterType == typeof (double))
                            {
                                args[i] = double.Parse(args[i].ToString());
                            }
                            else if (par.ParameterType == typeof (bool))
                            {
                                args[i] = bool.Parse(args[i].ToString());
                            }
                            else if (par.ParameterType.IsEnum)
                            {
                                args[i] = Enum.Parse(par.ParameterType, args[i].ToString(), true);
                            }
                            else
                            {
                                Debug.Log("Unknown parameter type: " + par.ParameterType);
                            }
                        }

                        if (minfo.IsStatic)
                        {
                            minfo.Invoke(null, args);
                        }
                        else
                        {
                            Object[] objs = Object.FindObjectsOfType(t);

                            foreach (Object o in objs)
                            {
                                t.InvokeMember(command,
                                    BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public |
                                    BindingFlags.NonPublic, null, o, args);
                            }
                        }
                    }
                }
                else
                {
                    Debug.Log("no command found");
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }
    }
}