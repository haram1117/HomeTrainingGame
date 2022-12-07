using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquatMainController : MonoBehaviour
{
    //0 for ready, 1 for playing, 2 for end
    public static int mode;
    public WifiSessionController wifiController;
    public SquatPlayer player; // 현재 게임을 진행중인 플레이어.
    public List<GameObject> stages; // 스테이지들을 관리하는 리스트
    public Text screenText;
    public Text scoreText;
    public GameObject panel;
    
    private int _curStageNo; // 현재 클리어중인 스테이지 넘버
    private int _hardness; // 각스테이지별 채력
    private int _n_squatted;
    
    private float time;
    private float time_limit;
    private float fin_time;
    private float ready_time = 10;

    // Start is called before the first frame update
    private void Start()
    {
        mode = 0;
        screenText.text = "스쿼트를 준비해 주세요.";
        time_limit = 40 * 1000;
        _curStageNo = 0;
        _hardness = 10;
        time = 0;
        fin_time = 987654321;
        _n_squatted = 0;
    }

    // 플레이어가 스쿼트를 했으면, Dig Attack 함수를 호출함.
    private void Update()
    {
        time += Time.deltaTime;
        if (time > ready_time && mode == 0)
        {
            mode = 1;
            Debug.Log("Game Started");
            time = 0;
            panel.SetActive(false);
            scoreText.text = "";
        }
        if(mode == 1)
        {
            
            scoreText.text = string.Format("시간 : {0:F1}", time) + "s";
            if (time > time_limit)
            {
                return;
            }

            if (player.didSquat())
            {
                print("squated");
                int damage = player.damage;
                if (wifiController.isEventExist())
                    damage *= 2;
                DigAttack(damage);
            }
        }

        if (mode == 2)
        {
            //Game Finished;
            
            for(int i =0; i<stages.Count; i++)
            {
                stages[i].SetActive(false);
            }
            panel.SetActive(true);
            scoreText.text = "게임끝! 걸린시간은 " + fin_time;
        }
    }

    // 현재 스테이지에 데미지를 입히고, 스테이지가 넘어갈 경우 다음 스테이지로 설정함.
    private bool DigAttack(int damage)  
    {
        _hardness -= damage;
        _n_squatted += 1;
        if (_hardness < 0)
        {
            GoDeeper();
            setStage();
            _hardness = 9;
            return true;
        }
        
        return false;
    }
    
    // 다음 스테이지로 변수들 변경
    private void GoDeeper()
    {
        _curStageNo++;
        if (_curStageNo == stages.Count)
        {
            fin_time = time;
            mode = 2;
            screenText.text = "Game Finished\nYour Score is " + time;
        }
        _hardness = _curStageNo * 2;
    }

    // 현재 스테이지 로드
    private void setStage()
    {
        if(_curStageNo != 0)
        {
            stages[(_curStageNo - 1)%stages.Count].SetActive(false);
            stages[_curStageNo%stages.Count].SetActive(true);
        }
    }

    private void disableAll()
    {
        for(int i =0; i<stages.Count; i++)
        {
            stages[i].SetActive(false);
        }
    }

    // 게임 결과를 리턴하는 함수.
    // TODO: 시간 측정을 만들어서 리턴해야할듯.
    public int result()
    {
        // TODO : return the score of played mini game
        return _curStageNo;
    }

    public float GetFinTime()
    {
        return fin_time;
    }
}
