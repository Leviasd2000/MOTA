using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using cfg.monster;
public class Braveplayer : MonoBehaviour
{   
    /// <summary>
    /// 玩家控制動作
    /// </summary>
    public Animator animator;           // 動畫控制器
    public float moveDistance = 1.0f;   // 每次按鍵移動的距離
    public float holdTime = 0.5f;       // 需要持續按住的時間（秒）
    private bool isMoving;              // 是否正在移動
    private Vector3 currentDirection;   // 當前移動方向
    private bool isRepeating;           // 是否進行重複移動
    private Coroutine repeatCoroutine;  // 當前執行的協程
    private Rigidbody2D rb;             // Rigidbody2D 組件
    private float holdTimer = 0.0f;     // 按鍵按住的計時器

    /// <summary>
    /// 玩家當前樓層
    /// </summary>
    public static int floor = 0;
    public static int maxfloor = 0;
    public static int minfloor = 0;


    /// <summary>
    /// 玩家背包
    /// </summary>
    public GameObject myBag;
    public GameObject SwordBox;
    public GameObject ShieldBox;
    private AudioManager audiomanager;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audiomanager = FindFirstObjectByType<AudioManager>();
        isMoving = false;
        isRepeating = false;
        SwordBox = GameObject.Find("SwordBox");
        ShieldBox = GameObject.Find("ShieldBox");
        myBag = GameObject.Find("Bag");
        if (myBag != null)
        {
            myBag.SetActive(false);  // 遊戲開始時關閉 bag
        }
        if (SwordBox != null)
        {
            SwordBox.SetActive(false);  // 遊戲開始時關閉 sword
        }
        if (ShieldBox != null)
        {
            ShieldBox.SetActive(false);  // 遊戲開始時關閉 shield
        }

    }
    private void Update()
    {
        // 獲取最新輸入方向
        Vector3 inputDirection = CheckInput();

        // 如果有新按鍵，更新方向並立即停止重複移動
        if (inputDirection != Vector3.zero)
        {
            // 如果方向改變，停止當前協程並重置計時器
            if (inputDirection != currentDirection)
            {
                StopCurrentRepeatMovement();  // 立即停止舊的移動
                holdTimer = 0.0f;             // 重置計時器
                isRepeating = false;          // 停止重複移動
                isMoving = false;             // 重置標誌
            }

            currentDirection = inputDirection;

            // 如果尚未開始移動，執行單次移動
            if (!isMoving)
            {
                MoveCharacter(currentDirection);
                Debug.Log("Movc");
            }

            

            // 累積按住時間
            holdTimer += Time.deltaTime;

            // 如果按住時間達到設定值，啟動持續移動
            if (holdTimer >= holdTime && !isRepeating)
            {
                isRepeating = true;
                // 撥放走路聲（如果還沒播放）
                audiomanager.PlayLoop("Walk");
                repeatCoroutine = StartCoroutine(RepeatMovement());
            }
        }
        else
        {
            // 沒有輸入時停止移動
            holdTimer = 0.0f; // 重置按住時間
            StopCurrentRepeatMovement();
            StopMoving();
        }

        Openmybag();
        OpenmyShield();
        OpenmySword();
    }

    // 檢測玩家的鍵盤輸入方向
    private Vector3 CheckInput()
    {
        Vector3 inputDirection = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.W)) inputDirection = Vector3.up;
        else if (Input.GetKeyDown(KeyCode.S)) inputDirection = Vector3.down;
        else if (Input.GetKeyDown(KeyCode.A)) inputDirection = Vector3.left;
        else if (Input.GetKeyDown(KeyCode.D)) inputDirection = Vector3.right;
        

        // 檢查方向鍵是否被按下（避免對角線輸入）
        if (Input.GetKey(KeyCode.W)) inputDirection = Vector3.up;
        else if (Input.GetKey(KeyCode.S)) inputDirection = Vector3.down;
        else if (Input.GetKey(KeyCode.A)) inputDirection = Vector3.left;
        else if (Input.GetKey(KeyCode.D)) inputDirection = Vector3.right;

        return inputDirection;
    }

    // 單次移動角色並觸發動畫
    private void MoveCharacter(Vector3 direction)
    {
        isMoving = true; // 標記角色為移動中

        // 計算目標位置
        Vector2 targetPosition = rb.position + (Vector2)direction * moveDistance;
        audiomanager.PlaySFX("Walk");
        targetPosition.x = Mathf.Round((targetPosition.x) * 2) / 2;
        targetPosition.y = Mathf.Round((targetPosition.y) * 2) / 2;

        // 移動角色
        rb.MovePosition(targetPosition);

        // 更新動畫參數
        animator.SetFloat("horizontal", direction.x);
        animator.SetFloat("vertical", direction.y);
        animator.SetBool("walk", true);

    }



    // 重複移動角色
    private IEnumerator RepeatMovement()
    {
        while (isRepeating)
        {
            // 計算目標位置並移動
            Vector2 targetPosition = rb.position + (Vector2)currentDirection * moveDistance;
            rb.MovePosition(targetPosition);

            // 更新動畫參數
            animator.SetFloat("horizontal", currentDirection.x);
            animator.SetFloat("vertical", currentDirection.y);
            animator.SetBool("walk", true);

            yield return new WaitForSeconds(0.02f); // 控制移動間隔
        }

        
    }

    // 停止當前協程
    public void StopCurrentRepeatMovement()
    {
        if (repeatCoroutine != null)
        {
            StopCoroutine(repeatCoroutine); // 停止協程
            audiomanager.StopLoop();            // 停止走路聲
            repeatCoroutine = null;        // 清除協程引用
            isRepeating = false;           // 停止重複移動
        }
    }

    // 停止移動並重置動畫
    public void StopMoving()
    {
        isMoving = false;                   // 停止移動標誌
        isRepeating = false;                // 停止重複移動
        animator.SetBool("walk", false);    // 停止動畫
    }

    private void Openmybag()
    {   
        if (Input.GetKeyDown(KeyCode.B))
        {
            myBag.SetActive(!myBag.activeSelf);
        }
    }

    private void OpenmySword()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            SwordBox.SetActive(!SwordBox.activeSelf);
        }
    }

    private void OpenmyShield()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ShieldBox.SetActive(!ShieldBox.activeSelf);
        }
    }

    private void OnCollisionEnter2D( Collision2D others)
    {
        // isMoving = true; // 標記角色為移動中

        // 計算目標位置
        Vector2 targetPosition = rb.position;

        targetPosition.x = Mathf.Round((targetPosition.x) * 2) / 2;
        targetPosition.y = Mathf.Round((targetPosition.y) * 2) / 2;

        // 移動角色
        rb.MovePosition(targetPosition);

        audiomanager.StopLoopAfterThisClip();

    }


}
