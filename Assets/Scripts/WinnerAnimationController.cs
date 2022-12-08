using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinnerAnimationController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject girl;
    [SerializeField] private GameObject boy;

    void Start()
    {
        Player[] players = GameManager.Instance.GetWinnerAndLoser();
        if(players[0].characterType == CharacterType.Boy)
        {
            boy.GetComponent<Animator>().SetTrigger("WinTrigger");
            girl.GetComponent<Animator>().SetTrigger("LoseTrigger");
        }
        else
        {
            boy.GetComponent<Animator>().SetTrigger("LoseTrigger");
            girl.GetComponent<Animator>().SetTrigger("WinTrigger");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
