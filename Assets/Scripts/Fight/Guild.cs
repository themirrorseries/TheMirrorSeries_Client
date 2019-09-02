using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guild : MonoBehaviour
{
    private Coroutine guildCoroutine;
    private float startPosY = 330f;
    private float endPosY = 280f;
    private float curPosY = 0;
    private Vector3 positionVec3 = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        curPosY = startPosY;
        positionVec3 = gameObject.GetComponent<RectTransform>().localPosition;

    }
    public void StartGuild()
    {
        guildCoroutine = StartCoroutine(GuildMove());
    }
    public void StopGuild()
    {
        StopCoroutine(guildCoroutine);
        gameObject.SetActive(false);
    }
    private IEnumerator GuildMove()
    {
        while (true)
        {
            while (curPosY > endPosY)
            {
                --curPosY;
                gameObject.GetComponent<RectTransform>().localPosition = new Vector3(positionVec3.x, curPosY, positionVec3.z);
                yield return new WaitForSeconds(0.01f);
            }
            while (curPosY < startPosY)
            {
                ++curPosY;
                gameObject.GetComponent<RectTransform>().localPosition = new Vector3(positionVec3.x, curPosY, positionVec3.z);
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}
