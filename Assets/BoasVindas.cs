using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoasVindas : MonoBehaviour
{
    private Text txtBoasVindas;
    void Start()
    {
        txtBoasVindas = GetComponent<Text>();
        Jogador jogadorLogado = JsonUtility.FromJson<Jogador>(PlayerPrefs.GetString("PlayerLogado"));

        txtBoasVindas.text = "Olá " + jogadorLogado.nick + "\n" +
                             "seu level é " + jogadorLogado.level + "\n";
    }

}
