// Chris Michaelis 12/21/2006

using System;

namespace ShapefileEditor
{
    public class RotateShapeClass
    {
        private GlobalFunctions g;
        private MapWindow.Events.ItemClickedEvent ItemClickedDelegate;
        private Forms.RotateShapeForm dlg = null;

        public RotateShapeClass(GlobalFunctions g)
        {
            this.g = g;

            ItemClickedDelegate = new MapWindow.Events.ItemClickedEvent(ItemClickedEvent);
            g.Events.AddHandler(ItemClickedDelegate);
        }

        public void ItemClickedEvent(string ItemName, ref bool Handled)
        {
            if (ItemName == GlobalFunctions.c_RotateShapeButton)
            {
                if (g.SelectedShapefileIsReadonly())
                {
                    MapWinUtility.Logger.Msg("The selected shapefile is read-only and cannot be edited.", "Read-Only Shapefile");
                    return;
                }

                RotateShapes();
                Handled = true;
            }
            else
            {
                if (dlg != null && !dlg.IsDisposed && dlg.Visible)
                    dlg.Close();
            }
        }

        private void RotateShapes()
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
                            // Show the dialog to get input -- rotation amount,
                            // rotate about point, etc. Note that dialog defaults
                            // to rotate about the centroid, which is calculated
                            // when the shape is set.
                            // Actual rotation will be done here as well.
                            dlg = new Forms.RotateShapeForm(g);

                            dlg.sf = sf;
                            dlg.Shape = sf.get_Shape((int)arr.GetByIndex(j));

                            // Note -- don't show modally; we want the user
                            // to be able to click the map to choose a point if needed
                            dlg.Show(g.MapWindowForm);
                            // However, execution should not proceed until the user has finished...
                            // So we use a really old-style waiting scheme
                            while (dlg.Visible) System.Windows.Forms.Application.DoEvents();

                            if (dlg.DialogResult == System.Windows.Forms.DialogResult.Cancel)
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
