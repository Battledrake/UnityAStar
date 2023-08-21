using System.Collections.Generic;
using UnityEngine;

public class DemoController : MonoBehaviour
{
    [SerializeField] private MapData _mapData;
    [SerializeField] private DemoUnit _demoUnit;

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
                    PathResult checkResult = Pathfinder.Instance.FindPath(startPosition, endPosition, _mapData.GetGraph(), out List<GraphPosition> pathPositions);
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
