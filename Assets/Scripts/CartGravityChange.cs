using UnityEngine;

namespace Game
{
    public class CartGravityChange: MonoBehaviour
    {
        [SerializeField] private float gravAngle;
        [SerializeField] private Collider2D toForget;

        public void Apply(Cart cart)
        {
            if (toForget != null)
                Physics2D.IgnoreCollision(toForget, this.GetComponent<Collider2D>());
            float gravRad = gravAngle * Mathf.Deg2Rad;
            Vector2 grav = new Vector2(Mathf.Cos(gravRad), Mathf.Sin(gravRad));
            cart.Gravity = grav;
            cart.transform.rotation = Helper.ZRot(gravAngle + 90);
            cart.Rigidbody.velocity = Vector2.zero;

        }
    }
}