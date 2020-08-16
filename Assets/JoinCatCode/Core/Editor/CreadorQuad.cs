using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor (typeof(CreadorQuadGameObject))]
public class CreadorQuad : Editor
{


    public override void OnInspectorGUI()
    {

        base.OnInspectorGUI();

        CreadorQuadGameObject obj = (CreadorQuadGameObject) target;
        if (GUILayout.Button("GenerarMalla"))
        {
            obj.GenerarMalla();
        }
    }
}
