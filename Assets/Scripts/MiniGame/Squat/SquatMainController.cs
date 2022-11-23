using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquatMainController : MonoBehaviour
{
    public WifiSessionController wifiController;
    public SquatPlayer player;
    public List<Object> stages;
    private int _curStageNo;
    private int _hardness;
    // Start is called before the first frame update
    private void Start()
    {
        _curStageNo = 0;
        // TODO : Provide Character to initial location
        // TODO : Setting camera, 
    }

    private void update()
    {
        if (player.didSquat())
        {
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
        // TODO : 스테이지 설정하기.
    }

    public int result()
    {
        // TODO : return the score of played mini game
        return _curStageNo;
    }
}
