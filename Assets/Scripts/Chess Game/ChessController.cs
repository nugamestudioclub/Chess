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

    public void UpdateGame()
    {
        foreach (Piece piece in pieces)
        {
            if (piece != null)
            {
                piece.GameUpdate();
            }
        }
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
        piece.statusManager.ApplyOnPieceMove((effect) => effect.Props.OnPieceMove(effect, piece, move));

        piece.hasMoved = true;

        // if piece captured king

        // else:

        if (sender != null)
        {
            sender.Switch();
        }

        board.StartCoroutine(board.FlipBoard());

        // apply any event

        int eventMove = activePlayer == TeamColor.White
            ? board.eventManager.WhiteNextEvent
            : board.eventManager.BlackNextEvent;

        Debug.Log(eventMove);
        Debug.Log(board.NumMoves);
        if (eventMove <= board.NumMoves)
        {
            if (activePlayer == TeamColor.White)
            {
                board.eventManager.SetNextWhiteEvent();
            }
            else
            {
                board.eventManager.SetNextBlackEvent();
            }
        }

        //

        if (tc == TeamColor.Black)
        {
            board.NumMoves++;
        }

        activePlayer = activePlayer == TeamColor.White ? TeamColor.Black : TeamColor.White;
        UpdateGame();
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