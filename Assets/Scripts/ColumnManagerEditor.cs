#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ColumnManager))]
public class ColumnManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ColumnManager manager = (ColumnManager)target;

        if (GUILayout.Button("Refresh"))
        {
            manager.RefreshCardList();
        }

        if (GUILayout.Button("Rearrange Column"))
        {
            manager.RearrangeColumn();
        }
    }
}
#endif
