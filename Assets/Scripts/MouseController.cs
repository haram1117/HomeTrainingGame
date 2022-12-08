using System;
using System.Collections;
using System.Collections.Generic;
using UnityChan;
using UnityEngine;
using UnityEngine.UI;

public class MouseController : MonoBehaviour
{
    //[SerializeField] private TCPManager tcpManager;

    private RectTransform rectTransform;

    private bool triggerEntered;

    private bool processing;
    private Button nowButton;
    private BoxCollider2D collider2D;
    
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.anchoredPosition = TCPManager.Instance.cursorPosition;
        if (TCPManager.Instance.isMouseClicked && triggerEntered && !processing)
        {
            processing = true;
            nowButton.onClick.Invoke();
            collider2D.enabled = false; 
            this.Invoke(() => processing = false, 1.0f);
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<Button>() != null)
        {
            Debug.Log(col.name);
            nowButton = col.GetComponent<Button>();
            collider2D = col.GetComponent<BoxCollider2D>();
            triggerEntered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Button>() != null)
        {
            nowButton = null;
            triggerEntered = false;
        }
    }
}
