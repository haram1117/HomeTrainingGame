using System;
using UnityEngine;

public class PlankMainController : MonoBehaviour
{
    public WifiSessionController wifiController;
    public PlankPlayer player; // 현재 게임을 진행중인 플레이어.
    public GameObject shield;
    public GameObject hairband;
    
    private float time;

    private long score;

    private bool isStart;
    private bool doMore;
    public void Start()
    {
        isStart = false;
        doMore = false;
    }

    public void Update()
    {
        int posScore = player.GetPositionScore();

        if (!isStart && posScore > 4 && doMore)
        {
            isStart = true;
        }
        
        //TODO : unity Chan 의 헤어벤드를 가지고 쉴드를 트래킹 하길 원함. 
        if (posScore > 0)
        {
            print(shield.transform.position);
            shield.transform.position = hairband.transform.position + Vector3.up * 0.01f;
            shield.SetActive(true);
        }
        else
        {
            isStart = false;
            doMore = false;
        }
        if(isStart)
        {
            time += Time.deltaTime;
        }
        score += posScore;
        Debug.Log(score);
    }

    public long getPlayerScore()
    {
        return score;
    }

    public float getPlayerTime()
    {
        return time;
    }
    
}
