using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuPauseComp : MonoBehaviour
{
    public static bool pausado;

    [SerializeField]
    private GameObject menuPausePanel;


    /// <summary>
    /// Metodo para reiniciar a scene
    /// </summary>
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);        
    }

    /// <summary>
    /// Metodo para pausar o jogo
    /// <param name="isPausado"></param>
    /// </summary>
    public void Pausado(bool isPausado)
    {
        pausado = isPausado;

        Time.timeScale = (pausado) ? 0 : 1;

        menuPausePanel.SetActive(pausado);
    }


    /// <summary>
    /// Metodo para carregar uma scene
    /// </summary>
    /// <param name="nomeScene">Nome da scene que sera carregada</param>
    public void CarregaScene(string nomeScene)
    {
        SceneManager.LoadScene(nomeScene);
    }


    // Start is called before the first frame update
    void Start()
    {
        pausado = false;
        Pausado(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
