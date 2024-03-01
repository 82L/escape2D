public class GridCoordinates
{
    private int m_x;
    private int m_y;
    public int X => m_x;
    public int Y => m_y;

    public GridCoordinates(int p_x, int p_y)
    {
        m_y = p_y;
        m_x = p_x;
    }
}