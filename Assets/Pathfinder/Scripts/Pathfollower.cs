using UnityEngine;
using System.Collections;

public class Pathfollower : MonoBehaviour {
    [SerializeField] float speed = 3f;

    public void FollowPath(Node[] nodes) {
        StartCoroutine(FollowPathIE(nodes));
    }

    IEnumerator FollowPathIE(Node[] nodes) {
        var i = 0;

        while (i < nodes.Length) {
            transform.position = Vector3.MoveTowards(transform.position, NodePos(), speed * Time.deltaTime);

            if (transform.position == NodePos())
                i++;

            yield return null;
        }

        Vector3 NodePos() => Vector3X.IgnoreY(nodes[i].transform.position, transform.position.y);
    }
}