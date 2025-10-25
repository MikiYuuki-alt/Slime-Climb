using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Prime31.TransitionKit;

public class EscapeToTitle : MonoBehaviour
{
    [SerializeField] private GameObject uiCanvas; // ← ゲージなどUIのCanvasをセット
    private bool isTransitioning = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isTransitioning)
        {
            StartCoroutine(GoToTitle());
        }
    }

    private IEnumerator GoToTitle()
    {
        isTransitioning = true;

        // --- UIを一時的に非表示 ---
        if (uiCanvas != null)
            uiCanvas.SetActive(false);

        // --- 演出用デリゲート設定 ---
        SquaresTransition transition = new SquaresTransition
        {
            squareColor = Color.black,
            duration = 1.2f,                 // 演出の長さ（少し長めに）
            fadedDelay = 0.3f,               // 暗転後の待機時間
            squareSize = new Vector2(13f, 9f),
            smoothness = 0.5f,
            nextScene = SceneManager.GetSceneByName("TitleScene").buildIndex
        };

        // --- TransitionKitで演出開始 ---
        TransitionKit.instance.transitionWithDelegate(transition);

        // --- シーン非同期ロード ---
        AsyncOperation loadOp = SceneManager.LoadSceneAsync("TitleScene");
        loadOp.allowSceneActivation = false;

        // --- 演出時間待機 ---
        yield return new WaitForSeconds(transition.duration + transition.fadedDelay);

        // --- シーン切り替え ---
        loadOp.allowSceneActivation = true;
        yield return new WaitUntil(() => loadOp.isDone);

        isTransitioning = false;
    }
}
