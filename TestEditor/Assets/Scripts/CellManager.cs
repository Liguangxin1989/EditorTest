
using UnityEngine;

namespace BubbleCouple
{
    public static class CellManager
    {
        public static readonly float BubbleRadius = 0.48f;
        public static readonly float EdgeSize;
        public static readonly int MinRowValue = -10;
        public static readonly int MaxRowValue = 10;

        private static float m_unitX;
        private static float m_unitY;
        private static float m_minSquareDistance;

        public struct CellID
        {
            public int x;
            public int y;

            static public implicit operator CellID(GridPosition grid)
            {
                CellID cellId = new CellID();
                cellId.x = grid.col;
                cellId.y = grid.row;
                return cellId;
            }
        }

        static CellManager()
        {
            EdgeSize = BubbleRadius * 2 / Mathf.Sqrt(3);

            m_unitX = EdgeSize * 1.5f;
            m_unitY = EdgeSize * Mathf.Sqrt(3);
            m_minSquareDistance = EdgeSize * EdgeSize * 0.75f;
        }

        public static CellID GetCellID(Vector2 position)
        {
            Vector2[] points = new Vector2[3];
            CellID[] ids = new CellID[3];

            float dist;
            float minDist = float.MaxValue;

            float fcx = position.x / m_unitX;
            float fcy = position.y / m_unitY;
            int cx = Mathf.FloorToInt(position.x / m_unitX);
            int cy = Mathf.FloorToInt(position.y / m_unitY);

            points[0].y = m_unitY * cy;
            points[1].y = m_unitY * (cy + 0.5f);
            points[2].y = m_unitY * (cy + 1.0f);

            if (cx % 2 == 0)
            {
                //   |\
                //   | \
                //   | /
                //   |/
                points[0].x = points[2].x = m_unitX * cx;
                points[1].x = m_unitX * (cx + 1);

                ids[0].x = cx;
                ids[1].x = cx + 1;
                ids[2].x = cx;

                ids[0].y = cy;
                ids[1].y = cy + 1;
                ids[2].y = cy + 1;
            }
            else
            {
                //   /| 
                //  / |
                //  \ |
                //   \|
                points[0].x = points[2].x = m_unitX * (cx + 1);
                points[1].x = m_unitX * cx;

                ids[0].x = cx + 1;
                ids[1].x = cx;
                ids[2].x = cx + 1;

                ids[0].y = cy;
                ids[1].y = cy + 1;
                ids[2].y = cy + 1;
            }

            int index = 0;
            for (int i = 0; i < 3; ++i)
            {
                dist = (position - points[i]).sqrMagnitude;
                if (dist < m_minSquareDistance)
                {
                    index = i;
                    break;
                }

                if (dist < minDist)
                {
                    minDist = dist;
                    index = i;
                }
            }

            return ids[index];
        }


        public static Vector2 GetCellPosition(CellID cellID)
        {
            float x = cellID.x * m_unitX;
            float y = 0;
            if (cellID.x % 2 == 0)
                y = m_unitY * cellID.y;
            else
                y = m_unitY * (cellID.y - 0.5f);

            return new Vector2(x, -y);
        }
    }
}