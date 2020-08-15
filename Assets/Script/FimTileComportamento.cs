using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FimTileComportamento : MonoBehaviour
{

    [Tooltip("Tempo de espera para destruir o TileBasico")]
    public float tempoDestruir = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<jogadorComportamento>())
        {
            //Como foi o jogador que passou, vamos criar um TileBasico no próximo ponto
            //Mas esse próximo ponto esta depois do último TileBasico presente na cena
            GameObject.FindObjectOfType<ControladorJogo>().SpawnProxTile();

            // E agora destroi esse TileBasico
            Destroy(transform.parent.gameObject, tempoDestruir);
        }
    }
}
