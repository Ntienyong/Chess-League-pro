using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Networking.Transport;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SpecialMove
{
    None = 0,
    EnPassant,
    Castling,
    Promotion
}


public class Chessboard : MonoBehaviour
{
    [Header("Art Stuff")]
    [SerializeField] private Material tileMaterial;
    [SerializeField] private float tileSize = 1.0f;
    [SerializeField] private float yOffset = 0.2f;
    [SerializeField] private Vector3 boardCenter = Vector3.zero;
    [SerializeField] private float deathSize = 0.3f;
    [SerializeField] private float deathSpacing = 0.3f;
    [SerializeField] private float dragOffset = 1.5f;
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private Transform rematchIndicator;
    [SerializeField] private Button rematchButton;

    [Header("Prefabs & Materials")]
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private GameObject[] prefabsOne;
    [SerializeField] private GameObject[] stadiums;
    [SerializeField] private Material[] teamMaterials;

    //LOGIC
    private ChessPiece[,] chessPieces;
    private ChessPiece currentlyDragging;
    private List<Vector2Int> availableMoves = new List<Vector2Int>();
    private List<ChessPiece> deadWhites = new List<ChessPiece>();
    private List<ChessPiece> deadBlacks = new List<ChessPiece>();
    private const int TILE_COUNT_X = 8;
    private const int TILE_COUNT_Y = 8;
    private GameObject[,] tiles;
    private Camera currentCamera;
    private Vector2Int currentHover;
    private Vector3 bounds;
    private bool isWhiteTurn;
    private bool isBlackTurn;
    private SpecialMove specialMove;
    private List<Vector2Int[]> moveList = new List<Vector2Int[]>();

    //Promotion Logic
    [SerializeField] private GameObject promotionUI;

    //Multiplayer logic
    private int playerCount = -1;
    private int currentTeam = -1;
    private bool localGame = false;
    private bool[] playerRematch = new bool[2];

    private void Start()
    {
        isWhiteTurn = true;

        GenerateAllTiles(tileSize, TILE_COUNT_X, TILE_COUNT_Y);
        SpawnAllpieces();
        PositionAllPieces();
        SpawnArena();

        //RegisterEvents();
    }

