using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace LinePaint
{

    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Text levelText;
        [SerializeField] private GameObject mainMenu, levelCompleteMenu;
        [SerializeField] private Button nextButton, retryBtn;

        public Text LevelText
        {
            get => levelText;
        }

        private void Start()
        {

            AudioListener.volume = PlayerPrefs.GetInt("SoundOn", 1);


            nextButton.onClick.AddListener(() => OnClick(nextButton));

            retryBtn.onClick.AddListener(() => OnClick(retryBtn));
        }

        private void OnClick(Button btn)
        {

            switch (btn.name)
            {

                case "Next btn":
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    break;
                case "Retry Button":
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    break;

            }
        }

        public void LevelCompleted()
        {
            mainMenu.SetActive(false);
            levelCompleteMenu.SetActive(true);

        }

    }

}