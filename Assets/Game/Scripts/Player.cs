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
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private int _currentAmmo;
    private int _maxAmmo = 50;

    private bool _isReloading = false;

    private UIManager _uiManager;
    //variable for hascoin
    private bool _hasCoin = false;
    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        //hide mouse cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _currentAmmo = _maxAmmo;
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        //if left click
        //   cast ray from centre point of main camera
        //TODO Reload
        if ((_currentAmmo > 0) && Input.GetMouseButton(0))
        {
            Shoot();
        }
        else
        {
            _muzzleFlash.SetActive(false);
            _audioSource.Stop();
        }

        if ( Input.GetKeyDown(KeyCode.R) && (_isReloading == false))
        {
            _isReloading = true;
            StartCoroutine(Reload());
        }
        
        //if escape key pressed
        //  unhide mouse cursor
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void Shoot()
    {
        _muzzleFlash.SetActive(true);
        _currentAmmo--;
        _uiManager.UpdateAmmoDisplay(_currentAmmo);
        if (!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
            
        Ray rayOrigin = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hitInfo;
        if (Physics.Raycast(rayOrigin, out hitInfo))
        {
            Debug.Log("Hit: " + hitInfo.transform.name);
            GameObject hitMarker = Instantiate(_hitMarkerPrefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal)) as GameObject;
            Destroy(hitMarker, 0.5f);
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

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(1.5f);
        _currentAmmo = _maxAmmo;
        _uiManager.UpdateAmmoDisplay(_currentAmmo);
        _isReloading = false;
    }

    public void CollectCoin()
    {
        _hasCoin = true;
        //update UI
    }

    public void SpendCoin()
    {
        _hasCoin = false;
        //update UI
    }

    public bool PlayerHasCoin()
    {
        return _hasCoin;
    }
}
