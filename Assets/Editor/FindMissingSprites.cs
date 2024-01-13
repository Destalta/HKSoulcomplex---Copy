using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


public class FindMissingSprites : EditorWindow
{
    [MenuItem("Window/Find Missing Sprites")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(FindMissingSprites));
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Find Missing Sprites"))
        {
            FindAndSelectMissingSprites();
        }
    }

    private static void FindAndSelectMissingSprites()
    {
        SpriteRenderer[] renderers = GameObject.FindObjectsOfType<SpriteRenderer>();
        List<GameObject> objectsWithMissingSprites = new List<GameObject>();

        foreach (SpriteRenderer renderer in renderers)
        {
            if (renderer.sprite == null)
            {
                objectsWithMissingSprites.Add(renderer.gameObject);
                Debug.Log("GameObject with missing sprite: " + renderer.gameObject.name, renderer.gameObject);
            }
        }

        Selection.objects = objectsWithMissingSprites.ToArray();
    }
}
