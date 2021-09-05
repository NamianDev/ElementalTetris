using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TetrisBlock : MonoBehaviour
{
    public Vector3 PivotPoint;
    private float previousTime;
    float fallSpeed;
    int countDestroyBlock;
    static int height = 15;
    static int width = 8;
    Spawn Spawner;
    DrawTetramino DT;

    int FirstDominationElement;
    int SecondDominationElement;

    private static Transform[,] grid = new Transform[width, height + 1];
    public static int[,] gridElementals = new int[width, height + 1];

    private int[] StringNumbers = new int[6];
    void Start()
    {
        Spawner = FindObjectOfType<Spawn>();
        DT = GetComponent<DrawTetramino>();

        fallSpeed = Spawner.fallTime;
        FirstDominationElement = -1;
        SecondDominationElement = -1;
    }
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            ValidMove(new Vector3(-1, 0, 0));
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            ValidMove(new Vector3(1, 0, 0));
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            transform.RotateAround(transform.TransformPoint(PivotPoint), new Vector3(0, 0, 1), 90);
            foreach (Transform child in transform)
            {
                child.Rotate(new Vector3(0, 0, 1), -90);
            }
            if (!ValidMove(Vector3.zero))
            {
                transform.RotateAround(transform.TransformPoint(PivotPoint), new Vector3(0, 0, 1), -90);
                foreach (Transform child in transform)
                {
                    child.Rotate(new Vector3(0, 0, 1), 90);
                }
            }

        }
        if (Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) ? fallSpeed / 10 : fallSpeed))
        {
            if (!ValidMove(new Vector3(0, -1, 0)))
            {
                if (CheckLose())
                {
                    return;
                } //Если проигрышь, то выходим из метода
                AddToGrid();
                CheckForLines();
                this.enabled = false;
                FindObjectOfType<DestroyTetraminoStatic>().DestroyThisObj();
                Spawner.NewTetramino();
            }
            previousTime = Time.time;
        }
    }
    bool ValidMove(Vector3 NewVector3)
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x + NewVector3.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y + NewVector3.y);

            if (roundedX < 0 || roundedX == width || roundedY < 0 || roundedY >= height)
            {
                return false; //Нельзя
            }
            else if (grid[roundedX, roundedY] != null)
            {
                return false; //Нельзя
            }
        }
        transform.position += NewVector3;
        return true; //Позиция изменилась
    }
    void AddToGrid()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            grid[roundedX, roundedY] = children;
            gridElementals[roundedX, roundedY] = DT.RM.CurentBlockElement;
        }
    }

    void CheckForLines()
    {
        for (int i = height - 1; i >= 0; i--)
        {
            if (HaveLine(i))
            {
                CheckElementsBeforeDelete(i);
                System.Array.Clear(StringNumbers, 0, 5);
            }
        }
    }
    bool CheckLose()
    {
        for (int j = 0; j < width; j++)
        {
            if (grid[j, height - 1] != null)
            {
                Debug.Log("EndGame");
                Time.timeScale = 0;
                return true;
            }
        }
        return false;
    }
    bool HaveLine(int lineID)
    {
        for (int j = 0; j < width; j++)
        {
            if (grid[j, lineID] == null)
                return false;
        }
        return true;
    }  //Проверка на полноту линии
    void DeleteLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            if (grid[j, i] == null)
            {
                continue;
            }
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
            gridElementals[j, i] = 0;

        }
        RowDown(i + 1);
        Spawner.Score += 100;
        Spawner.ScoreText.text = "Score:" + Spawner.Score;
    }
    void RowDown(int i)
    {
        for (int y = i; y < height; y++)
        {
            for (int j = 0; j < width; j++)
            {
                if (grid[j, y] != null)
                {
                    grid[j, y - 1] = grid[j, y];
                    gridElementals[j, y - 1] = gridElementals[j, y];
                    grid[j, y] = null;
                    gridElementals[j, y] = 0;
                    grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }

            }
        }
    }
    void CheckElementsBeforeDelete(int i)
    {
        #region
        for (int j = 0; j < width; j++)
        {
            switch (gridElementals[j, i])
            {
                case 0:
                    Debug.Log("Error");
                    return;
                case 1:
                    StringNumbers[0]++;
                    break;
                case 2:
                    StringNumbers[1]++;
                    break;
                case 3:
                    StringNumbers[2]++;
                    break;
                case 4:
                    StringNumbers[3]++;
                    break;
                case 5:
                    StringNumbers[4]++;
                    break;
                case 6:
                    StringNumbers[5]++;
                    break;
            }
        }
        //      Debug.Log(StringNumbers[0] + " " + StringNumbers[1] + " " + StringNumbers[2] + " " + StringNumbers[3] + " " + StringNumbers[4] + " " + StringNumbers[5]);
        for (int z = 0; z < StringNumbers.Length; z++)
        {
            switch (StringNumbers[z])
            {

                case 4:
                    if (FirstDominationElement == -1)
                    {
                        //       Debug.Log("Значение z " + z);
                        FirstDominationElement = z;
                        //        Debug.Log(FirstDominationElement + "   " + SecondDominationElement);
                    }
                    else if (FirstDominationElement != -1 && SecondDominationElement == -1)
                    {
                        SecondDominationElement = z;
                        //      Debug.Log("Значение z " + z);
                        //      Debug.Log(FirstDominationElement + "   " + SecondDominationElement);
                    }
                    break;
                case 5:
                case 6:
                case 7:
                case 8:
                    FirstDominationElement = z;
                    // Debug.Log("Значение z " + z);
                    //   Debug.Log(FirstDominationElement + "   " + SecondDominationElement);
                    break;

            }

        }
        #endregion

        switch (FirstDominationElement)
        {
            case -1:
                Debug.Log("Нет преобладающего элемента");
                DeleteLine(i);
                break;
            case 0:
                Debug.Log("Преобадащий элемент - огонь");
                FireElementDelete(i);
                break;
            case 1:
                Debug.Log("Преобадащий элемент - вода");
                WaterElementDelete(i);
                break;
            case 2:
                Debug.Log("Преобадащий элемент - земля");
                EarthElementDelete(i);
                break;
            case 3:
                Debug.Log("Преобадащий элемент - воздух");
                AirElementDelete(i);
                break;
            case 4:
                Debug.Log("Преобадащий элемент - тьма");
                DarkElementDelete(i);
                break;
            case 5:
                Debug.Log("Преобадащий элемент - молния");
                LightningElementDelete(i);
                break;
        }
    }
    #region ElementCombo
    void FireElementDelete(int i)
    {
        DeleteLine(i + 1);
        DeleteLine(i);
    }//Взаимодействие при преобладании огненных блоков
    void EarthElementDelete(int i)
    {
        if (i != 0)
        {
            DeleteLine(i);
        }
        DeleteLine(0);
    }//Взаимодействие при преобладании земляных блоков
    void LightningElementDelete(int i)
    {
        DeleteLine(i);

        int rand = Random.Range(0, 7);
        for (int z = height - 1; z >= 0; z--)
        {
            if (grid[rand, z] == null)
            {
                continue;
            }
            else if (countDestroyBlock <= 3)
            {
                countDestroyBlock += 1;
                Debug.Log(countDestroyBlock);
                Destroy(grid[rand, z].gameObject);
                grid[rand, z] = null;
                gridElementals[rand, z] = 0;
            }
        }
        countDestroyBlock = 0;
    }
    void WaterElementDelete(int i)
    {

        if (Spawner.fallTime < 0.8f)
        {
            Spawner.coof += 0.1f;
        }
        DeleteLine(i);
    }
    void DarkElementDelete(int i)
    {
        DeleteLine(i);
    }
    void AirElementDelete(int i)
    {
        for (int z = height - 3; z >= 0; z--)
        {
            if (!HaveLine(z))
            {
                DeleteLine(z);
                break;
            }

        }
        DeleteLine(i);

    }
    #endregion
}
