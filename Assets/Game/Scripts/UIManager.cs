using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _ammoText;
    [SerializeField] private GameObject _coinDisplay;
    public void UpdateAmmoDisplay(int count)
    {
        _ammoText.text = "Ammo: " + count;
    }

    public void ToggleCoinDisplay(bool playerHasCoin)
    {
         _coinDisplay.SetActive(playerHasCoin);
    }
}
