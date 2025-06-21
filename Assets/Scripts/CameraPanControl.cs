using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class CameraPanControl : MonoBehaviour
{
    private Camera mainCamera;
    private Vector3 dragOrigin;

    public float mapMinX, mapMaxX, mapMinY, mapMaxY;
    
    private bool isDragging = false; 

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // 1. UI Elemanının Üzerinde miyiz? (Bu kontrol aynı kalıyor)
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        // --- YENİ KONTROL BAŞLANGICI ---
        // Fareye ilk basıldığı an...
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Tıkladığımız yerden bir ışın gönder.
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero);

            // Eğer ışın bir Collider'a çarptıysa (yani bir BuildingPlot veya başka bir interaktif objeye tıkladıysak)...
            if (hit.collider != null)
            {
                // ...hiçbir şey yapma ve fonksiyondan çık.
                // Bu, tıklama olayını BuildingPlot'un OnMouseDown fonksiyonuna bırakır.
                return;
            }
        }
        // --- YENİ KONTROL SONU ---


        // Kaydırma mantığı (Bu kısımda değişiklik yok)
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            dragOrigin = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            isDragging = true;
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 currentMousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector3 difference = dragOrigin - currentMousePos;

            mainCamera.transform.position += difference;
        }
    }

    private void LateUpdate()
    {
        // Sınırlandırma (Bu kısımda değişiklik yok)
        Vector3 camPosition = transform.position;
        camPosition.x = Mathf.Clamp(camPosition.x, mapMinX, mapMaxX);
        camPosition.y = Mathf.Clamp(camPosition.y, mapMinY, mapMaxY);
        transform.position = camPosition;
    }
}