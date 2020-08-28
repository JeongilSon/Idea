using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngineInternal.XR.WSA;

public class NoneVRPlayer : MonoBehaviour
{  
    //카메라 및 캐릭터 이동관련
    [SerializeField] float cameraRotSpeed = 150f;
    [SerializeField] float moveDIrection = 10f;
    [SerializeField] float cameraMaxRot = 80;

    float turnValue;
    float moveValue;
    //논리값
    [SerializeField] public static bool clearCheck = false;
    //[SerializeField] bool gravityOn;
    //[SerializeField] bool deadCheck;
    [SerializeField] bool turnCheck;
    [SerializeField] bool movingCheck;
    //그외
    [SerializeField] Transform playerTr;
    [SerializeField] float rotSpeed = 5f;
    //[SerializeField] Enemy enemy;
    float xC;
    float yC;
    float turnningCount = 0;
    float movingCount = 0;
    //필요 컴포넌트
    [SerializeField] Camera camera;
    public GameObject rightArmLight;
    public GameObject leftArmLight;
    public GameObject ground;
    public AudioClip gravitySource, groundedSource, clearPointSource;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (clearCheck == false)
        {
            Moving();
            CameraRot();
            PlayerRot();
            RotGround();
        }
        
    }
    void Moving()
    {
        #region Axis 무빙
        //if (gravityOn == false && deadCheck == false)
        //{
        //    float moveDirX = Input.GetAxis("Horizontal");
        //    float moveDirZ = Input.GetAxis("Vertical");

        //    Vector3 moveHorizontal = transform.right * moveDirX;
        //    Vector3 moveVertical = transform.forward * moveDirZ;
        //    Vector3 velocity = (moveHorizontal + moveVertical).normalized * moveDIrection;
        //    rb.MovePosition(transform.position + velocity * Time.deltaTime);
        //}
        #endregion
        if (Input.GetKeyDown(KeyCode.W) && movingCheck == false)
        {
            moveValue = 1;
            StartCoroutine(TriggerMoving());

        }
        else if (Input.GetKeyDown(KeyCode.S) && movingCheck == false)
        {
            moveValue = -1;
            StartCoroutine(TriggerMoving());
        }
    }
    IEnumerator TriggerMoving()
    {
        movingCheck = true;
        movingCount = 0.1f;
        Vector3 moveDevelop = moveValue > 0 ? Vector3.forward : Vector3.back;
        while (movingCount < moveDIrection)
        {
            transform.Translate(moveDevelop * movingCount * Time.deltaTime);
            movingCount += 0.1f;
            yield return null;
        }
        movingCheck = false;
    }

    void RotGround()
    {
        //SoundManager.instance.PlaySingle(gravitySource);
        if (Input.GetKeyDown(KeyCode.E))
        {
            turnValue = 1;          
            if(turnCheck == false)
                StartCoroutine(MapTurnning());
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            turnValue = -1;
            if (turnCheck == false)
                StartCoroutine(MapTurnning());
        }
    }

    IEnumerator MapTurnning()
    {
        turnCheck = true;
        //rb.useGravity = true;
        turnningCount = 0;
        Vector3 turnDevelop = turnValue > 0 ? Vector3.right : Vector3.left;
        while (turnningCount < 90)
        {
            ground.transform.Rotate((turnDevelop), Space.Self);
            turnningCount += 1;
            yield return null;
        }
        turnCheck = false;
    }
    void PlayerRot()
    {
        float x = Input.GetAxis("Mouse X");
        Vector3 follwing = new Vector3(0, x, 0) * cameraRotSpeed;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(follwing));
    }
    void CameraRot()
    { 
        //상하 카메라 회전
        float xRotation = Input.GetAxisRaw("Mouse Y");
        float cameraRotationX = xRotation * cameraRotSpeed;
        xC -= cameraRotationX; // 카메라 회전값을 감산해야 마우스 위치대로 회전
        xC = Mathf.Clamp(xC, -cameraMaxRot, cameraMaxRot); //회전값을 최대치로 계산

        camera.transform.localEulerAngles = new Vector3(xC, 0, 0);        
    }
    void PlayerDead()
    {
        //deadCheck = true;
        Debug.Log("Hp = 0");
    }
    //private void OnCollisionEnter(Collider other)
    //{
    //    if (other.tag == "ClearBox")
    //    {
    //        if (Physics.Raycast(transform.position, Vector3.down, 5f, 1 << LayerMask.NameToLayer("ClearGround")))
    //        {
    //            //SoundManager.instance.PlaySingle(clearPointSource);
    //            clearCheck = true;
    //        }
            
    //    }
    //    if(other.tag == "Ground")
    //    {
    //        rb.useGravity = false;
    //    }
    //}
    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.collider.tag == null)
    //    {
    //        rb.useGravity = true;
    //    }
    //}    
    
}
    