﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    static public void Load_Level(int index)
    {
        SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
    }
}