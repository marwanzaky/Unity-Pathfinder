using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public static bool isTravelling = false;

    Pathfinder pathfinder;

    [SerializeField] float speed = 3f;

    void Start()
    {
        pathfinder = FindObjectOfType<Pathfinder>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isTravelling)
        {
            var path = pathfinder.Calc(GetCurrentNode(), Select());
            StartCoroutine(FollowPath(path));
        }
    }

    IEnumerator FollowPath(Node[] path)
    {
        isTravelling = true;
        int i = 0;

        while (i < path.Length)
        {
            transform.position = Vector3.MoveTowards(transform.position, NodePos(), speed * Time.deltaTime);

            if (transform.position == NodePos())
                i++;

            yield return null;
        }

        isTravelling = false;

        Vector3 NodePos() => Vector3X.IgnoreY(path[i].transform.position, transform.position.y);
    }

    public Node Select()
    {
        const bool DEBUG = false;
        const float MAX_DIS = 1000;

        var hit = RaycastHitX.MousePosHit(Camera.main, pathfinder.nodeLayer, MAX_DIS, DEBUG);

        if (pathfinder.nodeLayer == (pathfinder.nodeLayer | (1 << hit.collider.gameObject.layer)))
            return hit.collider.gameObject.GetComponent<Node>();

        return null;
    }

    public Node GetCurrentNode()
    {
        const float MAX_DIS = 1f;
        var hit = RaycastHitX.Cast(transform.position, Vector3.down, pathfinder.nodeLayer, MAX_DIS);

        if (hit.collider)
            return hit.collider.GetComponent<Node>();
        return null;
    }
}