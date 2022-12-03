using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

/// <summary>
/// Player의 현재 상태
/// </summary>
enum PlayerState
{
    Turn,
    Moving,
    Waiting
}
public class BoardGameManager : MonoBehaviour
{
    private static BoardGameManager instance = null;

    // TODO: 스타에 플레이어 도달 시 Gold 이용해서 사고 팔 수 있는 기능 (총 턴에 비례하는 골드필요량 지정)

    /// <summary>
    /// 현재 주사위를 던질 턴의 player id
    /// </summary>
    private int turnPlayerID;

    /// <summary>
    /// 현재 star의 위치 (0 ~ 15)
    /// </summary>
    public int starIndex;

    /// <summary>
    /// 현재 star level index (gold value)
    /// </summary>
    public int starLevelIndex;
    [SerializeField] private Player localPlayer;

    [SerializeField] private Transform[] stages;

    [SerializeField] private GameObject starPrefab;
    
    private int[] starLevel = { 10, 20, 30, 40, 50, 60, 70 };
    
    public static BoardGameManager Instance
    {
        get
        {
            if (null == instance)
                return null;
            return instance;
        }
    }
    [SerializeField] private BoardGameUIManager uiManager;

    private PlayerState[] playerStates;

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            playerStates = new PlayerState[] { PlayerState.Turn, PlayerState.Waiting}; // player 두 명으로 가정, host선 강제
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        stages = new Transform[16];
        for (int i = 0; i < 16; i++)
        {
            stages[i] = GameObject.Find($"Stage{i}").transform;
        }
        StarRandomGenerate();
        GameStart();
    }

    /// <summary>
    /// 게임 시작 시 실행 - Server 구축 및 JoinedRoom 완료 시
    /// </summary>
    private void GameStart()
    {
        if (playerStates[turnPlayerID] != PlayerState.Turn)
            return;
        else
        {
            uiManager.DiceUIOpen();
        }
    }

    /// <summary>
    /// 주사위 값 전달
    /// </summary>
    public void SetDiceValue(int value)
    {
        localPlayer.MoveCharacterWithStageNum(value);
    }

    /// <summary>
    /// 다움 턴 player ID 설정
    /// </summary>
    public void SetNextPlayer()
    {
        turnPlayerID = (turnPlayerID == 0) ? 1 : 0;
        if (turnPlayerID == 0) // TODO: 서로의 턴이 지나갔을 때 -> 미니게임 Start
        {
            
        }
    }

    /// <summary>
    /// 다음 사람 턴 시작 
    /// </summary>
    public void DoNextTurn()
    {
        uiManager.DiceUIOpen(); // TODO: RPC target.Others로 상대방 local에서 dice panel 열어지도록
    }

    private void StarRandomGenerate()
    {
        int randomStage = Random.Range(0, 16);
        Vector3 starLocation = new Vector3(stages[randomStage].position.x, stages[randomStage].position.y + 5.0f,
            stages[randomStage].position.z);
        Instantiate(starPrefab, starLocation, starPrefab.transform.localRotation);
        starIndex = randomStage;
    }

    /// <summary>
    /// 플레이어가 보유한 재화로 스타 구매
    /// </summary>
    public void PlayerGetStar()
    {
        // star UI
        uiManager.StarUIOpen();
       
    }

    /// <summary>
    /// 해당 스타를 구매하기 위해 필요한 재화 값
    /// </summary>
    public int GetGoldValueForStar()
    {
        return starLevel[starLevelIndex];
    }

    /// <summary>
    /// 다음 스타 레벨로 
    /// </summary>
    public void GoToNextLevelStar()
    {
        starLevelIndex++;
    }

}
