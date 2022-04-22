using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private AudioClip _coinPickupSound;
    
    //check for collision (onTrigger)
    //check if player
    //check for e key press
    //give player coin
    //play coin sound
    //destroy coin
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Player player = other.GetComponent<Player>();
                if (player != null)
                {
                    player.CollectCoin();
                    AudioSource.PlayClipAtPoint(_coinPickupSound, transform.position, 1f);
                    Destroy(this.gameObject);
                }
            }
            
            
        }
    }

}
