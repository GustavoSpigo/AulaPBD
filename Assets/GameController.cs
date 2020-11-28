using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameController : MonoBehaviour
{
    private int pontos;
    public Text txtMsg;
    private Jogador jogadorLogado;

    private void Start()
    {
        ComecaJogo();
        jogadorLogado = JsonUtility.FromJson<Jogador>(PlayerPrefs.GetString("PlayerLogado"));
    }
    public void BtnVai()
    {
        if(Random.Range(1f,20f) < 19)
        {
            pontos += 1;
            AtualizaMensagem();
        }
        else
        {
            ComecaJogo();
            AtualizaMensagem();
        }
    }
    public void BtnParar()
    {
        if (pontos > 0)
        {
            if (pontos > jogadorLogado.exp)
            {
                StartCoroutine(RegistraPontuacao());
            }
            else
            {
                ComecaJogo();
                txtMsg.text = "Está com medinho é?";
            }
        }
        else
        {
            txtMsg.text = "Aí não né...";
        }
    }
    private void ComecaJogo()
    {
        pontos = 0;
    }
    private void AtualizaMensagem()
    {
        if (pontos == 0)
        {
            txtMsg.text = "Tente novamente...";
        }
        else
        {
            txtMsg.text = "Sua pontuação é de " + pontos;
        }
    }
    /*private IEnumerator VerificaPontuacao()
    {
        WWWForm wwwf = new WWWForm();
        wwwf.AddField("nick", jogadorLogado.nick) ;

        using (var w = UnityWebRequest.Post(Config.EnderecoServer  + "listaJogador.php", wwwf))
        {
            yield return w.SendWebRequest();

            if (w.isNetworkError || w.isHttpError)
            {
                Debug.Log(w.error);
            }
            else
            {
                Jogadores jogadores = JsonUtility.FromJson<Jogadores>(w.downloadHandler.text);
                
                if(pontos > jogadores.objetos[0].exp)
                {
                    StartCoroutine(RegistraPontuacao());
                }
            }
        }
    }*/

    private IEnumerator RegistraPontuacao()
    {
        WWWForm wwwf = new WWWForm();
        wwwf.AddField("nick", jogadorLogado.nick);
        wwwf.AddField("pontos", pontos);

        using (var w = UnityWebRequest.Post(Config.EnderecoServer + "registraPontuacao.php", wwwf))
        {
            yield return w.SendWebRequest();

            if (w.isNetworkError || w.isHttpError)
            {
                Debug.Log(w.error);
            }
            else
            {
                if(w.downloadHandler.text.Equals("Update deu bom"))
                {
                    txtMsg.text = "Parabains! sua pontuação foi registrada com sucesso...";
                    jogadorLogado.exp = pontos;
                    PlayerPrefs.SetString("PlayerLogado", JsonUtility.ToJson(jogadorLogado));
                    ComecaJogo();
                }
                
            }
        }
    }
}
