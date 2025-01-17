﻿using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngineInternal.XR.WSA;

public class Player : MonoBehaviour
{
    [SerializeField] public static int roundCount = 1;

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
        //enemy = FindObjectOfType<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (clearCheck == false)
        {
            Moving();
            CameraRot();
            PlayerRot();
            //Jump();
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
    //void Jump()
    //{
    //    if(Input.GetButtonDown("Jump") && gravityOn == false && deadCheck == false)
    //    {
    //        rb.velocity = transform.up * 5f;
    //    }
    //}
    void RotGround()
    {
        SoundManager.instance.PlaySingle(gravitySource);
        if (Input.GetKeyDown(KeyCode.E))
        {
            turnValue = 1;
            rightArmLight.SetActive(true);
            //rb.useGravity = false;
            //rb.velocity = transform.up * 1.5f;
            //enemy.transform.Translate(enemy.transform.up * 1.5f);
            //ground.transform.Rotate(Vector3.forward * 90 * Time.deltaTime);    
            if (turnCheck == false)
                StartCoroutine(MapTurnning());
            //ground.transform.Rotate(0, 0, gameObject.transform.position.z * 30 * Time.deltaTime);            
            //gravityOn = true;
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            rightArmLight.SetActive(false);
            //rb.useGravity = true;
            //gravityOn = false;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            leftArmLight.SetActive(true);
            turnValue = -1;
            //rb.useGravity = false;
            //rb.velocity = transform.up * 1.5f;
            //enemy.transform.Translate(enemy.transform.up * 1.5f);
            if (turnCheck == false)
                StartCoroutine(MapTurnning());
            //ground.transform.Rotate(0, 0, gameObject.transform.position.z * -30 * Time.deltaTime);
            //gravityOn = true;
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            leftArmLight.SetActive(false);
            //rb.useGravity = true;
            //gravityOn = false;
        }
        #region 마우스 조작
        //if(Input.GetMouseButtonDown(0))
        //{
        //    rightArmLight.SetActive(true);
        //    rb.useGravity = false;
        //    rb.velocity = transform.up * 1.5f;
        //    //enemy.transform.Translate(enemy.transform.up * 1.5f);
        //    ground.transform.Rotate(playerTr.transform.forward * -90);
        //    //ground.transform.Rotate(0, 0, gameObject.transform.position.z * 30 * Time.deltaTime);            
        //    gravityOn = true;
        //}
        //else if (Input.GetMouseButtonUp(0))
        //{
        //    rightArmLight.SetActive(false);
        //    rb.useGravity = true;
        //    gravityOn = false;
        //}
        //if (Input.GetMouseButtonDown(1))
        //{
        //    leftArmLight.SetActive(true);
        //    rb.useGravity = false;
        //    rb.velocity = transform.up * 1.5f;
        //    //enemy.transform.Translate(enemy.transform.up * 1.5f);
        //    ground.transform.Rotate(playerTr.transform.forward * 90f);
        //    //ground.transform.Rotate(0, 0, gameObject.transform.position.z * 30 * Time.deltaTime);            
        //    gravityOn = true;
        //}
        //else if (Input.GetMouseButtonUp(1))
        //{
        //    leftArmLight.SetActive(false);
        //    rb.useGravity = true;
        //    gravityOn = false;
        //}
        #endregion
    }

    IEnumerator MapTurnning()
    {
        turnCheck = true;
        turnningCount = 0;
        Vector3 turnDevelop = turnValue > 0 ? Vector3.right : Vector3.left;
        while (turnningCount < 90)
        {
            ground.transform.Rotate((turnDevelop), Space.Self);
            turnningCount += 1;
            yield return null;
        }

        //if (ground.transform.rotation.z / 90 >= 0f && ground.transform.rotation.z / 90 < 1.1f)
        //{
        //    ground.transform.rotation = Quaternion.Euler(0, 0, 90);
        //}
        //else if (ground.transform.rotation.z / 90 > 1.2f &&  ground.transform.rotation.z / 90 <= 2f)
        //{
        //    Debug.Log(ground.transform.rotation.z);
        //    ground.transform.rotation = Quaternion.Euler(0, 0, 180);
        //}
        //else if(ground.transform.rotation.z / 90 <= 0f && ground.transform.rotation.z / 90 > -2.0f)
        //{
        //    ground.transform.rotation = Quaternion.Euler(0, 0, -90);
        //}
        //else if (ground.transform.rotation.z / 90 <= -2.0f)
        //{
        //    Debug.Log(ground.transform.rotation.z);
        //    ground.transform.rotation = Quaternion.Euler(0, 0, -180);
        //}
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
        Debug.Log("Hp = 0"); // 정일이 화이팅 :)
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ClearBox")
        {
            if (Physics.Raycast(transform.position, Vector3.down, 1f, 1 << LayerMask.NameToLayer("ClearGround")))
            {
                roundCount++;
                SoundManager.instance.PlaySingle(clearPointSource);
                clearCheck = true;
                if (roundCount < 3)
                {
                    clearCheck = false;
                }
            }
        }
    }
}
