using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LDtkUnity;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    public GameObject player;
    
    public GameObject map;
    public static SaveManager Instance { get; private set; }

    // 存檔資料夾的路徑
    private string GetSaveFolder(int slot) => Path.Combine(Application.persistentDataPath, $"slot{slot}");

    // 存檔 JSON 檔案路徑
    private string GetSavePath(int slot) => Path.Combine(GetSaveFolder(slot), "save.json");

    // 截圖檔案路徑
    private string GetScreenshotPath(int slot) => Path.Combine(GetSaveFolder(slot), "screenshot.png");

    private LDtkIid[] allEntities;
    private List<string> currentIDs;
    private GameObject currentMapInstance;
    public Camera mainCamera;
    public Inventory inventory;
    public string CurrentScene => SceneManager.GetActiveScene().name;
    public GameObject SaveUI;

    private int currentPage = 0; // 從第 1 頁開始
    private const int slotsPerPage = 6; // 每頁顯示幾個槽
    private const int totalSlots = 120;  // 總槽數

    [Header("框框")]
    public Sprite defaultSprite;       // 預設的藍框 Sprite

    [Header("箭頭")]
    public GameObject Leftarrow;
    public GameObject Rightarrow;

    [Header("角色屬性")]
    public List<TextMeshProUGUI> attributes;
    

    [Header("序號")]
    public List<TextMeshProUGUI> orders;

    [Header("頁數")]
    public TextMeshProUGUI page;

    [Header("儲存格")]
    public SaveSlot[] slotUIs; // 在 Inspector 拉 6 個 slot prefab

    private int currentSlot = 0;  // 預設選第 0 個 slot

    private SaveSlot currentSelected;

    private int currentIndex = 0; // 第 n 個 儲存槽

    private Saveclass[] saveclasses;

    

    private void Setproperties( List<TextMeshProUGUI> attrword , int currentslot)
    {
        int Hp = Braveattr.GetAttribute("Hp");
        int Atk = Braveattr.GetAttribute("Atk");
        int Def = Braveattr.GetAttribute("Def");
        int currentindex = currentslot % slotsPerPage;

        attrword[currentindex].text = $"<color=#FFFFFF>{Hp}/{Atk}/{Def}</color>";
    }

    private void Setpage(List<TextMeshProUGUI> No)
    {
        for (int i = 0; i < slotsPerPage; i++)
        {
            int index = currentPage * slotsPerPage + i;
            No[i].text = $"<color=#FFFFFF>No.{index}</color>";
        }
    }

    private void Setindex(TextMeshProUGUI page)
    {
        page.text = $"<color=#FFFFFF>{currentPage}/{totalSlots/slotsPerPage}</color>";
    }

    public void Awake()
    {
        map = GameObject.Find("new");
        allEntities = FindObjectsByType<LDtkIid>(FindObjectsSortMode.None);
        // 收集目前關卡的所有 Entity ID
        currentIDs = allEntities.Select(e => e.Iid).ToList(); // public static implicit operator string(LDtkIid iid) => iid.Iid; 會直接自動轉成 string
        player = FindFirstObjectByType<Braveplayer>().gameObject;
        
        // Singleton & Don't Destroy
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void NextPage()
    {
        int maxPage = (totalSlots - 1) / slotsPerPage;
        if (currentPage < maxPage)
        {
            currentPage++;
            RefreshPage();
            Setindex(page);
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            RefreshPage();
            Setindex(page);
        }
    }

    void RefreshPage()
    {
        for (int i = 0; i < slotsPerPage; i++)
        {
            int index = 6 * currentPage + i;

            string folderPath = Path.Combine(Application.persistentDataPath, "slot" + index);
            
            Image slotImage = slotUIs[i].GetComponent<Image>();

            if (Directory.Exists(folderPath))
            {
                string screenshotPath = Path.Combine(folderPath, "screenshot.png");
                if (File.Exists(screenshotPath))
                {
                    // 載入存檔截圖
                    byte[] imageBytes = File.ReadAllBytes(screenshotPath);
                    Texture2D tex = new Texture2D(2, 2);
                    tex.LoadImage(imageBytes);

                    slotImage.sprite = Sprite.Create(tex,
                        new Rect(0, 0, tex.width, tex.height),
                        new Vector2(0.5f, 0.5f));
                }
                else
                {
                    // 沒有截圖 → 顯示 defaultSprite
                    slotImage.sprite = defaultSprite;
                }
            }
            else
            {
                // 沒有存檔 → 顯示 defaultSprite
                slotImage.sprite = defaultSprite;
            }
        }
    }
    public void Start()
    {
        if (player == null)
            player = FindFirstObjectByType<Braveplayer>()?.gameObject;

        if (inventory == null)
            inventory = FindFirstObjectByType<Inventory>();
        
        if (SaveUI == null)
            SaveUI = GetComponentInChildren<Saveslot>(true).gameObject;

        Setpage(orders);
        Setindex(page);
        currentPage = 0;
        RefreshPage();

        SaveUI.SetActive(false); // 初始時隱藏存檔 UI

    }

    private IEnumerator CaptureScreenshot(int slotIndex, string path)
    {
        SaveUI.SetActive(false); // 暫時關閉UI
        yield return new WaitForEndOfFrame();

        int startX = 504;
        int startY = 0;
        int width = 1080;
        int height = 1080;

        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(startX, startY, width, height), 0, 0);
        tex.Apply();

        // 縮小到 260*260
        int targetSize = 260;
        Texture2D smallTex = new Texture2D(targetSize, targetSize, TextureFormat.RGB24, false);
        for (int y = 0; y < targetSize; y++)
            for (int x = 0; x < targetSize; x++)
            {
                Color color = tex.GetPixelBilinear((float)x / targetSize, (float)y / targetSize);
                smallTex.SetPixel(x, y, color);
            }
        smallTex.Apply();

        // 存成 PNG
        File.WriteAllBytes(path, smallTex.EncodeToPNG());

        Destroy(tex);

        SaveUI.SetActive(true);
        Debug.Log($"📸 Slot{slotIndex} 截圖完成: {path}");
    }

    public void SaveGame(int slotIndex)
    {
        Saveclass data = new Saveclass();

        // 建立 slot 專屬資料夾
        string slotFolder = Path.Combine(Application.persistentDataPath, $"slot{currentIndex}");
        if (!Directory.Exists(slotFolder))
            Directory.CreateDirectory(slotFolder);

        // 存檔路徑
        string savePath = Path.Combine(slotFolder, "save.json");
        string screenshotPath = Path.Combine(slotFolder, "screenshot.png");

        // 儲存截圖
        StartCoroutine(CaptureScreenshot(currentIndex, screenshotPath));

        // 填寫存檔資料
        data.playerPosition = player.transform.position;
        data.maincameraPosition = mainCamera.transform.position;

        data.recentfloor = Braveplayer.floor;
        data.maxfloor = Braveplayer.maxfloor;
        data.minfloor = Braveplayer.minfloor;

        data.Sword = Braveattr.Sword;
        data.Shield = Braveattr.Shield;

        data.Hp = Braveattr.GetAttribute("Hp");
        data.Atk = Braveattr.GetAttribute("Atk");
        data.Def = Braveattr.GetAttribute("Def");
        data.Gold = Braveattr.GetAttribute("Gold");
        data.Exp = Braveattr.GetAttribute("Exp");
        data.Fatigue = Braveattr.GetAttribute("Fatigue");
        data.Breath = Braveattr.GetAttribute("Breath");
        data.AttackCritical = Braveattr.AttackCritical;
        data.DefenseCritical = Braveattr.DefenseCritical;

        data.inventory = new List<ItemEntry>();
        foreach (var kvp in inventory.GetItemDictionary())
            data.inventory.Add(new ItemEntry { id = kvp.Key, quantity = kvp.Value });

        // 差集 = 已刪除
        var allExpectedIDs = GetAllEntityIDsInCurrentLevel();
        data.deletedEntityIDs = currentIDs.Except(allExpectedIDs).ToList();

        // 寫入 JSON
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);

        Debug.Log($"✅ Slot{currentIndex} 已儲存");
    }

    public void LoadGame(int slot)
    {
        string path = GetSavePath(slot);
        Debug.Log($"{path}");
        if (!File.Exists(path))
        {
            Debug.LogWarning("❌ 沒有存檔！");
            return;
        }

        string json = File.ReadAllText(path);
        Saveclass data = JsonUtility.FromJson<Saveclass>(json);
        StartCoroutine(ApplyLoadAfterDelay(data));
    }

    // 刪除指定的存檔槽
    public void DeleteSave(int slot)
    {
        string folderPath = Path.Combine(Application.persistentDataPath, "slot" + slot);
        Debug.Log($"{folderPath}");

        if (Directory.Exists(folderPath))
        {
            Directory.Delete(folderPath, true); // true = 遞迴刪除子資料夾與檔案
            Debug.Log($"刪除了存檔槽 {slot}");

            int slotIndex = slot % 6;

            slotUIs[slotIndex].GetComponent<Image>().sprite = defaultSprite;
        }
        else
        {
            Debug.LogWarning($"存檔槽 {slot} 不存在，無法刪除");
        }
    }

    IEnumerator ApplyLoadAfterDelay(Saveclass data)
    {
        player.transform.SetParent(null);

        // 找到場景中那張已存在的地圖
        var maybeMap = GameObject.Find("new"); // 或你地圖的名字
        if (maybeMap != null)
        {
            currentMapInstance = maybeMap;
        }

        // 清掉舊地圖
        if (currentMapInstance != null)
        {
            Destroy(currentMapInstance);
        }

        // 使用 Addressables 載入整張地圖 prefab
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>("NoBrave");

        yield return handle;

        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError("❌ 地圖載入失敗！");
            yield break;
        }

        currentMapInstance = Instantiate(handle.Result);
        currentMapInstance.name = "Map";  // 可以改回 "new" 以維持一致
        yield return new WaitForSeconds(0f); // 🔧 加這行！
        Debug.Log("✅ 地圖載入完成");

        // 重新載入場景
        yield return SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        yield return null; // 等待一幀讓所有 Awake 跑完

        // 重新取得物件
        inventory = FindFirstObjectByType<Inventory>();
        player = FindFirstObjectByType<Braveplayer>()?.gameObject;
        
        mainCamera = Camera.main;
        mainCamera.transform.position = data.maincameraPosition;
        player.transform.position = data.playerPosition;

        // 還原玩家當前樓層
        Braveplayer.floor = data.recentfloor;
        Braveplayer.maxfloor = data.maxfloor;
        Braveplayer.minfloor = data.minfloor;

        // 還原裝備欄
        Braveattr.Sword = data.Sword;
        Braveattr.Shield = data.Shield;
        
        // 還原玩家屬性
        Braveattr.SetAttribute("Hp", data.Hp);
        Braveattr.SetAttribute("Atk", data.Atk);
        Braveattr.SetAttribute("Def", data.Def);
        Braveattr.SetAttribute("Gold", data.Gold);
        Braveattr.SetAttribute("Exp", data.Exp);
        Braveattr.SetAttribute("Fatigue", data.Fatigue);
        Braveattr.SetAttribute("Breath", data.Breath);
        Braveattr.AttackCritical = data.AttackCritical;
        Braveattr.DefenseCritical = data.DefenseCritical;


        var allTrackers = FindObjectsByType<LDtkIid>(FindObjectsSortMode.None);
        foreach (var tracker in allTrackers)
        {
            if (data.deletedEntityIDs.Contains(tracker.Iid))
            {
                Destroy(tracker.gameObject);
            }
        }
        // 還原 Inventory
        inventory.ClearAllItems();
        foreach (var entry in data.inventory)
        {
            inventory.AddItem(entry.id, entry.quantity);
        }
        Debug.Log("✅ 載入完成");
    }

    private List<string> GetAllEntityIDsInCurrentLevel()
    {
         return FindObjectsByType<LDtkIid>(FindObjectsSortMode.None)
        .Select(e => e.Iid)
        .ToList();
    }

    IEnumerator RefreshAfterDelay()
    {
        yield return null; // 延遲一幀
        RefreshPage();
    }

    public void ToggleSaveUI()
    {
        if (SaveUI != null)
        {
            SaveUI.SetActive(!SaveUI.activeSelf);
        }
    }

    public void SelectSlot(int slotIndex)
    {
        currentSlot = slotIndex;  // 設定當前選擇的 slot

        if (currentSelected != null)
        {
            currentSelected.SetHighlight(false);
        }

        currentIndex = currentPage * 6 + slotIndex; 
        
        currentSelected = slotUIs[slotIndex];
        
        currentSelected.SetHighlight(true);

    }

    private void Update()
    {   
        for (int i = 0; i < 6; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i) || Input.GetKeyDown(KeyCode.Keypad1 + i))
            {
                currentSlot = 6 * currentPage + i;
                
                SelectSlot(i);
            }
        }

        if (Input.GetKeyDown(KeyCode.P) & (SaveUI.activeSelf))
        {
            SaveGame(currentIndex);  // 按 P 存檔
            Setproperties(attributes , currentIndex);
            StartCoroutine(RefreshAfterDelay());
            
        }

        if (Input.GetKeyDown(KeyCode.L) & (SaveUI.activeSelf))
        {
            LoadGame(currentIndex);  // 按 L 讀檔
            StartCoroutine(RefreshAfterDelay());
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            ToggleSaveUI(); // 按 U 開/關存檔 UI
            if (SaveUI.activeSelf)
            {
                player.GetComponent<Braveplayer>().StopCurrentRepeatMovement();
                player.GetComponent<Braveplayer>().StopMoving();
                player.GetComponent<Braveplayer>().enabled = false;
            }
            else
            {
                player.GetComponent<Animator>().enabled = true;
                player.GetComponent<Braveplayer>().enabled = true;
            }

                StartCoroutine(RefreshAfterDelay());
        }

        if (Input.GetKeyDown(KeyCode.Q) & (SaveUI.activeSelf))
        {
            DeleteSave(currentIndex); // 按 U 開/關存檔 UI
            StartCoroutine(RefreshAfterDelay());
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) & (SaveUI.activeSelf))
        {
            NextPage();
            Setpage(orders);
            slotUIs[currentSlot].SetHighlight(false);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) & currentPage>=1 & (SaveUI.activeSelf))
        {
            PreviousPage();
            Setpage(orders);
            slotUIs[currentSlot].SetHighlight(false);
        }
    }
}
