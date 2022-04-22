using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FabriceEgg : MonoBehaviour
{
    public GameManager GM;
    public GameObject Fabrice;
    // Start is called before the first frame update
    void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GM.KC.KC== true)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            Fabrice.SetActive(true);
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            Fabrice.SetActive(false);
        }
    }
}
