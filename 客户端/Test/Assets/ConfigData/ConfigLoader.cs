using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
namespace ConfigData
{
    public class ConfigLoader
    {
        #region AutoContext
        public static PathLibrary PathLibrary 
        {
            get 
            {
                if (_pathLibrary == null)
                {


                    string LibraryPath = Application.dataPath + "/ConfigData/PathLibrary.json";
                    string pathLibStr = File.ReadAllText(LibraryPath);
                    _pathLibrary = JsonMapper.ToObject<PathLibrary>(pathLibStr);
                }
                return _pathLibrary;
            }
        }
        private static PathLibrary _pathLibrary;
        #endregion
        private static Dictionary<Type, IConfigDataHandler> configDic;
        public static T GetConfigData<T>(int id) where T : BaseConfig 
        {
            if (configDic == null) InitConfigHandler();

            if (!configDic.TryGetValue(typeof(T), out var handler)) return null;

            if (handler is IConfigDataHandler<T> typedHandler)
                return typedHandler.GetConfigData(id);
            
            return null;
        }
        public static string GetLanguageById(int id)
        {
            LanguageConfig config = GetConfigData<LanguageConfig>(id);
            if(config == null)return null;

            return GlobalSetting.Instance.language switch
            {
                ELanguage.Chinese => config?.Chinese,
                ELanguage.English => config?.English,
                _ => config?.Chinese,
            };
        }
        private static void InitConfigHandler()
        {
            configDic = new Dictionary<Type, IConfigDataHandler>();

            string jsonPath = PathLibrary.jsonPath;
            List<Type> types = GetClassList<BaseConfig>();

            foreach (var configType in types)
            {
                if (!configType.IsSubclassOf(typeof(BaseConfig)))
                    continue;

                string className = configType.Name;
                string tablePath = Path.Combine(jsonPath, $"{className}.json");

                Type handlerType = typeof(ConfigDataJson<>).MakeGenericType(configType);
                var handler = (IConfigDataHandler)Activator.CreateInstance(handlerType);
                handler.SetTablePath(tablePath);

                configDic.Add(configType, handler);
            }
        }

        private static List<Type> GetClassList<T>()
        {
            Type type = typeof (T);
            var q = type.Assembly.GetTypes()
                 .Where(x => !x.IsAbstract)
                 .Where(x => !x.IsGenericTypeDefinition)
                 .Where(x => type.IsAssignableFrom(x));
            return q.ToList();
        }
    }
}