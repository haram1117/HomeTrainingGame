using Photon.Pun;
using Photon.Realtime;
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

    [SerializeField] private Player localPlayer;

    [SerializeField] private Player OtherPlayer;

    [SerializeField] private Transform[] stages;

    [SerializeField] private PhotonView PV;

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

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            stages = new Transform[16];
            for (int i = 0; i < 16; i++)
            {
                stages[i] = GameObject.Find($"Stage{i}").transform;
            }

            localPlayer = GameManager.Instance.localPlayer;
            OtherPlayer = GameManager.Instance.otherPlayer;

            localPlayer.SetRail();
            OtherPlayer.SetRail();

            // 스타가 없을 경우 생성, 룸 오브젝트이므로 마스터 클라이언트
            if (null == Star.Instance && PhotonNetwork.IsMasterClient)
            {
                StarRandomGenerate();
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        SetActiveObjects(true);
    }

    public void SetActiveObjects(bool flag)
    {
        localPlayer.gameObject.SetActive(flag);
        OtherPlayer.gameObject.SetActive(flag);
        if (Star.Instance)
        {
            Star.Instance.gameObject.SetActive(flag);
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
    /// 다음 사람 턴 시작 
    /// </summary>
    public void DoNextTurn()
    {
        // 다음 턴이 마스터 클라이언트인 경우
        if (!PhotonNetwork.IsMasterClient)
        {
            // 마스터 클라이언트에게 미니게임 로드를 요청
            this.Invoke(() => PV.RPC("LoadMinigame", RpcTarget.All), 1.0f);
        }
        else
        {
            // 상대방 local에서 dice panel 열림
            PV.RPC("DiceUIOpen", RpcTarget.Others);
        }
    }

    [PunRPC]
    private void LoadMinigame()
    {
        GameManager.Instance.remainTurn--;

        SetActiveObjects(false);

        if (PhotonNetwork.IsMasterClient)
        {
            //PhotonNetwork.LoadLevel("TestMinigame");
            if (GameManager.Instance.remainTurn % 2 == 1)
            {
                PhotonNetwork.LoadLevel("Plank");
            }
            else
            {
                PhotonNetwork.LoadLevel("Squat");
            }
        }
    }

    [PunRPC]
    private void DiceUIOpen()
    {
        uiManager.DiceUIOpen();
    }

    public void StarGenerateRequet()
    {
        PV.RPC("StarRandomGenerate", RpcTarget.MasterClient);
    }

    [PunRPC]
    private void StarRandomGenerate()
    {
        int prevLevel = -1;
        if (Star.Instance)
        {
            prevLevel = Star.Instance.starLevelIndex;
            PhotonNetwork.Destroy(Star.Instance.gameObject);
        }
        int randomStage = Random.Range(1, 16);
        Vector3 starLocation = new Vector3(stages[randomStage].position.x, stages[randomStage].position.y + 5.0f,
            stages[randomStage].position.z);
        this.Invoke(() => StarGenerate(starLocation, randomStage, prevLevel + 1), 1.0f);
    }

    private void StarGenerate(Vector3 starLocation, int starIndex, int starLevelIndex)
    {
        PhotonNetwork.InstantiateRoomObject("Prefabs/SoftStar", starLocation, Quaternion.Euler(-90f, 0f, 0f));
        Star.Instance.starIndex = starIndex;
        Star.Instance.starLevelIndex = starLevelIndex;
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
        return starLevel[Star.Instance.starLevelIndex];
    }
}
