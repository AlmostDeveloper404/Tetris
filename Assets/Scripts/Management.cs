using System.Collections.Generic;
using UnityEngine;

public class Management : MonoBehaviour
{
    public Tetramino[] Tetraminos;

    public Tetramino CurrentTetramino;
    float _timer;
    [SerializeField] float speedRate = 0.2f;

    public int Width;
    public int Height;

    public Dictionary<Vector2Int, Part> lots = new Dictionary<Vector2Int, Part>();


    private void Start()
    {        
        CreateTetramino();
    }

  
    private void Update()
    {
        _timer += Time.deltaTime;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            speedRate = 0.1f;
        }
        else
        {
            speedRate = 0.2f;
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (CanMoveTowards(Vector2Int.left))
            {
                CurrentTetramino.MoveTetramino(Vector2Int.left);
            }
            
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (CanMoveTowards(Vector2Int.right))
            {
                CurrentTetramino.MoveTetramino(Vector2Int.right);
            }

        }
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            CurrentTetramino.RotateCW();
            if (!CanMoveTowards(Vector2Int.zero))
            {
                CurrentTetramino.RotateCCW();
            }
        }
        if (_timer > speedRate)
        {
            _timer = 0;

            if (CanMoveTowards(Vector2Int.down))
            {
                CurrentTetramino.MoveTetramino(Vector2Int.down);
            }
            else
            {
                AddToDictionary();
                CheckFullLine();
                CreateTetramino();
            }
            
        }
    }
    bool CanMoveTowards(Vector2Int dir)
    {
        
        for (int i = 0; i < CurrentTetramino.parts.Length; i++)
        {
            Part part = CurrentTetramino.parts[i];
            Vector2Int nextPos=new Vector2Int(
                Mathf.RoundToInt( part.transform.position.x), 
                Mathf.RoundToInt(part.transform.position.y)
                )  + dir;
            
            
            if (nextPos.y < 0)
            {
                return false;
            }
            if (lots.ContainsKey(nextPos))
            {
                return false;
            }
            if (nextPos.x >= Width)
            {
                return false;
            }
            if (nextPos.x < 0)
            {
                return false;
            }
        }
        return true;

    }
    void AddToDictionary()
    {
        for (int i = 0; i < CurrentTetramino.parts.Length; i++)
        {
            Part part = CurrentTetramino.parts[i];
            Vector2Int key = new Vector2Int(
                Mathf.RoundToInt( part.transform.position.x), 
                Mathf.RoundToInt(part.transform.position.y)
                );
            lots.Add(key,part);
        }
    }

    public void CreateTetramino()
    {
        int randomIndex = Random.Range(0, Tetraminos.Length);
        CurrentTetramino = Instantiate(Tetraminos[randomIndex]);
    }
    void CheckFullLine()
    {
        bool[] allLines = new bool[Height];
        for (int y = 0; y < Height; y++)
        {
            bool isFullLine = true;
            for (int x = 0; x < Width; x++)
            {
                Vector2Int key = new Vector2Int(x, y);
                
                if (!lots.ContainsKey(key))
                {
                    isFullLine = false;
                    break;
                }
            }
            if (isFullLine)
            {
                allLines[y] = true;
            }
        }
        DeleteFullLines(allLines);
    }

    void DeleteFullLines(bool[] lines)
    {
        int deletedLinesCount = 0;

        for (int y = 0; y < Height; y++)
        {
            if (lines[y])
            {
                for (int x = 0; x < Width; x++)
                {
                    Vector2Int key = new Vector2Int(x,y);
                    Part part = lots[key];
                    lots.Remove(key);
                    part.DestroyPart();
                }
                deletedLinesCount++;
            }
            else
            {
                if (deletedLinesCount>0)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        Vector2Int key = new Vector2Int(x, y);
                        if (lots.ContainsKey(key))
                        {
                            Part part = lots[key];
                            part.MoveDown(Vector3.down*deletedLinesCount);
                            lots.Remove(key);
                            lots.Add(key + Vector2Int.down * deletedLinesCount, part);
                        }
                    }
                }
            }
        }
    }
    
}
