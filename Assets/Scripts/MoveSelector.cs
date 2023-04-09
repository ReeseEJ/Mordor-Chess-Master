/*
 * Copyright (c) 2018 Razeware LLC
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish,
 * distribute, sublicense, create a derivative work, and/or sell copies of the
 * Software in any work that is designed, intended, or marketed for pedagogical or
 * instructional purposes related to programming, coding, application development,
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works,
 * or sale is expressly withheld.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSelector : MonoBehaviour
{
    public GameObject moveLocationPrefab;
    public GameObject tileHighlightPrefab;
    public GameObject attackLocationPrefab;

    private GameObject tileHighlight;
    private GameObject movingPiece;
    private List<Vector2Int> moveLocations;
    private List<GameObject> locationHighlights;


    void Start()
    {
        this.enabled = false;
        tileHighlight = Instantiate(tileHighlightPrefab, Geometry.PointFromGrid(new Vector2Int(0, 0)),
            Quaternion.identity, gameObject.transform);
        tileHighlight.SetActive(false);
    }

    void Update()
    {
        Debug.Log("move selector: update");
        ExitState();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 point = hit.point;
            Vector2Int gridPoint = Geometry.GridFromPoint(point);

            tileHighlight.SetActive(true);
            tileHighlight.transform.position = Geometry.PointFromGrid(gridPoint);
            if (Input.GetMouseButtonDown(0))
            {
                // Reference Point 2: check for valid move location
                if (!moveLocations.Contains(gridPoint))
                {
                    return;
                }

                if (GameManager.instance.PieceAtGrid(gridPoint) == null)
                {
                    GameManager.instance.Move(movingPiece, gridPoint);
                }
                else
                {
                    GameManager.instance.CapturePieceAt(gridPoint);
                    GameManager.instance.Move(movingPiece, gridPoint);
                }
                // Reference Point 3: capture enemy piece here later
                ExitState();
            }
        }
        else
        {
            tileHighlight.SetActive(false);
        }
    }

    private void CancelMove()
    {
        this.enabled = false;

        foreach (GameObject highlight in locationHighlights)
        {
            Destroy(highlight);
        }

        GameManager.instance.DeselectPiece(movingPiece);
        TileSelector selector = GetComponent<TileSelector>();
        selector.EnterState();
    }

    public void EnterState(GameObject piece)
    {
        Debug.Log("enter state" + piece);
        movingPiece = piece;
        this.enabled = true;

        moveLocations = GameManager.instance.MovesForPiece(movingPiece);
        locationHighlights = new List<GameObject>();

        if (moveLocations.Count == 0)
        {
            CancelMove();
        }

        foreach (Vector2Int loc in moveLocations)
        {
            GameObject highlight;
            if (GameManager.instance.PieceAtGrid(loc))
            {
                highlight = Instantiate(attackLocationPrefab, Geometry.PointFromGrid(loc), Quaternion.identity, gameObject.transform);
            }
            else
            {
                highlight = Instantiate(moveLocationPrefab, Geometry.PointFromGrid(loc), Quaternion.identity, gameObject.transform);
            }
            locationHighlights.Add(highlight);
        }
    }
    /* added function to list possible live pieces for AI */
    /* pdave */
    private GameObject selectAI()
    {
        List<GameObject> AIpieces = new List<GameObject>();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Vector2Int gridPoint = Geometry.GridPoint(i, j);
                GameObject selectedPiece = GameManager.instance.PieceAtGrid(gridPoint);
                if (GameManager.instance.DoesPieceBelongToCurrentPlayer(selectedPiece))
                {
                    AIpieces.Add(selectedPiece);
                }
            }
        }
        //Debug.Log("Found " + AIpieces.Count + "AI pieces");
        int randomNumber = Random.Range(0, AIpieces.Count); // Generates a random integer between 1 and 100
        //Debug.Log("Random int: " + randomNumber);
        List<Vector2Int> movesforpiece = GameManager.instance.MovesForPiece(AIpieces[randomNumber]);

        if (movesforpiece.Count == 0)
        {
            return selectAI();
        }
        else
        {
            return AIpieces[randomNumber];
        }
    }

    /* added move AI function that makes a move*/
    /* pdave */
    private void MoveAI()
    {
        //Debug.Log("Inside MoveAI");
        this.enabled = true;
        //Debug.Log("this.enabled = true");

        /* want a way to get a specific black pawn */
        //Vector2Int gridPoint = Geometry.GridPoint(7,6);
        //GameObject selectedPiece = GameManager.instance.PieceAtGrid(gridPoint);
        //Debug.Log("The piece, " + selectedPiece + ", is selected!");

        GameObject selectedPiece = selectAI();
        //Debug.Log("The new piece is : " + selectedPiece);


        /* based on where the black pawn is, select a possible target*/
        List<Vector2Int> movesforpiece = GameManager.instance.MovesForPiece(selectedPiece);

        int randomNumber = Random.Range(0, movesforpiece.Count);

        //Debug.Log("Number of possible moves are " + movesforpiece.Count);
        //Debug.Log("Got possible movesforpiece " + movesforpiece[randomNumber]);


        // /* execute the move to the target */

        if (GameManager.instance.PieceAtGrid(movesforpiece[randomNumber]) == null)
        {
            GameManager.instance.Move(selectedPiece, movesforpiece[randomNumber]);
        }
        else
        {
            GameManager.instance.CapturePieceAt(movesforpiece[randomNumber]);
            GameManager.instance.Move(selectedPiece, movesforpiece[randomNumber]);
        }

        //private Stockfish stockfish;

        //GameManager.instance.Move(selectedPiece, movesforpiece[randomNumber]);

        this.enabled = false;
        //Debug.Log("this.enabled = false");

    }

    private void ExitState()
    {
        this.enabled = false;
        TileSelector selector = GetComponent<TileSelector>();
        tileHighlight.SetActive(false);
        GameManager.instance.DeselectPiece(movingPiece);
        movingPiece = null;
        GameManager.instance.NextPlayer();
        /* check if gamemanager's current player is black */
        /* pdave */

        //if (GameManager.instance.currentPlayer.name == "black")
        //{
            //Debug.Log("Current player is black");
        MoveAI();
            //GameManager.instance.NextPlayer();
        //}

        selector.EnterState();
        foreach (GameObject highlight in locationHighlights)
        {
            Destroy(highlight);
        }
    }
}
