using UnityEngine;

public static class TransforExtension
{
    const float maxX = 45;
    const float maxY = 45;

    const float groundX = 90.0f;
    const float groundY = 90.0f;

    public static (Vector2, Quaternion) GetPositionAndRotaion(this Transform _)
    {
        int delta = GameManager.maxLevelInGame * 5;

        float x = Random.Range(-maxX - delta, maxX + delta);
        float y = Random.Range(-maxY - delta, maxY + delta);

        Vector2 position = new Vector2(x, y);
        Quaternion rotation = Quaternion.Euler(Vector3.forward * Random.Range(0, 360.0f));
        return (position, rotation);
    }

    public static bool CanMove(this Vector2 _target)
    {
        bool canX = _target.x > -groundX && _target.x < groundX;
        bool canY = _target.y > -groundY && _target.y < groundY;

        return canX && canY;
    }
}