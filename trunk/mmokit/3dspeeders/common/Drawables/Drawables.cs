using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Drawables.DisplayLists;
using Drawables.Materials;

namespace Drawables
{
    public delegate bool ExecuteCallback(Material mat, bool draw);

    public class DrawablesSystem
    {
        public static DrawablesSystem system = new DrawablesSystem();

        public static int FirstPass = 0;
        public static int LastPass = 1000;


        Dictionary<int, Dictionary<Material, List<ExecuteCallback>>> passes = new Dictionary<int, Dictionary<Material, List<ExecuteCallback>>>();

        public void addItem(Material mat, ExecuteCallback callback)
        {
            addItem(mat, callback, LastPass);
        }

        public void addItem (Material mat, ExecuteCallback callback, int pass)
        {
            if (!passes.ContainsKey(pass))
                passes[pass] = new Dictionary<Material, List<ExecuteCallback>>();

            Dictionary<Material, List<ExecuteCallback>> passList = passes[pass];
            if (!passList.ContainsKey(mat))
                passList[mat] = new List<ExecuteCallback>();

            passList[mat].Add(callback);
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
            passes[pass][mat].Remove(callback);
        }

        public void removeAll ()
        {
            passes.Clear();
        }

        public void Execute ()
        {
            foreach(KeyValuePair<int,Dictionary<Material, List<ExecuteCallback>>> pass in passes)
            {
                foreach(KeyValuePair<Material,List<ExecuteCallback>> matList in pass.Value)
                {
                    matList.Key.Execute();
                    foreach(ExecuteCallback callback in matList.Value)
                    {
                        callback(matList.Key, false);
                    }
                }
            }
        }
    }
}
