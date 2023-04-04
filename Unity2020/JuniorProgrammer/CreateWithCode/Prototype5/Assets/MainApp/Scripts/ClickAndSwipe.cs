using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer), typeof(BoxCollider))]
public class ClickAndSwipe : MonoBehaviour
{
    private GameManager gameManager;
    private Camera cam;
    private TrailRenderer trail;
    private BoxCollider boxCollider;
    private Vector3 mousePos;
    private bool isSwiping = false;

    void Awake()
    {
        Init();
    }

    void Update()
    {
        if (gameManager.isGameActive)
        {
            InputMouseButton();
            if (isSwiping) UpdateMousePosition();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        var target = collision.gameObject.GetComponent<Target>();
        Debug.Log(target);
        // Destroy the target
        if (target) target.DestroyTarget();
    }

    private void Init()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        cam = Camera.main;
        trail = GetComponent<TrailRenderer>();
        boxCollider = GetComponent<BoxCollider>();
        trail.enabled = false;
        boxCollider.enabled = false;
    }

    private void InputMouseButton()
    {
        if (Input.GetMouseButtonDown(0)) UpdateComponents(true);
        else if (Input.GetMouseButtonUp(0)) UpdateComponents(false);
    }

    private void UpdateMousePosition()
    {
        mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));
        transform.position = mousePos;
    }

    private void UpdateComponents(bool _swiping)
    {
        isSwiping = _swiping;
        trail.enabled = _swiping;
        boxCollider.enabled = _swiping;
    }
}
