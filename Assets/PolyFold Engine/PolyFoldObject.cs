using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class PolyFoldObject : MonoBehaviour
{
    public bool isStatic;

    [HideInInspector]
    public float Mass;
    [HideInInspector]
    public float StaticFriction = 0.6f;
    [HideInInspector]
    public float DynamicFriction = 0.4f;
    [HideInInspector]
    public float Restitution = 0.05f;

    public Body Body;

    public void AttachBody(Body body)
    {
        Body = body;
        if(Body.Mass == 0)
        {
            Mass = Body.Mass;
        }

        if(isStatic)
        {
            Body.SetStatic();
        }

    }
}

[CustomEditor(typeof(PolyFoldObject))]
public class Object_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PolyFoldObject script = (PolyFoldObject)target;

        EditorGUI.BeginDisabledGroup(script.isStatic);
        script.Mass = EditorGUILayout.FloatField("Mass", script.Mass);
        EditorGUI.EndDisabledGroup();
        script.StaticFriction = EditorGUILayout.FloatField("StaticFriction", script.StaticFriction);
        script.DynamicFriction = EditorGUILayout.FloatField("DynamicFriction", script.DynamicFriction);
        script.Restitution = EditorGUILayout.FloatField("Restitution", script.Restitution);

        if (GUI.changed && Application.isPlaying)
        {
            if(script.isStatic)
            {
                script.Body.SetStatic();
            }
            else
            {
                script.Body.Shape.Initialize();
            }
            script.Body.Mass = script.Mass;
            script.Body.StaticFriction = script.StaticFriction;
            script.Body.DynamicFriction = script.DynamicFriction;
            script.Body.Restitution = script.Restitution;
        }
    }
}