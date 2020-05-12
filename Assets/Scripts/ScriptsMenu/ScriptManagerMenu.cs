using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ScriptManagerMenu : MonoBehaviour
{

    public Image screenFader;
    public float transitionTime = 1;

    public void Carga(string pNombreScene)
    {
        Color color = screenFader.color;
        screenFader.DOColor(new Color(color.r, color.g, color.b, 1), transitionTime).OnComplete(() =>
            {
                SceneManager.LoadScene(pNombreScene);
            }
        );
    }
}
