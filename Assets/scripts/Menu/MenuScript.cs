using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuScript : MonoBehaviour {

	Canvas thisMenu;
	public Canvas quitMenu;
	public Button startButton;
	public Button quitButton;

	// Use this for initialization
	void Start () {
		quitMenu = quitMenu.GetComponent<Canvas> ();
		startButton = startButton.GetComponent<Button>();
		quitButton = quitButton.GetComponent<Button> ();
		startButton.enabled = true;
		quitMenu.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ExitPress() {
		quitMenu.enabled = true;
		startButton.enabled = false;
		quitButton.enabled = false;
	}

	public void NoPress() {
		quitMenu.enabled = false;
		startButton.enabled = true;
		quitButton.enabled = true;
	}

	public void StartLevel() {
		Application.LoadLevel (1);
	}

	public void QuitLevel() {
		Application.Quit ();
	}
}
