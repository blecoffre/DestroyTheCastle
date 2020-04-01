using UnityEngine;

namespace Controller
{
    class CameraController : MonoBehaviour
    {
        [SerializeField]
        private float m_allowMoveDistFromBorder = 100.0f;
        public float MoveSpeed = 10.0f;
        public float LeftXLimit = 0.0f;
        public float RightXLimit = 135.0f;

        private void Update()
        {
            if (Input.GetKey(KeyCode.Mouse0) && !Utils.IsCursorOverUserInterface())
            {
                Move();
            }
        }

        private Vector3 MoveDir()
        {
            if (Input.mousePosition.x < m_allowMoveDistFromBorder)
                return Vector3.left;
            else if (Input.mousePosition.x > (Screen.width - m_allowMoveDistFromBorder))
                return Vector3.right;

            return Vector3.zero;
        }

        private void Move()
        {
            if(transform.position.x >= LeftXLimit && transform.position.x <= RightXLimit)
                transform.Translate(MoveDir() * MoveSpeed * Time.deltaTime);

            if (transform.position.x < LeftXLimit)
                transform.position = new Vector3(LeftXLimit, transform.position.y, transform.position.z);
            else if (transform.position.x > RightXLimit)
                transform.position = new Vector3(RightXLimit, transform.position.y, transform.position.z);
        }
    }
}
