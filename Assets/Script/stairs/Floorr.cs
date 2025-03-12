using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using LDtkUnity;
using System.Security.Cryptography;

public class FloorView : MonoBehaviour
{
    public GameObject platform;
    public GameObject Player;
    public Braveplayer player;
    public Camera floorCamera; // �Ӽh�۾�
    public Camera mainCamera;  // �D�n�e���۾�
    public RawImage floorDisplay; // UI��ܵe��
    private Transform renderCamTransform;  // Render Camera �� Transform
    public TextMeshProUGUI locate;
    private int relocation; // �ؼмӼh
    private Dictionary<int, GameObject> DefaultUpfloor = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> DefaultDownfloor = new Dictionary<int, GameObject>();
    private Vector3 vector = new Vector3(0, 13, 0);

    void Awake()
    {
        // �Ы� Render Texture
        RenderTexture renderTexture = new RenderTexture(512, 512, 16);
        renderTexture.Create();

        // �]�w�۾���V�� Render Texture
        floorCamera.targetTexture = renderTexture;

        // �]�w UI �� RawImage ��� Render Texture
        floorDisplay.texture = renderTexture;

        platform = GameObject.Find("FloorUI");
        platform.SetActive(false);
        renderCamTransform = floorCamera.transform;

        Player = FindFirstObjectByType<Braveplayer>().gameObject;
        player = FindFirstObjectByType<Braveplayer>();

    }

    private void Start()
    {
        DefaultUpfloor = Findallupstairs(DefaultUpfloor , "�W��");
        DefaultDownfloor = Findallupstairs(DefaultDownfloor, "�U��");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && platform.activeSelf != true)
        {
            platform.SetActive(true);
            relocation = Braveplayer.floor;
            locate.text = Braveplayer.floor + "     F";
            renderCamTransform.position = mainCamera.transform.position;
            renderCamTransform.rotation = mainCamera.transform.rotation;
            renderCamTransform.position += new Vector3(1, 0, 0); // �V�k���� 1 ���

            Player.GetComponent<Animator>().enabled = false;
            player.enabled = false;
        }
        if (platform.activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                Upfloor();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                Downfloor();
            }
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                TeleportPlayer(relocation, Braveplayer.floor);
                Braveplayer.floor = relocation;
                platform.SetActive(false);
                Player.GetComponent<Animator>().enabled = true;
                player.enabled = true;
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
        GameObject world = GameObject.Find("World"); // ����� World ����
        if (world == null)
        {
            Debug.LogError("World object not found!");
            return null;
        }
        foreach (Transform level in world.transform) // �M���Ĥ@�h Level1_x
        {
            char floor = level.name[^1];
            int number = (int)char.GetNumericValue(floor);
            Transform entities = level.Find("Entities"); // �d��Ӽh�� "Entities"
            if (entities == null)
            {
                Debug.LogError($"Entities not found in level: {level.name}");
                continue;
            }
            foreach (Transform child in entities) // �M�� Entities �U���Ҧ��l����
            {
                
                LDtkFields goals = child.GetComponent<LDtkFields>();
                if (goals == null)
                {
                    Debug.LogError($"LDtkFields component missing on {child.name}");
                    continue;
                }

                bool itemValue = goals.GetBool("Default");
                string category = goals.GetString("Portal");

                if (itemValue == true && category == type) // �ˬd item �O�_�ǰt
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
}
