using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpriteRenderer), true)]
[CanEditMultipleObjects]
public class CreateSpriteMaskFromSprite : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(10);

        if (GUILayout.Button("Create Sprite Mask"))
        {
            foreach (var targetObject in targets)
            {
                if (targetObject is SpriteRenderer spriteRenderer)
                {
                    CreateNewSpriteMask(spriteRenderer);
                }
            }
        }
    }

    private void CreateNewSpriteMask(SpriteRenderer spriteRenderer)
    {
        // Create a new SpriteMask GameObject
        GameObject spriteMaskGO = new GameObject(spriteRenderer.gameObject.name + "_SpriteMask");
        Undo.RegisterCreatedObjectUndo(spriteMaskGO, "Create Sprite Mask");

        // Add SpriteMask component to the new GameObject
        SpriteMask spriteMask = spriteMaskGO.AddComponent<SpriteMask>();
        spriteMask.sprite = spriteRenderer.sprite;

        // Parent the new GameObject to the same parent as the SpriteRenderer's GameObject
        Transform parentTransform = spriteRenderer.gameObject.transform;
        spriteMaskGO.transform.SetParent(parentTransform);

        // Reset local position and rotation to match the SpriteRenderer's GameObject
        spriteMaskGO.transform.localPosition = Vector3.zero;
        spriteMaskGO.transform.localRotation = Quaternion.identity;
        spriteMaskGO.transform.localScale = Vector3.one;

        // Select the new GameObject
        Selection.activeGameObject = spriteMaskGO;
    }
}
