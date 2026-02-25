using UnityEngine;

public class Enemy : Entity
{
    public Enemy_IdleState  idleState;
    public Enemy_MoveState  moveState;
    public Enemy_AttackState attackState;
    public Enemy_BattleState battleState;
    
    [Header("Movement")] 
    public float idleTime = 2;
    public float moveSpeed = 1.4f;
    [Range(0, 2)]
    public float moveAnimSpeedMultiplier = 1;

    [Header("Player Detection")]
    [SerializeField] private LayerMask playerType;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private float playerCheckDistance = 10;

    public RaycastHit2D PlayerDetection()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerCheck.position, Vector2.right * facingDirection, playerCheckDistance, playerType | groundType);
        
        // LayerMask.NameToLayer("Player")
        if (hit.collider == null || hit.collider.gameObject.gameObject.layer != GlobalStringsConfig.LayerMasks.Player)
        {
            // this is similar to 
            // hit.collider = null
            // hit.point = 0,0
            // hit.distance = 0;
            return default;

        }
        return hit;
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + (facingDirection * playerCheckDistance), playerCheck.position.y, 0));
    }
}
