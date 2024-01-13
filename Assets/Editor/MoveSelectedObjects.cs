using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class MoveSelectedObjects
{
    private const float MOVE_AMOUNT = 0.5f; // Default move amount
    private const float SHIFT_MOVE_AMOUNT = 0.1f; // Move amount when Shift key is held

    static MoveSelectedObjects()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private static bool isMoving = false;

    static void OnSceneGUI(SceneView sceneView)
    {
        if (Selection.transforms.Length == 0) return;

        // Determine the move amount based on whether the Shift key is held
        float currentMoveAmount = Event.current.shift ? SHIFT_MOVE_AMOUNT : MOVE_AMOUNT;

        if (Event.current.type == EventType.KeyDown)
        {
            // Check if 'J' key is held down
            if (Event.current.keyCode == KeyCode.J)
            {
                if (!isMoving)
                {
                    Undo.RecordObjects(Selection.transforms, "Move Z Position");
                    isMoving = true;
                }

                foreach (Transform t in Selection.transforms)
                {
                    t.position -= new Vector3(0, 0, currentMoveAmount);
                }

                Event.current.Use();
                sceneView.Repaint();
            }

            // Check if 'K' key is held down
            if (Event.current.keyCode == KeyCode.K)
            {
                if (!isMoving)
                {
                    Undo.RecordObjects(Selection.transforms, "Move Z Position");
                    isMoving = true;
                }

                foreach (Transform t in Selection.transforms)
                {
                    t.position += new Vector3(0, 0, currentMoveAmount);
                }

                Event.current.Use();
                sceneView.Repaint();
            }
        }
        else if (Event.current.type == EventType.KeyUp && (Event.current.keyCode == KeyCode.J || Event.current.keyCode == KeyCode.K))
        {
            isMoving = false;
        }
    }
}
