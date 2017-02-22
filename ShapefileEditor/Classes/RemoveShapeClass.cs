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
//1/28/2005 - Code is identical to open source version.
//1/29/2005 - Added code for removing the "remove shape" button
//1/29/2005 - Added code for re-labeling after the shapes have been edited. 
//********************************************************************************************************
using System;

namespace ShapefileEditor
{
	/// <summary>
	/// Summary description for RemoveShapeClass.
	/// </summary>
	public class RemoveShapeClass
	{
		private GlobalFunctions g;
		private MapWindow.Events.ItemClickedEvent ItemClickedDelegate;

		public RemoveShapeClass(GlobalFunctions g)
		{
			this.g = g;	
			
			ItemClickedDelegate = new MapWindow.Events.ItemClickedEvent(ItemClickedEvent);
			g.Events.AddHandler(ItemClickedDelegate);
		}

		public void ItemClickedEvent(string ItemName, ref bool Handled)
		{

			if (ItemName == GlobalFunctions.c_RemoveShapeButton)
			{
                if (g.SelectedShapefileIsReadonly())
                {
                    MapWinUtility.Logger.Msg("The selected shapefile is read-only and cannot be edited.", "Read-Only Shapefile");
                    return;
                }

				RemoveShapes();
				Handled = true;
			}
		} // End of 'public void ItemClickedEvent(string ItemName, ref bool Handled)'

		private void RemoveShapes()
		{
			try
			{
				System.Collections.SortedList arr = new System.Collections.SortedList();
				for (int i = 0; i < g.MapWin.View.SelectedShapes.NumSelected; i++)
					arr.Add(g.MapWin.View.SelectedShapes[i].ShapeIndex, g.MapWin.View.SelectedShapes[i].ShapeIndex);

				if (arr.Count > 0)
					if(MapWinUtility.Logger.Message("Do you wish to permanently remove the selected shapes?", "Confirm delete", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question, System.Windows.Forms.DialogResult.Yes) == System.Windows.Forms.DialogResult.Yes)
					{
						// Deletion has been confirmed.
						// actually delete the shapes now
						g.MapWin.View.ClearSelectedShapes();
						MapWinGIS.Shapefile sf = g.CurrentLayer;

                        g.CreateUndoPoint();

						if (sf.StartEditingShapes(true, null))
						{
							System.Diagnostics.Debug.WriteLine(sf.get_ErrorMsg(sf.LastErrorCode));
							for (int j = arr.Count - 1; j >= 0; j--)
								if(sf.EditDeleteShape((int)arr.GetByIndex(j)) == false)
									System.Diagnostics.Debug.WriteLine(sf.get_ErrorMsg(sf.LastErrorCode));
						
							if(sf.StopEditingShapes(true, true, null) == false)
                                MapWinUtility.Logger.Message("Failed to save the changes that were made.", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, System.Windows.Forms.DialogResult.OK);

                            g.MapWin.Plugins.BroadcastMessage("ShapefileEditor: Layer " + g.MapWin.Layers.CurrentLayer.ToString() + ": Shape Deleted");
						}

						//added 1/29/2005
						g.MapWin.Toolbar.ButtonItem("RemoveShapeButton").Enabled = false;
						g.MapWin.Toolbar.ButtonItem("RemoveShapeButton").Tooltip = "Remove shapes (a shape must be selected)";

                        g.UpdateView();
					}
			}
			catch (System.Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message.ToString());
			}
		} // end void RemoveShapes()
	} // end class
}
