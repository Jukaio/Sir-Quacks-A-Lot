﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    static public IEnumerator Load_Level(int index)
    {
        SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
        var scene = SceneManager.GetSceneByBuildIndex(index);

        yield return new WaitForSecondsRealtime(1.0f);

        if (!SceneManager.SetActiveScene(scene))
            Debug.Log("FAIL TO SET ACTIVE SCENE");

        yield break;
    }

    static public IEnumerator Unload_Level(int index)
    {
        if (SceneManager.GetSceneByBuildIndex(index).isLoaded)
            SceneManager.UnloadSceneAsync(index);
        else
            yield break;

    }
}
