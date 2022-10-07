using UnityEngine;

public static class TransforExtension
{
    const float maxX = 25;
    const float maxY = 25;

    public static (Vector2, Quaternion) GetPositionAndRotaion(this Transform _)
    {
        int delta = GameManager.maxLevelInGame * 5;

        float x = Random.Range(-maxX - delta, maxX + delta);
        float y = Random.Range(-maxY - delta, maxY + delta);

        Vector2 position = new Vector2(x, y);
        Quaternion rotation = Quaternion.Euler(Vector3.forward * Random.Range(0, 360.0f));
        return (position, rotation);
    }
}