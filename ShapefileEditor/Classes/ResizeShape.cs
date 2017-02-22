// Chris Michaelis 12/28/2006

using System;

namespace ShapefileEditor
{
    public class ResizeShapeClass
    {
        private GlobalFunctions g;
        private MapWindow.Events.ItemClickedEvent ItemClickedDelegate;

        public ResizeShapeClass(GlobalFunctions g)
        {
            this.g = g;

            ItemClickedDelegate = new MapWindow.Events.ItemClickedEvent(ItemClickedEvent);
            g.Events.AddHandler(ItemClickedDelegate);
        }

        public void ItemClickedEvent(string ItemName, ref bool Handled)
        {
            if (ItemName == GlobalFunctions.c_ResizeShapeButton)
            {
                if (g.SelectedShapefileIsReadonly())
                {
                    MapWinUtility.Logger.Msg("The selected shapefile is read-only and cannot be edited.", "Read-Only Shapefile");
                    return;
                }

                ResizeShapes();
                Handled = true;
            }
        }

        private void ResizeShapes()
        {
            try
            {
                System.Collections.SortedList arr = new System.Collections.SortedList();
                for (int i = 0; i < g.MapWin.View.SelectedShapes.NumSelected; i++)
                    arr.Add(g.MapWin.View.SelectedShapes[i].ShapeIndex, g.MapWin.View.SelectedShapes[i].ShapeIndex);

                if (arr.Count > 0)
                {
                    MapWinGIS.Shapefile sf = g.CurrentLayer;
                    if (sf.StartEditingShapes(true, null))
                    {
                        System.Diagnostics.Debug.WriteLine(sf.get_ErrorMsg(sf.LastErrorCode));
                        bool allCancelled = false;
                        for (int j = arr.Count - 1; j >= 0; j--)
                        {
                            // Show the dialog to get input -- resize amount
                            // Actual resizing done here also
                            Forms.ResizeShapeForm dlg = new Forms.ResizeShapeForm(g);

                            dlg.Shape = (int)arr.GetByIndex(j);
                            dlg.sf = sf;

                            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                            {
                                allCancelled = true;
                                break;
                            }
                        }

                        if (!allCancelled)
                        {
                            g.CreateUndoPoint();
                            if (sf.StopEditingShapes(true, true, null) == false)
                                MapWinUtility.Logger.Message("Failed to save the changes that were made.", "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error, System.Windows.Forms.DialogResult.OK);
                        }
                        else
                        {
                            sf.StopEditingShapes(false, true, null);
                            MapWinUtility.Logger.Message("Shape resizing has been cancelled - no changes were saved.", "Cancelled", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information, System.Windows.Forms.DialogResult.OK);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message.ToString());
            }
        }
    }
}
