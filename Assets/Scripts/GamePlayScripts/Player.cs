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
    private bool isMoving;
    private PlayerState state = PlayerState.Waiting;
    private static Player instance = null;
    private bool isJumping = false;
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
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if (!isJumping && state == PlayerState.Moving && isMoving)
        {
            var v = BoardGameManager.Instance.GetStageLocation(stageNum + 1).GetChild(0).position;
            StartCoroutine(DotheJump(v, 1.5f));
        }
    }

    /// <summary>
    /// 주사위 값에 따라 Player Stage 이동
    /// </summary>
    /// <param name="num">Dice Index (Dice Value: num + 1)</param>
    public void MoveCharacterWithStageNum(int num)
    {
        state = PlayerState.Moving;
        nowDiceValue = num + 1;
        destStageNum = stageNum + nowDiceValue - 1;
        isMoving = true;
        // for (int i = 0; i < nowDiceValue; i++)
        // {
        //     CharacterMove();
        // }
    }

    /// <summary>
    /// Character 한 칸 씩 이동
    /// </summary>
    private void CharacterMove()
    {
        //TODO: 더 사실적으로 점프 구현 가능 -> https://www.forrestthewoods.com/blog/solving_ballistic_trajectories/
        
    }

    /// <summary>
    /// Player의 stage 이동에 Jump 수행
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator DotheJump(Vector3 destination, float time)
    {
        // TODO: 점프 Location 재지정 필요, Rotation도 Lerp시키면서 이동할 수 있도록
        isJumping = true;
        var timer = 0.0f;
        var start = transform.position;
        var dist = (start - destination).magnitude;
        var maxHeight = dist * 0.5f; // heightVsDistanceFactor = 0.5f
        Vector3 axis;
        var doSpin = (dist >= 3.0f); // minDistForFlip = 3.0f
        // if (doSpin)
        // {
        //     axis = Vector3.Cross(start - destination, Vector3.up);
        // }

        while (timer <= time) {
     
            var vT = Vector3.Lerp(start, destination, timer/time);
            vT.y = Mathf.Sin(Mathf.PI * timer/time) * maxHeight;
            transform.position = vT;
            
            // if (doSpin) {
            //     transform.rotation = Quaternion.AngleAxis(360.0f * timer/time, axis);
            // }
            timer += Time.deltaTime;
            yield return null;
        }
     
        // transform.rotation = Quaternion.identity;
        transform.position = destination;
        if (destStageNum < stageNum)
            isMoving = false;
        else
        {
            stageNum++;
            isJumping = false;
        }
    }
}
