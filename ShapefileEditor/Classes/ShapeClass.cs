//********************************************************************************************************
//The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
//you may not use this file except in compliance with the License. You may obtain a copy of the License at 
//http://www.mozilla.org/MPL/ 
//Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
//ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
//limitations under the License. 
//
//The Original Code is MapWindow Open Source. 
//
//The Initial Developer of this version of the Original Code is Daniel P. Ames using portions created by 
//Utah State University and the Idaho National Engineering and Environmental Lab that were released as 
//public domain in March 2004.  
//
//Contributor(s): (Open source contributors should list themselves and their modifications here). 
//1/29/2005 - This code is identical to the public domain version - dpa
//********************************************************************************************************
using System;

namespace ShapefileEditor
{
	/// <summary>
	/// Summary description for ShapeClass.
	/// </summary>
	public class ShapeClass
	{
		private System.Collections.ArrayList m_Points;

		/// <summary>
		/// Constructor for the ShapeClass object.
		/// </summary>
		public ShapeClass()
		{
			m_Points = new System.Collections.ArrayList();
		}

		/// <summary>
		/// Adds a point to this shape.
		/// </summary>
		/// <param name="p">System.Drawing.Point to add.</param>
		public void AddPoint(PointD p)
		{
			m_Points.Add(p);
		}

		/// <summary>
		/// Removes a point from this shape.
		/// </summary>
		/// <param name="p">System.Drawing.Point to remove.</param>
		public void RemovePoint(System.Drawing.Point p)
		{
			try 
			{
				m_Points.Remove(p);
			}
			catch
			{
				return;
			}
		}

		/// <summary>
		/// Removes a point from this shape.
		/// </summary>
		/// <param name="Index">Index in the list to remove.</param>
		public void RemovePoint(int Index)
		{
			try
			{
				m_Points.RemoveAt(Index);
			}
			catch
			{
				return;
			}
		}

		/// <summary>
		/// Indexed access to all points.
		/// </summary>
		public PointD this [int Index]
		{
			get
			{
				if (Index >= 0 && Index < m_Points.Count)
					return (PointD)m_Points[Index];
				else
					throw new System.IndexOutOfRangeException();
			}
			set
			{
				if (Index > 0 && Index < m_Points.Count)
					m_Points[Index] = value;
			}
		}

		/// <summary>
		/// Creates a MapWinGIS.Shape object from the point data specified by the user.
		/// </summary>
		/// <returns>Returns a MapWinGIS.Shape object.</returns>
		public MapWinGIS.Shape ToMWShape(MapWinGIS.ShpfileType type)
		{
			if (m_Points.Count == 0)
				return null;

			MapWinGIS.Shape shp = new MapWinGIS.ShapeClass();
			MapWinGIS.Point pnt;

			int partNum = 0;
			int pointIndex = 0;

			bool bres;
			
			bres = shp.Create(type);
			bres = shp.InsertPart(0, ref partNum);

			for(int i = 0; i < m_Points.Count; i++)
			{
				pnt = new MapWinGIS.PointClass();
				pnt.x = ((PointD)m_Points[i]).x;
				pnt.y = ((PointD)m_Points[i]).y;
				pointIndex = i;
				bres = shp.InsertPoint(pnt, ref pointIndex);
				pnt = null;
			}
			
			return shp;
		}

		/// <summary>
		/// Returns the number of points that have been added to the shape.
		/// </summary>
		public int NumPoints
		{
			get
			{
				return m_Points.Count;
			}
		}
	}
}
