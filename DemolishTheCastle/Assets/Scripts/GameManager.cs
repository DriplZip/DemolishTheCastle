using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public enum GameMode
{
    idle,
    playing,
    levelEnd
}

public class GameManager : MonoBehaviour
{
    private static GameManager _gameManager;

    private int _level;
    private int _maxLevel;

    private int _shotsTaken;

    private GameObject _currentCastle;
    private GameMode _mode = GameMode.idle;

    private string _showing = "Show Slingshot";

    [Header("Set in Inspector")]
    
    public Text uiLevel;
    public Text uiShots;
    public Text uiButton;
    
    public Vector3 castlePosition;
    public GameObject[] castles;

    private void Start()
    {
        _gameManager = this;

        _level = 0;
        _maxLevel = castles.Length;

        StartLevel();
    }

    private void StartLevel()
    {
        if (_currentCastle != null) Destroy(_currentCastle);

        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject projectile in projectiles)
        {
            Destroy(projectile);
        }

        _currentCastle = Instantiate(castles[_level]);
        _currentCastle.transform.position = castlePosition;
        _shotsTaken = 0;
        
        SwitchView("Show Both");
        ProjectileLine.projectileLine.Clear();

        Goal.goalMet = false;
        
        UpdateGUI();

        _mode = GameMode.playing;
    }

    public void SwitchView(string view = "")
    {
        if (view == "") view = uiButton.text;
        
        _showing = view;

        switch (_showing)
        {
            case "Show Slingshot":
                FollowCam.pointOfInterest = null;
                uiButton.text = "Show Castle";
                break;
            case "Show Castle":
                FollowCam.pointOfInterest = _gameManager._currentCastle;
                uiButton.text = "Show Both";
                break;
            case "Show Both":
                FollowCam.pointOfInterest = GameObject.Find("ViewBoth");
                uiButton.text = "Show Slingshot";
                break;
        }
    }

    private void UpdateGUI()
    {
        uiLevel.text = "Level: " + (_level + 1) + " of " + _maxLevel;

        uiShots.text = "Shots Taken: " + _shotsTaken;
    }

    private void Update()
    {
        UpdateGUI();

        if (_mode == GameMode.playing && Goal.goalMet)
        {
            _mode = GameMode.levelEnd;
            
            SwitchView("Show Both");
            
            Invoke(nameof(NextLevel), 2f);
        }
    }

    private void NextLevel()
    {
        _level++;

        if (_level == _maxLevel) _level = 0;
        
        StartLevel();
    }

    public static void ShotFired()
    {
        _gameManager._shotsTaken++;
    }
}
