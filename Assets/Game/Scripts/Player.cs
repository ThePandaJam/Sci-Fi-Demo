using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController _controller;
    [SerializeField] private float _speed = 3.5f;
    private float _gravity = 9.81f;

    [SerializeField] private GameObject _muzzleFlash;

    [SerializeField] private GameObject _hitMarkerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        //hide mouse cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        //if left click
        //   cast ray from centre point of main camera
        if (Input.GetMouseButton(0))
        {
            _muzzleFlash.SetActive(true);
            Ray rayOrigin = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hitInfo;
            if (Physics.Raycast(rayOrigin, out hitInfo))
            {
                Debug.Log("Hit: " + hitInfo.transform.name);
                GameObject hitMarker = Instantiate(_hitMarkerPrefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal)) as GameObject;
                Destroy(hitMarker, 0.5f);
            }
        }
        else
        {
            _muzzleFlash.SetActive(false);
        }
        
        //if escape key pressed
        //  unhide mouse cursor
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, 0f, verticalInput);
        Vector3 velocity = direction * _speed;
        velocity.y -= _gravity;
        //reasign local space to world space values
        velocity = transform.transform.TransformDirection(velocity);
        _controller.Move(velocity * Time.deltaTime);
    }
}
