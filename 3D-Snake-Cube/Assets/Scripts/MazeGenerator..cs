using UnityEngine;
#if UNITY_EDITOR
using UnityEditor; // Потрібно для збереження об’єктів у редакторі
#endif

public class MazeGenerator : MonoBehaviour
{
    public int mazeWidth = 10;  // Ширина лабіринту
    public int mazeHeight = 10; // Висота лабіринту
    public GameObject wallPrefab; // Префаб для стін
    public float wallThickness = 0.1f; // Товщина стін для масштабування
    public float cellSize = 1.0f; // Розмір однієї клітинки лабіринту

    private int[,] maze; // Масив для зберігання структури лабіринту

    void Start()
    {
        GenerateMaze();
        RenderMaze();
    }

    void GenerateMaze()
    {
        // Ініціалізація масиву лабіринту
        maze = new int[mazeWidth, mazeHeight];

        // Алгоритм генерації лабіринту (DFS)
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                // Початково всі клітинки — стіни
                maze[x, y] = 1;
            }
        }

        // Стартова точка для лабіринту
        int startX = 0;
        int startY = 0;

        // Генеруємо лабіринт
        DepthFirstSearch(startX, startY);
    }

    void DepthFirstSearch(int x, int y)
    {
        maze[x, y] = 0; // Поточна клітинка стає шляхом

        // Напрямки для переміщення (випадковий порядок)
        int[] directions = { 0, 1, 2, 3 };
        ShuffleArray(directions);

        foreach (int direction in directions)
        {
            int nx = x;
            int ny = y;

            // Визначаємо напрямок
            switch (direction)
            {
                case 0: nx = x + 2; break; // Вправо
                case 1: nx = x - 2; break; // Вліво
                case 2: ny = y + 2; break; // Вверх
                case 3: ny = y - 2; break; // Вниз
            }

            // Перевіряємо межі та статус клітинки
            if (nx >= 0 && nx < mazeWidth && ny >= 0 && ny < mazeHeight && maze[nx, ny] == 1)
            {
                // Видаляємо стіну між клітинками
                maze[(x + nx) / 2, (y + ny) / 2] = 0;

                // Рекурсивно генеруємо шлях
                DepthFirstSearch(nx, ny);
            }
        }
    }

    void RenderMaze()
    {
        // Видалення попередніх елементів (якщо такі є)
        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject); // DestroyImmediate потрібен для редактора
        }

        // Генерація стін лабіринту
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                if (maze[x, y] == 1)
                {
                    // Розміщення стіни
                    Vector3 position = new Vector3(
                        x * cellSize - mazeWidth / 2 * cellSize,
                        0,
                        y * cellSize - mazeHeight / 2 * cellSize
                    );

                    GameObject wall = Instantiate(wallPrefab, position, Quaternion.identity, transform);
                    wall.transform.localScale = new Vector3(cellSize, wallThickness, cellSize);

                    // Додаємо стіни до сцени, щоб вони залишалися після виходу з Play Mode
#if UNITY_EDITOR
                    EditorUtility.SetDirty(wall); // Відмічаємо стіну як змінену
#endif
                }
            }
        }

        // Відмічаємо об’єкт як змінений
#if UNITY_EDITOR
        EditorUtility.SetDirty(gameObject);
#endif
    }

    void ShuffleArray(int[] array)
    {
        // Перемішуємо масив (для випадкового порядку)
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }
}
