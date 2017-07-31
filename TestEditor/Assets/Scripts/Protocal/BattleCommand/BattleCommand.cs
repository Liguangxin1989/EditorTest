using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thrift.Transport;
using Thrift.Protocol;

namespace BubbleCouple
{
    public abstract class BattleCommand
    {
        #region regist protocol

        private static Dictionary<EBattleCommandType, System.Reflection.ConstructorInfo> s_battleCommandConstructorDic;

        static BattleCommand()
        {
            s_battleCommandConstructorDic = new Dictionary<EBattleCommandType, System.Reflection.ConstructorInfo>();
            RegistAllBattleCommand();
        }

        private static void RegistAllBattleCommand()
        {
            System.Type[] types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types)
            {
                if (type.BaseType == typeof(BattleCommand))
                {
                    System.Reflection.ConstructorInfo[] constructors = type.GetConstructors(System.Reflection.BindingFlags.Instance |
                                             System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
                    foreach (System.Reflection.ConstructorInfo constructor in constructors)
                    {
                        System.Reflection.ParameterInfo[] parameterInfoArray = constructor.GetParameters();
                        if (1 == parameterInfoArray.Length && parameterInfoArray[0].ParameterType == typeof(StructBattleCommand))
                        {
                            BattleCommand cmd = constructor.Invoke(new object[] { null }) as BattleCommand;
                            s_battleCommandConstructorDic[cmd.Type] = constructor;
                            break;
                        }
                    }
                }
            }
        }

        public static BattleCommand CreateInstance(StructBattleCommand cmd)
        {
            EBattleCommandType type = (EBattleCommandType)cmd.Type;
            if (s_battleCommandConstructorDic.ContainsKey(type))
            {
                BattleCommand command = s_battleCommandConstructorDic[type].Invoke(new object[] { cmd }) as BattleCommand;
                return command;
            }

            return null;
        }
        #endregion

        private EBattleCommandType _type;
        public EBattleCommandType Type
        {
            get { return _type; }
        }

        private StructBattleCommand _cmdData;

        public StructBattleCommand CmdData
        {
            get { return _cmdData; }
            set { _cmdData = value; }
        }

        public int Side
        {
            get { return _cmdData != null ? _cmdData.Side : 2; }
        }

        protected BattleCommand(EBattleCommandType type)
        {
            _type = type;
        }

        public override string ToString()
        {
            return "BattleCommand: Type: " + Type.ToString() + ", Side: " + Side.ToString();
        }
    }
}
