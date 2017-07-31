using UnityEngine;
using System.Collections;
using LitJson;

namespace BubbleCouple
{
    public class GlobalConfig
    {
        public string GameServerAddress = "";
        public int GameServerPort = 0;
        public string EducationServerAddress = "";
        public int EducationServerPort = 0;
        public string LogServerAddress = "";
        public bool IsLogOpen = true;

        public enum EUserIDMode
        {
            MachineCode,        // 使用机器码做用户ID，唯一
            Guid,               // 使用Guid做用户ID，每次重新生成，用于测试创建账号功能
        }
        public EUserIDMode UserIdMode = EUserIDMode.MachineCode;
        public bool IsAndroidSimulator = false;     // 安卓模拟器无法摇一摇，使用固定房间号
        public string SimulatorRoomId = "";      // 固定房间号
        public bool UseQRCode = false;              // 是否使用二维码
        public bool IsShowShakeButton = false;      // 是否显示摇一摇按钮
        public bool IsOfflineModeEnable = false;    // 是否允许单机模式

        public static GlobalConfig Instance
        {
            get
            {
                return GameConfigMgr.Instance.m_GlobalConfig;
            }
        }

        public GlobalConfig() { }

        public GlobalConfig(string jsonString) : this()
        {
            JsonData obj = JsonMapper.ToObject(jsonString);
            if (obj["GameServerAddress"] != null)
            {
                GameServerAddress = (string)obj["GameServerAddress"];
            }
            if (obj["GameServerPort"] != null)
            {
                GameServerPort = (int)obj["GameServerPort"];
            }
            if (obj["EducationServerAddress"] != null)
            {
                EducationServerAddress = (string)obj["EducationServerAddress"];
            }
            if (obj["EducationServerPort"] != null)
            {
                EducationServerPort = (int)obj["EducationServerPort"];
            }
            if (obj["LogServerAddress"] != null)
            {
                LogServerAddress = (string)obj["LogServerAddress"];
            }
            if (obj["IsLogOpen"] != null)
            {
                IsLogOpen = (bool)obj["IsLogOpen"];
            }
            if (obj["UserIdMode"] != null)
            {
                int mode = (int)obj["UserIdMode"];
                UserIdMode = (EUserIDMode)mode;
            }
            if (obj["IsAndroidSimulator"] != null)
            {
                IsAndroidSimulator = (bool)obj["IsAndroidSimulator"];
            }
            if (obj["SimulatorRoomId"] != null)
            {
                SimulatorRoomId = (string)obj["SimulatorRoomId"];
            }
            if (obj["IsOfflineModeEnable"] != null)
            {
                IsOfflineModeEnable = (bool)obj["IsOfflineModeEnable"];
            }
        }

        public override string ToString()
        {
            JsonData obj = new JsonData();
            obj["GameServerAddress"] = GameServerAddress;
            obj["GameServerPort"] = GameServerPort;
            obj["EducationServerAddress"] = EducationServerAddress;
            obj["EducationServerPort"] = EducationServerPort;
            obj["LogServerAddress"] = LogServerAddress;
            obj["IsLogOpen"] = IsLogOpen;
            obj["UserIdMode"] = (int)UserIdMode;
            obj["IsAndroidSimulator"] = IsAndroidSimulator;
            obj["SimulatorRoomId"] = SimulatorRoomId;
            obj["IsOfflineModeEnable"] = IsOfflineModeEnable;

            return obj.ToJson();
        }
    }
}
