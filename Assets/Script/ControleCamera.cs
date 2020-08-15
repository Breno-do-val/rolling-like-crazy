using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ControleCamera : MonoBehaviour
{
    [Tooltip("O alvo a ser acompahado pela camera")]
    public Transform alvo;

    [Tooltip("Offset da camera em relaçao ao alvo")]
    public Vector3 offset = new Vector3(0, 3, -6);


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // verifica se o alvo existe
        if (alvo!= null)    
        {
            // Altera a posição da camera
            transform.position = alvo.position + offset;

            // Altera a rotação da camera em relação ao jogador
            transform.LookAt(alvo);
        }
    }
}


