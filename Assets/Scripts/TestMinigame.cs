using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMinigame : MonoBehaviour
{
    public PhotonView PV;

    public void FinishGame()
    {
        PV.RPC("LoadBoardGame", RpcTarget.MasterClient);
    }

    [PunRPC]
    private void LoadBoardGame()
    {
        PhotonNetwork.LoadLevel("MainBoardGame");
    }
}
