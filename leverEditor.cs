#if (UNITY_EDITOR) 
using static UnityEngine.GraphicsBuffer;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(lever))]
public class ExampleEditor : Editor
{
    public void OnSceneGUI()
    {
        var t = target as lever;
        var tr = t.transform;
        var position = tr.position;
        Handles.color = Color.yellow;
        var linePos = position + Vector3.down * t.Length;
        Handles.DrawLine(position, linePos, 3);
    }
}
#endif