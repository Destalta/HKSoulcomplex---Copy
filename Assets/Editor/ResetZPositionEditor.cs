using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class ResetZPositionEditor
{
    static ResetZPositionEditor()
    {
        SceneView.duringSceneGui += OnScene;
    }

    private static void OnScene(SceneView scene)
    {
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.O)
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                Vector3 position = obj.transform.position;
                if (position.z == 0)
                {
                    position.z = -0.25f;
                }
                else
                {
                    position.z = 0f;
                }

                // Record the object's state for undo
                Undo.RecordObject(obj.transform, "Change Z Position");

                // Apply the changed position
                obj.transform.position = position;
            }

            // Consume the event
            Event.current.Use();
        }
    }
}
