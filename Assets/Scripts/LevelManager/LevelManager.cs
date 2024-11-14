using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] Animator animator;

    void Awake()
    {
        animator.enabled = false;
    }

    IEnumerator LoadSceneAsync(string sceneName) {
        Debug.Log("Scene changing...");
        animator.enabled = true;
        animator.Play("StartTransition", 0, 0.0f);
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(sceneName);
        animator.Play("EndTransition", 0, 0.0f);
        Debug.Log("Scene changed");

        Player.Instance.transform.position = new Vector2(0, -4.5f);
    }

    public void LoadScene(string sceneName) {
        StartCoroutine(LoadSceneAsync(sceneName));
    }
}
