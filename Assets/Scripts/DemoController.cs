using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DemoController : MonoBehaviour
{
    public enum Mode
    {
        PathFinding,
        MapBuilding
    }
    [SerializeField] private MapData _mapData;
    [SerializeField] private DemoUnit _demoUnit;
    [SerializeField] private Mode _currentMode;

    //TODO: This will be used to stack paths for a waypoint like pathing
    private List<GraphPosition> _pathPositions;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (_demoUnit == null) return;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hitResult))
                {
                    GraphPosition startPosition = _mapData.GetGraphPositionFromWorld(_demoUnit.transform.position);
                    GraphPosition endPosition = _mapData.GetGraphPositionFromWorld(hitResult.point);
                    PathResult checkResult = _mapData.FindPath(startPosition, endPosition, out List<GraphPosition> pathPositions);
                    if (checkResult == PathResult.SearchSuccess || checkResult == PathResult.GoalUnreachable)
                    {
                        _demoUnit.Move(_mapData.GetWorldPositionsFromGraphPositions(pathPositions));
                    }
                }
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hitResult))
                {
                    GraphPosition hitPosition = _mapData.GetGraphPositionFromWorld(hitResult.point);
                    SetGraphPositionIsBlocked(hitPosition, false);
                }
            }
        }
        if (Input.GetMouseButton(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitResult))
            {
                GraphPosition hitPosition = _mapData.GetGraphPositionFromWorld(hitResult.point);
                SetGraphPositionIsBlocked(hitPosition, true);
            }
        }
    }

    private void SetGraphPositionIsBlocked(GraphPosition graphPosition, bool isBlocked)
    {
        if (isBlocked)
            _mapData.SetGraphPositionBlocked(graphPosition);
        else
            _mapData.SetGraphPositionUnblocked(graphPosition);
    }
}
