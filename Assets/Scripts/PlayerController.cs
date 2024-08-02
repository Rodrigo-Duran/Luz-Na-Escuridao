using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region Variables
    // VARIAVEIS GERAIS DO PLAYER
    // Direção do input
    private Vector2 direction;
    // Rigidbody
    private Rigidbody2D playerRB;
    // Animator
    private Animator animator;
    // Velocidade padrão do player
    [SerializeField] private float playerNormalSpeed = 3f;
    // Velocidade atual do player
    [SerializeField] private float playerActualSpeed;
    // Ponto de rotação que segue o mouse
    [SerializeField] private Transform rotatePoint;

    // REFERENCIAS DE OUTROS SCRIPTS
    [SerializeField] private GameController gameController;
    [SerializeField] private FlashLightLevelControl flashLightLevelControl;

    // SPRINT
    // Variavel para saber se o player esta correndo
    private bool playerIsRunning;
    public bool _playerIsRunning
    {
        get { return playerIsRunning; }
        set { playerIsRunning = value; }
    }

    // HIDE
    // Lista de sprites do player
    [SerializeField] private List<SpriteRenderer> spriteRendererList;
    // Variavel para saber se o player esta escondido
    private bool playerIsHidden;
    public bool _playerIsHidden
    {
        get { return playerIsHidden; }
        set { playerIsHidden = value; }
    }
    // Variavel para saber se o player pode se esconder
    private bool playerCanHide;
    public bool _playerCanHide
    {
        get { return playerCanHide; }
        set { playerCanHide = value; }
    }
    // Variavel para saber qual objeto de moita esta no range dele
    private GameObject bushToHide;
    public GameObject _bushToHide
    {
        get { return bushToHide; }
        set { bushToHide = value; }
    }
    // Variavel para saber a ultima posição do player antes de se esconder
    private Vector2 lastPositionBeforeHiding;

    // PLACAS
    // Variavel para saber se o player pode interagir com placas
    private bool playerCanInteract;
    public bool _playerCanInteract
    {
        get { return playerCanInteract; }
        set { playerCanInteract = value; }
    }

    private string hintContent;
    public string _hintContent
    {
        get { return hintContent; }
        set { hintContent = value; }
    }

    //[SerializeField] TextMeshProUGUI hintText;
    //[SerializeField] GameObject hintImage;

    // PLAYER HUD
    [SerializeField] GameObject playerInteractionWarning;
    [SerializeField] GameObject playerInteractImage;
    [SerializeField] GameObject playerInteractHUDImage;
    [SerializeField] private GameObject darkVision;

    // GAME HUD
    [SerializeField] private GameObject flashlightOn, flashlightOff;

    // AUDIO
    [SerializeField] private AudioSource playerSourceLoop;
    [SerializeField] private AudioSource playerSourceUnique;

    [SerializeField] private List<AudioClip> audioClipsLoop;
    [SerializeField] private List<AudioClip> audioClipsUnique;

    private string playerAction;
    private string apoio;

    

    #endregion

    #region MainMethods
    // Awake
    void Awake()
    {
        // Pegando componentes no próprio objeto
        playerRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Player não esta correndo 
        playerIsRunning = false;
        // Setando velocidade atual do player pra velocidade normal do player
        playerActualSpeed = playerNormalSpeed;
        // Player não esta escondido
        playerIsHidden = false;
        // Player não pode se esconder
        playerCanHide = false;
        // Player não pode interagir
        playerCanInteract = false;
        // Não existe moita no range do player
        bushToHide = null;

        //playerSourceLoop.clip = audioClipsLoop[0];
       // playerSourceLoop.Play();

        playerAction = "idle";
        apoio = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameController.gameIsPaused)
        { 
            // INPUT DE MOVIMENTAÇÃO
            // Pegando input horizontal e vertical do player
            direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            // PLAYER ROTATION
            // Se a rotação do objeto RotatePoint(Atrelado ao Player) estiver entre 0.75 e -0.7 o player rotaciona pra direita 
            if (rotatePoint.rotation.z > -0.7f && rotatePoint.rotation.z < 0.75f)
            {
                transform.eulerAngles = new Vector2(0, 0);
            }
            // Se não, o player rotaciona pra esquerda
            else if (rotatePoint.rotation.z < -0.8f || rotatePoint.rotation.z > 0.85f)
            {
                transform.eulerAngles = new Vector2(0, 180);
            }

            // ANIMAÇÃO
            // Se estiver tendo algum input
            if (direction.x != 0 || direction.y != 0)
            {
                
                if (playerIsRunning)
                {
                    playerAction = "running";
                    // Tocar a animação de correr
                    animator.SetInteger("transition", 2);
                }
                else
                {
                    playerAction = "walking";
                    // Tocar a animação de andar
                    animator.SetInteger("transition", 1);
                }
            }
            else
            {
                playerAction = "idle";
                // Tocar a animação idle
                //AINDA NÃO TEMOS ANIMAÇÃO IDLE, PORTANTO VOU COLOCAR PRA DEFAULT
                animator.SetInteger("transition", 3);
            }

            // HIDE
            // Se o jogador apertar o E
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Se ele puder se esconder, chamar método para se esconder
                if (playerCanHide) StartCoroutine(Hiding());

                // Se ele estiver escondido, chamar método para sair do esconderijo
                else if (playerIsHidden) StartCoroutine(ComingOutOfHiding());

            }

            // SPRINT
            // Enquanto o player estiver apertando o shift e puder correr
            if (Input.GetKey(KeyCode.LeftShift) && gameController._canRun)
            {
                // Player esta correndo
                playerIsRunning = true;
                // Velocidade atual do player é aumentada em 3x
                playerActualSpeed = playerNormalSpeed * 3;
            }
            // Se ele não estiver apertando o shift ou não puder correr
            else
            {
                // Player nao esta correndo
                playerIsRunning = false;
                // Velocidade atual do player volta a velocidade normal
                playerActualSpeed = playerNormalSpeed;
            }

            // INTERAÇÃO COM PLACAS
            /*if (Input.GetKeyDown(KeyCode.E) && playerCanInteract)
            {
                hintText.text = hintContent;
                hintImage.SetActive(true);
                //gameController.PauseGame();
            }*/

            // INTERAÇÃO GERAL
            if (playerCanHide || playerCanInteract)
            {
                //Debug.Log("POSSO INTERAGIR");
                //playerInteractionWarning.SetActive(true);
                playerInteractImage.SetActive(true);
                playerInteractHUDImage.SetActive(true);
            }
            else
            {
                //Debug.Log("NÃO POSSO INTERAGIR");
                //playerInteractionWarning.SetActive(false);
                playerInteractImage.SetActive(false);
                playerInteractHUDImage.SetActive(false);
            }

            // AUDIOS
            if (apoio != playerAction)
            {
                apoio = playerAction;
                switch (playerAction)
                {
                    case "idle":
                        //playerSourceLoop.clip = audioClipsLoop[2];
                        playerSourceLoop.clip = null;
                        playerSourceLoop.clip = null;
                        playerSourceLoop.Play();
                        break;

                    case "walking":
                        playerSourceLoop.clip = audioClipsLoop[0];
                        playerSourceLoop.Play();
                        break;

                    case "running":
                        playerSourceLoop.clip = audioClipsLoop[1];
                        playerSourceLoop.Play();
                        break;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        // Movendo o rigidbody do player através do input (direction)
        if(!playerIsHidden) playerRB.velocity = direction.normalized * playerActualSpeed;
    }
    #endregion

    #region HideHandler
    IEnumerator Hiding()
    {
        // Player não pode se esconder
        playerCanHide = false;
        // Player está escondido
        playerIsHidden = true;
        // Parando o player
        playerRB.velocity = new Vector2(0, 0);
        playerActualSpeed = 0;
        direction.x = 0;
        direction.y = 0;
        // Tocando Audio
        playerSourceUnique.clip = audioClipsUnique[0];
        playerSourceUnique.Play();
        // Lanterna está desligada
        flashLightLevelControl._flashLightIsActive = false;
        // Não é possível interagir com a lanterna
        flashLightLevelControl._canInteractWithFlashlight = false;
        //Timer de 1 segundo para realizar a ação de esconder
        //yield return new WaitForSeconds(0.01f);
        Debug.Log("SE ESCONDENDO");
        // Desabilitando todos os sprites do player, pra ele "desaparecer"
        foreach (SpriteRenderer sprite in spriteRendererList) sprite.enabled = false;
        // Guardando a ultima posição do player antes de se esconder
        lastPositionBeforeHiding = transform.position;
        // Movendo o player pra mesma posição do objeto da moita
        playerRB.MovePosition(bushToHide.transform.position);
        // Mudando o nível da luz pra 0 por último, pois eu preciso do nível da luz antigo para ter a referencia do objeto da moita
        flashLightLevelControl._lightLevel = 0;
        //DarkVision ativa
        darkVision.SetActive(true);
        //Alterar a imagem da lanterna na HUD (Desligada)
        flashlightOn.SetActive(false);
        flashlightOff.SetActive(true);
        yield return new WaitForSeconds(0.01f);
    }

    IEnumerator ComingOutOfHiding()
    {
        // Esperando 0.01 segundos
        yield return new WaitForSeconds(0.01f);
        // Voltando o player pra posição que ele estava antes de se esconder
        playerRB.MovePosition(lastPositionBeforeHiding);
        // Habilitando todos os sprites do player novamente
        foreach (SpriteRenderer sprite in spriteRendererList) sprite.enabled = true;
        // Player não esta escondido
        playerIsHidden = false;
        // Player pode se esconder
        playerCanHide = true;
        // Tocando Audio
        playerSourceUnique.clip = audioClipsUnique[1];
        playerSourceUnique.Play();
        // Ativando velocidade do player novamente
        playerActualSpeed = playerNormalSpeed;
        Debug.Log("APARECENDO");
        // Player pode interagir com a lanterna novamente
        flashLightLevelControl._canInteractWithFlashlight = true;
    }
    #endregion

}
