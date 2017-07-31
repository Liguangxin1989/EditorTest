using UnityEngine;
using System.Collections;
using Thrift.Protocol;
using System.Collections.Generic;

namespace BubbleCouple
{
    public sealed class CmdSceneTransform : BattleCommand
    {
        public Vector2 Translation
        {
            get
            {
                if (CmdData != null && CmdData.SceneTransform != null)
                {
                    return new Vector2((float)CmdData.SceneTransform.Translation.X, (float)CmdData.SceneTransform.Translation.Y);
                }

                return Vector2.zero;
            }
        }

        public float Rotation
        {
            get
            {
                if (CmdData != null && CmdData.SceneTransform != null)
                {
                    return (float)CmdData.SceneTransform.Rotation;
                }

                return 0;
            }
        }

        private CmdSceneTransform() : base(EBattleCommandType.CmdSceneTransform)
        {

        }

        private CmdSceneTransform(StructBattleCommand cmd) : this()
        {
            CmdData = cmd;
        }

        public CmdSceneTransform(Vector2 translation, float rotation) : this()
        {
            CmdData = new StructBattleCommand();
            CmdData.Side = 2;
            CmdData.Type = (int)Type;
            CmdData.SceneTransform = new SceneTransform();
            CmdData.SceneTransform.Translation = new Vector2D();
            CmdData.SceneTransform.Translation.X = translation.x;
            CmdData.SceneTransform.Translation.Y = translation.y;
            CmdData.SceneTransform.Rotation = rotation;
        }
    }
}
