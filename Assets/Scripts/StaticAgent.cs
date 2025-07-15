using UnityEngine;
using UnityEngine.AI;

public class StaticAgent : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;
    [SerializeField] GameObject destinationMarkerPrefab;
    [SerializeField] GameObject currentMarker;
    private LineRenderer pathLine;

    void Start()
    {
        Cursor.visible = true;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        pathLine = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                animator.SetFloat("Speed", 2f);
                animator.SetFloat("MotionSpeed", 1.0f);
                agent.SetDestination(hit.point);

                if (currentMarker != null) Destroy(currentMarker);

                if (destinationMarkerPrefab != null)
                {
                    currentMarker = Instantiate(destinationMarkerPrefab, hit.point, destinationMarkerPrefab.transform.rotation);
                    Debug.Log("마커 프리팹 생성");
                }

                pathLine.enabled = true;

                Debug.Log("마우스 포지션 : " + Input.mousePosition + " / hit.point : " + hit.point);
            }
        }

        if (!agent.hasPath && pathLine.enabled)
        {
            var path = agent.path;
            pathLine.positionCount = path.corners.Length;
            for (int i = 0; i < path.corners.Length; i++)
            {
                pathLine.SetPosition(i, path.corners[i]);
            }
            Debug.Log("Path 라인 셋 포지션 설정");
        }
        else
        {
            pathLine.positionCount = 0;
        }

        if (!agent.pathPending && agent.remainingDistance < 0.1f)
        {
            animator.SetFloat("Speed", 0);
            animator.SetFloat("MotionSpeed", 0);

            if (currentMarker != null)
            {
                Destroy(currentMarker);
                currentMarker = null;
            }

            pathLine.enabled = false;
        }
    }
}
