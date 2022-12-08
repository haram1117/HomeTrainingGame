using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TestMinigame : MonoBehaviour
{
    public PhotonView PV;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void FinishGame()
    {
        PV.RPC("SetScore", RpcTarget.All);
        PV.RPC("LoadBoardGame", RpcTarget.MasterClient);
        this.Invoke(() => PV.RPC("ShowResult", RpcTarget.All), 0.5f);
    }

    [PunRPC]
    private void SetScore()
    {
        float score = Random.Range(0, 101);
        GameManager.Instance.localPlayer.lastScore = score;
    }

    [PunRPC]
    private void LoadBoardGame()
    {
        PhotonNetwork.LoadLevel("MainBoardGame");
    }

    [PunRPC]
    private void ShowResult()
    {
        BoardGameUIManager uiManager = GameObject.Find("MainCanvas").GetComponent<BoardGameUIManager>();
        uiManager.ResultUIOpen();
        Destroy(this.gameObject);
    }
}
