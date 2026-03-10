#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LeafPile))]
public class LeafPileEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LeafPile pile = (LeafPile)target;

        if (GUILayout.Button("Generate Leaves"))
        {
            pile.GenerateLeaves();
        }

        if (pile.autoRegenerate)
        {
            if (GUILayout.Button("Regenerate Automatically"))
            {
                pile.GenerateLeaves();
            }
        }

        if (GUILayout.Button("Combine Leaves"))
        {
            pile.CombineLeavesMultiMaterial();
        }
    }
}
#endif
