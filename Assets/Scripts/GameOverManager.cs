using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
	[SerializeField] private Canvas canvas;
	[SerializeField] private TMP_Text brainsText;
	[SerializeField] private Button restartButton;

    // Start is called before the first frame update
    void Start()
    {
		this.restartButton.onClick.AddListener(() => {
			Time.timeScale = 1f;
			SceneManager.LoadScene(0);
		});
    }

    // Update is called once per frame
    void Update()
    {
		if (this.canvas.enabled == false && GameManager.Instance.Player.IsAlive == false) {
			this.canvas.enabled = true;
			this.brainsText.text = $"But you were level {GameManager.Instance.LevelController.PlayerLevel} and harvested {GameManager.Instance.BrainsHarvested} brains, which is nice!";
			Time.timeScale = 0f;
		}
    }
}
