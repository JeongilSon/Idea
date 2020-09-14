using OVR.OpenVR;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    // 상태 변수
    [SerializeField] bool isGround;
    [SerializeField] bool movingCheck;
    [SerializeField] bool turnCheck;
    [SerializeField] bool soundGround;

    public static bool clearCheck;

    //움직임 및 동작 제어
    int turnValue = 0;
    int turnningCount;
    int moveDirection = 50;
    int moveCount;
    int moveValue;

    //이동 시 속도나 물리제어
    [SerializeField] int gravityScale = 1;
    int speed = 2;

    //플레이어의 마지막 이동
    [SerializeField] Vector3 moveDir;

    //사운드
    public AudioClip gravityRightSource, gravityLeftSource, groundedSource, clearPointSource, moveSound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GroundCheck();
        CameraMoving();
        PlayerMoving();
        ClearCheck();
        WallCheck();
        Turnning();
        Cheat();
        transform.Translate(moveDir * Time.deltaTime * speed);
    }
    public void PlayerMoving()
    {
        //moveDir.z = Input.GetAxis("Vertical");
        //한칸씩 움직임
        if (movingCheck == false && turnCheck == false && isGround == true && clearCheck == false)
        {
            if (Input.GetKeyDown(KeyCode.W) || OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickUp))
            {
                moveValue = 1;
                if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickUp))
                {
                    moveValue = 1;
                }
                StartCoroutine(SmoothMoving());
            }
            else if (Input.GetKeyDown(KeyCode.S) || OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickDown))
            {
                moveValue = -1;
                if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickDown))
                {
                    moveValue = -1;
                }
                StartCoroutine(SmoothMoving());
            }
            else moveValue = 0;

        }
        if (!Physics.Raycast(transform.position, transform.up * -1f, 1f, 1 << LayerMask.NameToLayer("Ground")) && turnCheck == false)
        {
            isGround = false;
            soundGround = false;
        }
    }
    IEnumerator SmoothMoving()
    {
        movingCheck = true;
        moveCount = 0;
        SoundManager.instance.PlaySingle(moveSound);
        float moveDevelop = moveValue > 0 ? 1 : -1;
        while (moveCount < moveDirection)
        {
            moveDir.z = moveDevelop;
            moveCount += 1;
            yield return null;
        }
        movingCheck = false;
        moveDir.z = 0;
    }
    public void CameraMoving()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickRight) && turnCheck == false || Input.GetKeyDown(KeyCode.X))
        {
            transform.Rotate(0, 90, 0);
        }
        else if (OVRInput.GetDown(OVRInput.Button.SecondaryThumbstickLeft) || Input.GetKeyDown(KeyCode.Z))
        {
            transform.Rotate(0, -90, 0);
        }
    }
    public void GroundCheck()
    {
        if (!isGround)
        {
            moveDir.y = Physics.gravity.y * gravityScale;
        }
        if (Physics.Raycast(transform.position, transform.up * -1f, 1f, 1 << LayerMask.NameToLayer("Ground")))
        {                        
            isGround = true;         
            moveDir.y = 0;
            if (isGround == true && soundGround == false)
                SoundManager.instance.PlaySingle(groundedSource);
            soundGround = true;
        }
    }
    public void WallCheck()
    {
        if (Physics.Raycast(transform.position, this.transform.forward, 0.5f, 1 << LayerMask.NameToLayer("Ground")))
        {
            //moveDir.z = Mathf.Clamp(moveDir.z, 0, -1);
            if(movingCheck == true && moveValue ==  -1)
            {
                moveValue = -1;
            }
            else
            moveDir.z = 0;
            
        }
        else if (Physics.Raycast(transform.position, -this.transform.forward, 0.5f, 1 << LayerMask.NameToLayer("Ground")))
        {
            //moveDir.z = Mathf.Clamp(moveDir.z, 1, 0);
            if (movingCheck == true && moveValue == 1)
            {
                moveValue = 1;
            }
            else
            moveDir.z = 0;
        }
    }
    public void Turnning()
    {
        #region VR
        if (turnCheck == false && isGround)
        {
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
            {
                turnValue = 1;
                if (turnCheck == false)
                    SoundManager.instance.PlaySingle(gravityLeftSource);
                StartCoroutine(MapTurnning());
            }
            else if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
            {
                turnValue = -1;
                if (turnCheck == false)
                    SoundManager.instance.PlaySingle(gravityRightSource);
                StartCoroutine(MapTurnning());
            }
        }
        #endregion NoneVR

        #region NoneVR
        if (turnCheck == false && isGround)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                turnValue = 1;
                if (turnCheck == false)
                    SoundManager.instance.PlaySingle(gravityLeftSource);
                StartCoroutine(MapTurnning());
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                turnValue = -1;
                if (turnCheck == false)
                    SoundManager.instance.PlaySingle(gravityRightSource);
                StartCoroutine(MapTurnning());
            }
        }
        #endregion
    }
    IEnumerator MapTurnning()
    {
        turnCheck = true;
        turnningCount = 0;
        //Vector3 turnDevelop = turnValue > 0 ? transform.right : transform.right * -1f;
        Vector3 turnDevelop = turnValue > 0 ? Vector3.back : Vector3.forward;
        while (turnningCount < 90)
        {
            transform.Rotate(turnDevelop);
            turnningCount += 1;
            yield return null;
        }
        turnCheck = false;
        isGround = false;
    }
    public void ClearCheck()
    {
        if (Physics.Raycast(transform.position, transform.forward, 1f, 1 << LayerMask.NameToLayer("ClearBox")))
        {
            if (Physics.Raycast(transform.position, -transform.up, 1.5f, 1 << LayerMask.NameToLayer("ClearGround")))
            {
                SoundManager.instance.PlaySingle(clearPointSource);
                clearCheck = true;
            }
        }
    }
    public void Cheat()
    {
        if (Input.GetKeyDown(KeyCode.M) || OVRInput.GetDown(OVRInput.RawButton.X))
        {
            SoundManager.instance.PlaySingle(clearPointSource);
            clearCheck = true;
        }
    }

}