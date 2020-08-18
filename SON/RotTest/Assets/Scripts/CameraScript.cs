using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraScript : MonoBehaviour
{
    float xC;
    float yC;
    float cameraRotSpeed = 5;
    float cameraMaxRot = 100;
    [SerializeField] int moveDirection = 1;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();   
    }

    // Update is called once per frame
    void Update()
    {
        CameraRot();
        Move();
    }
    void Move()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.Translate(Vector3.forward * moveDirection);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            transform.Translate(Vector3.back * moveDirection);
        }
    }
    void CameraRot()
    {
        //상하 카메라 회전
        float yRotation = Input.GetAxisRaw("Mouse Y");
        float xRotation = Input.GetAxisRaw("Mouse X");
        float cameraRotationY = yRotation * cameraRotSpeed;
        float cameraRotationX = xRotation * cameraRotSpeed;
        yC -= cameraRotationY; // 카메라 회전값을 감산해야 마우스 위치대로 회전
        xC -= cameraRotationX;
        yC = Mathf.Clamp(yC, -cameraMaxRot, cameraMaxRot); //회전값을 최대치로 계산
        xC = Mathf.Clamp(xC, -cameraMaxRot, cameraMaxRot);

        transform.localEulerAngles = new Vector3(yC, -xC, 0);
    }
}
