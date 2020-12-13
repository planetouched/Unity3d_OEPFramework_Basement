using System;
using System.Collections.Generic;
using Basement.BLFramework.Core.Context;
using Basement.BLFramework.Core.Model;
using Basement.BLFramework.Core.Reference.Description;
using Basement.BLFramework.Core.ThroughEvent;
using Basement.BLFramework.Essential.EssentialRandom;
using Basement.Common;
using Random = Basement.BLFramework.Essential.EssentialRandom.Random;

namespace Basement.BLFramework.Essential.Path
{
    public static class PathUtil
    {
        private static IList<string[]> GetParts(string path)
        {
            var selectors = new List<string[]>();
            string[] parts = path.Split('.');
            if (parts.Length % 2 == 1)
                throw new Exception("GetParts(string path) " + path);

            for (int i = 0; i < parts.Length; i += 2)
                selectors.Add(new[] { parts[i], parts[i + 1] });
            return selectors;
        }
        
        public static ModelsPath GetModelPath(IContext context, RawNode node)
        {
            string path;
            Random random = null;
            
            if (node.IsDictionary() && node.CheckKey("path"))
            {
                path = node.GetString("path");
                
                if (node.CheckKey("random"))
                {
                    random = GetModelPath(context, node.GetString("random"), null).GetSelf<Random>();
                }
            }
            else
            {
                path = node.ToString();
            }
            
            return GetResult(context, path, random);
        }

        public static ModelsPath GetModelPath(IContext context, string path, IRandom random)
        {
            return GetResult(context, path, random);
        }

        private static ModelsPath GetResult(IContext context, string path, IRandom random)
        {
            var parts = GetParts(path);
            var models = new List<IModel>();
            var result = new ModelsPath();
            
            IModel current = null;
            
            for (int i = 0; i < parts.Count; i++)
            {
                current = current == null ? context.GetChild(parts[i][0]) : current.GetChild(parts[i][0]);

                if (SelectPathUtil.IsSimple(parts[i][1]))
                {
                    models.Add(current);
                    current = current.GetChild(parts[i][1]);
                    models.Add(current);
                }
                else
                {
                    var allKeys = new List<string>();
                    allKeys.AddRange(((IReferenceCollection)current).dataSource.GetNode().GetSortedKeys());
                    var affectedKeys = SelectPathUtil.GetAffectedKeys(parts[i][1], allKeys);
                    var availableModels = new List<IReferenceModel>();
                    
                    foreach (var key in affectedKeys)
                    {
                        var testModel = (IReferenceModel)current.GetChild(key);
                        if (testModel.selectable && testModel.CheckAvailable())
                            availableModels.Add(testModel);
                    }
                    
                    if (affectedKeys.Count == 0)
                    {
                        return null;
                    }                    
                    
                    models.Add(current);
                    current = availableModels[random.Range(0, availableModels.Count)];
                    models.Add(current);
                }
            }
            
            result.Set(models, false);
            return result;
        }

        public static RawNode GetRawNode(IContext context, string path, IRandom random)
        {
            var parts = GetParts(path);
            RawNode current = context.repositories;

            for (int i = 0; i < parts.Count; i++)
            {
                current = current.GetNode(parts[i][0]);

                if (SelectPathUtil.IsSimple(parts[i][1]))
                {
                    current = current.GetNode(parts[i][1]);
                }
                else
                {
                    var allKeys = new List<string>();
                    allKeys.AddRange(current.GetSortedKeys());
                    var affectedKeys = SelectPathUtil.GetAffectedKeys(parts[i][1], allKeys);
                    if (affectedKeys.Count == 0)
                    {
                        return null;
                    }

                    var selectedKey = affectedKeys[random.Range(0, affectedKeys.Count)];
                    current = current.GetNode(selectedKey);
                }
            }

            return current;
        }

        public static T GetDescription<T>(IContext context, string path, IRandom random) where T : class, IDescription
        {
            var parts = GetParts(path);
            IDescription current = null;
            
            for (int i = 0; i < parts.Count; i++)
            {
                current = current == null ? context.dataSources.GetChild(parts[i][0]) : current.GetChild(parts[i][0]);

                if (SelectPathUtil.IsSimple(parts[i][1]))
                {
                    current = current.GetChild(parts[i][1]);
                }
                else
                {
                    var allKeys = new List<string>();
                    allKeys.AddRange(current.GetNode().GetSortedKeys());
                    var affectedKeys = SelectPathUtil.GetAffectedKeys(parts[i][1], allKeys);
                    if (affectedKeys.Count == 0)
                    {
                        return null;
                    }                    
                    
                    var selectedKey = affectedKeys[random.Range(0, affectedKeys.Count)];
                    current = current.GetChild(selectedKey);
                }
            }

            return (T)current;
        }
    }
}
