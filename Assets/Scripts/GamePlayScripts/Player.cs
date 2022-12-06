using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum CharacterType
{
    Boy, Girl
}
public class Player : MonoBehaviour, IPunObservable
{
    // TODO: 플레이어 점프 이용한 스테이지 간 이동 구현
    /// <summary>
    /// 보유 중인 Star 개수
    /// </summary>
    private int starCount = 0;

    /// <summary>
    /// 보유 중인 Gold 개수
    /// </summary>
    private int goldCount = 40;

    /// <summary>
    /// 이번 턴에서 수행한 Dice Rolling Value
    /// </summary>
    private int nowDiceValue;

    /// <summary>
    /// 한 턴에서 목표하는 stage num
    /// </summary>
    private int destStageNum;

    /// <summary>
    /// 현재 속해있는 stage의 number(0 ~ 15)
    /// </summary>
    private int stageNum = 0;

    /// <summary>
    /// player가 가야할 stage가 남음
    /// </summary>
    private int stageLeft;

    /// <summary>
    /// Player Mesh
    /// </summary>
    [SerializeField] private Transform playerMeshTransform;
    
    /// <summary>
    /// player Animator
    /// </summary>
    private Animator animator;

    /// <summary>
    /// Instantiate에 사용할 boyPrefab
    /// </summary>
    [SerializeField] private GameObject boyPrefab;

    /// <summary>
    /// Instantiate에 사용할 girlPrefab
    /// </summary>
    [SerializeField] private GameObject girlPrefab;

    /// <summary>
    /// Player의 캐릭터 타입
    /// </summary>
    [SerializeField] private CharacterType characterType;

    /// <summary>
    /// Player의 Photon View 컴포넌트
    /// </summary>
    [SerializeField] private PhotonView PV;

    /// <summary>
    /// 주사위 후 최종 목표위치가 star 위치일 때 true
    /// </summary>
    private bool canGetStar;
    
    private PlayerState state = PlayerState.Waiting;
    private bool oneFinished;

    [SerializeField] private Rail rail;
    private int currentSeg = 0;
    private float transition;
    private bool isJumpingCompleted;
    [SerializeField] private float speed = 2.5f;
    
    
    private static readonly int JumpEndTrigger = Animator.StringToHash("JumpEndTrigger");
    private static readonly int JumpTrigger = Animator.StringToHash("JumpTrigger");

    private void Awake()
    {
        SetCharacterType();
        DontDestroyOnLoad(this.gameObject);

        if (!PV.IsMine)
        {
            Destroy(transform.Find("MainCamera").gameObject);
        }
    }

    private void Start()
    {
        if (characterType == CharacterType.Boy)
        {
            GameObject character = Instantiate(boyPrefab, Vector3.zero, Quaternion.identity);
            character.transform.SetParent(transform.GetChild(0).transform, false);
            playerMeshTransform = character.transform;
        }
        else
        {
            GameObject character = Instantiate(girlPrefab, Vector3.zero, Quaternion.identity);
            character.transform.SetParent(transform.GetChild(0).transform, false);
            playerMeshTransform = character.transform;
        }
        animator = GetComponentInChildren<Animator>();
        rail = GameObject.Find($"RailFor{characterType}").GetComponent<Rail>();

        transform.position = rail.nodes[0].position;
        transform.rotation = rail.nodes[0].rotation;
    }

    private void Update()
    {
        if (!rail)
            return;
        if (!isJumpingCompleted && state == PlayerState.Moving && stageLeft > 0)
        {
            CharacterMove();
        }
    }

    /// <summary>
    /// 주사위 값에 따라 Player Stage 이동
    /// </summary>
    /// <param name="num">Dice Index (Dice Value: num + 1)</param>
    public void MoveCharacterWithStageNum(int num)
    {
        isJumpingCompleted = false;
        // animator.SetTrigger(JumpTrigger);
        PV.RPC("SetTriggerRPC", RpcTarget.All, JumpTrigger);
        state = PlayerState.Moving;
        nowDiceValue = num + 1;
        destStageNum = stageNum + nowDiceValue - 1;
        stageLeft = nowDiceValue;
    }

