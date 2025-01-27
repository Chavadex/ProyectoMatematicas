using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorManager : MonoBehaviour
{
    [SerializeField] private int coindCollected;
    [SerializeField] private int coinsToCollect;
    [SerializeField] private bool canWin;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject timer;
    [SerializeField] private GameObject defaultPanel;


    private void Start()
    {
        coindCollected = 0;
        canWin = false;
        victoryPanel.SetActive(false);
    }

    private void Update()
    {
        if(coindCollected == coinsToCollect)
        {
            canWin = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            coindCollected++;
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("WinZone") && canWin)
        {
            victoryPanel.SetActive(true);
            Destroy(timer.gameObject);
            Destroy(defaultPanel.gameObject);
        }
    }
}
