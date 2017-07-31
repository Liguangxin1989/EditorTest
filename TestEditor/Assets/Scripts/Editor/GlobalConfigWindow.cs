using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

namespace BubbleCouple
{
    public class GlobalConfigWindow : EditorWindow
    {
        private static readonly string InnerDevGameServerAddress = "192.168.1.100";    // 内网开发服
        private static readonly string InnerTestGameServerAddress = "192.168.1.146";    // 内网测试服
        private static readonly string OnlineGameServerAddress = "puzzle-td.cytxcn.com.cn";     // 外网服
        private static readonly int DefaultGameServerPort = 11001;
        private static readonly string DevEducationServerAddress = "http://192.168.1.121";   // 内网开发服
        private static readonly string TestEducationServerAddress = "http://dev05.cytxcn.net";   // 内网测试服
        private static readonly string OnlineEducationServerAddress = "http://101.200.154.144";   // 外网服
        private static readonly int DefaultEducationServerPort = 3000;
        private static readonly string DevLogServerAddress = "http://192.168.1.121:1337/log"; // 内网开发服
        private static readonly string TestLogServerAddress = "http://192.168.11.11:1337/log"; // 内网测试服
        private static readonly string OnlineLogServerAddress = "http://log.cytxcn.net:1337/log"; // 外网服

        private string[] ServerAddressOptions = new string[]
        {
            "内网开发服",
            "内网测试服",
            "外网服",
            "自定义"
        };

        private int GameServerAddressIndex = 3;
        private int EducationServerAddressIndex = 3;
        private int LogServerAddressIndex = 3;

        GlobalConfig globalConfig;
        private string assetPath;

        [MenuItem("Assets/GlobalConfig")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(GlobalConfigWindow));
        }

        void OnEnable()
        {
            assetPath = Application.dataPath + "/StreamingAssets/Config/GlobalConfig.json";
            if (File.Exists(assetPath))
            {
                string jsonString = File.ReadAllText(assetPath);
                globalConfig = new GlobalConfig(jsonString);
            }
            else
            {
                globalConfig = new GlobalConfig();
            }

            if (globalConfig.GameServerAddress == InnerDevGameServerAddress)
            {
                GameServerAddressIndex = 0;
            }
            else if (globalConfig.GameServerAddress == InnerTestGameServerAddress)
            {
                GameServerAddressIndex = 1;
            }
            else if (globalConfig.GameServerAddress == OnlineGameServerAddress)
            {
                GameServerAddressIndex = 2;
            }
            else
            {
                GameServerAddressIndex = 3;
            }

            if (globalConfig.EducationServerAddress == DevEducationServerAddress)
            {
                EducationServerAddressIndex = 0;
            }
            else if (globalConfig.EducationServerAddress == TestEducationServerAddress)
            {
                EducationServerAddressIndex = 1;
            }
            else if (globalConfig.EducationServerAddress == OnlineEducationServerAddress)
            {
                EducationServerAddressIndex = 2;
            }
            else
            {
                EducationServerAddressIndex = 3;
            }

            if (globalConfig.LogServerAddress == DevLogServerAddress)
            {
                LogServerAddressIndex = 0;
            }
            else if (globalConfig.LogServerAddress == TestLogServerAddress)
            {
                LogServerAddressIndex = 1;
            }
            else if (globalConfig.LogServerAddress == OnlineLogServerAddress)
            {
                LogServerAddressIndex = 2;
            }
            else
            {
                LogServerAddressIndex = 3;
            }
        }

