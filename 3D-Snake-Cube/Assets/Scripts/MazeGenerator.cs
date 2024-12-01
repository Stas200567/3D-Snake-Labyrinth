using UnityEngine;

public class MazeGeneratorOnLowestPlane : MonoBehaviour
{
    public GameObject wallPrefab;   // ������ ����
    public GameObject targetPlane;  // Plane 6 (���� ����� �����)
    public int mazeWidth = 10;      // ������ �������� (������� ������ �� X)
    public int mazeHeight = 10;     // ������� �������� (������� ������ �� Z)
    public float wallWidth = 1.0f;  // ������ �����
    public float wallHeight = 2.0f; // ������ �����
    public float wallDepth = 1.0f;  // ������� �����

    private void Start()
    {
        GenerateMaze();
    }

    void GenerateMaze()
    {
        // ��������� �������� ����
        ClearExistingWalls();

        // ������� ����� (Plane 6)
        Vector3 planePosition = targetPlane.transform.position;
        Quaternion planeRotation = targetPlane.transform.rotation;

        // ��������� ����� ��� ��������
        Vector3 mazeStart = planePosition - targetPlane.transform.right * (mazeWidth * wallWidth / 2)
                                             - targetPlane.transform.forward * (mazeHeight * wallWidth / 2)
                                             + Vector3.up * (wallHeight / 2); // ϳ����� ���� �� ������ �����

        // ��������� ��������
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int z = 0; z < mazeHeight; z++)
            {
                // ����� ��� ��������� ���
                if ((x % 2 == 0 || z % 2 == 0) && Random.value > 0.3f)
                {
                    // ���������� ������� �����
                    Vector3 wallPosition = mazeStart + targetPlane.transform.right * (x * wallWidth)
                                                        + targetPlane.transform.forward * (z * wallWidth);

                    // ��������� �����
                    GameObject wall = Instantiate(wallPrefab, wallPosition, Quaternion.identity);

                    // ��������� ����� ������� Plane 6
                    wall.transform.rotation = planeRotation;
                    wall.transform.localScale = new Vector3(wallWidth, wallHeight, wallDepth);

                    // ������ ����� �������� ��'������ Plane 6
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
