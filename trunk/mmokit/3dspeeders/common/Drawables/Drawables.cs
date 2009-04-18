using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Drawables.DisplayLists;
using Drawables.Materials;

namespace Drawables
{
    public delegate bool ExecuteCallback(Material mat, object tag);

    public class ExecuteItem
    {
        public ExecuteCallback callback = null;
        public object tag = null;

        public bool call ( Material mat )
        {
            if (callback != null)
                return callback(mat, tag);

            return false;
        }
    }

    public class DrawablesSystem
    {
        public static DrawablesSystem system = new DrawablesSystem();

        public static int FirstPass = 0;
        public static int LastPass = 1000;

        Dictionary<int, Dictionary<Material, List<ExecuteItem>>> passes = new Dictionary<int, Dictionary<Material, List<ExecuteItem>>>();

        public void addItem(Material mat, ExecuteCallback callback)
        {
            addItem(mat, callback, LastPass,null);
        }

        public void addItem(Material mat, ExecuteCallback callback, object tag)
        {
            addItem(mat, callback, LastPass,tag);
        }

        public void addItem (Material mat, ExecuteCallback callback, int pass)
        {
            addItem(mat, callback, pass, null);
        }

        public void addItem (Material mat, ExecuteCallback callback, int pass, object tag)
        {
            if (!passes.ContainsKey(pass))
                passes[pass] = new Dictionary<Material, List<ExecuteItem>>();

            Dictionary<Material, List<ExecuteItem>> passList = passes[pass];
            if (!passList.ContainsKey(mat))
                passList[mat] = new List<ExecuteItem>();

            ExecuteItem item = new ExecuteItem();
            item.callback = callback;
            item.tag = tag;
            passList[mat].Add(item);
        }

        public void removeItem(Material mat, ExecuteCallback callback)
        {
            removeItem(mat,callback,LastPass);
        }

        public void removeItem (Material mat, ExecuteCallback callback, int pass)
        {
            if (!passes.ContainsKey(pass))
                return;
            if (!passes[pass].ContainsKey(mat))
                return;
            foreach (ExecuteItem i in passes[pass][mat])
            {
                if (i.callback == callback)
                {
                    passes[pass][mat].Remove(i);
                    return;
                }
            }
        }

        public void removeAll ()
        {
            passes.Clear();
        }

        public void Execute ()
        {
            foreach(KeyValuePair<int,Dictionary<Material, List<ExecuteItem>>> pass in passes)
            {
                foreach(KeyValuePair<Material,List<ExecuteItem>> matList in pass.Value)
                {
                    matList.Key.Execute();
                    foreach (ExecuteItem item in matList.Value)
                        item.call(matList.Key);
                }
            }
        }
    }
}