        void OnGUI()
        {
            GameServerAddressIndex = EditorGUILayout.Popup("游戏服务器", GameServerAddressIndex, ServerAddressOptions);
            if (GameServerAddressIndex == 0)
            {
                globalConfig.GameServerAddress = InnerDevGameServerAddress;
                globalConfig.GameServerPort = DefaultGameServerPort;
                EditorGUILayout.LabelField("地址:", globalConfig.GameServerAddress);
                EditorGUILayout.LabelField("端口:", globalConfig.GameServerPort.ToString());
            }
            else if (GameServerAddressIndex == 1)
            {
                globalConfig.GameServerAddress = InnerTestGameServerAddress;
                globalConfig.GameServerPort = DefaultGameServerPort;
                EditorGUILayout.LabelField("地址:", globalConfig.GameServerAddress);
                EditorGUILayout.LabelField("端口:", globalConfig.GameServerPort.ToString());
            }
            else if (GameServerAddressIndex == 2)
            {
                globalConfig.GameServerAddress = OnlineGameServerAddress;
                globalConfig.GameServerPort = DefaultGameServerPort;
                EditorGUILayout.LabelField("地址:", globalConfig.GameServerAddress);
                EditorGUILayout.LabelField("端口:", globalConfig.GameServerPort.ToString());
            }
            else if (GameServerAddressIndex == 3)
            {
                globalConfig.GameServerAddress = EditorGUILayout.TextField("地址:", globalConfig.GameServerAddress);
                globalConfig.GameServerPort = EditorGUILayout.IntField("端口:", globalConfig.GameServerPort);
            }
            EditorGUILayout.Space();

            EducationServerAddressIndex = EditorGUILayout.Popup("教育点服务器", EducationServerAddressIndex, ServerAddressOptions);
            if (EducationServerAddressIndex == 0)
            {
                globalConfig.EducationServerAddress = DevEducationServerAddress;
                globalConfig.EducationServerPort = DefaultEducationServerPort;
                EditorGUILayout.LabelField("地址:", globalConfig.EducationServerAddress);
                EditorGUILayout.LabelField("端口:", globalConfig.EducationServerPort.ToString());
            }
            else if (EducationServerAddressIndex == 1)
            {
                globalConfig.EducationServerAddress = TestEducationServerAddress;
                globalConfig.EducationServerPort = DefaultEducationServerPort;
                EditorGUILayout.LabelField("地址:", globalConfig.EducationServerAddress);
                EditorGUILayout.LabelField("端口:", globalConfig.EducationServerPort.ToString());
            }
            else if (EducationServerAddressIndex == 2)
            {
                globalConfig.EducationServerAddress = OnlineEducationServerAddress;
                globalConfig.EducationServerPort = DefaultEducationServerPort;
                EditorGUILayout.LabelField("地址:", globalConfig.EducationServerAddress);
                EditorGUILayout.LabelField("端口:", globalConfig.EducationServerPort.ToString());
            }
            else if (EducationServerAddressIndex == 3)
            {
                globalConfig.EducationServerAddress = EditorGUILayout.TextField("地址:", globalConfig.EducationServerAddress);
                globalConfig.EducationServerPort = EditorGUILayout.IntField("端口:", globalConfig.EducationServerPort);
            }
            EditorGUILayout.Space();

            LogServerAddressIndex = EditorGUILayout.Popup("日志服务器", LogServerAddressIndex, ServerAddressOptions);
            if (LogServerAddressIndex == 0)
            {
                globalConfig.LogServerAddress = DevLogServerAddress;
                EditorGUILayout.LabelField("地址:", globalConfig.LogServerAddress);
            }
            else if (LogServerAddressIndex == 1)
            {
                globalConfig.LogServerAddress = TestLogServerAddress;
                EditorGUILayout.LabelField("地址:", globalConfig.LogServerAddress);
            }
            else if (LogServerAddressIndex == 2)
            {
                globalConfig.LogServerAddress = OnlineLogServerAddress;
                EditorGUILayout.LabelField("地址:", globalConfig.LogServerAddress);
            }
            else if (LogServerAddressIndex == 3)
            {
                globalConfig.LogServerAddress = EditorGUILayout.TextField("地址:", globalConfig.LogServerAddress);
            }
            globalConfig.IsLogOpen = EditorGUILayout.Toggle("是否开启日志:", globalConfig.IsLogOpen);
            EditorGUILayout.Space();

            globalConfig.UserIdMode = (GlobalConfig.EUserIDMode)EditorGUILayout.EnumPopup("账号ID模式:", (System.Enum)globalConfig.UserIdMode);
            EditorGUILayout.Space();

            globalConfig.IsAndroidSimulator = EditorGUILayout.Toggle("是否使用固定房间号:", globalConfig.IsAndroidSimulator);
            if (globalConfig.IsAndroidSimulator)
            {
                globalConfig.SimulatorRoomId = EditorGUILayout.TextField("固定房间号:", globalConfig.SimulatorRoomId);
            }
            EditorGUILayout.Space();

            globalConfig.IsOfflineModeEnable = EditorGUILayout.Toggle("是否开启单机模式:", globalConfig.IsOfflineModeEnable);
            EditorGUILayout.Space();

            if (GUILayout.Button("保存"))
            {
                File.WriteAllText(assetPath, globalConfig.ToString());
            }
        }
    }
}
