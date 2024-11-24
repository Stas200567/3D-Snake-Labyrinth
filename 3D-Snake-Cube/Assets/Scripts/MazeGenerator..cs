using UnityEngine;
#if UNITY_EDITOR
using UnityEditor; // ������� ��� ���������� �ᒺ��� � ��������
#endif

public class MazeGenerator : MonoBehaviour
{
    public int mazeWidth = 10;  // ������ ��������
    public int mazeHeight = 10; // ������ ��������
    public GameObject wallPrefab; // ������ ��� ���
    public float wallThickness = 0.1f; // ������� ��� ��� �������������
    public float cellSize = 1.0f; // ����� ���� ������� ��������

    private int[,] maze; // ����� ��� ��������� ��������� ��������

    void Start()
    {
        GenerateMaze();
        RenderMaze();
    }

    void GenerateMaze()
    {
        // ����������� ������ ��������
        maze = new int[mazeWidth, mazeHeight];

        // �������� ��������� �������� (DFS)
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                // ��������� �� ������� � ����
                maze[x, y] = 1;
            }
        }

        // �������� ����� ��� ��������
        int startX = 0;
        int startY = 0;

        // �������� �������
        DepthFirstSearch(startX, startY);
    }

    void DepthFirstSearch(int x, int y)
    {
        maze[x, y] = 0; // ������� ������� ��� ������

        // �������� ��� ���������� (���������� �������)
        int[] directions = { 0, 1, 2, 3 };
        ShuffleArray(directions);

        foreach (int direction in directions)
        {
            int nx = x;
            int ny = y;

            // ��������� ��������
            switch (direction)
            {
                case 0: nx = x + 2; break; // ������
                case 1: nx = x - 2; break; // ����
                case 2: ny = y + 2; break; // �����
                case 3: ny = y - 2; break; // ����
            }

            // ���������� ��� �� ������ �������
            if (nx >= 0 && nx < mazeWidth && ny >= 0 && ny < mazeHeight && maze[nx, ny] == 1)
            {
                // ��������� ���� �� ���������
                maze[(x + nx) / 2, (y + ny) / 2] = 0;

                // ���������� �������� ����
                DepthFirstSearch(nx, ny);
            }
        }
    }

    void RenderMaze()
    {
        // ��������� ��������� �������� (���� ��� �)
        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject); // DestroyImmediate ������� ��� ���������
        }

        // ��������� ��� ��������
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                if (maze[x, y] == 1)
                {
                    // ��������� ����
                    Vector3 position = new Vector3(
                        x * cellSize - mazeWidth / 2 * cellSize,
                        0,
                        y * cellSize - mazeHeight / 2 * cellSize
                    );

                    GameObject wall = Instantiate(wallPrefab, position, Quaternion.identity, transform);
                    wall.transform.localScale = new Vector3(cellSize, wallThickness, cellSize);

                    // ������ ���� �� �����, ��� ���� ���������� ���� ������ � Play Mode
#if UNITY_EDITOR
                    EditorUtility.SetDirty(wall); // ³������ ���� �� ������
#endif
                }
            }
        }

        // ³������ �ᒺ�� �� �������
#if UNITY_EDITOR
        EditorUtility.SetDirty(gameObject);
#endif
    }

    void ShuffleArray(int[] array)
    {
        // ��������� ����� (��� ����������� �������)
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }
}
