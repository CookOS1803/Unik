using UnityEngine;
using Zenject;

public class PlayerWeapon : Weapon
{
    [SerializeField] private float backstabDot = 0.5f;
    [Inject] private AIManager aiManager;
    private Transform player;

    [Inject]
    void SetPlayer(PlayerController controller)
    {
        player = controller.transform;
    }

    protected override void OnHit(Collider collider)
    {
        var enemy = collider.GetComponent<EnemyController>();

        if (enemy != null && aiManager.player == null)
        {
            Vector3 playerToEnemy =  enemy.transform.position - player.position;
            float dot = Vector3.Dot(enemy.transform.forward, playerToEnemy);

            if (dot >= backstabDot)
            {
                enemy.Die();
            }
        }
        
        base.OnHit(collider);
    }    
}
