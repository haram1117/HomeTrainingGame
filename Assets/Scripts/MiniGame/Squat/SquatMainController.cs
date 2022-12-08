using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquatMainController : MonoBehaviour
{
    //0 for ready, 1 for playing, 2 for end
    public static int mode;
    public SquatPlayer player; // 현재 게임을 진행중인 플레이어.
    public List<GameObject> stages; // 스테이지들을 관리하는 리스트
    public Text screenText;
    public Text scoreText;
    public GameObject panel;
    public GameObject gameFinBtn;
    public VNectBarracudaRunner runner;
    public int damage;
    public PhotonView PV;

    private int _curStageNo; // 현재 클리어중인 스테이지 넘버
    private int _hardness; // 각스테이지별 채력
    private int _n_squatted;
    
    private float time;
    private float time_limit;
    private float fin_time;
    private float ready_time = 10;
    private WifiSessionController wifiController;

    // Start is called before the first frame update
    private void Start()
    {
        wifiController = WifiSessionController.getInstance();
        DontDestroyOnLoad(this.gameObject);
        gameFinBtn.SetActive(false);
        mode = 0;
        screenText.text = "스쿼트를 준비해 주세요.";
        time_limit = 200;
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
                if (wifiController.isEventExist())
                    damage *= 2;
                DigAttack(damage);
            }
        }

        if (mode == 2)
        {
            return;
        }
    }
    
    //Game Finished;
    private void EndGame()
    {
        for(int i =0; i<stages.Count; i++)
        {
            stages[i].SetActive(false);
        }
        panel.SetActive(true);
        runner.enabled= false;
        gameFinBtn.SetActive(true);
        scoreText.text = "게임끝! 걸린시간은 " + fin_time;
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
            EndGame();
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

    public float GetPlayerScore()
    {
        return time_limit - fin_time;
    }
    
    /// <summary>
    /// 게임이 끝나고 누를 버튼.
    /// </summary>
    public void FinishBtnOnClick()
    {
        PV.RPC("SetScore", RpcTarget.All);
        PV.RPC("LoadBoardGame", RpcTarget.MasterClient);
        this.Invoke(() => PV.RPC("ShowResult", RpcTarget.All), 1.0f);
    }

    [PunRPC]
    private void SetScore()
    {
        float score = GetPlayerScore();
        GameManager.Instance.localPlayer.lastScore = score;
    }

    [PunRPC]
    private void LoadBoardGame()
    {
        PhotonNetwork.LoadLevel("MainBoardGame");
    }

    [PunRPC]
    private void ShowResult()
    {
        BoardGameUIManager uiManager = GameObject.Find("MainCanvas").GetComponent<BoardGameUIManager>();
        uiManager.ResultUIOpen();
        Destroy(this.gameObject);
    }
}
