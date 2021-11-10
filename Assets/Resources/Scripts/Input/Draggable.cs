﻿using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(EnlargeOnHover))]
public class Draggable : MonoBehaviour
{
    public GameObject Ghost;

    private SpriteRenderer SpriteRenderer;
    private Vector3 StartPosition;
    private bool IsDragging;
    private Collider2D[] OverlappingColliders = new Collider2D[1];
    private ContactFilter2D ContactFilter = new ContactFilter2D
    {
        useTriggers = true
    };

    void Start()
    {
        StartPosition = transform.position;
        SpriteRenderer = GetComponent<SpriteRenderer>();
        AddGhost();
    }

    void Update()
    {
        if (IsDragging)
        {
            var mouseWorldPoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            transform.position = new Vector3(mouseWorldPoint.x, mouseWorldPoint.y, transform.position.z);
        }
    }

    public void DragStart()
    {
        IsDragging = true;
        SpriteRenderer.sortingLayerName = "Drag";
    }

    public void DragEnd()
    {
        if (IsDragging)
        {
            IsDragging = false;
            SpriteRenderer.sortingLayerName = "Default";

            if (GetComponent<Collider2D>().OverlapCollider(ContactFilter, OverlappingColliders) > 0)
            {
                FindObjectOfType<Combiner>()?.Combine(gameObject, OverlappingColliders[0].gameObject);
            }

            transform.position = StartPosition;
        }
    }

    private void AddGhost()
    {
        Ghost = new GameObject("ghost");

        var ghostSpriteRenderer = Ghost.AddComponent<SpriteRenderer>();
        ghostSpriteRenderer.sprite = SpriteRenderer.sprite;
        ghostSpriteRenderer.SetOpacity(0.4f);
        ghostSpriteRenderer.sortingLayerName = "Ghost";

        Ghost.transform.SetParent(null);
        Ghost.transform.position = transform.position;
    }
}
