using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScene : MonoBehaviour {

    private Color m_gizmosColor = Color.yellow;
    private bool m_isSphere = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnDrawGizmos()
    {
        Color oldColor = Gizmos.color;
        Gizmos.color = m_gizmosColor;
        if (m_isSphere)
        {
            Gizmos.DrawSphere(this.transform.position, 1);
        }
        else
        {
            Gizmos.DrawCube(this.transform.position, Vector3.one);
        }
        Gizmos.color = oldColor;
    }

    public void ChangeColor()
    {
        m_gizmosColor = m_gizmosColor == Color.yellow ? Color.red : Color.yellow;
    }

    public void ToggleShape()
    {
        m_isSphere = !m_isSphere;
    }
}
