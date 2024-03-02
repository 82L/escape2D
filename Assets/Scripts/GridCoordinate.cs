using System;

public class GridCoordinate
{
    private int m_x;
    private int m_y;
    public int X => m_x;
    public int Y => m_y;

    public GridCoordinate(int p_x, int p_y)
    {
        m_y = p_y;
        m_x = p_x;
    }

    public override bool Equals(object obj)
    {
        if (obj is null)
        {
            return false;
        }
        if (!(obj is GridCoordinate))
        {
            return false;
        }

        return m_x == ((GridCoordinate)obj).m_x &&
               m_y == ((GridCoordinate)obj).m_y;
    }

    protected bool Equals(GridCoordinate other)
    {
        return m_x == other.m_x && m_y == other.m_y;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(m_x, m_y);
    }
}