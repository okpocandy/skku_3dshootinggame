using System;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    private Camera _mainCamera;

    public GameObject UI_SniperZoom;
    public bool IsSniperZoom = false;
    public GameObject UI_Crosshair;
    public float SniperZoomInSize = 15f;
    public float SniperZoomOutSize = 60f;
    
    private void Start() 
    {
        _mainCamera = Camera.main;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            IsSniperZoom = !IsSniperZoom;
            if(IsSniperZoom)
            {
                UI_SniperZoom.SetActive(true);
                UI_Crosshair.SetActive(false);
                _mainCamera.fieldOfView = SniperZoomInSize;
            }
            else
            {
                UI_SniperZoom.SetActive(false);
                UI_Crosshair.SetActive(true);
                _mainCamera.fieldOfView = SniperZoomOutSize;
            }
        }

        if(Input.GetKey(KeyCode.LeftAlt))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else if(Input.GetKeyUp(KeyCode.LeftAlt))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
