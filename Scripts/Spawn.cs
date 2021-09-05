using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Spawn : MonoBehaviour
{
    public GameObject[] Tetraminoes;

    public Text ScoreText;
    public Text TimeText;
    public Text SpeedText;

    public float time;
    public float coof;

    int NextTetraminoID;
    bool FirstTetramino;
    public GameObject[] NextTetraminoObject;
    public Transform TransformNextObj;
    public ResourseManager RM;
    public float fallTime = 0.8f;
    public int Score;

    int minute;
    int second;
    void Start()
    {
        Application.targetFrameRate = 60;
        ScoreText.text = "Score:" + Score;
        NewTetramino();
    }

    public void NewTetramino()
    {
       
        if (!FirstTetramino)
        {
            RM.NextTetraminoElement = 0;
            Instantiate(Tetraminoes[Random.Range(0, Tetraminoes.Length)], transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(Tetraminoes[NextTetraminoID], transform.position, Quaternion.identity);

        }
        NextTetrammino();
    }
    public void NextTetrammino()
    {
        NextTetraminoID = Random.Range(0, Tetraminoes.Length);
        GameObject Gm = Instantiate(NextTetraminoObject[NextTetraminoID], TransformNextObj.transform.position, Quaternion.identity);
        Gm.transform.SetParent(TransformNextObj);
        FirstTetramino = true;
    }

    private void FixedUpdate()
    {
        time += 0.02f;
        minute = (int)(time / 60);
        fallTime = 0.8f - (minute * 0.1f) + coof;
        SpeedText.text = fallTime.ToString();
        second = (int)(time % 60);
        if (second < 10)
        {
            TimeText.text = minute + ":0" + second;
        }
        else
        {
            TimeText.text = minute + ":" + second;
        }
    }
}
