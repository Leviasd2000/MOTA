using TMPro;
using UnityEngine;

public class Collidershop : MonoBehaviour
{
    public GameObject Player;
    public GameObject MShopUI;
    public GameObject EShopUI;
    private Braveattr player;
    private Braveplayer Playermovement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        MShopUI = GameObject.Find("MonShop");
    }
    void Start()
    {
        player = FindFirstObjectByType<Braveattr>();
        Player = player.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (MShopUI != null)
            {
                MShopUI.SetActive(true);
                Debug.Log("§ä¨ì¤F");
               
            }
        }
    }
}
