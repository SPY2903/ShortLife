using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _settingPanel;
    [SerializeField] private GameObject _endGamePanel;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private ScriptableobjectPlayer _bossData, _playerData;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_pausePanel!= null && _settingPanel!= null)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0;
                if (!_pausePanel.activeInHierarchy)
                {
                    _pausePanel.SetActive(true);
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1;
                if (_pausePanel.activeInHierarchy || _settingPanel.activeInHierarchy)
                {
                    _pausePanel.SetActive(false);
                    _settingPanel.SetActive(false);
                }
            }
        }
        if(_bossData != null && _playerData != null)
        {
            if(_bossData._health == 0)
            {
                Cursor.lockState = CursorLockMode.None;
                text.text = "YOU WIN";
                _endGamePanel.SetActive(true);
            }
            if(_playerData._health == 0)
            {
                Cursor.lockState = CursorLockMode.None;
                text.text = "YOU LOST";
                _endGamePanel.SetActive(true);
            }
        }
    }
    public void Exit()
    {
        Application.Quit();
    }
}
