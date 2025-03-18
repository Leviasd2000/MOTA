using UnityEngine;
using UnityEngine.UI;
using LDtkUnity;
using System.Collections;

public class Portalup : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Transform goup;
    private LDtkFields _fields;
    public bool upstairs = true;
    private Transform playerTransform;
    private float minDistance = 0.9f; // 最小距离（大于此距离才触发传送）
    private Vector3 objectPosition;
    private Camera mainCamera;
    private AudioManager audiomanager;
    public Fade fade;
    public GameObject Player;


    void Awake()
    {
        // 在 Awake 或 Start 中初始化 Camera.main
        mainCamera = Camera.main;

        if (mainCamera != null)
        {
            Debug.Log("成功找到主相機！");
        }
        else
        {
            Debug.LogWarning("找不到主相機，請檢查標籤是否為 'MainCamera'！");
        }
    }

    void Start()
    {
        // 嘗試在當前 GameObject 上尋找 LDtkFields 組件
        _fields = GetComponent<LDtkFields>();
        if (_fields == null)
        {
            Debug.LogError("沒有找到 LDtkFields 組件，請確保它已附加到此物件上！");
            return;
        }

        if (_fields.GetEntityReference("downstair") != null)
        {
            LDtkReferenceToAnEntityInstance goods = _fields.GetEntityReference("downstair");
            LDtkIid name = goods.FindEntity();
            string abc = name.ToString();
            goup = transform.Find(abc);
            Debug.Log(abc);
            string result = abc.Split(' ')[0];
            Debug.Log(result); // 輸出：id
            goup = GameObject.Find(result).transform;
            Debug.Log(goup);
        }
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        objectPosition = transform.position;
        audiomanager = FindFirstObjectByType<AudioManager>();
        fade = FindFirstObjectByType<Fade>();
        Player = FindFirstObjectByType<Braveplayer>().gameObject;

}

    // Update is called once per frame
    void Update()
    {
        if (playerTransform == null)
        {
            Debug.Log("找不到player");
            Debug.Log(transform.position);
            return;
        }
        else if (objectPosition == null)
        {
            Debug.Log("找不到object");
            return;
        }
        float distance = Vector2.Distance(playerTransform.position, objectPosition);

        // 如果距离小于等于 0.9，则禁止传送
        if (distance <= minDistance)
        {
        
            upstairs = false;
        }
     }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && other.GetType().ToString() == "UnityEngine.BoxCollider2D" && upstairs)
        {
            Debug.Log("Player到了");
            TeleportPlayer();
            Braveplayer.floor += 1;
            Braveplayer.maxfloor = Braveplayer.floor;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && other.GetType().ToString() == "UnityEngine.BoxCollider2D")
        {
            Debug.Log("Player走了");
            upstairs = true ;
        }

    }
    private void TeleportPlayer()
    {
        mainCamera.transform.position += new Vector3(0, +13, 0);
        Debug.Log("Main Camera Position: " + mainCamera.transform.position);
        

        // 傳送玩家到目標傳送門的位置
        playerTransform.position = goup.position;

        Player.GetComponent<Braveplayer>().StopCurrentRepeatMovement();
        Player.GetComponent<Braveplayer>().StopMoving();
        Player.GetComponent<Braveplayer>().enabled = false;
        Player.GetComponent<Braveplayer>().enabled = true;

        audiomanager.Play("Stairs", false);

        fade.FadeIn();


        // 標記玩家進入傳送範圍，防止重複觸發
        upstairs = true;
    }
}

