using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemyLaunshBullet : MonoBehaviour {
    [Range(0.1f,20f)]
    public float radio = 2;
    [Range(0,360)]
    public int angle = 0;

    public Transform Launch;

    private void OnDrawGizmosSelected()
    {
        if (!Launch)
            Launch = transform;
        Handles.color = Color.yellow;
        var pos = Launch.position;
        var radioCenter = Mathf.Cos(angle * Mathf.Deg2Rad * .5f) * radio * Vector3.up + pos;
        var circleRadio = radio * Mathf.Sin(angle * Mathf.Deg2Rad * .5f);
        ///两个圆
        Handles.DrawWireDisc(radioCenter, Vector3.up, circleRadio);
        if (angle > 180)
        {
            Handles.DrawWireDisc(pos, Vector3.up, radio);
        }

        Vector3 pos1 = radioCenter + circleRadio * Vector3.right;
        Vector3 pos2 = radioCenter + circleRadio * Vector3.forward;
        ///五条线段
        Handles.DrawLine(pos, pos1);
        Handles.DrawLine(pos, radioCenter - circleRadio * Vector3.right);
        Handles.DrawLine(pos, pos2);
        Handles.DrawLine(pos, radioCenter - circleRadio * Vector3.forward);
        Handles.DrawLine(pos,pos + radio * Vector3.up);
        ///2条弧
        Handles.DrawWireArc(pos, Vector3.forward, radioCenter + circleRadio * Vector3.right - pos, angle, radio);
        Handles.DrawWireArc(pos, Vector3.right, radioCenter - circleRadio * Vector3.forward - pos, angle ,radio);
    }
}
