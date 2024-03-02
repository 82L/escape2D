using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Media;
using Random = UnityEngine.Random;

public class GameEscape
{
    public enum Turn
    {
        CHARACTER,
        ROCK,
    }
    public enum GameState
    {
        IDLE,
        PLAYING,
        END
    }

    private GameState m_currentGameState;
    
    private GridCoordinate m_characterPosition;

    private GridCoordinate m_winCoordinate;

    private List<GridCoordinate> m_nextMovesCharacter;

    private List<GridCoordinate> m_nextRocksPlaced;

    private bool m_characterHasWon;

    private int m_timerRock;

    private int m_currentTurnNumber;

    private int m_maxTurnNumber;

    private Turn m_resolutionTurn;

    private Dictionary<GridCoordinate, int> m_rocksPlaced;

    private int m_turnCount;

    public int GridWidth
    {
        private set;
        get;
    }

    public int GridHeight
    {
        private set;
        get;
    }

    public int CharacterDistance
    {
        private set;
        get;
    }

    public int RockNumber
    {
        private set;
        get;
    }

    public GridCoordinate CharacterPosition => m_characterPosition;
    public bool CharacterHasWon => m_characterHasWon;
    public GameState CurrentGameState => m_currentGameState;
    
    public GameEscape()
    {
        
    }
    public GameEscape(int p_gridWidth, int p_gridHeight, int p_characterDistance, int p_rockNumber, int p_timerRock, 
        int p_maxTurnNumber)
    {
       Init(p_gridWidth, p_gridHeight, p_characterDistance, p_rockNumber, p_timerRock, p_maxTurnNumber);
    }

    public void Init(int p_gridWidth, int p_gridHeight, int p_characterDistance, int p_rockNumber, int p_timerRock, 
        int p_maxTurnNumber)
    {
        Random.InitState((int)DateTime.Now.Ticks);
        GridWidth = p_gridWidth;
        GridHeight = p_gridHeight;
        CharacterDistance = p_characterDistance;
        RockNumber = p_rockNumber;
        m_characterPosition = new GridCoordinate(p_gridWidth / 2 + 1, p_gridHeight / 2 + 1);
        m_timerRock = p_timerRock;
        m_characterHasWon = false;
        m_maxTurnNumber = p_maxTurnNumber;
        m_currentTurnNumber = 0;
        m_currentGameState = GameState.IDLE;
        GenerateWinCoordinate();
    }

    private void GenerateWinCoordinate()
    {
        int l_determine = Random.Range(0, 1);
        int l_minX = 0, l_maxX, l_minY = 0, l_maxY;
        if (l_determine == 0)
        {
            l_maxX = GridWidth - 1;
            l_maxY = 1;
        }
        else
        {
            l_maxY = GridHeight - 1;
            l_maxX = 1;
        }

        int l_xCoord = Random.Range(l_minX, l_maxX);
        int l_yCoord = Random.Range(l_minY, l_maxY);
        if (l_determine == 0)
        {
            l_yCoord *= GridHeight - 1;
        }
        else
        {
            l_xCoord *= GridWidth - 1;
        }

        m_winCoordinate = new GridCoordinate(l_xCoord, l_yCoord);

    }

    public bool ValidateInputs(List<GridCoordinate> p_movementCoordinateList, List<GridCoordinate> p_rocksCoordinateList)
    {
        if (IsRockListValid(p_rocksCoordinateList) && IsMovementListValid(p_movementCoordinateList)
            && !m_nextMovesCharacter.Any() && !m_nextRocksPlaced.Any())
        {
            m_nextMovesCharacter = p_movementCoordinateList;
            m_nextRocksPlaced = p_rocksCoordinateList;
            m_resolutionTurn = Turn.CHARACTER;
            return true;
        }
        return false;
    }

