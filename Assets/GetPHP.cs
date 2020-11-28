using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GetPHP : MonoBehaviour
{
    public Text textoDaTela;
    public InputField inputNick;
    public InputField inputSenha;
    public AudioSource som;

    public Button btnLogoff;
    public Button btnNovoUsuario;
    public Toggle salvarSenha;

    private string senhaCriptografada;

    public void BuscarJogador()
    {
        textoDaTela.text = "Carregando...";
        
        if(inputNick.text.Equals("touro") && inputSenha.text.Equals("toureiro"))
        {
            som.Play();
        }

        if (inputNick.text.Trim().Equals(string.Empty))
        {
            textoDaTela.text = "Favor informar o usuário";
        }
        else if (inputSenha.text.Trim().Equals(string.Empty) && PlayerPrefs.GetInt("SalvarSenha", 0)==0)
        {
            textoDaTela.text = "Favor informar a senha";
        }
        else
        {
            StartCoroutine(listarJogadores());
        }

        
    }
    IEnumerator listarJogadores()
    {
        WWWForm wwwf = new WWWForm();
        wwwf.AddField("nick", inputNick.text);
        //wwwf.AddField("pass", MD5.Md5Sum(inputSenha.text));

        using (var w = UnityWebRequest.Post(Config.EnderecoServer + "listaJogador.php", wwwf))
        {
            yield return w.SendWebRequest();
            
            if(w.isNetworkError || w.isHttpError)
            {
                Debug.Log(w.error);
            }
            else
            {
                Jogadores jogadores = JsonUtility.FromJson<Jogadores>(w.downloadHandler.text);

                textoDaTela.text = "";

                if (jogadores.objetos.Count.Equals(0))
                {
                    textoDaTela.text = "Usuário não encontrado";
                }
                else if(jogadores.objetos.Count == 1)
                {
                    if (jogadores.objetos[0].senha.Equals(MD5.Md5Sum(inputSenha.text)) || 
                        jogadores.objetos[0].senha.Equals(senhaCriptografada))
                    {
                        textoDaTela.text = "Logando...";
                        Jogador jogadorLogado = jogadores.objetos[0];

                        PlayerPrefs.SetString("PlayerLogado", JsonUtility.ToJson(jogadorLogado));
                        PlayerPrefs.SetInt("SalvarSenha", salvarSenha.isOn ? 1 : 0);
                        
                        /*
                        PlayerPrefs.SetString("nick", jogadores.objetos[0].nick);
                        PlayerPrefs.SetString("email", jogadores.objetos[0].email);
                        PlayerPrefs.SetInt("level", jogadores.objetos[0].level);
                        */

                        SceneManager.LoadScene("Jogo", LoadSceneMode.Single);
                    }
                    else
                    {
                        textoDaTela.text = "Usuário encontrado porém senha incorreta";
                    }
                    
                }

                /*
                foreach (Jogador cadaJogador in jogadores.objetos) 
                {
                    textoDaTela.text += cadaJogador.nick + "\n";
                }
                */
            }
        }
    }

    private void Start()
    {
        if (PlayerPrefs.GetString("PlayerLogado")!=string.Empty)
        {
            Jogador jogadorLogado = JsonUtility.FromJson<Jogador>(PlayerPrefs.GetString("PlayerLogado"));
            inputNick.text = jogadorLogado.nick;
            inputSenha.Select();
            inputSenha.ActivateInputField();

            btnLogoff.interactable = true;

            if (PlayerPrefs.GetInt("SalvarSenha", 0) == 1)
            {

                inputNick.interactable = false;
                senhaCriptografada = jogadorLogado.senha;
                inputSenha.gameObject.SetActive(false);
                salvarSenha.gameObject.SetActive(false);
            }
            else
            {
                salvarSenha.isOn = false;
            }
        }
        else
        {
            btnLogoff.interactable = false;
        }
        btnNovoUsuario.interactable = !btnLogoff.interactable;
    }
}
