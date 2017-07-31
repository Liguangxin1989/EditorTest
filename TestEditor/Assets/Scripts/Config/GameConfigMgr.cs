using System;
using UnityEngine;
using System.Collections.Generic;
using Common;

namespace BubbleCouple
{ 
    // 描述所有全局的配置数据
    // 配置常量配置数据
    // 提供数据之间的关联还有提供询问函数
    public sealed class GameConfigMgr : Singleton<GameConfigMgr>
    {
        //标记位， 是否已经初始化
        private bool isInited;
        #region private member
        private class ConfigHandler
        {
            public readonly string m_path;
            public readonly Func<string, byte[], bool> m_handler;

            public ConfigHandler(string path, Func<string, byte[], bool> handler)
            {
                m_path = path;
                m_handler = handler;
            }
        }
        private const char m_splitComma = ',';
        private Dictionary<string, ConfigHandler> m_configHandlerDic;
        #endregion

        public GlobalConfig m_GlobalConfig;
        public SysConfig m_SysConfig; //系统配置文件
        public SoundConfig m_SoundConfig; //音效对照表
        public MessageConfig m_MessageConfig; //信息配置信息

        private GameConfigMgr()
        {
            isInited = false;
        }

        public bool Init()
        {
            if(!isInited)
            {
                string path = StreamingAssetURL.m_streamingAssetURL + "Config/";

                m_configHandlerDic = new Dictionary<string, ConfigHandler>()
                {
                    { "GlobalConfig", new ConfigHandler(System.IO.Path.Combine(path,"GlobalConfig.json"),LoadGlobalConfig)},
                    { "SysConfig", new ConfigHandler(System.IO.Path.Combine(path,"syscfg_syscfg.csv"),LoadSysConfig)},
                    { "LevelConfig", new ConfigHandler(System.IO.Path.Combine(path,"scn_level.csv"),LoadLevelConfig)},
                    //{ "MessageConfig", new ConfigHandler(System.IO.Path.Combine(path,"dictionary_dictionary.csv"),LoadMessageConfig)},
                    { "SoundConfig", new ConfigHandler(System.IO.Path.Combine(path,"soundInfo_soundInfo.csv"),LoadSoundConfig)},
                };

                isInited = LoadConfigSyn();
            }
            return isInited;
        }

        private bool LoadConfigSyn()
        {
            foreach(KeyValuePair<string, ConfigHandler> kv in m_configHandlerDic)
            {
                if(!ConfigLoader.LoadTextFileSyn(kv.Key, kv.Value.m_path, kv.Value.m_handler))
                {
                    return false;
                }
            }
            return true;
        }

        #region load handler

        private bool LoadGlobalConfig(string fileName, byte[] contentTxt)
        {
            if (m_GlobalConfig == null)
            {
                string str = System.Text.Encoding.UTF8.GetString(contentTxt);
                m_GlobalConfig = new GlobalConfig(str);
                return true;
            }

            return false;
        }

        private bool LoadSoundConfig(string fileName, byte[] contentTxt)
        {
            if(m_SoundConfig == null)
            {
                m_SoundConfig = new SoundConfig();
            }
            string[] csvContent = ConfigLoader.GetCSVContent(contentTxt);
            if(csvContent.Length > 1)
            {
                return m_SoundConfig.LoadSoundConfig(csvContent);
            }
            return false;
        }

        private bool LoadMessageConfig(string fileName, byte[] contentTxt)
        {
            if(m_SoundConfig == null)
            {
                m_MessageConfig = new MessageConfig();
            }
            string[] csvContent = ConfigLoader.GetCSVContent(contentTxt);
            if(csvContent.Length > 1)
            {
                return m_MessageConfig.LoadMessageConfig(csvContent);
            }
            return false;
        }

        private bool LoadSysConfig(string fileName, byte[] contentTxt)
        {
            if(m_SysConfig == null)
            {
                m_SysConfig = new SysConfig();
            }
            string[] csvContent = ConfigLoader.GetCSVContent(contentTxt);
            if(csvContent.Length > 1)
            {
                return m_SysConfig.LoadSysConfig(csvContent);
            }
            return false;
        }

        private bool LoadLevelConfig(string fileName, byte[] contentTxt)
        {
            string[] csvContent = ConfigLoader.GetCSVContent(contentTxt);
            if (csvContent.Length > 1)
            {
                return LevelConfig.Instance.LoadLevelInfo(csvContent);
            }
            return false;
        }

        #endregion
    }

}