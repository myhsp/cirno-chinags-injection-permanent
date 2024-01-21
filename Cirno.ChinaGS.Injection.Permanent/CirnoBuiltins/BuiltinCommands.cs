using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using GS.Unitive.Framework.Core;
using CirnoFramework.Interface.Commands;

namespace CirnoBuiltins
{
    [Export(typeof(ICommand))]
    [Command("Builtin.ShadowLantern")]
    public class ShadowLantern : ICommand
    {
        public string Execute(IAddonContext context, DateTime start, DateTime end, string[] args)
        {
            string result = string.Empty;
            Utils.ShadowLantern(context, args[0], start, end);
            return result;
        }
    }

    [Export(typeof(ICommand))]
    [Command("Builtin.ShadowLanternLTP")]
    public class ShadowLanternLTP : ICommand
    {
        public string Execute(IAddonContext context, DateTime start, DateTime end, string[] args)
        {
            string result = string.Empty;
            Utils.ShadowLanternLTP(context, args[0], start, end);
            return result;
        }
    }

    [Export(typeof(ICommand))]
    [Command("Builtin.AddPosterTemplate")]
    public class AddPosterTemplate : ICommand
    {
        public string Execute(IAddonContext context, DateTime start, DateTime end, string[] args)
        {
            string result = string.Empty;
            result = Utils.AddPosterTemplate(context, args[0]).ToString();
            return result;
        }
    }

    [Export(typeof(ICommand))]
    [Command("Builtin.RemovePosterTemplate")]
    public class RemovePosterTemplate : ICommand
    {
        public string Execute(IAddonContext context, DateTime start, DateTime end, string[] args)
        {
            string result = string.Empty;
            Utils.RemovePosterTemplate(context, Guid.Parse(args[0]));
            return result;
        }
    }

    [Export(typeof(ICommand))]
    [Command("Builtin.AddMultiMediaVisualTemplate")]
    public class AddMultiMediaVisualTemplate : ICommand
    {
        public string Execute(IAddonContext context, DateTime start, DateTime end, string[] args)
        {
            string result = string.Empty;
            Utils.AddMultiMediaVisualTemplate(context, args[0]);
            return result;
        }
    }

    [Export(typeof(ICommand))]
    [Command("Builtin.ClearAllPosterTemplate")]
    public class ClearAllPosterTemplate : ICommand
    {
        public string Execute(IAddonContext context, DateTime start, DateTime end, string[] args)
        {
            string result = string.Empty;
            Utils.ClearAllPosterTemplate(context);
            return result;
        }
    }

    [Export(typeof(ICommand))]
    [Command("Builtin.RemoveVisualTemplate")]
    public class RemoveVisualTemplate : ICommand
    {
        public string Execute(IAddonContext context, DateTime start, DateTime end, string[] args)
        {
            string result = string.Empty;
            Utils.RemoveVisualTemplate(context, args[0]);
            return result;
        }
    }

    [Export(typeof(ICommand))]
    [Command("Builtin.WriteJson")]
    public class WriteJson : ICommand
    {
        public string Execute(IAddonContext context, DateTime start, DateTime end, string[] args)
        {
            string result = string.Empty;
            Utils.WriteJson(context, args[0], args[1]);
            return result;
        }
    }

    [Export(typeof(ICommand))]
    [Command("Builtin.DownloadFile")]
    public class DownloadFile : ICommand
    {
        public string Execute(IAddonContext context, DateTime start, DateTime end, string[] args)
        {
            string result = string.Empty;
            Utils.DownloadFile(context, args[0], args[1]);
            return result;
        }
    }
}
