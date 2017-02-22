//********************************************************************************************************
//File Name: Events.cs
//Description: Public class used by shapefile editor to define events. 
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
//1/29/2005 - Code is identical to the 3.0 public domain version.
//********************************************************************************************************
using events = MapWindow.Events;

namespace MapWindow
{
	/// <summary>
	/// Event dispatcher for MapWindow Plugin events.
	/// </summary>
	public class Events
	{
		public Events() { }

		
		public delegate void InitializeEvent(MapWindow.Interfaces.IMapWin MapWin, int ParentHandle);
		public delegate void ItemClickedEvent(string ItemName, ref bool Handled);
		public delegate void LayerRemovedEvent(int Handle);
		public delegate void LayersAddedEvent(MapWindow.Interfaces.Layer[] Layers);
		public delegate void LayersClearedEvent();
		public delegate void LayerSelectedEvent(int Handle);
		public delegate void LegendDoubleClickEvent(int Handle, MapWindow.Interfaces.ClickLocation Location, ref bool Handled);
		public delegate void LegendMouseDownEvent(int Handle, int Button, MapWindow.Interfaces.ClickLocation Location, ref bool Handled);
		public delegate void LegendMouseUpEvent(int Handle, int Button, MapWindow.Interfaces.ClickLocation Location, ref bool Handled);
		public delegate void MapDragFinishedEvent(System.Drawing.Rectangle Bounds, ref bool Handled);
		public delegate void MapExtentsChangedEvent();
		public delegate void MapMouseDownEvent(int Button, int Shift, int x, int y, ref bool Handled);
		public delegate void MapMouseMoveEvent(int ScreenX, int ScreenY, ref bool Handled);
		public delegate void MapMouseUpEvent(int Button, int Shift, int x, int y, ref bool Handled);
		public delegate void MessageEvent(string msg, ref bool Handled);
		public delegate void ProjectLoadingEvent(string ProjectFile, string SettingsString);
		public delegate void ProjectSavingEvent(string ProjectFile, ref string SettingsString);
		public delegate void ShapesSelectedEvent(int Handle, MapWindow.Interfaces.SelectInfo SelectInfo);
		public delegate void TerminateEvent();

		private events.InitializeEvent m_Initialize;
		private events.ItemClickedEvent m_ItemClicked;
		private events.LayerRemovedEvent m_LayerRemoved;
		private events.LayersAddedEvent m_LayersAdded;
		private events.LayersClearedEvent m_LayersCleared;
		private events.LayerSelectedEvent m_LayerSelected;
		private events.LegendDoubleClickEvent m_LegendDoubleClick;
		private events.LegendMouseDownEvent m_LegendMouseDown;
		private events.LegendMouseUpEvent m_LegendMouseUp;
		private events.MapDragFinishedEvent m_MapDragFinished;
		private events.MapExtentsChangedEvent m_MapExtentsChanged;
		private events.MapMouseDownEvent m_MapMouseDown;
		private events.MapMouseMoveEvent m_MapMouseMove;
		private events.MapMouseUpEvent m_MapMouseUp;
		private events.MessageEvent m_Message;
		private events.ProjectLoadingEvent m_ProjectLoading;
		private events.ProjectSavingEvent m_ProjectSaving;
		private events.ShapesSelectedEvent m_ShapesSelected;
		private events.TerminateEvent m_Terminate;
		
		public void AddHandler(events.InitializeEvent d) {if(d != null) m_Initialize += d;}
		public void AddHandler(events.ItemClickedEvent d) {if(d != null) m_ItemClicked += d;}
		public void AddHandler(events.LayerRemovedEvent d) {if(d != null) m_LayerRemoved += d;}
		public void AddHandler(events.LayersAddedEvent d) {if(d != null) m_LayersAdded += d;}
		public void AddHandler(events.LayersClearedEvent d) {if(d != null) m_LayersCleared += d;}
		public void AddHandler(events.LayerSelectedEvent d) {if(d != null) m_LayerSelected += d;}
		public void AddHandler(events.LegendDoubleClickEvent d) {if(d != null) m_LegendDoubleClick += d;}
		public void AddHandler(events.LegendMouseDownEvent d) {if(d != null) m_LegendMouseDown += d;}
		public void AddHandler(events.LegendMouseUpEvent d) {if(d != null) m_LegendMouseUp += d;}
		public void AddHandler(events.MapDragFinishedEvent d) {if(d != null) m_MapDragFinished += d;}
		public void AddHandler(events.MapExtentsChangedEvent d) {if(d != null) m_MapExtentsChanged += d;}
		public void AddHandler(events.MapMouseDownEvent d) {if(d != null) m_MapMouseDown += d;}
		public void AddHandler(events.MapMouseMoveEvent d) {if(d != null) m_MapMouseMove += d;}
		public void AddHandler(events.MapMouseUpEvent d) {if(d != null) m_MapMouseUp += d;}
		public void AddHandler(events.MessageEvent d) {if(d != null) m_Message += d;}
		public void AddHandler(events.ProjectLoadingEvent d) {if(d != null) m_ProjectLoading += d;}
		public void AddHandler(events.ProjectSavingEvent d) {if(d != null) m_ProjectSaving += d;}
		public void AddHandler(events.ShapesSelectedEvent d) {if(d != null) m_ShapesSelected += d;}
		public void AddHandler(events.TerminateEvent d) {if(d != null) m_Terminate += d;}
	
