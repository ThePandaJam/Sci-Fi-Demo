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
    
    private bool _hasCoin = false;
    private UIManager _uiManager;

    [SerializeField] private GameObject _weapon;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        
        _currentAmmo = _maxAmmo;
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    void Update()
    {
        CalculateMovement();
 
        if (_weapon.active && (_currentAmmo > 0) && Input.GetMouseButton(0))
        {
            Shoot();
        }
        else
        {
            StopShooting();
        }

        if ( Input.GetKeyDown(KeyCode.R) && (_isReloading == false))
        {
            _isReloading = true;
            StartCoroutine(Reload());
        }
        
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
        velocity = transform.transform.TransformDirection(velocity);
        
        _controller.Move(velocity * Time.deltaTime);
    }

    void Shoot()
    {
        _muzzleFlash.SetActive(true);
        _currentAmmo--;
        _uiManager.UpdateAmmoDisplay(_currentAmmo);
        
        Ray rayOrigin = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hitInfo;
        
        if (!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
        
        if (Physics.Raycast(rayOrigin, out hitInfo))
        {
            Debug.Log("Hit: " + hitInfo.transform.name);
            GameObject hitMarker = Instantiate(_hitMarkerPrefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal)) as GameObject;
            Destroy(hitMarker, 0.5f);

            Destructible crate = hitInfo.transform.GetComponent<Destructible>();
            if (crate != null)
            {
                crate.DestroyCrate();
            }
        }
    }

    void StopShooting()
    {
        _muzzleFlash.SetActive(false);
        _audioSource.Stop();
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
    }

    public void SpendCoin()
    {
        _hasCoin = false;
    }

    public bool PlayerHasCoin()
    {
        return _hasCoin;
    }

    public void EnableWeapons()
    {
        _weapon.SetActive(true);
    }
}
