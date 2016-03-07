using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour 
{
	void Start()
	{
		ScoreController.ScoreChanged += SetScore;
	}

	void OnDestroy()
	{
		ScoreController.ScoreChanged -= SetScore;
	}

	void SetScore(int score)
	{
		_score.text = score.ToString();
	}

	public void Show()
	{
		_menuPanel.SetActive(true);
		GameController.i.SetPauseGame(true);
	}

	public void Hide()
	{
		_menuPanel.SetActive(false);
		GameController.i.SetPauseGame(false);
	}

	public void NewGame()
	{
		_menuPanel.SetActive(false);
		GameController.i.NewGame();
	}

	[SerializeField] Text		_score;
	[SerializeField] GameObject	_menuPanel;
}
