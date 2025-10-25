using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    [Header("プレイヤーPrefab")]
    public GameObject player1Prefab;
    public GameObject player2Prefab;

    [Header("スポーン位置")]
    public Transform player1Spawn;
    public Transform player2Spawn;

    [Header("カメラ")]
    public Camera camera1;
    public Camera camera2;

    [Header("Virtual Cameras")]
    public CinemachineVirtualCamera vcam1;
    public CinemachineVirtualCamera vcam2;

    [Header("UI")]
    public GameObject centerLine;    // Canvas 内の中央線
    public GameObject chargeBarSolo; // ソロ用チャージバー
    public GameObject chargeBarP1;   // デュオ用プレイヤー1チャージバー
    public GameObject chargeBarP2;   // デュオ用プレイヤー2チャージバー

    void Start()
    {
        int playerCount = PlayerPrefs.GetInt("PlayerCount", 1);

        // Player1生成
        GameObject p1 = Instantiate(player1Prefab, player1Spawn.position, Quaternion.identity);
        vcam1.Follow = p1.transform;

        // Player1 の ChargeJump1P にチャージバーとスポーン位置をセット
        ChargeJump1P jump1 = p1.GetComponent<ChargeJump1P>();
        jump1.respawnPosition = player1Spawn.position;

        if (playerCount == 2)
        {
            jump1.chargeBar = chargeBarP1.GetComponent<CircularProgressBar>();

            // Player2生成
            GameObject p2 = Instantiate(player2Prefab, player2Spawn.position, Quaternion.identity);
            vcam2.Follow = p2.transform;

            ChargeJump2P jump2 = p2.GetComponent<ChargeJump2P>();
            jump2.chargeBar = chargeBarP2.GetComponent<CircularProgressBar>();
            jump2.respawnPosition = player2Spawn.position;

            // 画面分割
            camera1.rect = new Rect(0, 0.5f, 1, 0.5f);
            camera2.rect = new Rect(0, 0, 1, 0.5f);
            camera2.gameObject.SetActive(true);

            if (centerLine != null) centerLine.SetActive(true);

            if (chargeBarSolo != null) chargeBarSolo.SetActive(false);
            if (chargeBarP1 != null) chargeBarP1.SetActive(true);
            if (chargeBarP2 != null) chargeBarP2.SetActive(true);
        }
        else
        {
            jump1.chargeBar = chargeBarSolo.GetComponent<CircularProgressBar>();

            camera1.rect = new Rect(0, 0, 1, 1);
            camera2.gameObject.SetActive(false);

            if (centerLine != null) centerLine.SetActive(false);

            if (chargeBarSolo != null) chargeBarSolo.SetActive(true);
            if (chargeBarP1 != null) chargeBarP1.SetActive(false);
            if (chargeBarP2 != null) chargeBarP2.SetActive(false);
        }
    }
}
