using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ScalingSurface : MonoBehaviour
{
    public GameObject[] prefabsToPlace;
    public float spacing = 1.0f;
    public bool preventDuplicateInRow = true;
    public string containerName = "PrefabContainer";
    public bool isVertical = false;
    public Vector3 turbulence = Vector3.zero; // Add a public field for turbulence

    private Transform prefabContainer;
    private List<SpriteRenderer> lastRowRenderers = new List<SpriteRenderer>();

    private Vector3 lastPosition;
    private Vector3 lastScale;

    void Start()
    {
        lastPosition = transform.position;
        lastScale = transform.localScale;
        UpdatePrefabs();
    }

    void Update()
    {
        if (lastPosition != transform.position || lastScale != transform.localScale)
        {
            lastPosition = transform.position;
            lastScale = transform.localScale;
            UpdatePrefabs();
        }
    }

    private void UpdatePrefabs()
    {
        if (prefabsToPlace == null || prefabsToPlace.Length == 0)
            return;

        if (prefabContainer == null)
            prefabContainer = new GameObject(containerName).transform;
        else
            ClearPrefabContainer();

        Bounds parentBounds = GetParentBounds();

        int iterations = isVertical ? Mathf.FloorToInt(parentBounds.size.y / spacing) : Mathf.FloorToInt(parentBounds.size.x / spacing);

        for (int i = 0; i < iterations; i++)
        {
            GameObject selectedPrefab = GetRandomPrefabForNextRow();

            Vector3 prefabPosition = isVertical ? new Vector3(
                parentBounds.center.x,
                parentBounds.min.y + i * spacing,
                parentBounds.center.z
            ) : new Vector3(
                parentBounds.min.x + i * spacing,
                parentBounds.center.y,
                parentBounds.center.z
            );

            if ((isVertical && prefabPosition.y + spacing > parentBounds.max.y) || (!isVertical && prefabPosition.x + spacing > parentBounds.max.x))
            {
                break;
            }

            // Add turbulence before instantiating the prefab
            prefabPosition += new Vector3(
                Random.Range(-turbulence.x, turbulence.x),
                Random.Range(-turbulence.y, turbulence.y),
                Random.Range(-turbulence.z, turbulence.z)
            );

            GameObject newPrefab = Instantiate(selectedPrefab, prefabPosition, Quaternion.identity, prefabContainer);
            SpriteRenderer newRenderer = newPrefab.GetComponent<SpriteRenderer>();
            if (newRenderer != null)
            {
                lastRowRenderers.Add(newRenderer);
            }
        }

        lastRowRenderers.Clear();
    }


    private GameObject GetRandomPrefabForNextRow()
    {
        GameObject selectedPrefab;

        selectedPrefab = prefabsToPlace[Random.Range(0, prefabsToPlace.Length)];

        if (preventDuplicateInRow && lastRowRenderers.Count > 0)
        {
            SpriteRenderer lastRenderer = lastRowRenderers[lastRowRenderers.Count - 1];
            SpriteRenderer selectedRenderer = selectedPrefab.GetComponent<SpriteRenderer>();
            if (lastRenderer != null && selectedRenderer != null && lastRenderer.sprite == selectedRenderer.sprite)
            {
                selectedPrefab = GetRandomPrefabForNextRow();
            }
        }

        return selectedPrefab;
    }

    private Bounds GetParentBounds()
    {
        Renderer parentRenderer = GetComponentInParent<Renderer>();

        if (parentRenderer == null)
        {
            Renderer[] childRenderers = GetComponentsInChildren<Renderer>();
            Bounds combinedBounds = new Bounds(transform.position, Vector3.zero);
            foreach (Renderer renderer in childRenderers)
            {
                combinedBounds.Encapsulate(renderer.bounds);
            }
            return combinedBounds;
        }
        else
        {
            return parentRenderer.bounds;
        }
    }

    private void ClearPrefabContainer()
    {
        int childCount = prefabContainer.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(prefabContainer.GetChild(i).gameObject);
        }
    }
}
