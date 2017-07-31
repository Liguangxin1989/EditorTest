using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BubbleCouple;

namespace Common
{
    public sealed class AudioMgr : Singleton<AudioMgr>
    {
        public enum EPlayMode
        {
            Once,
            Loop,
        }

        public enum ESimilarProcess
        {
            Overlay = 1,
            Replace = 2,
            Ignore = 3,
        }

        public class AudioItem
        {
            public AudioSource audio;
            public int id;
            public int group;
            public Guid guid;
        }

        private List<AudioItem> m_audios;
        private GameObject m_audioParent;

        private AudioMgr()
        {
            m_audioParent = null;
            m_audios = new List<AudioItem>();
            if(m_audioParent == null)
            {
                m_audioParent = new GameObject("AudioParent");
                GameObject.DontDestroyOnLoad(m_audioParent);
            }

            for(int i = 0; i < 6; ++i)
            {
                AudioItem tmp;
                tmp = new AudioItem();
                tmp.audio = ObjectEX.AddComponent<AudioSource>(m_audioParent);
                tmp.id = 0;
                tmp.group = 0;
                tmp.guid = Guid.Empty;
                m_audios.Add(tmp);
            }
        }

        public static bool PlayAudio(int audioId, Guid guid)
        {
            return Instance.DoPlayAudio(audioId, guid);
        }

        public static bool PlayAudio(int audioId)
        {
            return Instance.DoPlayAudio(audioId, Guid.Empty);
        }

        public static void StopAudio(int audioId)
        {
            Instance.DoStopAudio(audioId);
        }

        public static void StopAudio(int group, int audioId)
        {
            Instance.DoStopAudio(group, audioId);
        }

        public static void StopAudio(Guid guid, int audioId = -1)
        {
            Instance.DoStopAudio(guid, audioId);
        }

        public static void SetAudioMute(int ground, bool isMute)
        {
            Instance.DoSetAudioMute(ground, isMute);
        }

        public static void ClearAudioComponent(int atLeastResidueNum = 6)
        {
            Instance.DoClearAudioCompoent(atLeastResidueNum);
        }

        //----
        public bool DoPlayAudio(int audioId, Guid guid)
        {
            SoundConfig.Sound sound = null;
            if((sound = ConfigHelper.GetSoundInfor(audioId)) == null)
            {
                return false;
            }

            //查找是否有同类
            if(sound.type != (int)ESimilarProcess.Overlay)
            {
                for(int index = 0; index < m_audios.Count; ++index)
                {
                    if(m_audios[index].audio.isPlaying && m_audios[index].group == sound.group)
                    {
                        if(sound.type == (int)ESimilarProcess.Replace && m_audios[index].id != audioId)
                        {
                            m_audios[index].audio.Stop();
                            //m_audios[index].audio.Play();
                            m_audios[index].audio.clip = Resources.Load<AudioClip>("Audio\\" + sound.file);
                            if (m_audios[index].audio.clip != null)
                            {
                                m_audios[index].audio.Play();
                                m_audios[index].id = audioId;
                                m_audios[index].guid = guid;
                                m_audios[index].audio.loop = (sound.playMode == (int)EPlayMode.Loop);
                            }
                        }
                        return true;
                    }
                }
            }

            AudioClip clip;
            int idlePort;
            if((idlePort = FindIdlePort()) != -1)
            {
                clip = Resources.Load<AudioClip>("Audio\\" + sound.file);

                if(clip != null)
                {
                    m_audios[idlePort].id = audioId;
                    m_audios[idlePort].group = sound.group;
                    m_audios[idlePort].guid = guid;
                    if ((sound.group == 1 && !GameCore.Instance.m_globalData.OpenMusic)
                        || (sound.group != 1 && !GameCore.Instance.m_globalData.OpenSound))
                    {
                        PlayAudio(m_audios[idlePort].audio, clip, sound.playMode == (int) EPlayMode.Loop, true);
                    }
                    else
                    {
                        PlayAudio(m_audios[idlePort].audio, clip, sound.playMode == (int)EPlayMode.Loop);
                    }
                    
                }
                else
                {
                    return false;
                }
            }

            return true;

        }

        private int FindIdlePort()
        {
            int index;
            for(index = 0; index < m_audios.Count; ++index)
            {
                if(!m_audios[index].audio.isPlaying)
                {
                    break;
                }
            }

            //如果找不到空闲的, 申请空间, 继续播放
            if(index == m_audios.Count)
            {
                AudioItem tmp = new AudioItem();
                tmp.audio = ObjectEX.AddComponent<AudioSource>(m_audioParent);
                if(tmp.audio != null)
                {
                    tmp.id = 0;
                    tmp.guid = Guid.Empty;
                    m_audios.Add(tmp);
                }
                else
                {
                    return -1;
                }
            }

            return index;
        }

        public void DoStopAudio(int group, int audioId)
        {
            for(int i = 0; i < m_audios.Count; ++i)
            {
                if(m_audios[i].group == group)
                {
                    if(audioId == 0 || audioId == m_audios[i].id)
                    {
                        m_audios[i].audio.Stop();
                    }
                }
            }
        }

        public void DoStopAudio(Guid guid, int audioId)
        {
            for(int i = 0; i < m_audios.Count; ++i)
            {
                if(m_audios[i].guid == guid)
                {
                    if(audioId == -1 || audioId == m_audios[i].id)
                    {
                        m_audios[i].audio.Stop();
                    }
                }
            }
        }

        private void DoStopAudio(int audioId)
        {
            for(int i = 0; i < m_audios.Count; ++i)
            {
                if(m_audios[i].id == audioId)
                {
                    m_audios[i].audio.Stop();
                }
            }
        }

        private void DoSetAudioMute(int ground, bool isMute)
        {
            for (int i = 0; i < m_audios.Count; ++ i)
            {
                if (ground == 0 || ground == m_audios[i].group)
                {
                    m_audios[i].audio.mute = isMute;
                }
            }   
        }

        private void DoClearAudioCompoent(int atLeastResidueNum)
        {
            List<AudioItem> rmIndexList = new List<AudioItem>();
            int num = m_audios.Count;
            foreach(var item in m_audios)
            {
                if (!item.audio.isPlaying)
                {
                    rmIndexList.Add(item);
                }
            }

            foreach(var item in rmIndexList)
            {
                if (num <= atLeastResidueNum)
                {
                    break;
                }
                GameObject.Destroy(item.audio);
                m_audios.Remove(item);
                --num;
            }
        }

        public static void PlayAudio(AudioSource _as, AudioClip clip, bool isloop = false, bool isMute = false)
        {
            if(_as != null && clip != null)
            {
                _as.Stop();
                _as.clip = clip;
                _as.loop = isloop;
                //_as.volume = 1f;
                _as.mute = isMute;
                _as.Play();
            }
        }
    }
}
