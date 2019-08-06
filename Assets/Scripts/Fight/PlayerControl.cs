using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
{
    private bool isDown = false;
    private float lastClickTime = 0;
    public float rotateSpeed = 1f;
    [SerializeField]
    private GameObject glasses;
    private float startRotate;
    public int maxRotate = 135;

    // Start is called before the first frame update
    void Start()
    {
        startRotate = glasses.transform.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.touchCount <= 0 || Input.touchCount >= 2)
        // {
        //     return;
        // }
        // if (Input.touchCount == 1)
        // {
        //     if (Input.touches[0].phase == TouchPhase.Stationary)
        //     {
        //         if ((glasses.transform.eulerAngles.y > startRotate - maxRotate) && (Input.touches[0].position.x <= Screen.width / 2))
        //         {
        //             glasses.transform.Rotate(Vector3.up, -rotateSpeed, Space.World);
        //         }
        //         else if ((glasses.transform.eulerAngles.y < startRotate + maxRotate) && (Input.touches[0].position.x > Screen.width / 2))
        //         {
        //             glasses.transform.Rotate(Vector3.up, +rotateSpeed, Space.World);
        //         }
        //     }
        // }
        if (isDown)
        {
            glasses.transform.Rotate(Vector3.down, rotateSpeed, Space.World);
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
