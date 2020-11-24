using System;
using System.Collections.Generic;
using UnityEngine;

public class IAPlayerController : MonoBehaviour
{
    [SerializeField]
    private bool isIAPlayerBlack;

    private static IAPlayerController _instance;
    public GameController gameController;
    public static IAPlayerController Instance()
    {

        if (_instance == null)
        {
            GameObject resourceObject = Resources.Load<GameObject>("IAPlayerController");
            if (resourceObject != null)
            {
                GameObject instanceObject = Instantiate(resourceObject);
                _instance = instanceObject.GetComponent<IAPlayerController>();
                DontDestroyOnLoad(instanceObject);
            }
            else
                Debug.Log("Resource does not have a definition for IAPlayerController");
        }
        return _instance;

    }
    public bool GetIsIAPlayerBlack() 
    {
        return isIAPlayerBlack;
    }
    public void SetGameController()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        
    }

    private List<List<string>> MakePicesMatrix()
    {
        List<List<Piece>> PicesMatriz = gameController.GetCurrentTable().GetPiecesPosition();
        List<List<string>> picesMatrix = new List<List<string>>();
        foreach (List<Piece> piceRow in PicesMatriz)
        {
            List<string> picesCharRow = new List<string>();
            foreach (Piece pice in piceRow)
            {
                if (!pice)
                    picesCharRow.Add("");
                else
                {
                    string letter = "";
                    if (pice.GetIsBlack())
                        letter = "b";
                    else
                        letter = "w";
                    if(pice.GetKingStatus())
                        letter.ToUpper();
                     picesCharRow.Add(letter);
                }
            }
            picesMatrix.Add(picesCharRow);
        }
        return picesMatrix;
    }
    private void PrintPicesMatrix(List<List<string>> matrix)
    {
        Debug.Log("pices");
        foreach (List<string> piceRow in matrix)
        {
            Debug.Log(String.Join("", piceRow.ToArray()));
        }
    }
    private bool IsKing(string pice)
    {
        return pice.ToUpper() == pice;
    }
    private bool IsPlaybile(List<List<string>> matrix, int row, int colum)
    {
        int limit = matrix.Count-1;
        if ( ( (row < 0) || (row > limit) ) || ( ( colum < 0 ) || ( colum > limit ) ) )
            return false;

        return matrix[row][colum] == "";
    }
    private bool IsEatable(List<List<string>> matrix, int row, int colum)
    {
        int maxLength = matrix.Count - 1;
        for (int i = -1; i <= 1; i++)
        {
            if ( (row + i > maxLength) || (row + i < 0) )
                continue;
            if (i == 0)
                continue;
            for (int j = -1; j <=1 ; j++)
            {
                if ((colum + j > maxLength) || (colum + j < 0))
                    continue;

                if  (j == 0)
                    continue;

                int newColum = colum + j;
                int newRow = row + i;

                int destinationRow = newRow;
                int destinationColum = newColum;

                if (i < 0)
                    destinationRow -= 1;
                else
                    destinationRow += 1;
                if (j < 0)
                    destinationColum -= 1;
                else
                    destinationColum += 1;

                if ((destinationRow > maxLength) || (destinationRow < 0))
                    continue;

                if ((destinationColum > maxLength) || (destinationColum < 0))
                    continue;

                if ((isIAPlayerBlack) && ( matrix[newRow][newColum].ToLower() == "w"))
                    if(matrix[destinationRow][destinationColum] == "")
                        return true;

                if ((!isIAPlayerBlack) && (matrix[newRow][newColum].ToLower() == "b"))
                    if (matrix[destinationRow][destinationColum] == "")
                        return true;
            }
        }
        return false;
    }
    //EssaFunção irá retornar um vetor de 3 posições sendo a primeira  o valor a linha a segunda a e a terceira a linha e coluna 

    private int[] CanIaEat(List<List<string>> matrix, int row, int colum)
    {
        int maxLength = matrix.Count - 1;

        for (int i = -1; i <= 1; i++)
        {
            if ((row + i > maxLength) || (row + i < 0))
                continue;
            if (i == 0)
                continue;
            for (int j = -1; j <= 1; j++)
            {

                if ((colum + j > maxLength) || (colum + j < 0))
                    continue;

                if (j == 0)
                    continue;

                if (((isIAPlayerBlack) && (matrix[row + i][colum + j].ToLower() == "w")) || ((!isIAPlayerBlack) && (matrix[row + i][colum + j].ToLower() == "b")) )
                {
                    int targetRow = row + i;
                    int targetColum = colum + j;

                    if ( ( (j < 0) && (targetColum - 1 < 0) ) || ( (j > 0) && (targetColum + 1 > maxLength) ) )
                        continue;
                    if ( ( (i < 0) && (targetRow - 1 < 0) ) || ( (i > 0) && (targetRow + 1 > maxLength) ) )
                        continue;

                    int destinationRow = targetRow;
                    int destinationColum = targetColum;

                    if (i < 0)
                        destinationRow -= 1;
                    else
                        destinationRow += 1;
                    if (j < 0)
                        destinationColum -= 1;
                    else
                        destinationColum += 1;

                    if ((destinationRow > maxLength) || (destinationRow < 0))
                        continue;

                    if ((destinationColum > maxLength) || (destinationColum < 0))
                        continue;

                    if (matrix[destinationRow][destinationColum] != "")
                        continue;

                    int value = 10;
                    if (IsEatable(matrix, destinationRow, destinationColum))
                        value -= 1;
                    string oldeString = matrix[targetRow][targetColum];
                    matrix[targetRow][targetColum] = "";
                    int[] carriedValue = CanIaEat(matrix, destinationRow, destinationColum);
                    if (carriedValue != null)
                        value += carriedValue[0];
                    matrix[targetRow][targetColum] = oldeString;
                    return new int[3] { value, destinationRow, destinationColum };
                    

                }

                
            }
        }
        return null;
    }
    private int[] NeedToEscape(List<List<string>> matrix, bool isUp, int row, int colum)
    {
        if (IsEatable(matrix, row, colum))
        {
            return SimpleMove(matrix, isUp, row,  colum);
        }
        return null;
    }
    private int[] ChoseAMove(Dictionary<string, int> moves)
    {
        if (moves.Keys.Count == 0)
            return null;
        List<string> bestMoves = new List<string>();
        int bestValue = -1;
        foreach (string key in moves.Keys)
        {
            if(moves[key] == bestValue)
            {
                bestMoves.Add(key);
            }
            if (moves[key] > bestValue)
            {
                bestValue = moves[key];
                bestMoves = new List<string>();
                bestMoves.Add(key);

            }
        }
        if (bestMoves.Count == 1)
        {
            string key = bestMoves[0];
            string[] position = key.Split('-');
            return new int[3] { bestValue, int.Parse(position[0]), int.Parse(position[1]) };
        }
        if (bestMoves.Count > 1)
        {
            int max = bestMoves.Count;
            int chosedMovent = UnityEngine.Random.Range(0, max);
            string key = bestMoves[chosedMovent];
            string[] position = key.Split('-');
            return new int[3] { bestValue, int.Parse(position[0]), int.Parse(position[1]) };
        }
        return null;
    }
    private int[] SimpleMove(List<List<string>> matrix, bool isUp, int row, int colum)
    {
        bool isKing = IsKing(matrix[row][colum]);
        int max = matrix.Count - 1;
        Dictionary<string, int> moves = new Dictionary<string, int>();
        int currentOffset = 1;
        if ( ( (isUp) && ((row - currentOffset) > -1) ) || ( isKing ) )
        {
            // esquerda inferior 
            if (IsPlaybile(matrix, row - currentOffset, colum -  currentOffset))
            {
                moves.Add((row - currentOffset).ToString() + "-" + (colum -  currentOffset).ToString(), 1);
                if (IsEatable(matrix, (row - currentOffset), (colum -  currentOffset)))
                {
                    int value = moves[(row - currentOffset).ToString() + "-" + (colum -  currentOffset).ToString()];
                    moves[(row - currentOffset).ToString() + "-" + (colum -  currentOffset).ToString()] = value - 1;

                }
            }
            // direita inferior 
            if (IsPlaybile(matrix, row - currentOffset, colum + currentOffset))
            {
                moves.Add((row - currentOffset).ToString() + "-" + (colum + currentOffset).ToString(), 1);
                if (IsEatable(matrix, (row - currentOffset), (colum + currentOffset)))
                {
                    int value = moves[(row - currentOffset).ToString() + "-" + (colum + currentOffset).ToString()];
                    moves[(row - currentOffset).ToString() + "-" + (colum + currentOffset).ToString()] = value - 1;

                }
            }
        }
        if (((!isUp) && ((row + currentOffset) > max)) || (isKing))
        {
            // esquerda inferior 
            if (IsPlaybile(matrix, row + currentOffset, colum - currentOffset))
            {
                moves.Add((row + currentOffset).ToString() + "-" + (colum - currentOffset).ToString(), 1);
                if (IsEatable(matrix, (row + currentOffset), (colum -  currentOffset)))
                {
                    int value = moves[(row + currentOffset).ToString() + "-" + (colum -  currentOffset).ToString()];
                    moves[(row + currentOffset).ToString() + "-" + (colum -  currentOffset).ToString()] = value - 1;

                }
            }
            // direita inferior 
            if (IsPlaybile(matrix, row + currentOffset, colum + currentOffset))
            {
                moves.Add((row + currentOffset).ToString() + "-" + (colum + currentOffset).ToString(), 1);
                if (IsEatable(matrix, (row + currentOffset), (colum + currentOffset)))
                {
                    int value = moves[(row + currentOffset).ToString() + "-" + (colum + currentOffset).ToString()];
                    moves[(row + currentOffset).ToString() + "-" + (colum + currentOffset).ToString()] = value - 1;

                }
            }
        }
        return ChoseAMove(moves);
    }
    //Tadas aas funções retorma em 0 o valor 1 linha 2 coluna nesta ordem
    private int[] SimulateMove(List<List<string>> matrix, bool isUp, int row,int colum)
    {
        int[] eatMovemt = CanIaEat(matrix, row, colum);

        if (eatMovemt != null)
            return eatMovemt;

        int[] escapeMove = NeedToEscape(matrix, isUp, row, colum);
        if (escapeMove != null)
        {
            escapeMove[0] += 5;
            return escapeMove;
        }

        int[] simpleMovemt =  SimpleMove(matrix, isUp, row, colum);
        if (simpleMovemt != null)
            return simpleMovemt;
        return null;
    
    }
    /**
     * ChoseAMove retorno 
     * jogada[0] points
     * jogada[1] piceRow
     * jogada[2] piceColum
     * jogada[3] targetRow
     * jogada[4] targetColum
     * 
     * **/
    private int[] ChoseAMove(List<List<string>> matrix,bool isUp)
    {
        int matrixLengh = matrix.Count;
        int bestValue = -1;
        List<List<int>> bestMoves = new List<List<int>>();
        /**
         * bestMoves[X][0] piceRow
         * bestMoves[X][1] piceColum
         * bestMoves[X][2] targetRow
         * bestMoves[X][3] targetColum
         * **/
        for (int row = 0; row < matrixLengh; row++)
        {
            for (int colum = 0; colum < matrixLengh; colum++)
            {
                if (( (matrix[row][colum] == "B" || matrix[row][colum] == "b") && isIAPlayerBlack) || ( (matrix[row][colum] == "W" || matrix[row][colum] == "w") && !isIAPlayerBlack))
                {
                    int[] move =  SimulateMove(matrix, isUp, row, colum);
                    if (move == null)
                        continue;

                    List<int> moveCorrent = new List<int>();
                    int points = move[0];
                    if(bestValue == points)
                    {
                        moveCorrent.Add(row);
                        moveCorrent.Add(colum);
                        moveCorrent.Add(move[1]);
                        moveCorrent.Add(move[2]);
                        bestMoves.Add(moveCorrent);
                    }
                    if (points > bestValue)
                    {
                        bestMoves = new List<List<int>>();
                        moveCorrent.Add(row);
                        moveCorrent.Add(colum);
                        moveCorrent.Add(move[1]);
                        moveCorrent.Add(move[2]);
                        bestMoves.Add(moveCorrent);
                        bestValue = points;
                    }
                }
                  
            }
        }
        if (bestMoves.Count == 1)
        {
            List<int> theBestMove = bestMoves[0];
            return new int[5]  { bestValue, theBestMove[0], theBestMove[1], theBestMove[2], theBestMove[3] };
        }
        if (bestMoves.Count> 1)
        {
            int max = bestMoves.Count;
            int chosedMovent = UnityEngine.Random.Range(0, max);
            List<int> theBestMove = bestMoves[chosedMovent];
            return new int[5] { bestValue, theBestMove[0], theBestMove[1], theBestMove[2], theBestMove[3] };
        }
        return null;
    }
    private bool GetIsUp()
    {
        List<List<Piece>> PicesMatriz = gameController.GetCurrentTable().GetPiecesPosition();
        foreach (List<Piece> piceRow in PicesMatriz)
        {
            List<string> picesCharRow = new List<string>();
            foreach (Piece pice in piceRow)
            {
                if (pice)
                {
                    if ((pice.GetIsBlack() && isIAPlayerBlack) || (!pice.GetIsBlack() && !isIAPlayerBlack))
                    {
                        return pice.GetIsUp();
                    }
                }
            }
        }
        return false;
    }
    public bool MakeAMove()
    {
        List<List<string>> picesMatrix = MakePicesMatrix();
        //PrintPicesMatrix(picesMatrix);
        bool isUp = GetIsUp();
        int[] movemnt = ChoseAMove(picesMatrix, isUp);
        if (movemnt == null)
            return false;
        int points = movemnt[0];
        int piceRow = movemnt[1];
        int piceColum = movemnt[2];
        int targetRow = movemnt[3];
        int targetColum = movemnt[4];


        string piceName = piceRow.ToString() + " " + piceColum.ToString();

        GameObject piceSelected = gameController.GetCurrentTable().GetPiecesPosition()[piceRow][piceColum].gameObject;
        if (!piceSelected)
        {
            Debug.LogError("No Pice with the name => " + piceName);
            return false;
        }

        piceSelected.GetComponent<PieceTrigger>().OnMouseDown();
        

        string boardPiceName =  targetRow.ToString() + " " + targetColum.ToString() + " board";
        GameObject boardPiceSelected = gameController.GetCurrentTable().GetCurrentBoard().GetBoardMatrix()[targetRow][targetColum].gameObject;
        if (!boardPiceSelected)
        {
            Debug.LogError("No Board Pice with the name => " + boardPiceName);
            return false;
        }
        boardPiceSelected.GetComponent<BoardTrigger>().OnMouseDown();
        return true;
    }
}
