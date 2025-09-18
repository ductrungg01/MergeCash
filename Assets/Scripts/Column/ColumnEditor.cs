#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Column))]
public class ColumnEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Column column = (Column)target;

        if (GUILayout.Button("Generate cards from debug values"))
        {
            column.GenerateCardFromDebugCards();
        }

        if (GUILayout.Button("Refresh"))
        {
            column.RefreshCardList();
        }

        if (GUILayout.Button("Rearrange Column"))
        {
            column.RearrangeColumn();
        }
    }
}
#endif
