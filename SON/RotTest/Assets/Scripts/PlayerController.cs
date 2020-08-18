using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public int playerHp = 5;
    [SerializeField] float cameraRotSpeed = 150f;
    [SerializeField] float moveDIrection = 2;
    [SerializeField] float cameraMaxRot = 80;
    [SerializeField] bool gravityOn;
    [SerializeField] bool deadCheck;
    [SerializeField] bool turnCheck;
    [SerializeField] Transform playerTr;
    [SerializeField] float rotSpeed = 5f;
    //[SerializeField] Enemy enemy;
    float xC;
    float yC;
    float turnningCount = 0;
    [SerializeField] Camera camera;
    public GameObject rightArmLight;
    public GameObject leftArmLight;
    public GameObject ground;
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
        Moving();
        CameraRot();
        PlayerRot();
        Jump();
        if(deadCheck == false)
        RotGround();
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
        if(Input.GetKeyDown(KeyCode.W))
        {
            transform.Translate(Vector3.forward * moveDIrection);
        }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            transform.Translate(Vector3.back * moveDIrection);
        }
    }

    void Jump()
    {
        if(Input.GetButtonDown("Jump") && gravityOn == false && deadCheck == false)
        {
            rb.velocity = transform.up * 5f;
        }
    }
    void RotGround()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            turnCheck = false;
            rightArmLight.SetActive(true);
            //rb.useGravity = false;
            //rb.velocity = transform.up * 1.5f;
            //enemy.transform.Translate(enemy.transform.up * 1.5f);
            //ground.transform.Rotate(Vector3.forward * 90 * Time.deltaTime);    
            if(turnCheck == false)
            StartCoroutine(MapTurnning());
            //ground.transform.Rotate(0, 0, gameObject.transform.position.z * 30 * Time.deltaTime);            
            //gravityOn = true;
        }
        //else if(Input.GetKeyUp(KeyCode.E))
        //{
        //    rightArmLight.SetActive(false);
        //    //rb.useGravity = true;
        //    //gravityOn = false;
        //}
        if (Input.GetKeyDown(KeyCode.Q))
        {
            leftArmLight.SetActive(true);
            //rb.useGravity = false;
            //rb.velocity = transform.up * 1.5f;
            //enemy.transform.Translate(enemy.transform.up * 1.5f);
            ground.transform.Rotate(Vector3.back * 90);
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
        turnningCount = 0;
        float value = Mathf.Lerp(0, 90f, Time.deltaTime);
        while (turnningCount < 90)
        {
            ground.transform.RotateAround(playerTr.transform.position, Vector3.forward, value);
            turnningCount += value;
            yield return null;
        }
        if(ground.transform.rotation.z % 90 != 0)
        {

        }
        turnCheck = true;
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
        if(other.tag == "ClearBox")
        {
            Destroy(gameObject);
        }
        else if(other.tag == "Bullet")
        {
            playerHp--;
            if(playerHp == 0)
            {
                PlayerDead();
            }
        }
    }
}
    