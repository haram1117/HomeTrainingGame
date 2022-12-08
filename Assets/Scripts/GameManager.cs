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

    public int remainTurn = 1;

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

    public Player[] GetWinnerAndLoser()
    {
        if (localPlayer.GetNowStar() == otherPlayer.GetNowStar())
        {
            if (localPlayer.GetNowGold() >= otherPlayer.GetNowGold())
            {
                return new Player[] { localPlayer, otherPlayer };
            }
            else
            {
                return new Player[] { otherPlayer, localPlayer };
            }
        }
        else
        {
            if (localPlayer.GetNowStar() > otherPlayer.GetNowStar())
            {
                return new Player[] { localPlayer, otherPlayer };
            }
            else
            {
                return new Player[] { otherPlayer, localPlayer };
            }
        }
        
    }
}
