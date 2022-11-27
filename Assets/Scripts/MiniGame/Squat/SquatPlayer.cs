    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquatPlayer : MonoBehaviour
{

    public int damage;
    public string name;
    public string characterId;

    private bool _sit;
    private bool _didSquat;
    
    // 이전 didSquat 함수 호출 시기와 비교해서 스쿼트를 진행완료했는지 여부를 리턴합니다.
    public bool didSquat()
    {
        bool temp = _didSquat;
        _didSquat = false;
        return temp;
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        _didSquat = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (VNectModel.positions[3] < 60 && !_sit)
        {
            _sit = true;
        }
        if (VNectModel.positions[3] > 73 && _sit)
        {
            _sit = false;
            _didSquat = true;
        }
    }
    
}
