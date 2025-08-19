using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ClickToMove : MonoBehaviour
{
    NavMeshAgent m_Agent;
    [SerializeField] Camera m_Camera;
    RaycastHit m_HitInfo = new RaycastHit();

    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift)) 
        {
            var ray = m_Camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out m_HitInfo))
                m_Agent.destination = m_HitInfo.point;
        }
    }
}
