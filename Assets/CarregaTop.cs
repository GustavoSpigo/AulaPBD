using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class CarregaTop : MonoBehaviour
{
    public Text[] txtPosicoes;
    public Text[] txtPontuacoes;
    private Jogador jogadorLogado;

    private void Start()
    {
        if (!PlayerPrefs.GetString("PlayerLogado", string.Empty).Equals(string.Empty))
        {
            jogadorLogado = JsonUtility.FromJson<Jogador>(PlayerPrefs.GetString("PlayerLogado"));
        }
        
        StartCoroutine(CarregaTops());
    }

    private IEnumerator CarregaTops()
    {
        WWWForm wwwf = new WWWForm();

        using (var w = UnityWebRequest.Post(Config.EnderecoServer  + "listaTops.php", wwwf))
        {
            yield return w.SendWebRequest();

            if (w.isNetworkError || w.isHttpError)
            {
                Debug.Log(w.error);
            }
            else
            {
                Jogadores jogadores = JsonUtility.FromJson<Jogadores>(w.downloadHandler.text);

                for (int i = 0; i < 3; i++)
                {
                    txtPosicoes[i].text = jogadores.objetos[i].nick;
                    txtPontuacoes[i].text = jogadores.objetos[i].exp.ToString();
                    if (!PlayerPrefs.GetString("PlayerLogado", string.Empty).Equals(string.Empty))
                    {
                        if (jogadorLogado.nick.Equals(jogadores.objetos[i].nick))
                        {
                            txtPosicoes[i].color = new Color(1, 0, 1);
                            txtPontuacoes[i].color = new Color(1, 0, 1);
                        }
                    }
                    
                }
            }
        }
    }
}