    /// <summary>
    /// Character 한 칸 씩 이동
    /// </summary>
    private void CharacterMove()
    {
        var m = currentSeg == rail.nodes.Length - 1 ? (rail.nodes[0].position - rail.nodes[currentSeg].position).magnitude : (rail.nodes[currentSeg + 1].position - rail.nodes[currentSeg].position).magnitude;
        float s = (Time.deltaTime * 1 / m) * speed;
        transition += s;
        
        if (transition > 1)
        {
            transition = 0;
            currentSeg++;
            if (currentSeg == rail.nodes.Length)
            {
                currentSeg = 0;
            }
            else
            {
                // 한 칸 전진
                if (currentSeg % 2 == 0)
                {
                    stageLeft--;
                    state = PlayerState.Waiting;
                    // animator.SetTrigger(JumpEndTrigger);
                    PV.RPC("SetTriggerRPC", RpcTarget.All, JumpEndTrigger);
                    if (stageLeft > 0)
                    {
                        Invoke(nameof(MoveAgain), 0.5f);
                    }
                    else 
                    {
                        // 플레이어가 가야하는 마지막 칸이 Star가 있는 칸인 경우
                        if ((int)Math.Truncate((double)currentSeg / 2) == BoardGameManager.Instance.starIndex)
                        {
                            BoardGameManager.Instance.PlayerGetStar();
                        }
                        else
                        {
                            // // 턴 종료 flag
                            // isJumpingCompleted = true;
                            // BoardGameManager.Instance.SetNextPlayer();
                            // BoardGameManager.Instance.DoNextTurn();
                            TurnFinish();
                        }
                    }
                }
            }
        }
        else if (transition < 0)
        {
            transition = 1;
            currentSeg--;
            if (currentSeg == -1)
            {
                currentSeg = rail.nodes.Length - 2;
            }
        }

        if (currentSeg != rail.nodes.Length - 1)
        {
            transform.position = rail.CatmullPosition(currentSeg, transition);
            transform.rotation = rail.Orientation(currentSeg, transition);
            playerMeshTransform.localPosition = Vector3.zero;
            playerMeshTransform.localRotation = Quaternion.identity;
        }
    }                                                                                                      

    /// <summary>
    /// 다음 칸으로 말을 옮김
    /// </summary>
    private void MoveAgain()
    {
        // animator.SetTrigger(JumpTrigger);
        PV.RPC("SetTriggerRPC", RpcTarget.All, JumpTrigger);
        state = PlayerState.Moving;
    }

    /// <summary>
    /// 해당 재화를 구매할 수 있는 지 
    /// </summary>
    /// <returns></returns>
    public bool CanGetStar(out int gold)
    {
        int requireGoldValue = BoardGameManager.Instance.GetGoldValueForStar();
        if (goldCount >= requireGoldValue)
        {
            goldCount -= requireGoldValue;
            gold = goldCount;
            starCount++;
            return true;
        }
        gold = goldCount;
        return false;
    }

    public int GetNowGold()
    {
        return goldCount;
    }

    public int GetNowStar()
    {
        return starCount;
    }

    public void TurnFinish()
    {
        // 턴 종료 flag
        isJumpingCompleted = true;
        BoardGameManager.Instance.SetNextPlayer();
        BoardGameManager.Instance.DoNextTurn();
    }

    private void SetCharacterType()
    {
        GameObject lobbyManager = GameObject.Find("LobbyManager");
        if (lobbyManager != null)
        {
            characterType = lobbyManager.GetComponent<LobbyManager>().selectedCharcter;
            Destroy(lobbyManager);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(characterType);
        }
        else
        {
            characterType = (CharacterType)stream.ReceiveNext();
        }
    }

    [PunRPC]
    private void SetTriggerRPC(int Trigger)
    {
        animator.SetTrigger(Trigger);
    }
}