    private void Update()
    {

        if (!currentCamera)
        {
            currentCamera = Camera.main;
            return;
        }

        RaycastHit info;
        Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out info, 100, LayerMask.GetMask("Tile", "Hover", "Highlight")))
        {
            //Get the indexes of the tile that is hit
            Vector2Int hitPosition = LookupTileIndex(info.transform.gameObject);

            //Hovering over a tile after not hovering any tile
            if (currentHover == -Vector2Int.one)
            {
                currentHover = hitPosition;
                tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
            }

            //Hovering a different tile after hovering a previous tile
            if (currentHover != hitPosition)
            {
                tiles[currentHover.x, currentHover.y].layer = (ContainsValidMove(ref availableMoves, currentHover)) ? LayerMask.NameToLayer("Highlight") : LayerMask.NameToLayer("Tile");
                currentHover = hitPosition;
                tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
            }

            //If we press down on the mouse button
            if (Input.GetMouseButtonDown(0))
            {
                if (chessPieces[hitPosition.x, hitPosition.y] != null)
                {
                    //is it your turn?
                    if ((chessPieces[hitPosition.x, hitPosition.y].team == 0 && isWhiteTurn && currentTeam == 0) || (chessPieces[hitPosition.x, hitPosition.y].team == 1 && isBlackTurn && currentTeam == 1))
                    {
                        currentlyDragging = chessPieces[hitPosition.x, hitPosition.y];

                        //Get a list of where I can go, highlight tiles as well
                        availableMoves = currentlyDragging.GetAvailableMoves(ref chessPieces, TILE_COUNT_X, TILE_COUNT_Y);

                        //Get a list of special moves as well
                        specialMove = currentlyDragging.GetSpecialMoves(ref chessPieces, ref moveList, ref availableMoves);

                        PreventCheck();

                        HighlightTiles();

                    }
                }
            }

            //If we are releasing mouse button
            if (currentlyDragging != null && Input.GetMouseButtonUp(0))
            {
                Vector2Int previousPosition = new Vector2Int(currentlyDragging.currentX, currentlyDragging.currentY);

                if (ContainsValidMove(ref availableMoves, new Vector2Int(hitPosition.x, hitPosition.y)))
                {
                    MoveTo(previousPosition.x, previousPosition.y, hitPosition.x, hitPosition.y);

                    //Net Implementation
                    NetMakeMove mm = new NetMakeMove();
                    mm.originalX = previousPosition.x;
                    mm.originalY = previousPosition.y;
                    mm.destinationX = hitPosition.x;
                    mm.destinationY = hitPosition.y;
                    mm.teamId = currentTeam;
                    Client.Instance.SendToServer(mm);
                }
                else
                {
                    currentlyDragging.SetPosition(GetTileCenter(previousPosition.x, previousPosition.y));
                    currentlyDragging = null;
                    RemoveHighlightTiles();
                    
                }

            }
        }
        else
        {
            if (currentHover != -Vector2Int.one)
            {
                tiles[currentHover.x, currentHover.y].layer = (ContainsValidMove(ref availableMoves, currentHover)) ? LayerMask.NameToLayer("Highlight") : LayerMask.NameToLayer("Tile");
                currentHover = -Vector2Int.one;
            }

            if (currentlyDragging && Input.GetMouseButtonUp(0))
            {
                currentlyDragging.SetPosition(GetTileCenter(currentlyDragging.currentX, currentlyDragging.currentY));
                currentlyDragging = null;
                RemoveHighlightTiles();
            }
        }

        //When dragging a piece
        if (currentlyDragging)
        {
            Plane horizontalPlane = new Plane(Vector3.up, Vector3.up * yOffset);
            float distance = 0.0f;
            if (horizontalPlane.Raycast(ray, out distance))
                currentlyDragging.SetPosition(ray.GetPoint(distance) + Vector3.up * dragOffset);
        }
    }

    private void GenerateAllTiles(float tileSize, int tileCountX, int tileCountY)
    {
        yOffset += transform.position.y;
        bounds = new Vector3((tileCountX / 2) * tileSize, 0, (tileCountY / 2) * tileSize) + boardCenter;

        tiles = new GameObject[tileCountX, tileCountY];
        for (int x = 0; x < tileCountX; x++)
            for (int y = 0; y < tileCountY; y++)
                tiles[x, y] = GenerateSingeleTile(tileSize, x, y);
    }

    private GameObject GenerateSingeleTile(float tileSize, int x, int y)
    {
        GameObject tileObject = new GameObject(string.Format("X:{0}, Y:{0}", x, y));
        tileObject.transform.parent = transform;

        Mesh mesh = new Mesh();
        tileObject.AddComponent<MeshFilter>().mesh = mesh;
        tileObject.AddComponent<MeshRenderer>().material = tileMaterial;

        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(x * tileSize, yOffset, y * tileSize) - bounds;
        vertices[1] = new Vector3(x * tileSize, yOffset, (y + 1) * tileSize) - bounds;
        vertices[2] = new Vector3((x + 1) * tileSize, yOffset, y * tileSize) - bounds;
        vertices[3] = new Vector3((x + 1) * tileSize, yOffset, (y + 1) * tileSize) - bounds;

        int[] tris = new int[] { 0, 1, 2, 1, 3, 2 };
        mesh.vertices = vertices;
        mesh.triangles = tris;

        mesh.RecalculateNormals();

        tileObject.layer = LayerMask.NameToLayer("Tile");
        tileObject.AddComponent<BoxCollider>();
        return tileObject;
    }

    private void SpawnAllpieces()
    {
        chessPieces = new ChessPiece[TILE_COUNT_X, TILE_COUNT_Y];

        int whiteTeam = 0, blackTeam = 1;

        //White team
        chessPieces[0, 0] = SpawnHomeSinglePiece(ChessPieceType.Rook, whiteTeam);
        chessPieces[1, 0] = SpawnHomeSinglePiece(ChessPieceType.Knight, whiteTeam);
        chessPieces[2, 0] = SpawnHomeSinglePiece(ChessPieceType.Bishop, whiteTeam);
        chessPieces[3, 0] = SpawnHomeSinglePiece(ChessPieceType.Queen, whiteTeam);
        chessPieces[4, 0] = SpawnHomeSinglePiece(ChessPieceType.King, whiteTeam);
        chessPieces[5, 0] = SpawnHomeSinglePiece(ChessPieceType.Bishop, whiteTeam);
        chessPieces[6, 0] = SpawnHomeSinglePiece(ChessPieceType.Knight, whiteTeam);
        chessPieces[7, 0] = SpawnHomeSinglePiece(ChessPieceType.Rook, whiteTeam);
        for (int i = 0; i < TILE_COUNT_X; i++)
            chessPieces[i, 1] = SpawnHomeSinglePiece(ChessPieceType.Pawn, whiteTeam);

        //Black team
        chessPieces[0, 7] = SpawnAwaySinglePiece(ChessPieceType.Rook, blackTeam);
        chessPieces[1, 7] = SpawnAwaySinglePiece(ChessPieceType.Knight, blackTeam);
        chessPieces[2, 7] = SpawnAwaySinglePiece(ChessPieceType.Bishop, blackTeam);
        chessPieces[3, 7] = SpawnAwaySinglePiece(ChessPieceType.Queen, blackTeam);
        chessPieces[4, 7] = SpawnAwaySinglePiece(ChessPieceType.King, blackTeam);
        chessPieces[5, 7] = SpawnAwaySinglePiece(ChessPieceType.Bishop, blackTeam);
        chessPieces[6, 7] = SpawnAwaySinglePiece(ChessPieceType.Knight, blackTeam);
        chessPieces[7, 7] = SpawnAwaySinglePiece(ChessPieceType.Rook, blackTeam);
        for (int i = 0; i < TILE_COUNT_Y; i++)
            chessPieces[i, 6] = SpawnAwaySinglePiece(ChessPieceType.Pawn, blackTeam);

    }

    private ChessPiece SpawnHomeSinglePiece(ChessPieceType type, int team)
    {
        ChessPiece cp = new ChessPiece();
        if (MenuUI.Instance.homePieceTypeIndex == 0)
        {
            //ChessPiece cp = Instantiate(prefabs[(int)type - 1], transform).GetComponent<ChessPiece>();
            cp = Instantiate(prefabs[(int)type - 1], transform).GetComponent<ChessPiece>();

            cp.type = type;
            cp.team = team;
            //cp.GetComponent<MeshRenderer>().material = teamMaterials[((team == 0) ? 0 : 6) + ((int)type - 1)];
            //cp.GetComponent<MeshRenderer>().material.color = MenuUI.Instance.homePiecesColor[MenuUI.Instance.homeTeamColorIndex];
        }
        if (MenuUI.Instance.homePieceTypeIndex == 1)
        {
            //ChessPiece cp = Instantiate(prefabs[(int)type - 1], transform).GetComponent<ChessPiece>();
            cp = Instantiate(prefabsOne[(int)type - 1], transform).GetComponent<ChessPiece>();

            cp.type = type;
            cp.team = team;
            //cp.GetComponent<MeshRenderer>().material = teamMaterials[((team == 0) ? 0 : 6) + ((int)type - 1)];
            cp.GetComponent<MeshRenderer>().material.color = MenuUI.Instance.homePiecesColor[MenuUI.Instance.homeTeamColorIndex];
        }

        return cp;
    }

    private ChessPiece SpawnAwaySinglePiece(ChessPieceType type, int team)
    {
        ChessPiece cp = new ChessPiece();
        if (MenuUI.Instance.awayPieceTypeIndex == 0)
        {
            //ChessPiece cp = Instantiate(prefabs[(int)type - 1], transform).GetComponent<ChessPiece>();
            cp = Instantiate(prefabs[(int)type - 1], transform).GetComponent<ChessPiece>();

            cp.type = type;
            cp.team = team;
            //cp.GetComponent<MeshRenderer>().material = teamMaterials[((team == 0) ? 0 : 6) + ((int)type - 1)];
            cp.GetComponent<MeshRenderer>().material.color = MenuUI.Instance.awayPiecesColor[MenuUI.Instance.awayTeamColorIndex];
        }
        if(MenuUI.Instance.awayPieceTypeIndex == 1)
        {
            //ChessPiece cp = Instantiate(prefabs[(int)type - 1], transform).GetComponent<ChessPiece>();
            cp = Instantiate(prefabsOne[(int)type - 1], transform).GetComponent<ChessPiece>();

            cp.type = type;
            cp.team = team;
            //cp.GetComponent<MeshRenderer>().material = teamMaterials[((team == 0) ? 0 : 6) + ((int)type - 1)];
            cp.GetComponent<MeshRenderer>().material.color = MenuUI.Instance.awayPiecesColor[MenuUI.Instance.awayTeamColorIndex];
        }

        return cp;
    }
    private void SpawnArena()
    {
        int index = MenuUI.Instance.arenaIndex;
        GameObject arena = new GameObject();
        arena = Instantiate(stadiums[index], transform.position, transform.rotation);

    }

    private void PositionAllPieces()
    {
        for (int x = 0; x < TILE_COUNT_X; x++)
            for (int y = 0; y < TILE_COUNT_Y; y++)
                if (chessPieces[x, y] != null)
                    PositionSinglePiece(x, y, true);
    }

    private void PositionSinglePiece(int x, int y, bool force = false)
    {
        chessPieces[x, y].currentX = x;
        chessPieces[x, y].currentY = y;
        chessPieces[x, y].SetPosition(GetTileCenter(x, y), force);
    }

    private Vector3 GetTileCenter(int x, int y)
    {
        return new Vector3(x * tileSize, yOffset, y * tileSize) - bounds + new Vector3(tileSize / 2, 0, tileSize / 2);
    }

    private void HighlightTiles()
    {
        for (int i = 0; i < availableMoves.Count; i++)
            tiles[availableMoves[i].x, availableMoves[i].y].layer = LayerMask.NameToLayer("Highlight");
    }
    private void RemoveHighlightTiles()
    {
        for (int i = 0; i < availableMoves.Count; i++)
            tiles[availableMoves[i].x, availableMoves[i].y].layer = LayerMask.NameToLayer("Tile");

        availableMoves.Clear();

    }

    //Checkmate
    private void CheckMate(int team)
    {
        DisplayVictory(team);
        isBlackTurn = false;
        isWhiteTurn = false;
        GameUI.Instance.blackTimerOn = false;
        GameUI.Instance.whiteTimerOn = false;
    }

    private void DisplayVictory(int winningTeam)
    {
        victoryScreen.SetActive(true);
        victoryScreen.transform.GetChild(winningTeam).gameObject.SetActive(true);
    }

    public void OnRematchButton()
    {
        if(localGame)
        {

            NetRematch wrm = new NetRematch();
            wrm.teamId = 0;
            wrm.wantRematch = 1;
            Client.Instance.SendToServer(wrm);

            NetRematch brm = new NetRematch();
            brm.teamId = 1;
            brm.wantRematch = 1;
            Client.Instance.SendToServer(brm);
        }
        else
        {
            NetRematch rm = new NetRematch();
            rm.teamId = currentTeam;
            rm.wantRematch = 1;
            Client.Instance.SendToServer(rm);
        }
    }
    public void GameReset()
    {
        //nt currentNumber = (int)UnityEngine.Random.Range(0f, 2f);

        //UI
        rematchButton.interactable = true;

        rematchIndicator.transform.GetChild(0).gameObject.SetActive(false);
        rematchIndicator.transform.GetChild(1).gameObject.SetActive(false);

        victoryScreen.transform.GetChild(0).gameObject.SetActive(false);
        victoryScreen.transform.GetChild(1).gameObject.SetActive(false);
        victoryScreen.SetActive(false);

        //Fields reset
        currentlyDragging = null;
        availableMoves.Clear();
        moveList.Clear();
        playerRematch[0] = playerRematch[1] = false;

        //Clean up
        for (int x = 0; x < TILE_COUNT_X; x++)
        {
            for (int y = 0; y < TILE_COUNT_Y; y++)
            {
                if (chessPieces[x, y] != null)
                    Destroy(chessPieces[x, y].gameObject);

                chessPieces[x, y] = null;
            }
        }
        for (int i = 0; i < deadWhites.Count; i++)
            Destroy(deadWhites[i].gameObject);
        for (int i = 0; i < deadBlacks.Count; i++)
            Destroy(deadBlacks[i].gameObject);

        deadWhites.Clear();
        deadBlacks.Clear();

        SpawnAllpieces();
        PositionAllPieces();
        GameUI.Instance.whiteTimeLeft = MenuUI.Instance.timeLeft;
        GameUI.Instance.blackTimeLeft = MenuUI.Instance.timeLeft;
        GameUI.Instance.blackTimerOn = false;
        GameUI.Instance.whiteTimerOn = false;
        GameUI.Instance.whiteTimerText.text = "00 : 00";
        GameUI.Instance.blackTimerText.text = "00 : 00";
        isWhiteTurn = true;
        currentTeam = 0;
        


        //isGameRestarted = true;

        //SceneManager.LoadScene("GameMatch");
    }

    public void OnMenuButton()
    {
        NetRematch rm = new NetRematch();
        rm.teamId = currentTeam;
        rm.wantRematch = 0;
        Client.Instance.SendToServer(rm);

        GameReset();
        GameUI.Instance.OnLeaveFromGameMenu();

        Invoke("ShutdownRelay", 1.0f);

        // Reset some values
        playerCount = -1;
        currentTeam = -1;
    }

    //Special Moves
    private void ProcessSpecialMove()
    {
        if (specialMove == SpecialMove.EnPassant)
        {
            var newMove = moveList[moveList.Count - 1];
            ChessPiece myPawn = chessPieces[newMove[1].x, newMove[1].y];
            var targetPawnPosition = moveList[moveList.Count - 2];
            ChessPiece enemyPawn = chessPieces[targetPawnPosition[1].x, targetPawnPosition[1].y];

            if (myPawn.currentX == enemyPawn.currentX)
            {
                if (myPawn.currentY == enemyPawn.currentY - 1 || myPawn.currentY == enemyPawn.currentY + 1)
                {
                    if (enemyPawn.team == 0)
                    {
                        deadWhites.Add(enemyPawn);
                        enemyPawn.SetScale(Vector3.one * deathSize);
                        enemyPawn.SetPosition(
                        new Vector3(8 * tileSize, yOffset * 2, -1 * tileSize) - bounds
                        + new Vector3(tileSize / 2, 0, tileSize / 2)
                        + (Vector3.forward * deathSpacing) * deadWhites.Count);
                    }
                    else
                    {
                        deadBlacks.Add(enemyPawn);
                        enemyPawn.SetScale(Vector3.one * deathSize);
                        enemyPawn.SetPosition(
                        new Vector3(-1 * tileSize, yOffset * 2, 8 * tileSize) - bounds
                        + new Vector3(tileSize / 2, 0, tileSize / 2)
                        + (Vector3.back * deathSpacing) * deadBlacks.Count);
                    }

                    chessPieces[enemyPawn.currentX, enemyPawn.currentY] = null;
                }
            }
        }
        

        if (specialMove == SpecialMove.Promotion)
        {
            Vector2Int[] lastMove = moveList[moveList.Count - 1];
            ChessPiece targetPawn = chessPieces[lastMove[1].x, lastMove[1].y];

            if (targetPawn.type == ChessPieceType.Pawn)
            {
                promotionUI.SetActive(true);
                isWhiteTurn = false;
                isBlackTurn = false;
                GameUI.Instance.blackTimerOn = false;
                GameUI.Instance.whiteTimerOn = false;
            }
        }

        if (specialMove == SpecialMove.Castling)
        {
            Vector2Int[] lastMove = moveList[moveList.Count - 1];

            //Left Rook
            if (lastMove[1].x == 2)
            {
                if (lastMove[1].y == 0) //White team
                {
                    ChessPiece rook = chessPieces[0, 0];
                    chessPieces[3, 0] = rook;
                    PositionSinglePiece(3, 0);
                    chessPieces[0, 0] = null;
                }
                else if (lastMove[1].y == 7) //Black side
                {
                    ChessPiece rook = chessPieces[0, 7];
                    chessPieces[3, 7] = rook;
                    PositionSinglePiece(3, 7);
                    chessPieces[0, 7] = null;
                }
            }
            //Right Rook
            else if (lastMove[1].x == 6)
            {
                if (lastMove[1].y == 0) //White team
                {
                    ChessPiece rook = chessPieces[7, 0];
                    chessPieces[5, 0] = rook;
                    PositionSinglePiece(5, 0);
                    chessPieces[7, 0] = null;
                }
                else if (lastMove[1].y == 7) //Black side
                {
                    ChessPiece rook = chessPieces[7, 7];
                    chessPieces[5, 7] = rook;
                    PositionSinglePiece(5, 7);
                    chessPieces[7, 7] = null;
                }
            }
        }
    }
    public void OnQueenButton()
    {
        Vector2Int[] lastMove = moveList[moveList.Count - 1];
        ChessPiece targetPawn = chessPieces[lastMove[1].x, lastMove[1].y];
        if (targetPawn.team == 0 && lastMove[1].y == 7) // White team
        {
            ChessPiece newQueen = SpawnHomeSinglePiece(ChessPieceType.Queen, 0);
            newQueen.transform.position = chessPieces[lastMove[1].x, lastMove[1].y].transform.position;
            Destroy(chessPieces[lastMove[1].x, lastMove[1].y].gameObject);
            chessPieces[lastMove[1].x, lastMove[1].y] = newQueen;
            PositionSinglePiece(lastMove[1].x, lastMove[1].y);
            isBlackTurn  = true;
            GameUI.Instance.blackTimerOn = true;

        }
        if (targetPawn.team == 1 && lastMove[1].y == 0) //Black team
        {
            ChessPiece newQueen = SpawnAwaySinglePiece(ChessPieceType.Queen, 1);
            newQueen.transform.position = chessPieces[lastMove[1].x, lastMove[1].y].transform.position;
            Destroy(chessPieces[lastMove[1].x, lastMove[1].y].gameObject);
            chessPieces[lastMove[1].x, lastMove[1].y] = newQueen;
            PositionSinglePiece(lastMove[1].x, lastMove[1].y);
            isWhiteTurn = true;
            GameUI.Instance.whiteTimerOn = true;

        }

        promotionUI.SetActive(false);
    }
    public void OnBishopButton()
    {
        Vector2Int[] lastMove = moveList[moveList.Count - 1];
        ChessPiece targetPawn = chessPieces[lastMove[1].x, lastMove[1].y];
        if (targetPawn.team == 0 && lastMove[1].y == 7) // White team
        {
            ChessPiece newBishop = SpawnHomeSinglePiece(ChessPieceType.Bishop, 0);
            newBishop.transform.position = chessPieces[lastMove[1].x, lastMove[1].y].transform.position;
            Destroy(chessPieces[lastMove[1].x, lastMove[1].y].gameObject);
            chessPieces[lastMove[1].x, lastMove[1].y] = newBishop;
            PositionSinglePiece(lastMove[1].x, lastMove[1].y);
            isBlackTurn = true;
            GameUI.Instance.blackTimerOn = true;

        }
        if (targetPawn.team == 1 && lastMove[1].y == 0) //Black team
        {
            ChessPiece newBishop = SpawnAwaySinglePiece(ChessPieceType.Bishop, 1);
            newBishop.transform.position = chessPieces[lastMove[1].x, lastMove[1].y].transform.position;
            Destroy(chessPieces[lastMove[1].x, lastMove[1].y].gameObject);
            chessPieces[lastMove[1].x, lastMove[1].y] = newBishop;
            PositionSinglePiece(lastMove[1].x, lastMove[1].y);
            isWhiteTurn = true;
            GameUI.Instance.whiteTimerOn = true;
        }

        promotionUI.SetActive(false);
    }
    public void OnKnightButton()
    {
        Vector2Int[] lastMove = moveList[moveList.Count - 1];
        ChessPiece targetPawn = chessPieces[lastMove[1].x, lastMove[1].y];
        if (targetPawn.team == 0 && lastMove[1].y == 7) // White team
        {
            ChessPiece newKnight = SpawnHomeSinglePiece(ChessPieceType.Knight, 0);
            newKnight.transform.position = chessPieces[lastMove[1].x, lastMove[1].y].transform.position;
            Destroy(chessPieces[lastMove[1].x, lastMove[1].y].gameObject);
            chessPieces[lastMove[1].x, lastMove[1].y] = newKnight;
            PositionSinglePiece(lastMove[1].x, lastMove[1].y);
            isBlackTurn = true;
            GameUI.Instance.blackTimerOn = true;

        }
        if (targetPawn.team == 1 && lastMove[1].y == 0) //Black team
        {
            ChessPiece newKnight = SpawnAwaySinglePiece(ChessPieceType.Knight, 1);
            newKnight.transform.position = chessPieces[lastMove[1].x, lastMove[1].y].transform.position;
            Destroy(chessPieces[lastMove[1].x, lastMove[1].y].gameObject);
            chessPieces[lastMove[1].x, lastMove[1].y] = newKnight;
            PositionSinglePiece(lastMove[1].x, lastMove[1].y);
            isWhiteTurn = true;
            GameUI.Instance.whiteTimerOn = true;

        }

        promotionUI.SetActive(false);
    }
    public void OnRookButton()
    {
        Vector2Int[] lastMove = moveList[moveList.Count - 1];
        ChessPiece targetPawn = chessPieces[lastMove[1].x, lastMove[1].y];
        if (targetPawn.team == 0 && lastMove[1].y == 7) // White team
        {
            ChessPiece newRook = SpawnHomeSinglePiece(ChessPieceType.Rook, 0);
            newRook.transform.position = chessPieces[lastMove[1].x, lastMove[1].y].transform.position;
            Destroy(chessPieces[lastMove[1].x, lastMove[1].y].gameObject);
            chessPieces[lastMove[1].x, lastMove[1].y] = newRook;
            PositionSinglePiece(lastMove[1].x, lastMove[1].y);
            isBlackTurn = true;
            GameUI.Instance.blackTimerOn = true;

        }
        if (targetPawn.team == 1 && lastMove[1].y == 0) //Black team
        {
            ChessPiece newRook = SpawnAwaySinglePiece(ChessPieceType.Rook, 1);
            newRook.transform.position = chessPieces[lastMove[1].x, lastMove[1].y].transform.position;
            Destroy(chessPieces[lastMove[1].x, lastMove[1].y].gameObject);
            chessPieces[lastMove[1].x, lastMove[1].y] = newRook;
            PositionSinglePiece(lastMove[1].x, lastMove[1].y);
            isWhiteTurn = true;
            GameUI.Instance.whiteTimerOn = true;

        }

        promotionUI.SetActive(false);
    }

    private void PreventCheck()
    {
        ChessPiece targetKing = null;
        for (int x = 0; x < TILE_COUNT_X; x++)
            for (int y = 0; y < TILE_COUNT_Y; y++)
                if (chessPieces[x, y] != null)
                    if (chessPieces[x, y].type == ChessPieceType.King)
                        if (chessPieces[x, y].team == currentlyDragging.team)
                            targetKing = chessPieces[x, y];

        //Since we're sending ref availableMoves, we will be deleting moves that put us in check
        SimulateMoveForSinglePiece(currentlyDragging, ref availableMoves, targetKing);
    }

    private void SimulateMoveForSinglePiece(ChessPiece cp, ref List<Vector2Int> moves, ChessPiece targetKing)
    {
        // Save the current values, to reset after function call
        int actualX = cp.currentX;
        int actualY = cp.currentY;
        List<Vector2Int> movesToRemove = new List<Vector2Int>();

        // Going through all the moves, simulate them and check if we're in check
        for (int i = 0; i < moves.Count; i++)
        {
            int simX = moves[i].x;
            int simY = moves[i].y;

            Vector2Int kingPositionThisSim = new Vector2Int(targetKing.currentX, targetKing.currentY);

            //Did we simulate the king's move
            if (cp.type == ChessPieceType.King)
                kingPositionThisSim = new Vector2Int(simX, simY);

            // Copy the [,] and not a reference
            ChessPiece[,] simulation = new ChessPiece[TILE_COUNT_X, TILE_COUNT_Y];
            List<ChessPiece> simAttackingPieces = new List<ChessPiece>();
            for (int x = 0; x < TILE_COUNT_X; x++)
            {
                for (int y = 0; y < TILE_COUNT_Y; y++)
                {
                    if (chessPieces[x, y] != null)
                    {
                        simulation[x, y] = chessPieces[x, y];
                        if (simulation[x, y].team != cp.team)
                            simAttackingPieces.Add(simulation[x, y]);
                    }
                }

            }

            //Simulate that move
            simulation[actualX, actualY] = null;
            cp.currentX = simX;
            cp.currentY = simY;
            simulation[simX, simY] = cp;

            //Did one of the piece got taken down during our simulation
            var deadPiece = simAttackingPieces.Find(c => c.currentX == simX && c.currentY == simY);
            if (deadPiece != null)
                simAttackingPieces.Remove(deadPiece);

            //Get all the simulated attacking pieces moves
            List<Vector2Int> simMoves = new List<Vector2Int>();
            for (int a = 0; a < simAttackingPieces.Count; a++)
            {
                var pieceMoves = simAttackingPieces[a].GetAvailableMoves(ref simulation, TILE_COUNT_X, TILE_COUNT_Y);
                for (int b = 0; b < pieceMoves.Count; b++)
                    simMoves.Add(pieceMoves[b]);
            }

            //Is the Kingin trouble? if so, remove move
            if (ContainsValidMove(ref simMoves, kingPositionThisSim))
            {
                movesToRemove.Add(moves[i]);
            }

            //Restore the actual CP data
            cp.currentX = actualX;
            cp.currentY = actualY;
        }

        //Remove from the current available move list
        for (int i = 0; i < movesToRemove.Count; i++)
            moves.Remove(movesToRemove[i]);
    }

    private bool CheckForCheckmate()
    {
        var lastMove = moveList[moveList.Count - 1];
        int targetTeam = (chessPieces[lastMove[1].x, lastMove[1].y].team == 0) ? 1 : 0;

        List<ChessPiece> attackingPieces = new List<ChessPiece>();
        List<ChessPiece> defendingPieces = new List<ChessPiece>();
        ChessPiece targetKing = null;
        for (int x = 0; x < TILE_COUNT_X; x++)
            for (int y = 0; y < TILE_COUNT_Y; y++)
                if (chessPieces[x, y] != null)
                {
                    if (chessPieces[x, y].team == targetTeam)
                    {
                        defendingPieces.Add(chessPieces[x, y]);
                        if (chessPieces[x, y].type == ChessPieceType.King)
                            targetKing = chessPieces[x, y];
                    }
                    else
                    {
                        attackingPieces.Add(chessPieces[x, y]);
                    }
                }

        //Is the king attacked right now
        List<Vector2Int> currentAvailableMoves = new List<Vector2Int>();
        for (int i = 0; i < attackingPieces.Count; i++)
        {
            var pieceMoves = attackingPieces[i].GetAvailableMoves(ref chessPieces, TILE_COUNT_X, TILE_COUNT_Y);
            for (int b = 0; b < pieceMoves.Count; b++)
                currentAvailableMoves.Add(pieceMoves[b]);

        }

        //Are we under check right now
        if (ContainsValidMove(ref currentAvailableMoves, new Vector2Int(targetKing.currentX, targetKing.currentY)))
        {
            //Can the king be protected by another piece
            for (int i = 0; i < defendingPieces.Count; i++)
            {
                List<Vector2Int> defendingMoves = defendingPieces[i].GetAvailableMoves(ref chessPieces, TILE_COUNT_X, TILE_COUNT_Y);

                //Since we're sending ref availableMoves, we will be deleting moves that are putting us under check
                SimulateMoveForSinglePiece(defendingPieces[i], ref defendingMoves, targetKing);

                if (defendingMoves.Count != 0)
                    return false;
            }

            return true; //Checkmate
        }

        return false;

    }

    //Operations
    private bool ContainsValidMove(ref List<Vector2Int> moves, Vector2Int pos)
    {
        for (int i = 0; i < moves.Count; i++)
            if (moves[i].x == pos.x && moves[i].y == pos.y)
                return true;

        return false;
    }

    private Vector2Int LookupTileIndex(GameObject hitInfo)
    {
        for (int x = 0; x < TILE_COUNT_X; x++)
            for (int y = 0; y < TILE_COUNT_Y; y++)
                if (tiles[x, y] == hitInfo)
                    return new Vector2Int(x, y);

        return -Vector2Int.one; //Invalid
    }

    private IEnumerator BlackTimerCountdown()
    {
        while(GameUI.Instance.blackTimerOn == true)
        {
            yield return new WaitForSeconds(1.0f);
            GameUI.Instance.blackTimeLeft--;

            float minutes = Mathf.FloorToInt(GameUI.Instance.blackTimeLeft / 60);
            float seconds = Mathf.FloorToInt(GameUI.Instance.blackTimeLeft % 60);

            GameUI.Instance.blackTimerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);

            //if (GameUI.Instance.blackTimeLeft <= 30f)
            //{
            //    GameUI.Instance.blackTimerSprite.GetComponent<Image>().material.color = Color.red;
            //}


            if (GameUI.Instance.blackTimeLeft <= 0)
            {
                CheckMate(0);
                GameUI.Instance.blackTimerOn = false;
            }
        }
    }
    private IEnumerator WhiteTimerCountdown()
    {
        while (GameUI.Instance.whiteTimerOn == true)
        {
            yield return new WaitForSeconds(1.0f);
            GameUI.Instance.whiteTimeLeft--;

            float minutes = Mathf.FloorToInt(GameUI.Instance.whiteTimeLeft / 60);
            float seconds = Mathf.FloorToInt(GameUI.Instance.whiteTimeLeft % 60);

            GameUI.Instance.whiteTimerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);

            //if (GameUI.Instance.blackTimeLeft <= 30f)
            //{
            //    GameUI.Instance.whiteTimerSprite.GetComponent<Image>().material.color = Color.red;
            //}


            if (GameUI.Instance.whiteTimeLeft <= 0)
            {
                CheckMate(1);
                GameUI.Instance.whiteTimerOn = false;
            }
        }
    }

    private void MoveTo(int originalX, int originalY, int x, int y)
    {
        ChessPiece cp = chessPieces[originalX, originalY];
        Vector2Int previousPosition = new Vector2Int(originalX, originalY);

        //Is there another piece on the target position?
        if (chessPieces[x, y] != null)
        {
            ChessPiece ocp = chessPieces[x, y];

            if (cp.team == ocp.team)
                return;

            //If its the enemy team
            if (ocp.team == 0)
            {
                if (ocp.type == ChessPieceType.King)
                    CheckMate(1);

                deadWhites.Add(ocp);
                ocp.SetScale(Vector3.one * deathSize);
                ocp.SetPosition(
                new Vector3(8 * tileSize, yOffset * 2, -1 * tileSize) - bounds
                + new Vector3(tileSize / 2, 0, tileSize / 2)
                + (Vector3.forward * deathSpacing) * deadWhites.Count);
            }
            else
            {
                if (ocp.type == ChessPieceType.King)
                    CheckMate(0);

                deadBlacks.Add(ocp);
                ocp.SetScale(Vector3.one * deathSize);
                ocp.SetPosition(
                new Vector3(-1 * tileSize, yOffset * 2, 8 * tileSize) - bounds
                + new Vector3(tileSize / 2, 0, tileSize / 2)
                + (Vector3.back * deathSpacing) * deadBlacks.Count);
            }
        }

        chessPieces[x, y] = cp;
        chessPieces[previousPosition.x, previousPosition.y] = null;

        PositionSinglePiece(x, y);

        isWhiteTurn = !isWhiteTurn;
        if (!isWhiteTurn)
        {
            isBlackTurn = true;
        }

        if (!isWhiteTurn)
        {
            GameUI.Instance.blackTimerOn = true;
            GameUI.Instance.whiteTimerOn = false;
            
        }
        else if (isWhiteTurn)
        {
            GameUI.Instance.blackTimerOn = false;
            GameUI.Instance.whiteTimerOn = true;

        }
        StartCoroutine(BlackTimerCountdown());
        StartCoroutine(WhiteTimerCountdown());


        if (localGame)
            currentTeam = (currentTeam == 0) ? 1 : 0;

        moveList.Add(new Vector2Int[] { previousPosition, new Vector2Int(x, y) });

        ProcessSpecialMove();

        if(currentlyDragging)
            currentlyDragging = null;
        RemoveHighlightTiles();

        if (CheckForCheckmate())
            CheckMate(cp.team);


        return;
    }

    #region
    private void RegisterEvents()
    {
        //NetUtility.S_WELCOME += OnWelcomeServer;
        NetUtility.S_MAKE_MOVE += OnMakeMoveServer;
        NetUtility.S_REMATCH += OnRematchServer;

        NetUtility.C_WELCOME += OnWelcomeClient;
        NetUtility.C_START_GAME += OnStartGameClient;
        NetUtility.C_MAKE_MOVE += OnMakeMoveClient;
        NetUtility.C_REMATCH += OnRematchClient;

        //GameUI.Instance.SetLocalGame += OnSetLocalGame;
    }

    private void UnRegisterEvents()
    {
        //NetUtility.S_WELCOME -= OnWelcomeServer;
        NetUtility.S_MAKE_MOVE -= OnMakeMoveServer;
        NetUtility.S_REMATCH -= OnRematchServer;

        NetUtility.C_WELCOME -= OnWelcomeClient;
        NetUtility.C_START_GAME -= OnStartGameClient;
        NetUtility.C_MAKE_MOVE -= OnMakeMoveClient;
        NetUtility.C_REMATCH -= OnRematchClient;

        //GameUI.Instance.SetLocalGame -= OnSetLocalGame;
    }

    //Server
    private void OnWelcomeServer(NetMessage msg, NetworkConnection cnn)
    {
        Debug.Log("Message from server to client");
        //Client has connected, assign a team and return the message back to him
        NetWelcome nw = msg as NetWelcome;

        //Assign a team
        nw.AssignedTeam = ++playerCount;
        Debug.Log(playerCount);
        //nw.AssignedTeam = MenuUI.Instance.homeAndAwayButtonClicked;

        //Return back to client
        Server.Instance.SendToClient(cnn, nw);

        //If full, start the game
        //if (playerCount == 1)
        //    Server.Instance.Broadcast(new NetStartGame());

    }
    private void OnMakeMoveServer(NetMessage msg, NetworkConnection cnn)
    {
        //Receive the message, broadcast it back
        NetMakeMove mm = msg as NetMakeMove;

        //Receive and just broadcast it back
        Server.Instance.Broadcast(mm);

    }
    private void OnRematchServer(NetMessage msg, NetworkConnection cnn)
    {
        Server.Instance.Broadcast(msg);
    }

    //Client
    private void OnWelcomeClient(NetMessage msg)
    {
        //Recieve the connection message
        NetWelcome nw = msg as NetWelcome;

        //Assign the team
        //if(nw.AssignedTeam == 0)
        //{
        //    currentTeam = nw.AssignedTeam;
        //    GameUI.Instance.whiteTimerText = GameObject.Find("WhiteTimerText").GetComponent<TextMeshProUGUI>();
        //    GameUI.Instance.blackTimerText = GameObject.Find("BlackTimerText").GetComponent<TextMeshProUGUI>();
        //}
        //else if(nw.AssignedTeam == 1)
        //{
        //    currentTeam = 0;
        //    GameUI.Instance.whiteTimerText = GameObject.Find("BlackTimerText").GetComponent<TextMeshProUGUI>();
        //    GameUI.Instance.blackTimerText = GameObject.Find("WhiteTimerText").GetComponent<TextMeshProUGUI>();
        //}

        currentTeam = nw.AssignedTeam;
        //currentTeam = MenuUI.Instance.opponentTeam;

        Debug.Log($"My assigned team is {nw.AssignedTeam}");

        //if (localGame && currentTeam == 0 || localGame && currentTeam == 1)
        //    Server.Instance.Broadcast(new NetStartGame());
    }

    private void OnStartGameClient(NetMessage obj)
    {
        //We just need to change the camera
        if (MenuUI.Instance.homeAndAwayButtonClicked == 1)
        {
            GameUI.Instance.ChangeCamera(CameraAngle.blackTeam);

        }
        else if (MenuUI.Instance.homeAndAwayButtonClicked == 0)
        {
            GameUI.Instance.ChangeCamera(CameraAngle.whiteTeam);
        }
        //GameUI.Instance.ChangeCamera((currentTeam == 0) ? CameraAngle.whiteTeam : CameraAngle.blackTeam);
    }
    private void OnMakeMoveClient(NetMessage msg)
    {
        NetMakeMove mm = msg as NetMakeMove;

        Debug.Log($"MM : {mm.teamId} : {mm.originalX} {mm.originalY} -> {mm.destinationX} {mm.destinationY}");
        if(mm.teamId != currentTeam)
        {
            ChessPiece target = chessPieces[mm.originalX, mm.originalY];

            availableMoves = target.GetAvailableMoves(ref chessPieces, TILE_COUNT_X, TILE_COUNT_Y);
            specialMove = target.GetSpecialMoves(ref chessPieces, ref moveList, ref availableMoves);

            MoveTo(mm.originalX, mm.originalY, mm.destinationX, mm.destinationY);
        }

    }
    private void OnRematchClient(NetMessage msg)
    {
        //Receive the connection message
        NetRematch rm = msg as NetRematch;

        //Set the boolean for rematch
        playerRematch[rm.teamId] = rm.wantRematch == 1;

        //Activate the piece of UI
        if (rm.teamId != currentTeam)
        {
            rematchIndicator.transform.GetChild((rm.wantRematch == 1) ? 0 : 1).gameObject.SetActive(true);
            if(rm.wantRematch != 1)
            {
                rematchButton.interactable = false;
            }

        }

        //if both players want to rematch
        if (playerRematch[0] && playerRematch[1])
            GameReset();
    }

    //
    private void ShutdownRelay()
    {
        Client.Instance.Shutdown();
        Server.Instance.Shutdown();
    }
    private void OnSetLocalGame(bool v)
    {
        playerCount = -1;
        currentTeam = -1;
        localGame = v;
    }

    #endregion

}
