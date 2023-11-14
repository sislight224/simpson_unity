using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageItem : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI contentText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetContent(string name, string content)
    {
        nameText.text = name;
        contentText.text = "New topic found: " + content;
    }
}
