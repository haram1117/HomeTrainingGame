using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    // TODO: 플레이어 점프 이용한 스테이지 간 이동 구현
    /// <summary>
    /// 보유 중인 Star 개수
    /// </summary>
    private int starCount;

    /// <summary>
    /// 보유 중인 Gold 개수
    /// </summary>
    private int goldCount;

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

    private PlayerState state = PlayerState.Waiting;
    private static Player instance = null;
    private bool oneFinished;

    [SerializeField] private Rail rail;
    private int currentSeg = 0;
    private float transition;
    private bool isJumpingCompleted;
    [SerializeField] private float speed = 2.5f;
    
    
    private static readonly int JumpEndTrigger = Animator.StringToHash("JumpEndTrigger");
    private static readonly int JumpTrigger = Animator.StringToHash("JumpTrigger");

    public static Player Instance
    {
        get
        {
            if (null == instance)
                return null;
            return instance;
        }
    }

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            animator = GetComponentInChildren<Animator>();
        }
        else
        {
            Destroy(this.gameObject);
        }
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
        animator.SetTrigger(JumpTrigger);
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
        float m;
        m = currentSeg == rail.nodes.Length - 1 ? (rail.nodes[0].position - rail.nodes[currentSeg].position).magnitude : (rail.nodes[currentSeg + 1].position - rail.nodes[currentSeg].position).magnitude;
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
                     animator.SetTrigger(JumpEndTrigger);
                     if (stageLeft > 0)
                     {
                         Invoke(nameof(MoveAgain), 0.5f);
                     }
                     else
                     {
                         // 턴 종료 flag
                         isJumpingCompleted = true;
                         BoardGameManager.Instance.SetNextPlayer();
                         BoardGameManager.Instance.DoNextTurn();
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

    private void MoveAgain()
    {
        animator.SetTrigger(JumpTrigger);
        state = PlayerState.Moving;
    }
}
