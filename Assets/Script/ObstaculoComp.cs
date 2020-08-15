using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObstaculoComp : MonoBehaviour
{
    [Tooltip("Particle system da explosao")]
    public GameObject  explosao;

    [Tooltip("Quanto tempo antes de reiniciar o jogo")]
    public float tempoEspera = 2.0f;
    
    [Tooltip("Acesso para o componente MeshREnderer")]
    MeshRenderer mr = new MeshRenderer();

    [Tooltip("Acesso para o componente BoxColdier")]
    BoxCollider bc = new BoxCollider();

    private void OnCollisionEnter(Collision collision){
        
        if(collision.gameObject.GetComponent<jogadorComportamento>())
        {
            Destroy(collision.gameObject);
            Invoke("ResetaJogo", tempoEspera);
        }
    }

    
    void ResetaJogo()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    // Start is called before the first frame update
    void Start()
    {
        // Acesso aos componentes
        mr = GetComponent<MeshRenderer>();
        bc = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        // Detectando se houve clique com o botão (opcao 0) ou toque na tela
        // if(Input.GetMouseButton(0)) {           
              
        //     // Verifica se esse toque atingiu algum objeto 
        //     TocarObjetos(Input.mousePosition);                   
        // }          
              
        // Verifica se esse toque atingiu algum objeto 
        TocarObjetos(Input.mousePosition);                   
       
    }

    private static void TocarObjetos(Vector2 toque){

        // Conevertemos a posição do toque (Screen Space) para um Ray
        Ray toqueRay = Camera.main.ScreenPointToRay(toque);

        // Objeto que ira salvar informações de um possível objeto que tocado
        RaycastHit hit;

        if(Physics.Raycast(toqueRay, out hit)){
            hit.transform.SendMessage("ObjetoTocado", SendMessageOptions.DontRequireReceiver);
        }
    }

    public void ObjetoTocado(){
        
        if(explosao != null){
            // Cria o efeito de explosao
            var particulas = Instantiate(explosao, transform.position, Quaternion.identity);
            // Destroi as particulas
            Destroy(particulas, 1.0f);
        }
        
        // Desabilitando o MeshRendered e o BoxColider
        // Garante que nada irá influenciar nesse obstáculo novamente
        mr.enabled = false;
        bc.enabled = false;

        // Destroi este obstaculo
        Destroy(this.gameObject);
    }
}