    private void TurnResolution()
    {
        if ( m_currentGameState != GameState.END && (m_nextMovesCharacter.Any() || m_nextRocksPlaced.Any()))
        {
            m_currentGameState = GameState.PLAYING;
            if ( m_nextMovesCharacter.Any() && (m_resolutionTurn == Turn.CHARACTER || !m_nextRocksPlaced.Any()))
            {
                DoNextCharacterMove();
                m_resolutionTurn = Turn.ROCK;
            }
            else if(m_nextRocksPlaced.Any() && (m_resolutionTurn == Turn.ROCK || !m_nextMovesCharacter.Any()))
            {
                PlaceNextRock();
                m_resolutionTurn = Turn.CHARACTER;
            }

            if (m_nextMovesCharacter.Any() || m_nextRocksPlaced.Any() || m_currentGameState == GameState.END ) return;

            UpdateRocks();

            IncreaseTurn(); 
        }

    }

    private void DoNextCharacterMove()
    {
        GridCoordinate l_coordinate = m_nextMovesCharacter[0];
        m_nextMovesCharacter.RemoveAt(0);
        if (!m_rocksPlaced.ContainsKey(l_coordinate))
        {
            m_characterPosition = l_coordinate;
            TestCharacterOnWinCoordinate();
        }
        else
        {
            m_nextMovesCharacter.Clear();
        }
    }

    private void TestCharacterOnWinCoordinate()
    {
        if (m_characterPosition == m_winCoordinate)
        {
            m_currentGameState = GameState.END;
            m_characterHasWon = true;
        }
    }
    private void PlaceNextRock()
    {
        GridCoordinate l_coordinate = m_nextRocksPlaced[0];
        m_nextRocksPlaced.RemoveAt(0);
        if (l_coordinate != m_characterPosition)
        {
            m_rocksPlaced.Add(l_coordinate,m_timerRock);
        }

    }

    private void UpdateRocks()
    {
        foreach (var rockCoordinate in m_rocksPlaced.Keys)
        {
            m_rocksPlaced[rockCoordinate]--;
            if (m_rocksPlaced[rockCoordinate] < 0)
            {
                m_rocksPlaced.Remove(rockCoordinate);
            }
        }
    }

    private void IncreaseTurn()
    {
        m_currentTurnNumber++;
        if (m_currentTurnNumber >= m_maxTurnNumber)
        {
            m_currentGameState = GameState.END;
        }
        else
        {
            m_currentGameState = GameState.IDLE;
        }
    }

    private bool IsMovementListValid(List<GridCoordinate> p_movementCoordinateList)
    {
        if (p_movementCoordinateList.Count != CharacterDistance)
        {
            return false;
        }
        GridCoordinate lastCoord = m_characterPosition;
        foreach (var l_gridCoordinate in p_movementCoordinateList)
        {
            if (((l_gridCoordinate.X == lastCoord.X &&
                  (l_gridCoordinate.Y - 1 == lastCoord.Y || l_gridCoordinate.Y + 1 == lastCoord.Y)) ||
                 (l_gridCoordinate.Y == lastCoord.Y &&
                  (l_gridCoordinate.X - 1 == lastCoord.X || l_gridCoordinate.X + 1 == lastCoord.X)) ) &&
                !m_rocksPlaced.ContainsKey(l_gridCoordinate))
            {
                lastCoord = l_gridCoordinate;
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    private bool IsRockListValid(List<GridCoordinate> p_rocksCoordinateList)
    {
        if (p_rocksCoordinateList.Count != RockNumber)
        {
            return false;
        }
        if (p_rocksCoordinateList.Any(l_gridCoordinate => Equals(l_gridCoordinate, m_characterPosition) || 
                                                          !m_rocksPlaced.ContainsKey(l_gridCoordinate)))
        {
            return false;
        }

        return true;
    }

    public string[,] GetGameCurrenGridState()
    {
        string[, ] l_grid = new string[GridWidth, GridHeight];
        foreach (var rockCoordinate in m_rocksPlaced.Keys)
        {
            l_grid[rockCoordinate.X, rockCoordinate.Y] = m_rocksPlaced[rockCoordinate].ToString();
        }
        l_grid[m_characterPosition.X, m_characterPosition.Y] = "c";
        return l_grid;
    }
    public IEnumerable<GameState> DoStepResolution()
    {
        TurnResolution();
        yield return m_currentGameState;
    }
    public void DoQuickResolution()
    {
        do
        {
            TurnResolution();
        } while (m_currentGameState == GameState.PLAYING);
    }
}