		public void RemoveHandler(events.InitializeEvent d) {if(d != null) m_Initialize -= d;}
		public void RemoveHandler(events.ItemClickedEvent d) {if(d != null) m_ItemClicked -= d;}
		public void RemoveHandler(events.LayerRemovedEvent d) {if(d != null) m_LayerRemoved -= d;}
		public void RemoveHandler(events.LayersAddedEvent d) {if(d != null) m_LayersAdded -= d;}
		public void RemoveHandler(events.LayersClearedEvent d) {if(d != null) m_LayersCleared -= d;}
		public void RemoveHandler(events.LayerSelectedEvent d) {if(d != null) m_LayerSelected -= d;}
		public void RemoveHandler(events.LegendDoubleClickEvent d) {if(d != null) m_LegendDoubleClick -= d;}
		public void RemoveHandler(events.LegendMouseDownEvent d) {if(d != null) m_LegendMouseDown -= d;}
		public void RemoveHandler(events.LegendMouseUpEvent d) {if(d != null) m_LegendMouseUp -= d;}
		public void RemoveHandler(events.MapDragFinishedEvent d) {if(d != null) m_MapDragFinished -= d;}
		public void RemoveHandler(events.MapExtentsChangedEvent d) {if(d != null) m_MapExtentsChanged -= d;}
		public void RemoveHandler(events.MapMouseDownEvent d) {if(d != null) m_MapMouseDown -= d;}
		public void RemoveHandler(events.MapMouseMoveEvent d) {if(d != null) m_MapMouseMove -= d;}
		public void RemoveHandler(events.MapMouseUpEvent d) {if(d != null) m_MapMouseUp -= d;}
		public void RemoveHandler(events.MessageEvent d) {if(d != null) m_Message -= d;}
		public void RemoveHandler(events.ProjectLoadingEvent d) {if(d != null) m_ProjectLoading -= d;}
		public void RemoveHandler(events.ProjectSavingEvent d) {if(d != null) m_ProjectSaving -= d;}
		public void RemoveHandler(events.ShapesSelectedEvent d) {if(d != null) m_ShapesSelected -= d;}
		public void RemoveHandler(events.TerminateEvent d) {if(d != null) m_Terminate -= d;}

		public void FireInitializeEvent(MapWindow.Interfaces.IMapWin MapWin, int ParentHandle) {if(m_Initialize != null) m_Initialize(MapWin, ParentHandle);}
		public void FireItemClickedEvent(string ItemName, ref bool Handled) {if(m_ItemClicked != null) m_ItemClicked(ItemName, ref Handled);}
		public void FireLayerRemovedEvent(int Handle) {if(m_LayerRemoved != null) m_LayerRemoved(Handle);}
		public void FireLayersAddedEvent(MapWindow.Interfaces.Layer[] Layers) {if(m_LayersAdded != null) m_LayersAdded(Layers);}
		public void FireLayersClearedEvent() {if(m_LayersCleared != null) m_LayersCleared();}
		public void FireLayerSelectedEvent(int Handle) {if(m_LayerSelected != null) m_LayerSelected(Handle);}
		public void FireLegendDoubleClickEvent(int Handle, MapWindow.Interfaces.ClickLocation Location, ref bool Handled) {if(m_LegendDoubleClick != null) m_LegendDoubleClick(Handle, Location, ref Handled);}
		public void FireLegendMouseDownEvent(int Handle, int Button, MapWindow.Interfaces.ClickLocation Location, ref bool Handled) {if(m_LegendMouseDown != null) m_LegendMouseDown(Handle, Button, Location, ref Handled);}
		public void FireLegendMouseUpEvent(int Handle, int Button, MapWindow.Interfaces.ClickLocation Location, ref bool Handled) {if(m_LegendMouseUp != null) m_LegendMouseUp(Handle, Button, Location, ref Handled);}
		public void FireMapDragFinishedEvent(System.Drawing.Rectangle Bounds, ref bool Handled) {if(m_MapDragFinished != null) m_MapDragFinished(Bounds, ref Handled);}
		public void FireMapExtentsChangedEvent() {if(m_MapExtentsChanged != null) m_MapExtentsChanged();}
		public void FireMapMouseDownEvent(int Button, int Shift, int x, int y, ref bool Handled) {if(m_MapMouseDown != null) m_MapMouseDown(Button, Shift, x, y, ref Handled);}
		public void FireMapMouseMoveEvent(int ScreenX, int ScreenY, ref bool Handled) {if(m_MapMouseMove != null) m_MapMouseMove(ScreenX, ScreenY, ref Handled);}
		public void FireMapMouseUpEvent(int Button, int Shift, int x, int y, ref bool Handled) {if(m_MapMouseUp != null) m_MapMouseUp(Button, Shift, x, y, ref Handled);}
		public void FireMessageEvent(string msg, ref bool Handled) {if(m_Message != null) m_Message(msg, ref Handled);}
		public void FireProjectLoadingEvent(string ProjectFile, string SettingsString) {if(m_ProjectLoading != null) m_ProjectLoading(ProjectFile, SettingsString);}
		public void FireProjectSavingEvent(string ProjectFile, ref string SettingsString) {if(m_ProjectSaving != null) m_ProjectSaving(ProjectFile, ref SettingsString);}
		public void FireShapesSelectedEvent(int Handle, MapWindow.Interfaces.SelectInfo SelectInfo) {if(m_ShapesSelected != null) m_ShapesSelected(Handle, SelectInfo);}
		public void FireTerminateEvent() {if(m_Terminate != null) m_Terminate();}
	}
}
