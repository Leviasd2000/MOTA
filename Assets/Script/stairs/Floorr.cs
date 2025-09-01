using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using LDtkUnity;
using System.Security.Cryptography;
using UnityEngine.PlayerLoop;
using UnityEngine.EventSystems;

public class FloorView : MonoBehaviour
{
    public GameObject platform;
    public GameObject Player;
    public Braveplayer player;
    public Camera floorCamera; // 樓層相機
    public Camera mainCamera;  // 主要畫面相機
    public RawImage floorDisplay; // UI顯示畫面
    private Transform renderCamTransform;  // Render Camera 的 Transform
    public TextMeshProUGUI locate;
    private int relocation; // 目標樓層
    private Dictionary<int, GameObject> DefaultUpfloor = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> DefaultDownfloor = new Dictionary<int, GameObject>();
    private Fade fade;
    private Vector3 vector = new Vector3(0, 13, 0);
    private AudioManager audioManager;

    void Awake()
    {
        // 創建 Render Texture
        RenderTexture renderTexture = new RenderTexture(512, 512, 16);
        renderTexture.Create();

        // 設定相機渲染到 Render Texture
        floorCamera.targetTexture = renderTexture;

        // 設定 UI 的 RawImage 顯示 Render Texture
        floorDisplay.texture = renderTexture;

        platform = GameObject.Find("FloorUI");
        platform.SetActive(false);
        renderCamTransform = floorCamera.transform;

        Player = FindFirstObjectByType<Braveplayer>().gameObject;
        player = FindFirstObjectByType<Braveplayer>();
        fade = FindFirstObjectByType<Fade>();   
        audioManager = FindFirstObjectByType<AudioManager>();

    }

    private void Start()
    {
        DefaultUpfloor = Findallupstairs(DefaultUpfloor , "上樓");
        DefaultDownfloor = Findallupstairs(DefaultDownfloor, "下樓");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && platform.activeSelf != true)
        {
            OnClick_ShowfloorPlatform();
        }
        if (platform.activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (Braveplayer.maxfloor > relocation)
                {
                    Upfloor();
                    audioManager.PlaySFX("Click");
                }
                else
                {
                    audioManager.PlaySFX("Stop");
                }
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (Braveplayer.minfloor < relocation)
                {
                    Downfloor();
                    audioManager.PlaySFX("Click");
                }
                else
                {
                    audioManager.PlaySFX("Stop");
                }
                
            }
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                TeleportPlayer(relocation, Braveplayer.floor);
                fade.FadeIn();
                Braveplayer.floor = relocation;
                platform.SetActive(false);
                Player.GetComponent<Animator>().enabled = true;
                player.enabled = true;
                audioManager.PlaySFX("Portal");
            }
        }
        if (Input.GetKeyDown(KeyCode.G) && platform.activeSelf == true)
        {
            platform.SetActive(false);
            Player.GetComponent<Animator>().enabled = true;
            player.enabled = true;
        }
    }

    void Upfloor()
    {
        relocation += 1;
        locate.text = relocation + "     F";
        floorCamera.transform.position += new Vector3(0, +13, 0);

    }

    void Downfloor()
    {
        relocation -= 1;
        locate.text = relocation + "     F";
        floorCamera.transform.position += new Vector3(0, -13, 0);
    }

    private Dictionary<int, GameObject> Findallupstairs(Dictionary<int, GameObject> all , string type )
    {
        GameObject world = GameObject.Find("World"); // 先找到 World 物體
        if (world == null)
        {
            Debug.LogError("World object not found!");
            return null;
        }
        foreach (Transform level in world.transform) // 遍歷第一層 Level1_x
        {
            string[] parts = level.name.Split('_');
            int number = int.Parse(parts[^1]); // 取最後一段字串轉 int
            Transform entities = level.Find("Entities"); // 查找該層的 "Entities"
            if (entities == null)
            {
                Debug.LogError($"Entities not found in level: {level.name}");
                continue;
            }
            foreach (Transform child in entities) // 遍歷 Entities 下的所有子物體
            {
                
                LDtkFields goals = child.GetComponent<LDtkFields>();
                if (goals == null)
                {
                    Debug.LogError($"LDtkFields component missing on {child.name}");
                    continue;
                }

                bool itemValue = goals.GetBool("Default");
                string category = goals.GetString("Portal");

                if (itemValue == true && category == type) // 檢查 item 是否匹配
                {
                    Debug.Log($"Stairs '{itemValue}' found: {child.name}");
                    all[number] = child.gameObject;
                }
            }

        }
        return all;
    }

    private void TeleportPlayer(int relocation , int location)
    {
        int increment = relocation - location;
        if (increment > 0)
        {
            mainCamera.transform.position += increment * vector;
            GameObject goalstairs = DefaultDownfloor[relocation];
            Player.transform.position = goalstairs.transform.position;
            goalstairs.GetComponent<Portaldown>().downstairs = false;
        }
        else if (increment < 0)
        {
            mainCamera.transform.position += increment * vector;
            GameObject goalstairs = DefaultUpfloor[relocation];
            Player.transform.position = goalstairs.transform.position;
            goalstairs.GetComponent<Portalup>().upstairs = false;
        }
        else
        {
            GameObject goalstairs = DefaultDownfloor[relocation];
            Player.transform.position = goalstairs.transform.position;
            goalstairs.GetComponent<Portaldown>().downstairs = false;
        }
       
        Debug.Log("Main Camera Position: " + mainCamera.transform.position);
    }

    public void OnClick_ShowfloorPlatform()
    {

        platform.SetActive(true);
        relocation = Braveplayer.floor;
        locate.text = Braveplayer.floor + "     F";
        renderCamTransform.position = mainCamera.transform.position;
        renderCamTransform.rotation = mainCamera.transform.rotation;
        renderCamTransform.position += new Vector3(1, 0, 0); // 向右偏移 1 單位

        Player.GetComponent<Animator>().enabled = false;
        player.enabled = false;
        EventSystem.current.SetSelectedGameObject(null);
    }

}
