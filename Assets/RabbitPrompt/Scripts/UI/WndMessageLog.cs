using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WndMessageLog : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;

    // Start is called before the first frame update
    void Start()
    {
        textMeshPro.text = "ddd";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetContent( string content)
    {
        textMeshPro.text = content;
    }
}
