using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WndMessagePanel : MonoBehaviour
{
    public MessageItem item;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AddMessage(string name, string content)
    {
        int childCount = transform.childCount;
        if(childCount > 3)
        {
            GameObject firstChild = transform.GetChild(1).gameObject;
            Destroy(firstChild);
        }

        MessageItem p = Instantiate(item, transform, true);
        p.SetContent(name, content);
        
        p.gameObject.SetActive(true);
        p.gameObject.AddComponent<DestroyGameObject>();

    }
}
