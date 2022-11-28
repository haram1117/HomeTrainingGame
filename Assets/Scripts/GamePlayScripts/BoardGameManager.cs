using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardGameManager : MonoBehaviour
{
    private static BoardGameManager instance = null;

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

    public static BoardGameManager Instance
    {
        get
        {
            if (null == instance)
                return null;
            return instance;
        }
    }

    int SetDiceValue()
    {
        int value = Random.Range(1, 7);
        return value;
    }
}
