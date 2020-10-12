using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    PiecesConstructor piecesConstructor;
    TableConstructor tableConstructor;
    List<List<BoardPiece>> playbleBoard;
    private bool turnController = false; // true black, false white 

    private static GameController _instance;
    public static GameController instance()
    {
        if (_instance != null)
        {
            return _instance;
        }

        _instance = FindObjectOfType<GameController>();

        if (_instance == null)
        {
            GameObject resourceObject = Resources.Load<GameObject>("GameController");
            if (resourceObject != null)
            {
                GameObject instanceObject = Instantiate(resourceObject);
                _instance = instanceObject.GetComponent<GameController>();
                DontDestroyOnLoad(instanceObject);
            }
            else
                Debug.Log("Resource does not have a definition for PiecesContructor");
        }
        return _instance;
    }

    private void Awake()
    {
        tableConstructor = TableConstructor.instance();
        piecesConstructor = PiecesConstructor.instance();
        playbleBoard = tableConstructor.GetPlaybleArea();
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                Debug.Log(hit.transform.gameObject.name);
                //playbleBoard[0].Find(hit.transform.gameObject.GetComponent<BoardPiece>());
            }
        }
    }

}
