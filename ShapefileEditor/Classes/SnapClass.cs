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
//1/29/2005 - This code is identical to the public domain version.
//********************************************************************************************************
using System;

namespace ShapefileEditor
{

	public struct SnapData
	{
		public int shpIndex;
		public int pointIndex;
		public PointD point;
		public SnapData(int shpIndex, int pointIndex, PointD point)
		{
			this.shpIndex = shpIndex;
			this.pointIndex = pointIndex;
			this.point = point;
		}
	}

	/// <summary>
	/// Summary description for SnapClass.
	/// </summary>
	public class SnapClass
	{
		//private System.Collections.SortedList m_list;
		private System.Collections.Hashtable m_lists = new System.Collections.Hashtable();
        private System.Collections.Hashtable m_sfs = new System.Collections.Hashtable();

		private int errCnt;
		/// <summary>
		/// Constructor.
		/// Bulds a snap list from a MapWinGIS.Shapefile.
		/// </summary>
		/// <param name="sf">MapWinGIS.Shapefile used to create the list of points to snap on.</param>
		public SnapClass(MapWindow.Interfaces.IMapWin MapWin)
		{
            int startPos = MapWin.Layers.CurrentLayer;
            int endPos = MapWin.Layers.CurrentLayer+1;
            if (MapWin.Menus[GlobalFunctions.c_SnapToAllLayersButton].Checked)
            {
                startPos = 0;
                endPos = MapWin.Layers.NumLayers;
            }

            for (int i = startPos; i < endPos; i++)
            {
                if (MapWin.Layers[i].LayerType == MapWindow.Interfaces.eLayerType.LineShapefile || MapWin.Layers[i].LayerType == MapWindow.Interfaces.eLayerType.PointShapefile || MapWin.Layers[i].LayerType == MapWindow.Interfaces.eLayerType.PolygonShapefile)
                {
                    m_sfs.Add(MapWin.Layers[i].Handle, (MapWinGIS.Shapefile)MapWin.Layers[i].GetObject());
                    MapWinGIS.Shapefile m_sf = (MapWinGIS.Shapefile)m_sfs[MapWin.Layers[i].Handle];

                    try
                    {
                        m_lists.Add(MapWin.Layers[i].Handle, new System.Collections.SortedList());
                        System.Collections.SortedList m_list = (System.Collections.SortedList)m_lists[MapWin.Layers[i].Handle];

                        int numShapes = m_sf.NumShapes;
                        //int numPoints = 0;

                        for (int j = 0; j < numShapes; j++)
                        {
                            AddShapeData(j, MapWin.Layers[i].Handle);
                        }
                    }
                    catch
                    {
                    }
                }
            }
		}

