using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Prime31.TransitionKit;

public class EscapeToTitle : MonoBehaviour
{
    [SerializeField] private GameObject uiCanvas; // �� �Q�[�W�Ȃ�UI��Canvas���Z�b�g
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

        // --- UI���ꎞ�I�ɔ�\�� ---
        if (uiCanvas != null)
            uiCanvas.SetActive(false);

        // --- ���o�p�f���Q�[�g�ݒ� ---
        SquaresTransition transition = new SquaresTransition
        {
            squareColor = Color.black,
            duration = 1.2f,                 // ���o�̒����i�������߂Ɂj
            fadedDelay = 0.3f,               // �Ó]��̑ҋ@����
            squareSize = new Vector2(13f, 9f),
            smoothness = 0.5f,
            nextScene = SceneManager.GetSceneByName("TitleScene").buildIndex
        };

        // --- TransitionKit�ŉ��o�J�n ---
        TransitionKit.instance.transitionWithDelegate(transition);

        // --- �V�[���񓯊����[�h ---
        AsyncOperation loadOp = SceneManager.LoadSceneAsync("TitleScene");
        loadOp.allowSceneActivation = false;

        // --- ���o���ԑҋ@ ---
        yield return new WaitForSeconds(transition.duration + transition.fadedDelay);

        // --- �V�[���؂�ւ� ---
        loadOp.allowSceneActivation = true;
        yield return new WaitUntil(() => loadOp.isDone);

        isTransitioning = false;
    }
}
