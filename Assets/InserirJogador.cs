using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class InserirJogador : MonoBehaviour
{
    public InputField inputNick;
    public InputField inputSenha;
    public InputField inputEmail;
    public Dropdown inputGenero;

    public Text msgErro;
    public void Inserir()
    {
        if (ValidacaoLocal())
        {
            StartCoroutine(VerificaSeExisteNick());
        }
        
    }
    IEnumerator VerificaSeExisteNick()
    {
        WWWForm wwwf = new WWWForm();
        wwwf.AddField("nick", inputNick.text);
        wwwf.AddField("email", inputEmail.text);

        using (var w = UnityWebRequest.Post("http://localhost/jogos/verificaNickEmail.php", wwwf))
        {
            yield return w.SendWebRequest();
            if (w.isNetworkError || w.isHttpError)
            {
                Debug.Log(w.error);
            }
            else
            {
                if (w.downloadHandler.text.Equals("Nick e Email nao repete"))
                {
                    StartCoroutine(inserirJogador());
                }
                else
                {
                    //deu erro
                }

            }
        }
    }
    IEnumerator inserirJogador()
    {
        WWWForm wwwf = new WWWForm();
        wwwf.AddField("nick", inputNick.text);
        wwwf.AddField("email", inputEmail.text);
        wwwf.AddField("senha", MD5.Md5Sum(inputSenha.text));
        wwwf.AddField("genero", inputGenero.options[inputGenero.value].text);

        using (var w = UnityWebRequest.Post("http://localhost/jogos/inserirJogador.php", wwwf))
        {
            yield return w.SendWebRequest();

            if (w.isNetworkError || w.isHttpError)
            {
                Debug.Log(w.error);
            }
            else
            {
                if(w.downloadHandler.text.Equals("Insert deu bom"))
                {
                    //Foi inserido
                }
                else
                {
                    //deu erro
                }
                
            }
        }
    }

    private bool ValidacaoLocal()
    {
        if (inputNick.text.Trim().Length < 3)
        {
            msgErro.text = "O nick precisa ter 3 ou mais caracteres";
            return false;
        }
        if(inputNick.text.Trim().Contains(" "))
        {
            msgErro.text = "O nick não pode conter espaços";
            return false;
        }
        if (inputEmail.text.Trim().Length == 0)
        {
            msgErro.text = "Informe o e-mail";
            return false;
        }
        return true;
    }
}