		/// <summary>
		/// Finds the nearest point to the requested location that is within the tolerance radius.
		/// </summary>
		/// <param name="ProjectedRadius">The tolerance radius in projected units used to search for the nearest point that can be snapped to. </param>
		/// <param name="x">x coordinate in projected map units</param>
		/// <param name="y">y coordinate in projected map units</param>
		/// <param name="BestPoint">A PointD class with the location of the nearest point to snap to if there is are any within the tolerance, null if no points are found.</param>
		/// <returns>Returns true if there is a point to snap to.</returns>
        public bool CanSnap(double ProjectedRadius, double x, double y, ref System.Collections.ArrayList BestPoints)
		{
            PointD BestPoint = null;
            System.Collections.IDictionaryEnumerator ie = m_lists.GetEnumerator();

            while (ie.MoveNext())
            {
                int snaplayeridx = (int)ie.Key;

                if (((System.Collections.SortedList)m_lists[snaplayeridx]).Count == 0)
                    continue;

                try
                {
                    double val = (x - ProjectedRadius);
                    int first = FindFirst(((System.Collections.SortedList)m_lists[snaplayeridx]), val);
                    double bestX = x + ProjectedRadius, bestY = y + ProjectedRadius;
                    double curX;

                    if (first == -1)
                    {
                        System.Diagnostics.Debug.WriteLine("Could not find any points to snap to");
                        return false;
                    }

                    curX = (double)((System.Collections.SortedList)m_lists[snaplayeridx]).GetKey(first);

                    while (curX < (x + ProjectedRadius))
                    {
                        if (Math.Abs(curX - x) < Math.Abs(bestX - x))
                        {	// the current x is closer to the x value than the previous best
                            // now we think we have a new best x, try to find a better y value
                            System.Collections.SortedList tList = (System.Collections.SortedList)((System.Collections.SortedList)m_lists[snaplayeridx]).GetByIndex(first);
                            for (int j = 0; j < tList.Count; j++)
                            {
                                double curY = (double)tList.GetKey(j);

                                if (Math.Abs(curY - y) < Math.Abs(bestY - y))
                                {	// the current y is closer to the y value than the previous best
                                    BestPoint = new PointD(curX, curY);
                                    if (BestPoints == null) BestPoints = new System.Collections.ArrayList();
                                    BestPoints.AddRange((System.Collections.ArrayList)tList.GetByIndex(j));

                                    // only reset the bestX and bestY when both are better than the previous.
                                    bestX = curX;
                                    bestY = curY;
                                }
                            }
                        }
                        first++;
                        if (first < ((System.Collections.SortedList)m_lists[snaplayeridx]).Count)
                            curX = (double)((System.Collections.SortedList)m_lists[snaplayeridx]).GetKey(first);
                        else
                            break;
                    }
                }
                catch (System.Exception ex)
                {
                    errCnt++;
                    System.Diagnostics.Debug.WriteLine(errCnt + " " + ex.Message);
                }
            }

            if (BestPoints == null)
                return false;
            else
                return true;
		}
		
		/// <summary>
		/// Finds the nearest point to the requested location that is within the tolerance radius.
		/// </summary>
		/// <param name="ProjectedRadius">The tolerance radius in projected units used to search for the nearest point that can be snapped to.</param>
		/// <param name="x">x coordinate in projected map units</param>
		/// <param name="y">y coordinate in projected map units</param>
		/// <param name="curShape">The shape that is currently being created.  Points from this shape are also checked.</param>
		/// <param name="BestPoint">A PointD class with the location of the nearest point to snap to if there is are any within the tolerance, null if no points are found.</param>
		/// <returns>Returns true if there is a point to snap to.</returns>
		public bool CanSnap(double ProjectedRadius, double x, double y, ShapeClass curShape, ref System.Collections.ArrayList BestPoints)
		{
            System.Collections.IDictionaryEnumerator ie = m_lists.GetEnumerator();
            PointD myBest = null;
            System.Collections.ArrayList myBestPoints = null;

            while (ie.MoveNext())
            {
                int snaplayeridx = (int)ie.Key;

                // find any points that are within the tolerance in curShape.
                for (int i = 0; i < curShape.NumPoints; i++)
                {
                    PointD pt = curShape[i];
                    double d = pt.Dist(x, y);

                    if (d < ProjectedRadius)
                    {
                        if (myBest != null)
                        {
                            if (d < myBest.Dist(x, y))
                                myBest = pt;
                        }
                        else
                            myBest = pt;
                    }
                }

                if (myBest != null)
                {
                    SnapData d = new SnapData(-1, -1, myBest);
                    myBestPoints = new System.Collections.ArrayList();
                    myBestPoints.Add(d);
                }

                if (CanSnap(ProjectedRadius, x, y, ref BestPoints))
                {	// work with that best point.
                    if (myBest != null)
                    {
                        PointD tPoint = ((SnapData)BestPoints[0]).point;
                        if (myBest.Dist(x, y) < tPoint.Dist(x, y))
                        {
                            if (BestPoints == null) BestPoints = new System.Collections.ArrayList();
                            BestPoints.AddRange(myBestPoints);
                        }
                    }
                }
                else
                {
                    if (myBestPoints != null)
                    {
                        if (BestPoints == null) BestPoints = new System.Collections.ArrayList();
                        BestPoints.AddRange(myBestPoints);
                    }
                }

            }

            if (BestPoints == null)
                return false;
            else
                return true;
		}

