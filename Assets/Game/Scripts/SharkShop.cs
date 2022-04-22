using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkShop : MonoBehaviour
{
    [SerializeField] private AudioSource _weaponSaleAudio;
    private UIManager _uiManager;
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }
    //check for collision with player
    //check for E key

    //if player has coin
    //  remove coin from player
    //  update inventory display
    //  play win sound
    //else
    //  debug log "you have no coin"
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Player player = other.GetComponent<Player>();
                if (player != null)
                {
                    if (player.PlayerHasCoin())
                    {
                        player.SpendCoin();
                        _weaponSaleAudio.Play();
                        _uiManager.ToggleCoinDisplay(false);
                        //sell weapon
                        player.EnableWeapons();
                    }
                    else
                    {
                        Debug.Log("You have no coins! Get outta here!");
                    }
                    
                }
            }
            
            
        }
    }
}
