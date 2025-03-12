using UnityEngine;
using Yarn.Unity;

public class NPC : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private string m_TalkToNode;
    private string npcname;

    private void Start()
    {
        npcname = gameObject.name;
    }

    private void OnTriggerEnter(Collider other)
    {
        StartDialouge();
    }

    public void StartDialouge()
    {
        FindFirstObjectByType<DialogueRunner>().StartDialogue(m_TalkToNode);
    }
}
