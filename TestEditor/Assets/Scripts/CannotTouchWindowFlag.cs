using UnityEngine;
using System.Collections;
using System;

namespace BubbleCouple
{
    public class CannotTouchWindowFlag : Common.BaseFlag
    {
        public enum EWindowType
        {
            E_AntiAddiction,    // ������
            E_Win,              // ʤ��
            E_Fail,             // ʧ��
            E_Pause,            // ��ͣ
            E_SwitchBackground, // �к�̨
        }

        public CannotTouchWindowFlag():base()
        {

        }

        public bool AntiAddiction
        {
            get { return GetBitValue((int)EWindowType.E_AntiAddiction); }
            set { SetBitValue((int)EWindowType.E_AntiAddiction, value); }
        }

        public bool WinWindow
        {
            get { return GetBitValue((int)EWindowType.E_Win); }
            set { SetBitValue((int)EWindowType.E_Win, value); }
        }

        public bool FailWindow
        {
            get { return GetBitValue((int)EWindowType.E_Fail); }
            set { SetBitValue((int)EWindowType.E_Fail, value); }
        }

        public bool SwitchBackground
        {
            get { return GetBitValue((int)EWindowType.E_SwitchBackground); }
            set { SetBitValue((int)EWindowType.E_SwitchBackground, value); }
        }

        public UInt32 GetFlagData()
        {
            return m_flagData;
        }
    }
}