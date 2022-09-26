using Biocrowds.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RegularGridMarkerSpawner : MarkerSpawner
{
    public override IEnumerator CreateMarkers(List<Cell> cells, List<Auxin> auxins)
    {
        _auxinsContainer = new GameObject("Markers").transform;
        _cellSize = cells[0].transform.localScale.x;

        // Generate a number of markers for each Cell
        for (int c = 0; c < cells.Count; c++)
        {

            
            StartCoroutine(PopulateCell(cells[c], auxins, c));

        }

        yield break;
    }

    private IEnumerator PopulateCell(Cell cell, List<Auxin> auxins, int cellIndex)
    {
        var bounds = new Bounds(cell.transform.position, Vector3.one * (_cellSize - MarkerRadius));
        var floatCorrection = MarkerRadius / 4f;
        int count = 0;

        for (float _x = bounds.min.x; _x <= bounds.max.x + floatCorrection; _x += MarkerRadius)
        {
            for (float _z = bounds.min.z; _z <= bounds.max.z + floatCorrection; _z += MarkerRadius)
            {
                Vector3 targetPosition = new Vector3(_x, 0f, _z);

                if (HasObstacleNearby(targetPosition) || !IsOnNavmesh(targetPosition))
                    continue;
                
                // Creates new Marker and sets its data
                Auxin newMarker = Instantiate(auxinPrefab, targetPosition, Quaternion.identity, _auxinsContainer);
                newMarker.transform.localScale = Vector3.one * MarkerRadius;
                newMarker.name = "Marker [" + cellIndex + "][" + count + "]";
                newMarker.Cell = cell;
                newMarker.Position = targetPosition;
                newMarker.ShowMesh(SceneController.ShowAuxins);

                auxins.Add(newMarker);
                cell.Auxins.Add(newMarker);
                count++;
            }
            MarkerRadius += MarkerDecay;
        }
        yield break;
    }

}
