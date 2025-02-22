using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BurgerCollection : MonoBehaviour
{
    private int Burger = 0;

    public TextMeshProUGUI burgersText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Burger")
        {
            Burger++;
            burgersText.text = "Burgers: " + Burger.ToString();
            Debug.Log(Burger);
            Destroy(other.gameObject);
        }
    }
}
