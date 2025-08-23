using UnityEngine;
using LDtkUnity;
using System.Collections;

public class Portaldown : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Transform godown;
    private Transform level_loc;
    private LDtkFields _fields;
    public bool downstairs = false;
    private Transform playerTransform;
    private float minDistance = 0.9f; // 最小距离（大于此距离才触发传送）
    private Vector3 objectPosition;
    private Camera mainCamera;
    private AudioManager audiomanager;
    public Fade fade;
    public GameObject Player;
    public FloorView floorView;

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

        if (_fields.GetEntityReference("upstair") != null)
        {

            LDtkReferenceToAnEntityInstance goods = _fields.GetEntityReference("upstair");
            LDtkIid name = goods.FindEntity();
            LDtkIid level_name = goods.FindLevel();
            string abc = name.ToString();
            string def = level_name.ToString();
            Debug.Log(abc);
            string result = abc.Split(' ')[0];
            string final = def.Split(' ')[0];
            Debug.Log(final); // 輸出：id
            godown = GameObject.Find(result).transform;
            level_loc = GameObject.Find(final).transform;
            Debug.Log(level_loc.position);
            


        }
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        objectPosition = transform.position;
        audiomanager = FindFirstObjectByType<AudioManager>();
        fade = FindFirstObjectByType<Fade>();
        Player = FindFirstObjectByType<Braveplayer>().gameObject;
        floorView = FindFirstObjectByType<FloorView>();

    }
    // Update is called once per frame
    void Update()
    {

        float distance = Vector2.Distance(playerTransform.position, objectPosition);
      
        // 如果距离小于等于 0.9f，则禁止传送
        if (distance <= minDistance)
        {
            
            downstairs = false;
        }
        

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && other.GetType().ToString() == "UnityEngine.BoxCollider2D" && downstairs)
        {
            Debug.Log("Player到了");
            StartCoroutine(TeleportPlayer());
            Braveplayer.floor -= 1;
            if (Braveplayer.minfloor > Braveplayer.floor)
            {
                Braveplayer.minfloor = Braveplayer.floor;
            }
            Debug.Log(Braveplayer.floor);
            
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && other.GetType().ToString() == "UnityEngine.BoxCollider2D")
        {
            Debug.Log("Player走了");
            downstairs = true;
        }

    }
    private IEnumerator TeleportPlayer()
{
        mainCamera.transform.position += new Vector3(0, -13, 0);
        // 傳送玩家到目標傳送門的位置
        playerTransform.position = godown.position;

        Player.GetComponent<Braveplayer>().StopCurrentRepeatMovement();
        Player.GetComponent<Braveplayer>().StopMoving();
        Player.GetComponent<Braveplayer>().enabled = false;
       
        
        audiomanager.PlaySFX("Stairs");

        fade.FadeIn();

        yield return new WaitForSeconds(1f);

        Player.GetComponent<Braveplayer>().enabled = true;

        // 標記玩家進入傳送範圍，防止重複觸發
        downstairs = false;
    }
}