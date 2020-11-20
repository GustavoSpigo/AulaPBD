using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GetPHP : MonoBehaviour
{
    public Text textoDaTela;
    public InputField inputNick;
    public InputField inputSenha;
    public AudioSource som;

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
        else if (inputSenha.text.Trim().Equals(string.Empty))
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

        using (var w = UnityWebRequest.Post("http://localhost/jogos/listaJogador.php", wwwf))
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
                    if (jogadores.objetos[0].senha.Equals(MD5.Md5Sum(inputSenha.text)))
                    {
                        textoDaTela.text = "Usuário encontrado e senha correta";
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
}
