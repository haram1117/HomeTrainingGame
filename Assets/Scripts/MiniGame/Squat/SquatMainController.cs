using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquatMainController : MonoBehaviour
{
    public WifiSessionController wifiController;
    public SquatPlayer player;
    public List<GameObject> stages;
    private int _curStageNo;
    private int _hardness;
    // Start is called before the first frame update
    private void Start()
    {
        _curStageNo = 0;
        _hardness = 10;
        // TODO : Provide Character to initial location
        // TODO : Setting camera, 
    }

    private void Update()
    {
        if (player.didSquat())
        {
            print("squated");
            int damage = player.damage;
            if (wifiController.isEventExist())
                damage *= 2;
            DigAttack(damage);
        }
    }

    private bool DigAttack(int damage)  
    {
        _hardness -= damage;
        if (_hardness < 0)
        {
            GoDeeper();
            setStage();
            _hardness = 10;
            return true;
        }
        
        return false;
    }
    
    private void GoDeeper()
    {
        _curStageNo++;
        _hardness = _curStageNo * 2;
    }

    private void setStage()
    {
        if(_curStageNo != 0)
        {
            stages[_curStageNo - 1].SetActive(false);
            stages[_curStageNo].SetActive(true);
        }
    }

    public int result()
    {
        // TODO : return the score of played mini game
        return _curStageNo;
    }
}
