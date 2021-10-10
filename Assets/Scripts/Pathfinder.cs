using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Pathfinder : MonoBehaviour {
    #region Singletone

    public static Pathfinder Instance { get; private set; }

    void Singletone() {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);
    }

    #endregion

    [SerializeField] Node startNode;
    [SerializeField] Node endNode;

    public Node StartNode {
        get => startNode;
    }

    public Node EndNode {
        get => endNode;
    }

    void Awake() {
        Instance = null;
    }

    void Start() {
        Singletone();
    }

    public void FindPath(Node endNode) {
        StartCoroutine(FindPathIE(endNode));
    }

    IEnumerator FindPathIE(Node endNode) {
        var path = new List<Node>();
        var nearest = startNode;

        this.endNode = endNode;
        path.Add(startNode);

        for (int i = 0; i < 3; i++) {
            if (nearest == endNode) {
                Debug.Log("Pathfinder found the path node!", nearest);
                yield break;
                // return path.ToArray();
            }

            nearest = nearest.FindNearestNode();
            path.Add(nearest);

            Debug.Log("Nearest node found", nearest.gameObject);
            yield return new WaitForSeconds(2);
        }

        Debug.Log("result", nearest);
    }
}