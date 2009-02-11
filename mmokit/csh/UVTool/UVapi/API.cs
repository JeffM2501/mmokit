using System;
using System.Collections.Generic;
using System.Text;

using UVapi;

namespace UVapi.FileIO
{
   public interface IFileIOPlugin
   {
       string getName();
       string getExtension();
       string getDescription();

       bool read(string file, Model model);
   }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=true)]
    public sealed class FileIOPluginAttribute : Attribute
    {
    }
}
