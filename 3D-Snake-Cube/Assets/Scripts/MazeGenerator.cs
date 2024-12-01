using UnityEngine;

public class MazeGeneratorOnLowestPlane : MonoBehaviour
{
    public GameObject wallPrefab;   // Префаб стіни
    public GameObject targetPlane;  // Plane 6 (сама нижня плита)
    public int mazeWidth = 10;      // Ширина лабіринту (кількість секцій по X)
    public int mazeHeight = 10;     // Довжина лабіринту (кількість секцій по Z)
    public float wallWidth = 1.0f;  // Ширина стінки
    public float wallHeight = 2.0f; // Висота стінки
    public float wallDepth = 1.0f;  // Глибина стінки

    private void Start()
    {
        GenerateMaze();
    }

    void GenerateMaze()
    {
        // Видаляємо попередні стіни
        ClearExistingWalls();

        // Позиція плити (Plane 6)
        Vector3 planePosition = targetPlane.transform.position;
        Quaternion planeRotation = targetPlane.transform.rotation;

        // Початкова точка для лабіринту
        Vector3 mazeStart = planePosition - targetPlane.transform.right * (mazeWidth * wallWidth / 2)
                                             - targetPlane.transform.forward * (mazeHeight * wallWidth / 2)
                                             + Vector3.up * (wallHeight / 2); // Підняти стіни на висоту плити

        // Генерація лабіринту
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int z = 0; z < mazeHeight; z++)
            {
                // Логіка для створення стін
                if ((x % 2 == 0 || z % 2 == 0) && Random.value > 0.3f)
                {
                    // Розрахунок позиції стінки
                    Vector3 wallPosition = mazeStart + targetPlane.transform.right * (x * wallWidth)
                                                        + targetPlane.transform.forward * (z * wallWidth);

                    // Створення стінки
                    GameObject wall = Instantiate(wallPrefab, wallPosition, Quaternion.identity);

                    // Оригінація стінок відносно Plane 6
                    wall.transform.rotation = planeRotation;
                    wall.transform.localScale = new Vector3(wallWidth, wallHeight, wallDepth);

                    // Робимо стінки дочірніми об'єктами Plane 6
                    wall.transform.SetParent(targetPlane.transform);
                }
            }
        }
    }

    void ClearExistingWalls()
    {
        foreach (Transform child in targetPlane.transform)
        {
            if (child.gameObject.CompareTag("Wall"))
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }
}
