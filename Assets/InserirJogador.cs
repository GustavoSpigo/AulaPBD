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
    public void Inserir()
    {
        StartCoroutine(inserirJogador());
    }

    IEnumerator inserirJogador()
    {
        WWWForm wwwf = new WWWForm();
        wwwf.AddField("nick", inputNick.text);
        wwwf.AddField("email", inputEmail.text);
        wwwf.AddField("senha", MD5.Md5Sum(inputSenha.text));
        wwwf.AddField("genero", inputGenero.value);

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
}
