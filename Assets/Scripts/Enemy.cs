using UnityEngine;

public abstract class Enemy : Character
{
    protected Transform _player;

    protected override void Awake()
    {
        base.Awake();
        GameObject playerObject = GameObject.FindWithTag("Player");

        if (playerObject == null)
        {
            Debug.LogError("No hay Player para obtener su Transform.");
            return;
        }

        _player = playerObject.transform;

    }

    // Update is called once per frame
    void Update()
    {
        if (IsDead || !_player) return;
        Behave();
    }

    protected abstract void Behave();

    protected override void Die()
    {
        Destroy(gameObject);
    }
}
