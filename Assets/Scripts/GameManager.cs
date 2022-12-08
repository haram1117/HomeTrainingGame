using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PhotonView PV;

    public Player localPlayer;
    public Player otherPlayer;

    public int remainTurn = 10;

    private static GameManager instance = null;

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

    public static GameManager Instance
    {
        get
        {
            if (null == instance)
                return null;
            return instance;
        }
    }

    public void FinishGame()
    {
        PV.RPC("LoadWinnerScene", RpcTarget.MasterClient);
    }

    [PunRPC]
    public void LoadWinnerScene()
    {
        PhotonNetwork.LoadLevel("Winner");
    }
}
