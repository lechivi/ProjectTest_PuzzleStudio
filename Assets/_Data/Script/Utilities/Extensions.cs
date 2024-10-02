using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static IEnumerable<Coord> GetNeighborCoordinate(Coord coord)
		{
			int col = coord.Col;
			int row = coord.Row;

			if (col % 2 == 0) // even column
			{
				yield return new Coord(col, row + 1);
				yield return new Coord(col + 1, row);
				yield return new Coord(col + 1, row - 1);
				yield return new Coord(col, row - 1);
				yield return new Coord(col - 1, row - 1);
				yield return new Coord(col - 1, row);
			}
			else // odd column
			{
				yield return new Coord(col, row + 1);
				yield return new Coord(col + 1, row + 1);
				yield return new Coord(col + 1, row);
				yield return new Coord(col, row - 1);
				yield return new Coord(col - 1, row);
				yield return new Coord(col - 1, row + 1);
			}
		}

		public static Coord GetHexNeighborWithDirection(Coord coord, float direction)
		{
			int col = coord.Col;
			int row = coord.Row;

			if (col % 2 == 0) // even column
			{
				switch (direction)
				{
					default:
						return new Coord(col, row + 1);
					case 1:
						return new Coord(col + 1, row);
					case 2:
						return new Coord(col + 1, row - 1);
					case 3:
						return new Coord(col, row - 1);
					case 4:
						return new Coord(col - 1, row - 1);
					case 5:
						return new Coord(col - 1, row);
				}
			}
			else // odd column
			{
				switch (direction)
				{
					default:
						return new Coord(col, row + 1);
					case 1:
						return new Coord(col + 1, row + 1);
					case 2:
						return new Coord(col + 1, row);
					case 3:
						return new Coord(col, row - 1);
					case 4:
						return new Coord(col - 1, row);
					case 5:
						return new Coord(col - 1, row + 1);
				}
			}
		}
}
