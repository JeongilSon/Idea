using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explanation : MonoBehaviour
{
    [SerializeField] Transform cam;
    [SerializeField] GameObject exp;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        OnExp();
        //exp.transform.rotation = Quaternion.Euler(180, 0, -90);
        //exp.transform.LookAt(cam);

    }
    private void OnExp()
    {
        if (OVRInput.Get(OVRInput.RawButton.Y) || Input.GetKeyDown(KeyCode.N))
        {
            exp.SetActive(true);
        }
        else if (OVRInput.GetUp(OVRInput.RawButton.Y) || Input.GetKeyDown(KeyCode.N))
        {
            exp.SetActive(false);
        }
    }
}
