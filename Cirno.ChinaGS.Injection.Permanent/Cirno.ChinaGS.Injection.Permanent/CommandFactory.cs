using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using CirnoFramework.Interface.Commands;

namespace Cirno.ChinaGS.Injection.Permanent
{
    public class CommandFactory
    {
        [ImportMany(typeof(ICommand))]
        public IEnumerable<ICommand> commands { get; set; }

        [ImportMany(typeof(IStartupCommand))]
        public IEnumerable<IStartupCommand> startups { get; set; }

        [ImportMany(typeof(IPackageInfo))]
        public IEnumerable<IPackageInfo> packageInfos { get; set; }

        public CommandFactory()
        {
        }

        public void ExecuteUpdate()
        {
            try
            {
                string path = Path.Combine(Program.AddonContext.Addon.Location, "CommandLib");
                foreach (string file in Directory.GetFiles(path, "*.dll.cpm"))
                {
                    string target = file.Replace(".dll.cpm", ".dll");
                    try
                    {
                        if (File.Exists(target))
                        {
                            File.Delete(target);
                        }
                        File.Copy(file, target);
                    }
                    catch (Exception ex)
                    {
                        Program.WriteLog("Fail to replace file!" + file + ";" + target, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Program.WriteLog("Fail to update assembly files!", ex);
            }
        }

        public void ComposeCommandAssembly()
        {
            string path = Path.Combine(Program.AddonContext.Addon.Location, "CommandLib");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            AggregateCatalog composables = new AggregateCatalog();
            composables.Catalogs.Add(new DirectoryCatalog(path, "*.dll"));
            CompositionContainer container = new CompositionContainer(composables, new ExportProvider[0]);
            container.ComposeParts(this);
        }

        public string ExecuteCommand(IRemoteCommandGeneric command, ref int errorCode)
        {
            return ExecuteCommand(command.command_name, Convert.ToDateTime(command.start_time),
                Convert.ToDateTime(command.end_time), command.args.ToArray(), ref errorCode);
        }

        public string ExecuteCommand(string command, DateTime start, DateTime end, string[] args, ref int errorCode)
        {
            string res = string.Empty;

            if (!(commands == null || commands.Count<ICommand>() < 1))
            {
                command = command.Trim();
                foreach (ICommand cmd in commands)
                {
                    object[] attr = cmd.GetType().GetCustomAttributes(typeof(CommandAttribute), false);
                    if (!(attr.Count<object>() < 1))
                    {
                        if (((CommandAttribute)attr[0]).Command.ToLower() == command.ToLower())
                        {
                            try
                            {
                                res = cmd.Execute(Program.AddonContext, start, end, args);
                                errorCode = 0;
                                return res;
                            }
                            catch (Exception ex)
                            {
                                res = "Exception: " + ex.Message;
                                errorCode = -100;
                                return res;
                            }
                        }
                    }
                }
                res = "Unknown command.";
                errorCode = -200;
                return res;
            }
            res = "Empty command lib.";
            errorCode = -300;
            return res;
        }

        public string ExecuteStartupCommand(ref int errorCode)
        {
            string res = "";
            if (!(startups == null || startups.Count<IStartupCommand>() < 1))
            {
                foreach (IStartupCommand startup in startups)
                {
                    try
                    {
                        res += (startup.Execute(Program.AddonContext, new string[0]) + "\n");
                        errorCode = 0;
                    }
                    catch (Exception ex)
                    {
                        res += ("Exception: " + ex.Message + "in " + startup.GetType().ToString() + " \n");
                        errorCode = -100;
                    }
                }
                return res;
            }
            else
            {
                res = "No startup command loaded.";
                errorCode = 0;
                return res;
            }
        }
    }
}
