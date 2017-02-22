// Chris Michaelis 12/28/2006

using System;

namespace ShapefileEditor
{
    public class InsertStockShapeClass
    {
        private GlobalFunctions g;
        private MapWindow.Events.ItemClickedEvent ItemClickedDelegate;

        public InsertStockShapeClass(GlobalFunctions g)
        {
            this.g = g;

            ItemClickedDelegate = new MapWindow.Events.ItemClickedEvent(ItemClickedEvent);
            g.Events.AddHandler(ItemClickedDelegate);
        }

        public void ItemClickedEvent(string ItemName, ref bool Handled)
        {
            if (ItemName == GlobalFunctions.c_InsertStockShapeButton)
            {
                if (g.SelectedShapefileIsReadonly())
                {
                    MapWinUtility.Logger.Msg("The selected shapefile is read-only and cannot be edited.", "Read-Only Shapefile");
                    return;
                }

                Forms.InsertStockShapeForm dlg = new Forms.InsertStockShapeForm(g);
                dlg.Show();
                Handled = true;
            }
        }
    }
}
