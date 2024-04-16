using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
	public static SceneChangeManager instance;

	Animator anim = new();

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public void SetAnim(Animator playerAnim) => anim = playerAnim;

	public void CallSceneChange(string sceneName)
	{
		if (anim != null)
			anim.SetTrigger("FadeBlack");

		StartCoroutine(ChangeScene(sceneName));
	}

	public void QuitGame() => Application.Quit();

	private IEnumerator ChangeScene(string sceneToChange)
	{
		yield return new WaitForSeconds(1.5f);
		SceneManager.LoadScene(sceneToChange);
	}
}
