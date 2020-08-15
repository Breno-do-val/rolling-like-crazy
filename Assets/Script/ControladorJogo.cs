using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;


/// <summary>
/// Classe para controlar a parte principal do jogo
/// </summary>
public class ControladorJogo : MonoBehaviour
{
    [Tooltip("Referencia para o TileBasico")]
    public Transform tile;

    [Tooltip("Referencia para o Obstaculo")]
    public Transform obstaculo;

    [Tooltip("Ponto para se deslocar o TileBasicoInicial")]
    public Vector3 pontoInicial = new Vector3(0, 0, -5);


    [Tooltip("Quantdade de Tiles iniciais")]
    [Range(1, 20)]
    public int numSpawnIni;


    [Tooltip("Numero de tiles sem obstaculos")]
    [Range(1,4)]
    public int numTilesSemOBS = 4;


    /// <summary>
    /// Local para spawn do proximo Tile
    /// </summary>
    private Vector3 proxTilePos;


    /// <summary>
    /// Rotação do próximo Tile
    /// </summary>
    private Quaternion proxTileRot;



    // Start is called before the first frame update
    void Start()
    {

        Advertisement.Initialize("3769005");
        proxTilePos = pontoInicial;
        proxTileRot = Quaternion.identity;

        for(int i = 0; i < numSpawnIni; i++)
        {
            SpawnProxTile( i >= numTilesSemOBS);
        }
    }


    public void SpawnProxTile(bool spawnObstaculos = true)
    {
        var novoTile = Instantiate(tile, proxTilePos, proxTileRot);

        var proxTile = novoTile.Find("PontoSpawn");
        proxTilePos  = proxTile.position;
        proxTileRot  = proxTile.rotation;

        //Verifica se já podemos criar tiles com obstaculos
        if(!spawnObstaculos) 
            return;

        //Primeiro devemos buscar todos os locais possíveis
        var pontosObstaculos = new List<GameObject>();

        //Varrer os GOs filhos buscando os pontos de spawn
        foreach(Transform filho in novoTile)
        {
            //Vamos verificar se possui a TAG PontoSpawn
            if(filho.CompareTag("ObsSpawn"))
            {
                //Se for o adicionamos na lista com potencial ponto de spawn
                pontosObstaculos.Add(filho.gameObject);
            }
        }

        // Garantir que existe pelo menos um spawn point disponivel
        if(pontosObstaculos.Count > 0)
        {
            //Vamos pegar um ponto aleatorio
            var pontoSpawn = pontosObstaculos[Random.Range(0, pontosObstaculos.Count)];

            //Vamos guardar a posicao desse ponto de spawn
            var obsSpawnPos = pontoSpawn.transform.position;

            //Cria um novo obstaculo
            var novoObs = Instantiate(obstaculo, obsSpawnPos, Quaternion.identity);

            //Faz ele ser filho do TileBasico.PontoSpawn (centro, esqueda ou direita)
            //Outra forma de fazer isso eh no proprio Instantiate. Ja existe uma sobrecarga para adicionarmos um parent
            novoObs.SetParent(pontoSpawn.transform);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }


}


