using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
{
    private bool isDown = false;
    private float lastClickTime = 0;
    public float rotateSpeed = 1f;
    [SerializeField]
    private GameObject glasses;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isDown)
        {
            glasses.transform.Rotate(Vector3.down * (Time.time - lastClickTime), rotateSpeed, Space.World);
            lastClickTime = Time.time;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isDown = true;
        lastClickTime = Time.time;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        isDown = false;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        isDown = false;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("进入");
    }
}
