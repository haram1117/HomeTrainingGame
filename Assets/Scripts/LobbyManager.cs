using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

// 마스터(매치 메이킹) 서버와 룸 접속을 담당
public class LobbyManager : MonoBehaviourPunCallbacks {
    private const byte maxPlayer = 2;

    public Text connectionInfoText; // 네트워크 정보를 표시할 텍스트
    public Button joinButton; // 룸 접속 버튼

    // 게임 실행과 동시에 마스터 서버 접속 시도
    private void Start() {
        PhotonNetwork.ConnectUsingSettings();

        joinButton.interactable = false;
        connectionInfoText.text = "마스터 서버에 접속 중...";
    }

    // 마스터 서버 접속 성공시 자동 실행
    public override void OnConnectedToMaster() {
        joinButton.interactable = true;
        connectionInfoText.text = "온라인 : 마스터 서버와 연결됨";
    }

    // 마스터 서버 접속 실패시 자동 실행
    public override void OnDisconnected(DisconnectCause cause) {
        joinButton.interactable = false;
        connectionInfoText.text = "오프라인 : 마스터 서버와 연결되지 않음\n접속 재시도...";

        PhotonNetwork.ConnectUsingSettings();
    }

    // 룸 접속 시도
    public void Connect() {
        joinButton.interactable = false;

        if (PhotonNetwork.IsConnected)
        {
            connectionInfoText.text = "방에 접속 중...";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            connectionInfoText.text = "오프라인 : 마스터 서버와 연결되지 않음\n접속 재시도...";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // (빈 방이 없어)랜덤 룸 참가에 실패한 경우 자동 실행
    public override void OnJoinRandomFailed(short returnCode, string message) {
        connectionInfoText.text = "빈 방이 없음, 새로운 방 생성...";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayer });
    }

    // 룸에 참가 완료된 경우 자동 실행
    public override void OnJoinedRoom() {
        connectionInfoText.text = "방 참가 성공\n현재 인원 " + PhotonNetwork.CurrentRoom.PlayerCount + " / " + maxPlayer;

        if (PhotonNetwork.CurrentRoom.PlayerCount == maxPlayer)
        {
            PhotonNetwork.LoadLevel("MainBoardGame");
        }
    }

    // 룸에 다른 플레이어가 참가한 경우 자동 실행
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == maxPlayer)
        {
            PhotonNetwork.LoadLevel("MainBoardGame");
        }
    }
}