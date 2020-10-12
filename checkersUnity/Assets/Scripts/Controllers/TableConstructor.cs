using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableConstructor : MonoBehaviour
{
    [SerializeField]
    private Transform centralPosition;
    GameObject boardGameObject;
    private Board board;

    private void Awake()
    {
        BoardCounstructor();

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BoardCounstructor() 
    {
        boardGameObject = Resources.Load<GameObject>("Board");
        Instantiate(boardGameObject, centralPosition);
        board = boardGameObject.GetComponent<Board>();
        var boardPieces = board.GetBoardMatrix();
        Debug.Log(boardPieces);
        //int totalBoardPieces = boardPieces.Length *
    }
}
