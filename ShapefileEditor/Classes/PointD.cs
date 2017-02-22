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
//1/29/2005 This code is identical to the public domain version.
//********************************************************************************************************
using System;

namespace ShapefileEditor
{
	/// <summary>
	/// Summary description for PointD.
	/// </summary>
	public class PointD
	{
		private double m_x, m_y;

		/// <summary>
		/// PointD constructor.
		/// </summary>
		/// <param name="x">x value</param>
		/// <param name="y">y value</param>
		public PointD(double x, double y)
		{
			m_x = x;
			m_y = y;
		}

		/// <summary>
		/// x value
		/// </summary>
		public double x
		{
			get
			{
				return m_x;
			}
		}

		/// <summary>
		/// y value
		/// </summary>
		public double y
		{
			get
			{
				return m_y;
			}
		}

		/// <summary>
		/// Calculates the distance between this point and pt2.
		/// </summary>
		/// <param name="pt2">The second point.</param>
		/// <returns>Returns the distance between this point and pt2.</returns>
		public double Dist(PointD pt2)
		{
			return Math.Sqrt(Math.Pow(m_x - pt2.x, 2.0) + Math.Pow(m_y - pt2.y, 2.0));
		}

		/// <summary>
		/// Calculates the distance between this point and the coordinates given
		/// </summary>
		/// <param name="x">x coordinate.</param>
		/// <param name="y">y coordinate.</param>
		/// <returns>Returns the distance between this point and (x, y).</returns>
		public double Dist(double x, double y)
		{
			return Math.Sqrt(Math.Pow(m_x - x, 2.0) + Math.Pow(m_y - y, 2.0) );
		}

		/// <summary>
		/// Calculates the distance between two points.
		/// </summary>
		/// <param name="pt1">Point 1.</param>
		/// <param name="pt2">Point 2.</param>
		/// <returns>Returns the distance between pt1 and pt2.</returns>
		public static double Dist(PointD pt1, PointD pt2)
		{
			return Math.Sqrt(Math.Pow(pt1.x - pt2.x, 2.0) + Math.Pow(pt2.y - pt2.y, 2.0));
		}

		/// <summary>
		/// Calculates the distance between two points.
		/// </summary>
		/// <param name="pt1">Point 1</param>
		/// <param name="x2">x2 coordinate.</param>
		/// <param name="y2">y2 coordinate.</param>
		/// <returns>The distance between pt1 and (x2, y2).</returns>
		public static double Dist(PointD pt1, double x2, double y2)
		{
			return Math.Sqrt(Math.Pow(pt1.x - x2, 2.0) + Math.Pow(pt1.y - y2, 2.0));
		}

		/// <summary>
		/// Calcluates the distance between two points.
		/// </summary>
		/// <param name="x1">x1 coordinate.</param>
		/// <param name="y1">y1 coordinate.</param>
		/// <param name="x2">x2 coordinate.</param>
		/// <param name="y2">y2 coordinate.</param>
		/// <returns>Returns the distance between the two points.</returns>
		public static double Dist(double x1, double y1, double x2, double y2)
		{
			return Math.Sqrt(Math.Pow(x2 - x1, 2.0) + Math.Pow(y2 - y1, 2.0));
		}
	}

}
