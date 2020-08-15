using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class jogadorComportamento : MonoBehaviour
{
    public enum TipoMovimentoHorizontal
    {
        Acelerometro,
        Touch
    }

    public TipoMovimentoHorizontal movimentoHorizontal = 
        TipoMovimentoHorizontal.Acelerometro;

    /// <summary>
    /// Uma referencia para o componente Rigidbody
    /// </summary>   
    private Rigidbody rb;

    [Tooltip("A velocidade que o jogador irá se esquivar para os lados")]
    [Range(0,10)]
    public float velocidadeEsquiva = 5.0f;

    [Tooltip("A velocidade que o jogador irá se esquivar para frente")]
    [Range(0,10)]
    public float velocidadeRolamento = 5.0f;

    [Header("Atributos responsaveis pelo swipe")]
    [Tooltip("Determina qual a distanci que o dedo do jogador deve deslocar pela tela para ser considerada um swipe")]
    public float minDisSwipe = 2.0f;

    [Tooltip("Distancia que a bola ira percorrer atravaes do swipe")]
    public float swipeMove = 2.0f;    

    private Vector2 toqueInicio;

    // Start is called before the first frame update
    void Start()
    {
        // Obter acesso ao componente Rigidbody associado a esse GO (GameObject)
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        // Verifica se o jogo estah pausado
        if(MenuPauseComp.pausado) return;

        // Verificar para qual lado o jogador deseja esquivar
        var velocidadeHorizontal = Input.GetAxis("Horizontal") * velocidadeEsquiva;     // Input.GetAxis varia entre -1(botão A - esquerda) e +1(Botão D - direita). 
        
        #if UNITY_STANDALONE || UNITY_EDITOR || UNITY_WEBPLAYER
            // Detectando se houve clique com o botão (opcao 0) ou toque na tela
            if(Input.GetMouseButton(0)) {
            
                velocidadeHorizontal = CalculaMovimento(Input.mousePosition);                                     
            }

        #elif UNITY_IOS || UNITY_ANDROID   

            if (movimentoHorizontal == TipoMovimentoHorizontal.Acelerometro){
                // Move a bola baseada na direcao de acelerometro
                velocidadeHorizontal = Input.acceleration.x * velocidadeRolamento;
            } 
            else {  // Movimento por touch
                // Detectando exclusivamente via touch
                if(Input.touchCount > 0){
                    // Obtendo o primeiro touch ba tela dentro do frame
                    Touch toque = Input.touches[0];         

                    // Convertendo para Viewport space (entre 0 e 1)
                    // Usando por touch
                    velocidadeHorizontal = CalculaMovimento(toque.position);

                    // Usando movimento swipe
                    // Lembrando que podemos ter o jogo pelo swipe e touch ao mesmo tempo
                    SwipeTeleport(toque);

                    // Verifica se esse toque atingiu algum objeto
                    TocarObjetos(toque);  
                }
            }
        #endif

        var forcaMovimento = new Vector3(velocidadeHorizontal,0, velocidadeRolamento);

        // Time.delta nos retorna o tempo gasto no frame anterior
        // algo em torno de 1/60ps
        // Usamos esse valor para garantir que o nosso jogador se desloque com a mesma velocidade
        // não importa o hardware
        forcaMovimento *= (Time.deltaTime * 60);

        // Aplicar uma força para que a bola se desloque
        //rb.AddForce(velocidadeHorizontal, 0, velocidadeRolamento);                      //Parametros de entrada -> (float x, float y, float z)
        rb.AddForce(forcaMovimento);                      //Parametros de entrada -> (float x, float y, float z)
    }

    private float CalculaMovimento(Vector2 screenSpaceCoord){

        // Obtendo o primeiro touch ba tela dentro do frame        
        float direcaoX = 0;

        //Convertendo para Viewport space (entre 0 e 1)
        var pos = Camera.main.ScreenToViewportPoint(screenSpaceCoord);

        if (pos.x < 0.5){
            direcaoX = -1;
        } 
        else {
            direcaoX = 1;
        }

        return direcaoX * velocidadeEsquiva;
    }

    private void SwipeTeleport (Touch toque){
        
         // Verifica se eh o ponto onde o swipe começou
        if (toque.phase == TouchPhase.Began) {        
            toqueInicio = toque.position;           
        }
        // Verifica se o swipe acabou
        else if (toque.phase == TouchPhase.Ended){
            Vector2 toqueFim = toque.position;
            Vector3 direcaoMov;

            // Faz a diferenca entre o ponto final e inical do swipe
            float dif = toqueFim.x - toqueInicio.x;

            // Verifica se o swipe percorreu uma distancia suficiente para ser recnhecido como swipe
            if(Mathf.Abs(minDisSwipe) >= dif){
                // Determina a direcao do swipe
                if(dif < 0)
                    direcaoMov = Vector3.left;
                else
                    direcaoMov = Vector3.right;
            } 
            else {
                return;
            }

            //Raycast eh outra forma de detectar colisao
            RaycastHit hit;

            // Vamos verificar se o swipe nao vai causar colisao
            if(!rb.SweepTest(direcaoMov, out hit, swipeMove)) {
                rb.MovePosition(rb.position + (direcaoMov * swipeMove));
            }
        }
    }
}

