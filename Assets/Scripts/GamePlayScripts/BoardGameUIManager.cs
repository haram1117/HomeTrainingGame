using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BoardGameUIManager : MonoBehaviour
{    
    //TODO: 제스처 인식 연결 및 hand point를 사용한 마우스 포인터 구현
    //TODO: MainMenu UI 및 서버 연동 구현
    
    [Header("주사위 관련 UI")]
    [SerializeField] private GameObject dicePanel;
    [SerializeField] private Button diceRollBtn;
    [SerializeField] private Image[] diceImages;
    private bool isDiceRolling;
    private static readonly int CloseTrigger = Animator.StringToHash("CloseTrigger");

    [Header("스타 관련 UI")]
    [SerializeField] private GameObject starPanel;
    [SerializeField] private Button starGetBtn;
    [SerializeField] private TextMeshProUGUI goldNeedText;
    [SerializeField] private TextMeshProUGUI goldHaveText;
    [SerializeField] private TextMeshProUGUI warningMessage;
    [SerializeField] private TextMeshProUGUI completeMessage;


    [Header("플레이어 스탯")] 
    [SerializeField] private TextMeshProUGUI goldNum;
    [SerializeField] private TextMeshProUGUI starNum;
    
    private void Awake()
    {
        diceRollBtn.onClick.AddListener(DiceRoll);
        starGetBtn.onClick.AddListener(GetStar);
        starNum.text = Player.Instance.GetNowStar().ToString();
        goldNum.text = Player.Instance.GetNowGold().ToString();
    }

    private void Update()
    {
    }

    /// <summary>
    /// 주사위 패널 UI Open
    /// </summary>
    public void DiceUIOpen()
    {
        dicePanel.SetActive(true);
        diceRollBtn.gameObject.SetActive(true);
    }

    /// <summary>
    /// 주사위 Roll 시작
    /// </summary>
    private void DiceRoll()
    {
        if (!isDiceRolling)
        {
            isDiceRolling = true;
            diceRollBtn.gameObject.SetActive(false);
            StartCoroutine(DiceValueRoll());
        }
    }

    /// <summary>
    /// DiceRoll Animation을 위한 IEnumerator
    /// </summary>
    /// <returns></returns>
    private IEnumerator DiceValueRoll()
    {
        int value = -1;
        int lastIndex = -1;
        for (int i = 0; i < 30; i++)
        {
            value = Random.Range(0, 6);
            while (lastIndex == value)
                value = Random.Range(0, 6);
            diceImages[value].gameObject.SetActive(true);
            if(lastIndex != -1)
                diceImages[lastIndex].gameObject.SetActive(false);
            lastIndex = value;
            
            yield return new WaitForSeconds(0.05f);
        }

        isDiceRolling = false;
        BoardGameManager.Instance.SetDiceValue(value);
        Invoke(nameof(DiceUIClose), 0.3f);
    }

    /// <summary>
    /// 주사위 패널 UI Close
    /// </summary>
    private void DiceUIClose()
    {
        foreach (var elem in diceImages)
        {
            elem.gameObject.SetActive(false);
        }
        Animator animator = dicePanel.GetComponent<Animator>();
        animator.SetTrigger(CloseTrigger);
        this.Invoke(() => dicePanel.SetActive(false), 0.5f);
    }

    /// <summary>
    /// star UI Open
    /// </summary>
    public void StarUIOpen()
    {
        starPanel.SetActive(true);
        goldNeedText.text = $"필요한 골드 : {BoardGameManager.Instance.GetGoldValueForStar()}";
        goldHaveText.text = $"보유한 골드 : {Player.Instance.GetNowGold()}";
    }

    /// <summary>
    /// Get Star
    /// </summary>
    private void GetStar()
    {
        if (Player.Instance.CanGetStar(out var playerGold))
        {
            completeMessage.gameObject.SetActive(true);
            starNum.text = Player.Instance.GetNowStar().ToString();
            goldNum.text = playerGold.ToString();
            goldHaveText.text = $"보유한 골드 : {Player.Instance.GetNowGold()}";
            this.Invoke(()=>completeMessage.gameObject.SetActive(false), 0.5f);
        }
        else
        {
            warningMessage.gameObject.SetActive(true);
            this.Invoke(()=>warningMessage.gameObject.SetActive(false), 0.5f);
        }
        Animator animator = starPanel.GetComponent<Animator>();
        animator.SetTrigger(CloseTrigger);
        this.Invoke(()=>starPanel.gameObject.SetActive(false), 0.8f);
        this.Invoke(()=>Player.Instance.TurnFinish(), 1.0f);
    }
}

/// <summary>
/// Lambda 함수 Invoke를 위한 Utility
/// </summary>
public static class Utility
{
    public static void Invoke(this MonoBehaviour mb, Action f, float delay)
    {
        mb.StartCoroutine(InvokeRoutine(f, delay));
    }
 
    private static IEnumerator InvokeRoutine(System.Action f, float delay)
    {
        yield return new WaitForSeconds(delay);
        f();
    }
}