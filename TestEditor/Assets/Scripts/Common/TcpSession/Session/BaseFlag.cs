
using UnityEngine;
using System.Collections;
using System;

namespace Common
{
    public class BaseFlag
    {
        protected UInt32 m_flagData;

        public BaseFlag()
        {
            m_flagData = 0;
        }

        protected bool GetBitValue(int flag)
        {
            return ((1u << (int)flag) & m_flagData) != 0;
        }

        protected void SetBitValue(int flag, bool value)
        {
            if (value == true)
                m_flagData |= (1u << (int)flag);
            else
                m_flagData &= ~(1u << (int)flag);
        }
    }
}