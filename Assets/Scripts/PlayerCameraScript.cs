using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraScript : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerObject;
    public Transform playerCamera;

    private float xRotation = 0f;
	private bool isMenuOpen = false;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // updates the camera angle per frame according to the mouse axis'
    void Update()
    {
		// don't update the camera if the menu is open
		if (isMenuOpen)
			return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerObject.transform.Rotate(Vector3.up * mouseX);
    }

	public void PauseMouse(bool isMenuOpen) {
		this.isMenuOpen = isMenuOpen;
		
		if (this.isMenuOpen)
        	Cursor.lockState = CursorLockMode.None;
		else
        	Cursor.lockState = CursorLockMode.Locked;
	}
}