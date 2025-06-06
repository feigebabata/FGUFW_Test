using System;
using UnityEngine;
using static FGUsing;

namespace FGUFW.LocalConfig
{
    public abstract class LocalConfigSingleton : MonoSingleton<LocalConfigSingleton>
    {
        private static string fileName;
        protected override void Init()
        {
            base.Init();

            fileName = $"{this.GetType().FullName}.json";
            
            AppDomain.CurrentDomain.UnhandledException -= OnUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;

            Load();
        }

        public override void Dispose()
        {
            AppDomain.CurrentDomain.UnhandledException -= OnUnhandledException;
            base.Dispose();
            Save();
        }

        protected override bool IsDontDestroyOnLoad()
        {
            return true;
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Save();
        }

        public void Load()
        {
            var jsonText = FileHelper.LocaReadText(fileName);

            if (jsonText.IsNull())
            {
                SetConfigData(default);
            }
            else
            {
                var type = GetConfigDataType();
                var data = json2Object(jsonText, type);
                SetConfigData(data);
            }
        }

        public void Save()
        {
            var data = GetConfigData();
            var jsonText = toJson(data);
            FileHelper.LocalWrite(fileName, jsonText);
        }

        protected abstract object GetConfigData();

        /// <summary>
        /// data==default 则自己初始化
        /// </summary>
        /// <param name="data"></param>
        protected abstract void SetConfigData(object data);

        protected abstract Type GetConfigDataType();

    }
}