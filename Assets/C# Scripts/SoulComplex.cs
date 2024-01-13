using InControl.NativeDeviceProfiles;
using Modding;
using System.Collections.Generic;
using UnityEngine;
using WeaverCore;
using WeaverCore.Interfaces;

public class SoulComplex : WeaverMod
{
    public SoulComplex() : base("Soul Complex") { }

    public override string GetVersion()
    {
        return "1";
    }

    public override void Initialize()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnLoadScene;
    }

    private void OnLoadScene(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        if (scene.name == "Tutorial_01")
        {
            GameObject newObj = new GameObject("Enemy Remover");
            newObj.transform.parent = null;
            newObj.AddComponent<EnemyRemover>();
            TransitionPoint[] tps = GameObject.FindObjectsOfType<TransitionPoint>();
            for (int i = 0; i < tps.Length; i++)
            {
                if (tps[i].name == "right1")
                {
                    tps[i].transform.position = new Vector3(103.7f, 11.4f, 0);
                }
            }
        }
    }
}