using UnityEngine;
using Yarn.Unity;

public class Yarnmethod : MonoBehaviour
{
    private static Inventory inventory;

    private void Start()
    {
        inventory = FindFirstObjectByType<Inventory>();
    }

    [YarnFunction("GetBraveProperty")]
    public static int Leap(string Type)
    {
        int result = Braveattr.GetAttribute(Type); // ©I¥s GetAttribute()
        return result;
    }

    [YarnFunction("CheckItem")]
    public static bool Leak(string item)
    {
        return inventory.GetItemQuantity(item) > 0;
    }




}