		/// <summary>
		/// Finds the first key in the sorted list that is >= val
		/// </summary>
		/// <param name="lst">Sorted list of values.</param>
		/// <param name="val">Key value to search for.</param>
		/// <returns>Returns the first index of an item with a key >= val. If none are found -1 is returned.</returns>
		private int FindFirst(System.Collections.SortedList lst, double val)
		{  // do a binary search to find the first key occurance that is >= val
			int high = lst.Count - 1;
			int low = 0;
			int cur = low; // Initialize cur and curVal just in case there is only one value, which means low == high so the while loop will never execute.
			double curVal = (double)lst.GetKey(cur);

			while ((high - low > 2) && low != high)
			{
				cur = (high + low) / 2;
				curVal = (double)lst.GetKey(cur);

				if (curVal == val)
					return cur;
				else if (curVal < val)
				{
					low = cur;
				}
				else // (curVal > val)
				{
					high = cur;
				}
			}

			return low;
		}

		/// <summary>
		/// Adds new points to the SnapClass' point index.
		/// </summary>
		/// <param name="newShape">The shape containing data to add to the points index.</param>
		public void AddShapeData(int shpIndex, int snaplayeridx)
		{
			int numPoints = 0;
			double [] pts;
			System.Array arr = ((MapWinGIS.Shapefile)m_sfs[snaplayeridx]).QuickPoints(shpIndex, ref numPoints);
			pts = new double[arr.Length];
			arr.CopyTo(pts, 0);
			arr = null;

			for (int j = 0; j < pts.Length; j+=2)
			{
				if (((System.Collections.SortedList)m_lists[snaplayeridx]).ContainsKey(pts[j]))
				{	// the x value is already in the list.  Add the y value to the "values" list.
					System.Collections.SortedList yLst;
                    yLst = (System.Collections.SortedList)((System.Collections.SortedList)m_lists[snaplayeridx]).GetByIndex(((System.Collections.SortedList)m_lists[snaplayeridx]).IndexOfKey(pts[j]));
					if (yLst.ContainsKey(pts[j+1]) == false)
					{	// does not contain the y point yet
						SnapData data = new SnapData(shpIndex, (int)j/2, new PointD(pts[j], pts[j+1]));
						System.Collections.ArrayList l = new System.Collections.ArrayList();
						l.Add(data);
						yLst.Add(pts[j+1], l);
					}
					else
					{	// already containt the y point (duplicate point)
						SnapData data = new SnapData(shpIndex, (int)j/2, new PointD(pts[j], pts[j+1]));
						((System.Collections.ArrayList)yLst.GetByIndex(yLst.IndexOfKey(pts[j+1]))).Add(data);
					}
				}
				else
				{
					System.Collections.SortedList y_list = new System.Collections.SortedList();
					SnapData data = new SnapData(shpIndex, (int)j/2, new PointD(pts[j], pts[j+1]));
					System.Collections.ArrayList l = new System.Collections.ArrayList();
					l.Add(data);
					y_list.Add(pts[j+1], l);
                    ((System.Collections.SortedList)m_lists[snaplayeridx]).Add(pts[j], y_list);
				}
			}

			pts = null;

		}

		//Added by Lailin Chen to clear the SnapData list. 12/12/2005
		public void ClearSnapData()
		{
            foreach (System.Collections.DictionaryEntry a in m_lists)
            {
                ((System.Collections.SortedList)a.Value).Clear();
            }
            m_lists.Clear();
			//System.GC.Collect(); don't do this, let the VM determine when to collect!!!
		}
	}
}
