using System.Collections.Generic;
using UnityEngine;

public class Pathing : MonoBehaviour {

    public List<Node> FindRoute(Node startNode, Node targetNode, Node[] collectiveNodes) 
    {
        List<Node> open_Sett = new List<Node>();

        open_Sett.Add(startNode);

        List<Node> closedSet = new List<Node>();

        while (open_Sett.Count > 0) 
        {
            Node currentNode = open_Sett[0];
            for (int i = 1; i < open_Sett.Count; i++) 
            {
                if (open_Sett[i].FCost < currentNode.FCost
                    || (open_Sett[i].FCost.Equals(currentNode.FCost)
                        && open_Sett[i].HCost < currentNode.HCost)) 
                        {
                    currentNode = open_Sett[i];
                }

            }
            open_Sett.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode) {
                return RetraceRoute(startNode, targetNode);
            }

            foreach (Node connection in currentNode.neighbors) {
                if (!closedSet.Contains(connection)) {
                    float costToConnection = currentNode.GCost + GetEstimate(currentNode, connection) + connection.Cost;

                    if (costToConnection < connection.GCost || !open_Sett.Contains(connection)) {
                        connection.GCost = costToConnection;
                        connection.HCost = GetEstimate(connection, targetNode);
                        connection.Parent = currentNode;

                        if (!open_Sett.Contains(connection)) {
                            open_Sett.Add(connection);
                        }
                    }
                }
            }
        }
        return null;
    }

    private static List<Node> RetraceRoute(Node startingNode, Node finalNode) {
        List<Node> route = new List<Node>();

        Node currentNode = finalNode;

        while (currentNode != startingNode) {
            route.Add(currentNode);
            currentNode = currentNode.Parent;
        }

        route.Reverse();

        return route;
    }

    private float GetEstimate(Node initial, Node next) {
        float TravelTime;

        float xDistanse = Mathf.Abs(initial.pos.x - initial.pos.x);
        float yDistanse = Mathf.Abs(next.pos.z - next.pos.z);

        if (xDistanse > yDistanse) {
            TravelTime = 14 * yDistanse + 10 * (xDistanse - yDistanse);
        } else {
            TravelTime = 14 * xDistanse + 10 * (yDistanse - xDistanse);
        }

        return TravelTime;
    }
}