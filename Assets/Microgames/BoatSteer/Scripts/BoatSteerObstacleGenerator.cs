using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatSteerObstacleGenerator {
	// 2D Implementation of Bridson's Poisson-Disc algorithm
	// Generates obstacles in linear time
	private float width;
	private float height;

	// minimum distance between obstacles
	private float obstacleDistance;
	// minimum distance between obstacles, squared for comparisons
	private float obstacleDistance2;

	// Number of candidate points to generate about each point
	private int k;

	private List<Vector2> points;
	// Bridson's algorithm uses a grid of cells of size r/sqrt(2) to accelerate the search for near neighbours
	// basically, what allows this algorithm to run in linear time for a number of obstacles
	private float cellSize;
	private int[] cells;
	private int cellCountX;
	private int cellCountY;

	public BoatSteerObstacleGenerator(float width, float height, float obstacleDistance, int k) {
		this.width = width;
		this.height = height;
		this.obstacleDistance = obstacleDistance;
		obstacleDistance2 = obstacleDistance * obstacleDistance;
		this.k = k;


		cellSize = obstacleDistance/Mathf.Sqrt(2); // Actual size of grid cells in units
		cellCountX = (int)Mathf.Ceil(width/cellSize);
		cellCountY = (int)Mathf.Ceil(height/cellSize);

		points = new List<Vector2>();
		cells = new int[cellCountX * cellCountY];
	}

	private bool CellCheck(Vector2 point, int x, int y) {
		if (x < 0 || x >= cellCountX || 
			y < 0 || y >= cellCountY) {
			// Grid cell is out of bounds or nonexistent
			return false;
		}
		int index = cells[x + cellCountX * y];
		if (index < 0) {
			return false;
		}
		Vector2 other = points[index];
		return (point - other).sqrMagnitude < obstacleDistance2;
	}

	private bool TryInsertPoint(Vector2 point) {
		int x = (int)Mathf.Floor(point.x/cellSize);	
		int y = (int)Mathf.Floor(point.y/cellSize);	

		// Check neighbouring cells
		/*
			Because every cell is of the size r/sqrt(2),
			the cells we need to check are as follows
			+---+---+---+---+---+
			|   | x | x | x |   |
			+---+---+---+---+---+
			| x | x | x | x | x |
			+---+---+---+---+---+
			| x | x | O | x | x |
			+---+---+---+---+---+
			| x | x | x | x | x |
			+---+---+---+---+---+
			|   | x | x | x |   |
			+---+---+---+---+---+
		*/

		if (
			CellCheck(point, x  , y-2) ||
			CellCheck(point, x-1, y-2) ||
			CellCheck(point, x+1, y-2)
		) {
			return false;
		}
		for (int i = -1; i<=1; i++) {
			if (
				CellCheck(point, x  , y+i) ||
				CellCheck(point, x-2, y+i) ||
				CellCheck(point, x-1, y+i) ||
				CellCheck(point, x+1, y+i) ||
				CellCheck(point, x+2, y+i)
			) {
				return false;
			}
		}
		if (
			CellCheck(point, x  , y+2) ||
			CellCheck(point, x-1, y+2) ||
			CellCheck(point, x+1, y+2)
		) {
			return false;
		}
		points.Add(point);
		cells[x + cellCountX *  y] = points.Count - 1; 
		return true;
	}

	public void GenerateObstacleField() {
		// Reset 
		points.Clear();
		for (int i=0;i<cells.Length;i++) {
			cells[i] = -1;
		}

		// Begin algorithm in earnest
		List<Vector2> activeSet = new List<Vector2>();

		//Insert initial point
		float radius = obstacleDistance * Random.value;
		float theta = Random.value * 2f * Mathf.PI;
		Vector2 initialPoint = new Vector2(width/2f, height/2f) + new Vector2(Mathf.Sin(theta), Mathf.Cos(theta)) * radius;
		TryInsertPoint(initialPoint);
		activeSet.Add(initialPoint);

		activeLoop:
		while (activeSet.Count > 0) {
			// Select a point at random from the active set
			int index = (int)Mathf.Floor(Random.value * activeSet.Count);
			Vector2 activePoint = activeSet[index];

			for (int i=0; i<k; i++) {
				// generate a new point in the annulus of R..2R around the active point
				radius = obstacleDistance * (1f + Random.value);
				theta = Random.value * 2f * Mathf.PI;
				Vector2 newPoint = activePoint + new Vector2(Mathf.Sin(theta), Mathf.Cos(theta)) * radius;
				newPoint.x = Mathf.Clamp(newPoint.x, 0, width);
				newPoint.y = Mathf.Clamp(newPoint.y, 0, height);

				if (TryInsertPoint(newPoint)) {
					activeSet.Add(newPoint);
					goto activeLoop;
				}
			}
			// None of the generate candidate points were acceptable, so this point is (probably) too crowded.
			// Remove it from the active set
			activeSet.RemoveAt(index);
		}
	}

	public List<Vector2> GetPoints() {
		return points;
	}
}