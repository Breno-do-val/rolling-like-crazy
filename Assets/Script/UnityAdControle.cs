using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

#if UNITY_ADS
using UnityEngine.Advertisements;
#endif

public class UnityAdControle : MonoBehaviour
{
    [Tooltip("Controle para mostrar anuncios")]
    public static bool showAds = true;

    public static DateTime? proxTempoReward = null;

    //Referencia para o obstaculo
    public static ObstaculoComp obstaculo;

    public static void ShowAd()
    {

        //Opcoes para o Edge
        ShowOptions opcoes = new ShowOptions();
        opcoes.resultCallback = Unpause;

        if (Advertisement.IsReady())
        {
            Advertisement.Show(opcoes);
        }
        //Pausar o jogo enquanto o ad esta sendo mostrado
        MenuPauseComp.pausado = true;
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Metodo para mostrar Ad como recompensa
    /// </summary>
    public static void ShowRewardAd()
    {
        proxTempoReward = DateTime.Now.AddSeconds(15);

        if (Advertisement.IsReady())
        {
            MenuPauseComp.pausado = true;
            Time.timeScale = 0f;

            var opcoes = new ShowOptions
            {
                resultCallback = TratarMostrarResultado
            };
            Advertisement.Show(opcoes);
        }
    }

    public static void TratarMostrarResultado(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                obstaculo.Continue();
                break;

            case ShowResult.Skipped:
                Debug.Log("Ad pulado. Faz nada");
                break;

            case ShowResult.Failed:
                Debug.Log("Erro no ad. Faz nada");
                break;
        }

        MenuPauseComp.pausado = false;
        Time.timeScale = 1f;

    }

    public static void Unpause(ShowResult result)
    {
        //Quando o anuncio acabar
        //Sair do modo pausado
        MenuPauseComp.pausado = false;
        Time.timeScale = 1f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
