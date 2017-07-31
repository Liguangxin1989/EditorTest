using UnityEngine;

namespace Common
{
    /// <summary>
    /// 封装PlayerPrefs类
    /// </summary>
    public class DoPlayerPrefs
    {

        public static void SetBool(string _key, bool _vlaue)
        {
            if (PlayerPrefs.HasKey(_key))
            {
                PlayerPrefs.DeleteKey(_key);
            }
            PlayerPrefs.SetInt(_key, _vlaue ? 1 : 0);
            PlayerPrefs.Save();
        }

        public static void SetInt(string _key, int _vlaue)
        {
            if (PlayerPrefs.HasKey(_key))
            {
                PlayerPrefs.DeleteKey(_key);
            }
            PlayerPrefs.SetInt(_key, _vlaue);
            PlayerPrefs.Save();
        }

        public static void SetFloat(string _key, float _value)
        {
            if (PlayerPrefs.HasKey(_key))
            {
                PlayerPrefs.DeleteKey(_key);
            }
            PlayerPrefs.SetFloat(_key, _value);
            PlayerPrefs.Save();
        }


        public static void SetString(string _key, string _value)
        {
            if (PlayerPrefs.HasKey(_key))
            {
                PlayerPrefs.DeleteKey(_key);
            }
            PlayerPrefs.SetString(_key, _value);
            PlayerPrefs.Save();
        }

        public static bool GetBool(string _key)
        {
            if (PlayerPrefs.GetInt(_key) == 1)
                return true;
            return false;
        }

        public static bool GetBool(string _key, bool defVlaue)
        {
            int tmp = defVlaue ? 1 : 0;
            if (PlayerPrefs.GetInt(_key, tmp) == 1)
                return true;
            return false;
        }

        public static int GetInt(string _key)
        {
            return PlayerPrefs.GetInt(_key);
        }

        public static int GetInt(string _key, int defVlaue)
        {
            return PlayerPrefs.GetInt(_key, defVlaue);
        }

        public static float GetFloat(string _key)
        {
            return PlayerPrefs.GetFloat(_key);
        }

        public static float GetFloat(string _key, float defVlaue)
        {
            return PlayerPrefs.GetFloat(_key, defVlaue);
        }

        public static string GetString(string _key)
        {
            return PlayerPrefs.GetString(_key);
        }

        public static string GetString(string _key, string defVlaue)
        {
            return PlayerPrefs.GetString(_key, defVlaue);
        }

        public static bool HasKey(string _key)
        {
            return PlayerPrefs.HasKey(_key);
        }

        public static void DeleteKey(string _key)
        {
            if (PlayerPrefs.HasKey(_key))
            {
                PlayerPrefs.DeleteKey(_key);
                return;
            }
            return;
        }

    }
}
