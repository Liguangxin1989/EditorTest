using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleCouple
{
    public class LevelBall : MonoBehaviour
    {
        [SerializeField]
        public ELevelBallType Type;

        public int Id;

        public List<LevelBall> Connections = new List<LevelBall>();

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
