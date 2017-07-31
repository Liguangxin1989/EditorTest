using UnityEngine;
using System.Collections;

public class Launcher: MonoBehaviour
{

    void Awake()
    {
        BubbleCouple.Main.BootGame();
    }
}
