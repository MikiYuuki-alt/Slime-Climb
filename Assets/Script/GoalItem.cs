using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GoalItem : MonoBehaviour
{
    [Header("演出")]
    public GameObject clearEffectPrefab;
    public string titleSceneName = "TitleScene";
    public TMP_Text resultText;

    [Header("遅延")]
    public float delayBeforeReturn = 3f;

    [Header("SE")]
    public AudioClip goalSE;
    public AudioSource audioSource; // GoalItem にアタッチした AudioSource

    private int playerCount;
    private bool triggered = false;

    private void Start()
    {
        playerCount = PlayerPrefs.GetInt("PlayerCount", 1);

        if (resultText == null)
        {
            GameObject textObj = GameObject.Find("ResultText");
            if (textObj != null)
                resultText = textObj.GetComponent<TMP_Text>();
        }

        // AudioSource が未設定なら自分に追加
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered || !collision.CompareTag("Player")) return;
        triggered = true;

        // 🎵 ゴールSE 再生
        if (goalSE != null)
        {
            audioSource.clip = goalSE;
            audioSource.volume = 2.0f; // 1.0以上で音をブースト
            audioSource.Play();
        }

        // ✨ 演出
        if (clearEffectPrefab != null)
            Instantiate(clearEffectPrefab, transform.position, Quaternion.identity);

        // ゴールアイテムを無効化
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

        // 🏆 テキスト
        if (resultText != null)
        {
            resultText.gameObject.SetActive(true);
            resultText.alignment = TextAlignmentOptions.Center;
            resultText.fontSize = 120;
            resultText.color = Color.yellow;

            if (playerCount == 1)
            {
                resultText.text = "CLEAR!";
            }
            else
            {
                if (collision.gameObject.layer == LayerMask.NameToLayer("P1"))
                    resultText.text = "PLAYER 1 WIN!";
                else if (collision.gameObject.layer == LayerMask.NameToLayer("P2"))
                    resultText.text = "PLAYER 2 WIN!";
                else
                    resultText.text = "WIN!";
            }
        }

        Invoke(nameof(ReturnToTitle), delayBeforeReturn);
    }

    private void ReturnToTitle()
    {
        SceneManager.LoadScene(titleSceneName);
    }
}
