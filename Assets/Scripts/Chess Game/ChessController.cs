using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessController
{
    Board board;

    public ChessController(Board board)
    {
        this.board = board;
    }

    private List<Piece> pieces => board.Pieces;

    private TeamColor activePlayer
    {
        get => board.activePlayer;
        set => board.SetActivePlayer(value);
    }

    private bool gameOver
    {
        get => board.gameOver;
        set => board.SetGameOver(value);
    }
    

    // returns whether or not move was made
    public bool SendMove(TeamColor tc, Piece piece, ChessMove move, ChessPlayer sender = null)
    {
        if (tc != activePlayer || gameOver)
        {
            Debug.Log("wrong player");
            return false;
        }

        Debug.Log(tc + " " + activePlayer);

        board.SetPiecePosition(board.GetPieceID(piece.Position), move.destination);

        piece.hasMoved = true;

        // if piece captured king

        // else:

        if (sender != null)
        {
            sender.Switch();
        }

        board.StartCoroutine(board.FlipBoard());


        //

        if (tc == TeamColor.Black)
        {
            board.NumMoves++;
        }

        activePlayer = activePlayer == TeamColor.White ? TeamColor.Black : TeamColor.White;
        return true;
    }

    public void OnStart()
    {
        return;
    }

    public void OnEnd()
    {
        return;
    }

    public void OnUpdate()
    {
        return;
    }
}