using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    // 목표 : 다음 씬을 '비동기 방식'으로 로드하고 싶다.
    //        또한 로딩 진행률을 시각적으로 표현하고 싶다.

    // 속성:
    // - 다음 씬 번호(인덱스)
    public int NextSceneIndex = 2;

    // - 프로그래서 슬라이더바
    public Slider ProgressSlider;

    // - 프로그래스 텍스트
    public TextMeshProUGUI ProgressText;

    private void Start()
    {
        StartCoroutine(LoadNextScene_Coroutine());
    }

    private IEnumerator LoadNextScene_Coroutine()
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(NextSceneIndex);
        ao.allowSceneActivation = false;

        while(!ao.isDone)
        {
            ProgressSlider.value = ao.progress;
            ProgressText.text = $"{ao.progress * 100f}%";

            if(ao.progress >= 0.9f)
            {
                ao.allowSceneActivation = true;
            }
            
            yield return null;  // 1프레임 대기
        }
    }
    
}
