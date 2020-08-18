using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Clear : MonoBehaviour
{
    GameObject Goal;
    public GameObject Now;
    public GameObject After;

    // Start is called before the first frame update
    void Start()
    {
    Goal = GameObject.FindWithTag("Goal");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider coll){
        if(coll.tag=="Cam"){
            Now.SetActive(false);
            After.SetActive(true);
        }
    }
}
