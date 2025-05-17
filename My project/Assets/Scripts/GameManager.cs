using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private enum LevelName
    {
        None,
        Tutorial,
        Agresion,
        Football,
        Exclusion
    }
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private string levelScenePrefix = "Level";
    [SerializeField]
    private string mainMenuScene = "MainMenu";

    int currentLevel = 0;
    
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { 
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //EventoBot("Inicio de Juego")
        EventQueue.Instance().AddEvent(new GameEvent(EventType.GameStart));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeScene(string sceneToStart) {
        SceneManager.LoadScene(sceneToStart);
    }

    public void StartLevel(int id) {
        if (id > LevelManager.Instance.NextLevel())
            return;
        currentLevel = id;
        // EventoBot("Nivel Actual", id + 1) 
        EventQueue.Instance().AddEvent(new GameEvent(EventType.LevelStart, id));
        MenuNavigationManager.SetIntialMenuIndex(1);
        ChangeScene(string.Concat(levelScenePrefix, id));
    }

    public void Pause() {

    }
    public void Resume() {

    }

    public void BackToMainMenu() {
        MenuNavigationManager.SetIntialMenuIndex(0);
        ChangeScene(mainMenuScene);
    }

    public void Quit() {
        Application.Quit();
    }

    public void CompleteLevel(TensionCompletion tensionCompletion) {
        LevelManager.Instance.CompleteLevel(currentLevel, tensionCompletion);
        // EventoBot("Fin de Nivel", currentLevel) 
        EventQueue.Instance().AddEvent(new GameEvent(EventType.LevelEnd, currentLevel));
        ChangeScene(mainMenuScene);
    }

    public string GetNameOnIndex(int index)
    {
        return ((LevelName) index).ToString();
    }
}
