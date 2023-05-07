using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileMap;

namespace TES_TriangleEscapeSimulator.PathFinding
{
    public class PathFinder
    {
        //has not been tested properly; would've been used for an enemy that chases the player;
        //wasn't able to implement it due to time constraints;
        public List<Node> FindPath(Vector2 startPosition, Vector2 targetPosition, int[,] map)
        {
            Grid grid = new Grid(map);
            Node startNode = grid.GetNodeFromPosition(startPosition);
            Node targetNode = grid.GetNodeFromPosition(targetPosition);
            List<Node> path = new List<Node>();

            List<Node> openList = new List<Node>();
            HashSet<Node> closedList = new HashSet<Node>();
            openList.Add(startNode);

            //begin searching;
            while(openList.Count > 0)
            {
                Node node = openList[0];
                //find the node with the smallest costs;
                for (int i = 1; i < openList.Count; i++)
                {
                    if (openList[i].fCost < node.fCost ||
                        openList[i].fCost == node.fCost)
                    {
                        //only switch if hCosts are smaller;
                        if (openList[i].hCost < node.hCost)
                        {
                            node = openList[i];
                        }
                    }
                }

                //remove and add that node to the closed list;
                openList.Remove(node);
                closedList.Add(node);

                //was that node the target? if so, retrace the path;
                if (node == targetNode)
                {
                    path = GetPath(startNode, targetNode);
                    return path;
                }

                foreach(Node neighbour in grid.GetNeighbourNodes(node))
                {
                    //already in closed list, or not passable;
                    if (!neighbour.walkable || closedList.Contains(neighbour))
                    {
                        continue;
                    }

                    int neighboursnewCost = node.gCost + GetDistanceBetweenNodes(node, neighbour);
                    //if neighbours cost need to be updated, or neighbour is neither in closed, nor open;
                    if (neighboursnewCost < neighbour.gCost ||
                        !openList.Contains(neighbour))
                    {
                        neighbour.gCost = neighboursnewCost;
                        neighbour.hCost = GetDistanceBetweenNodes(neighbour, targetNode);
                        neighbour.parent = node;

                        //add neighbour to the open list if it is not already in there;
                        if (!openList.Contains(neighbour))
                        {
                            openList.Add(neighbour);
                        }
                    }
                }
            } //close while;
            //wasnt able to find a path, return empty list;
            return path;
        } //close FindPath;

        private List<Node> GetPath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();
            //at this point path has been secured;
            //endNode has to have the right parent, due to our efforts;
            Node actNode = endNode;
            while(actNode != startNode)
            {
                path.Add(actNode);
                actNode = actNode.parent;
            }
            //we traced backwards which means we have to reverse our list;
            path.Reverse();
            return path;
        }

        //heuristic costs;
        private int GetDistanceBetweenNodes(Node node1, Node node2)
        {
            int distanceY = Math.Abs(node1.gridY - node2.gridY);
            int distanceX = Math.Abs(node1.gridX - node2.gridX);

            return (distanceY + distanceX) * 10;
        }
    } //close class;
}
