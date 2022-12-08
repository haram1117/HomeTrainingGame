using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour, IPunObservable
{
    /// <summary>
    /// 현재 star의 위치 (0 ~ 15)
    /// </summary>
    public int starIndex;

    /// <summary>
    /// 현재 star level index (gold value)
    /// </summary>
    public int starLevelIndex;

    private static Star instance = null;

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

    public static Star Instance
    {
        get
        {
            if (null == instance)
                return null;
            return instance;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(starIndex);
            stream.SendNext(starLevelIndex);
        }
        else
        {
            starIndex = (int)stream.ReceiveNext();
            starLevelIndex = (int)stream.ReceiveNext();
        }
    }
}
