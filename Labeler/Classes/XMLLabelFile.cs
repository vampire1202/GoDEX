// ********************************************************************************************************
// <copyright  file="XMLLabelFile.cs" company="MapWindow.org">
//     Copyright (c) MapWindow.org. All rights reserved.
// </copyright>
// <license>
//   The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
//   you may not use this file except in compliance with the License. You may obtain a copy of the License at 
//   www.mozilla.org/MPL/ 
//   Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
//   ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
//   limitations under the License. 
// </license>
// <author>
//   The Initial Developer of this version of the Original Code is MapWindow Team.
// </author>
// <changelog>
//   Contributor(s): (Open source contributors should list themselves and their modifications here). 
// Change Log: 
// Date            Changed By      Notes
// 10 August 2009  Paul Meems      Added some fixes for bug 1379 and 1380 and 
//                                 made changes recommended by StyleCop
// </changelog>
// ********************************************************************************************************

namespace Labeler.Classes
{
    using System;
    using System.Globalization;
    using System.Xml;

    /// <summary>
    /// Class to handle the label file
    /// </summary>
    public class XMLLabelFile
    {
#region member variables
        /// <summary>The reference to MapWindow</summary>
        private MapWindow.Interfaces.IMapWin mapWin;
        
        /// <summary>The version of MapWindow</summary>
        private string mapWinVersion;
        
        /// <summary>The reference to the label file</summary>
        private XmlDocument xmlDoc = new XmlDocument();
#endregion

        /// <summary>
        /// Initializes a new instance of the XMLLabelFile class
        /// </summary>
        /// <param name="mapwin">The reference with MapWindow</param>
        /// <param name="mapWinVersion">The version of MapWindow</param>
        public XMLLabelFile(MapWindow.Interfaces.IMapWin mapwin, string mapWinVersion)
        {
            this.mapWin = mapwin;
            this.mapWinVersion = mapWinVersion;
        }

        /// <summary>
        /// Save the label info
        /// </summary>
        /// <param name="labelsForm">The label form</param>
        /// <param name="fileName">The file name</param>
        public void SaveLabelInfo(ref Forms.Label labelsForm, string fileName)
        {
            try
            {
                // Paul Meems 10 August 2009 Added
                // Bug 1379: Using commas vs points

                // m_doc = labels.lbl_XMLFile;
                // find out what mapwindow version is
                this.xmlDoc.LoadXml("<MapWindow version= '" + this.mapWinVersion + "'></MapWindow>");
                System.Xml.XmlElement root = this.xmlDoc.DocumentElement;

                XmlElement labelsElement = this.xmlDoc.CreateElement("Labels");
                XmlAttribute field = this.xmlDoc.CreateAttribute("Field");
                XmlAttribute field2 = this.xmlDoc.CreateAttribute("Field2");
                XmlAttribute font = this.xmlDoc.CreateAttribute("Font");
                XmlAttribute size = this.xmlDoc.CreateAttribute("Size");
                
                // Paul Meems 6/11/2009
                // Bug #913: Label setup ignores font style 
                XmlAttribute bold = this.xmlDoc.CreateAttribute("Bold");
                XmlAttribute italic = this.xmlDoc.CreateAttribute("Italic");
                XmlAttribute underline = this.xmlDoc.CreateAttribute("Underline");

                XmlAttribute color = this.xmlDoc.CreateAttribute("Color");
                XmlAttribute justification = this.xmlDoc.CreateAttribute("Justification");
                XmlAttribute useMinZoomLevel = this.xmlDoc.CreateAttribute("UseMinZoomLevel");
                
                // The following six attributes will only be written to the .lbl file if the version of MapWindow
                // is new enough.
                XmlAttribute scaled = this.xmlDoc.CreateAttribute("Scaled");
                XmlAttribute useShadows = this.xmlDoc.CreateAttribute("UseShadows");
                XmlAttribute shadowColor = this.xmlDoc.CreateAttribute("ShadowColor");
                XmlAttribute offset = this.xmlDoc.CreateAttribute("Offset");
                XmlAttribute standardViewWidth = this.xmlDoc.CreateAttribute("StandardViewWidth");
                XmlAttribute useLabelCollision = this.xmlDoc.CreateAttribute("UseLabelCollision");
                XmlAttribute removeDuplicateLabels = this.xmlDoc.CreateAttribute("RemoveDuplicateLabels");
                XmlAttribute rotationField = this.xmlDoc.CreateAttribute("RotationField");
                XmlAttribute labelAllParts = this.xmlDoc.CreateAttribute("LabelAllParts");

                XmlAttribute scale = this.xmlDoc.CreateAttribute("Scale");

                XmlAttribute appendLine1 = this.xmlDoc.CreateAttribute("AppendLine1");
                XmlAttribute appendLine2 = this.xmlDoc.CreateAttribute("AppendLine2");
                XmlAttribute prependLine1 = this.xmlDoc.CreateAttribute("PrependLine1");
                XmlAttribute prependLine2 = this.xmlDoc.CreateAttribute("PrependLine2");

                // save the layer label properties
                field.InnerText = labelsForm.field.ToString();
                field2.InnerText = labelsForm.field2.ToString();
                font.InnerText = labelsForm.font.Name;
                size.InnerText = labelsForm.font.Size.ToString(CultureInfo.InvariantCulture);
                
                // Paul Meems 6/11/2009
                // Bug #913: Label setup ignores font style 
                bold.InnerText = labelsForm.font.Bold.ToString();
                italic.InnerText = labelsForm.font.Italic.ToString();
                underline.InnerText = labelsForm.font.Underline.ToString();

                color.InnerText = labelsForm.color.ToArgb().ToString();
                justification.InnerText = ((int)labelsForm.alignment).ToString();
                useMinZoomLevel.InnerText = labelsForm.UseMinExtents.ToString();
                rotationField.InnerText = labelsForm.RotationField.ToString();

                scaled.InnerText = labelsForm.Scaled.ToString();
                useShadows.InnerText = labelsForm.UseShadows.ToString();
                shadowColor.InnerText = labelsForm.shadowColor.ToArgb().ToString();
                offset.InnerText = labelsForm.Offset.ToString();
                standardViewWidth.InnerText = labelsForm.StandardViewWidth.ToString(CultureInfo.InvariantCulture);
                useLabelCollision.InnerText = labelsForm.UseLabelCollision.ToString();
                removeDuplicateLabels.InnerText = labelsForm.RemoveDuplicates.ToString();
                labelAllParts.InnerText = labelsForm.LabelAllParts.ToString();

                if (labelsForm.extents != null)
                {
                    // Paul Meems Added rounding:
                    scale.InnerText = Math.Round(labelsForm.scale, 3).ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    scale.InnerText = "0";
                }

                appendLine1.InnerText = labelsForm.AppendLine1;
                appendLine2.InnerText = labelsForm.AppendLine2;
                prependLine1.InnerText = labelsForm.PrependLine1;
                prependLine2.InnerText = labelsForm.PrependLine2;

                // add the attributes to the Labels node
                labelsElement.Attributes.Append(appendLine1);
                labelsElement.Attributes.Append(appendLine2);
                labelsElement.Attributes.Append(prependLine1);
                labelsElement.Attributes.Append(prependLine2);
                labelsElement.Attributes.Append(field);
                labelsElement.Attributes.Append(field2);
                labelsElement.Attributes.Append(font);
                labelsElement.Attributes.Append(size);
                labelsElement.Attributes.Append(labelAllParts);
                
                // Paul Meems 6/11/2009
                // Bug #913: Label setup ignores font style 
                labelsElement.Attributes.Append(bold);
                labelsElement.Attributes.Append(italic);
                labelsElement.Attributes.Append(underline);

                labelsElement.Attributes.Append(color);
                labelsElement.Attributes.Append(justification);
                labelsElement.Attributes.Append(useMinZoomLevel);

                labelsElement.Attributes.Append(scaled);
                labelsElement.Attributes.Append(useShadows);
                labelsElement.Attributes.Append(shadowColor);
                labelsElement.Attributes.Append(offset);
                labelsElement.Attributes.Append(standardViewWidth);
                labelsElement.Attributes.Append(useLabelCollision);
                labelsElement.Attributes.Append(removeDuplicateLabels);
                labelsElement.Attributes.Append(rotationField);

                labelsElement.Attributes.Append(scale);

                MapWinGIS.Shapefile shpFile;
                shpFile = (MapWinGIS.Shapefile)this.mapWin.Layers[labelsForm.handle].GetObject();

                string name;
                for (int i = 0; i < labelsForm.labelShape.Count; i++)
                {
                    if ((int)labelsForm.labelShape[i] >= 1)
                    {
                        name = labelsForm.PrependLine1 + shpFile.get_CellValue(labelsForm.field - 1, (int)labelsForm.labelShape[i]-1).ToString() + labelsForm.AppendLine1;
                        if (labelsForm.field2 != 0)
                        {
                            name += Environment.NewLine;
                            name += labelsForm.PrependLine2;
                            name += shpFile.get_CellValue(labelsForm.field2 - 1, (int)labelsForm.labelShape[i]-1).ToString();
                            name += labelsForm.AppendLine2;
                        }

                        this.AddPointLabel((Forms.Point)labelsForm.points[i], name, labelsElement, true);
                    }
                }

                // Add all of the labels to the root
                root.AppendChild(labelsElement);

                // Save the label file
                this.xmlDoc.Save(fileName);
                
                // Save the xml of the .lbl file in the label struct (label struct passed as 'ref')
                labelsForm.xml_LblFile = this.xmlDoc.InnerXml;
            }
            catch (System.Exception ex)
            {
                this.mapWin.ShowErrorDialog(ex);
            }
        }

        /// <summary>
        /// load the label info
        /// </summary>
        /// <param name="mapWin">The reference to MapWindow</param>
        /// <param name="layer">The layer object</param>
        /// <param name="label">The label form</param>
        /// <param name="owner">The form owner</param>
        /// <returns>True on success else false</returns>
        public bool LoadLabelInfo(MapWindow.Interfaces.IMapWin mapWin, MapWindow.Interfaces.Layer layer, ref Forms.Label label, System.Windows.Forms.Form owner)
        {
            if (layer == null)
            {
                return false;
            }

            // make sure the file exists
            string filename = string.Empty;
            if (this.mapWin.View.LabelsUseProjectLevel)
            {
                if (this.mapWin.Project.FileName != null && this.mapWin.Project.FileName.Trim() != string.Empty)
                {
                    filename = System.IO.Path.GetFileNameWithoutExtension(this.mapWin.Project.FileName) + @"\" + System.IO.Path.ChangeExtension(System.IO.Path.GetFileName(layer.FileName), ".lbl");
                }
            }

            if (filename == string.Empty || !System.IO.File.Exists(filename))
            {
                filename = System.IO.Path.ChangeExtension(layer.FileName, ".lbl");
            }

            if (!System.IO.File.Exists(filename))
            {
                return false;
            }

            try
            {
                // load the xml file
                this.xmlDoc.Load(filename);

                // get the root of the file
                System.Xml.XmlElement root = this.xmlDoc.DocumentElement;

                label.points = new System.Collections.ArrayList();
                label.labelShape = new System.Collections.ArrayList();

                XmlNodeList nodeList = root.GetElementsByTagName("Labels");

                // Get the font
                int field = int.Parse(nodeList[0].Attributes.GetNamedItem("Field").InnerText);
                int field2 = 0;
                if (nodeList[0].Attributes.GetNamedItem("Field2") != null)
                {
                    field2 = int.Parse(nodeList[0].Attributes.GetNamedItem("Field2").InnerText);
                }

                string fontName = nodeList[0].Attributes.GetNamedItem("Font").InnerText;
                float size = float.Parse(nodeList[0].Attributes.GetNamedItem("Size").InnerText, CultureInfo.InvariantCulture);
                
                // Paul Meems 6/11/2009
                // Bug #913: Label setup ignores font style 
                bool bold = false, italic = false, underline = false;
                try
                {
                    if (nodeList[0].Attributes.GetNamedItem("Bold") != null)
                    {
                        bold = bool.Parse(nodeList[0].Attributes.GetNamedItem("Bold").InnerText);
                    }

                    if (nodeList[0].Attributes.GetNamedItem("Italic") != null)
                    {
                        italic = bool.Parse(nodeList[0].Attributes.GetNamedItem("Italic").InnerText);
                    }

                    if (nodeList[0].Attributes.GetNamedItem("Underline") != null)
                    {
                        underline = bool.Parse(nodeList[0].Attributes.GetNamedItem("Underline").InnerText);
                    }
                }
                catch
                {
                    // Do nothing. Older version of label file.
                }

                System.Drawing.Color color = System.Drawing.Color.FromArgb(int.Parse(nodeList[0].Attributes.GetNamedItem("Color").InnerText));
                int justification = int.Parse(nodeList[0].Attributes.GetNamedItem("Justification").InnerText);
                bool useMinZoom = bool.Parse(nodeList[0].Attributes.GetNamedItem("UseMinZoomLevel").InnerText);
                bool scaled = false;
                bool useShadows = false;
                System.Drawing.Color shadowColor = System.Drawing.Color.White;
                int offset = 0;
                double standardViewWidth = 0.0;
                bool useLabelCollision = false;
                bool removeDuplicateLabels = false;
                string rotationField = string.Empty;
                bool labelAllParts = false;

                try
                {
                    if (nodeList[0].Attributes.GetNamedItem("Scaled") != null)
                    {
                        scaled = bool.Parse(nodeList[0].Attributes.GetNamedItem("Scaled").InnerText);
                    }

                    if (nodeList[0].Attributes.GetNamedItem("UseShadows") != null)
                    {
                        useShadows = bool.Parse(nodeList[0].Attributes.GetNamedItem("UseShadows").InnerText);
                    }

                    if (nodeList[0].Attributes.GetNamedItem("ShadowColor") != null)
                    {
                        shadowColor = System.Drawing.Color.FromArgb(int.Parse(nodeList[0].Attributes.GetNamedItem("ShadowColor").InnerText));
                    }

                    if (nodeList[0].Attributes.GetNamedItem("Offset") != null)
                    {
                        offset = int.Parse(nodeList[0].Attributes.GetNamedItem("Offset").InnerText);
                    }

                    if (nodeList[0].Attributes.GetNamedItem("StandardViewWidth") != null)
                    {
                        standardViewWidth = double.Parse(nodeList[0].Attributes.GetNamedItem("StandardViewWidth").InnerText, CultureInfo.InvariantCulture);
                    }

                    if (nodeList[0].Attributes.GetNamedItem("LabelAllParts") != null)
                    {
                        labelAllParts = bool.Parse(nodeList[0].Attributes.GetNamedItem("LabelAllParts").InnerText);
                    }
                }
                catch
                {
                    scaled = false;
                    useShadows = false;
                    shadowColor = System.Drawing.Color.White;
                    offset = 0;
                    standardViewWidth = 0.0;
                    labelAllParts = false;
                }

                if (nodeList[0].Attributes.GetNamedItem("UseLabelCollision") != null)
                {
                    useLabelCollision = bool.Parse(nodeList[0].Attributes.GetNamedItem("UseLabelCollision").InnerText);
                }

                if (nodeList[0].Attributes.GetNamedItem("RemoveDuplicateLabels") != null)
                {
                    removeDuplicateLabels = bool.Parse(nodeList[0].Attributes.GetNamedItem("RemoveDuplicateLabels").InnerText);
                }

                if (nodeList[0].Attributes.GetNamedItem("RotationField") != null)
                {
                    rotationField = nodeList[0].Attributes.GetNamedItem("RotationField").InnerText;
                }

                double xMin = 0;
                double yMin = 0;
                double xMax = 0;
                double yMax = 0;

                if (nodeList[0].Attributes["Scale"] != null)
                {
                    // Scale
                    MapWinGIS.Extents exts = new MapWinGIS.Extents();
                    exts.SetBounds(0, 0, 0, 0, 0, 0);
                    try
                    {
                        exts = this.ScaleToExtents(double.Parse(nodeList[0].Attributes["Scale"].InnerText, CultureInfo.InvariantCulture), this.mapWin.View.Extents);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.ToString());
                    }

                    xMin = exts.xMin;
                    xMax = exts.xMax;
                    yMin = exts.yMin;
                    yMax = exts.yMax;
                }
                else
                {
                    // Extents
                    xMin = double.Parse(nodeList[0].Attributes.GetNamedItem("xMin").InnerText.Replace(",", "."), CultureInfo.InvariantCulture);
                    yMin = double.Parse(nodeList[0].Attributes.GetNamedItem("yMin").InnerText.Replace(",", "."), CultureInfo.InvariantCulture);
                    xMax = double.Parse(nodeList[0].Attributes.GetNamedItem("xMax").InnerText.Replace(",", "."), CultureInfo.InvariantCulture);
                    yMax = double.Parse(nodeList[0].Attributes.GetNamedItem("yMax").InnerText.Replace(",", "."), CultureInfo.InvariantCulture);
                }

                // Paul Meems 6/11/2009
                // Bug #913: Label setup ignores font style 
                System.Drawing.FontStyle fstyle = new System.Drawing.FontStyle();
                if (bold)
                {
                    fstyle |= System.Drawing.FontStyle.Bold;
                }

                if (italic)
                {
                    fstyle |= System.Drawing.FontStyle.Italic;
                }

                if (underline)
                {
                    fstyle |= System.Drawing.FontStyle.Underline;
                }

                // Set all the properties of the label                
                label.font = new System.Drawing.Font(fontName, size, fstyle);

                label.color = color;
                label.field = field;
                label.field2 = field2;
                label.handle = layer.Handle;
                label.alignment = (MapWinGIS.tkHJustification)justification;
                label.UseMinExtents = useMinZoom;

                label.Scaled = scaled;
                label.UseShadows = useShadows;
                label.shadowColor = shadowColor;
                label.Offset = offset;
                label.StandardViewWidth = standardViewWidth;
                label.RotationField = rotationField;
                label.LabelAllParts = labelAllParts;

                if (nodeList[0].Attributes.GetNamedItem("UseLabelCollision") != null)
                {
                    label.UseLabelCollision = useLabelCollision;
                }

                if (nodeList[0].Attributes.GetNamedItem("RemoveDuplicateLabels") != null)
                {
                    label.RemoveDuplicates = removeDuplicateLabels;
                }

                if (nodeList[0].Attributes.GetNamedItem("AppendLine1") != null)
                {
                    label.AppendLine1 = nodeList[0].Attributes.GetNamedItem("AppendLine1").InnerText;
                }

                if (nodeList[0].Attributes.GetNamedItem("AppendLine2") != null)
                {
                    label.AppendLine2 = nodeList[0].Attributes.GetNamedItem("AppendLine2").InnerText;
                }

                if (nodeList[0].Attributes.GetNamedItem("PrependLine1") != null)
                {
                    label.PrependLine1 = nodeList[0].Attributes.GetNamedItem("PrependLine1").InnerText;
                }

                if (nodeList[0].Attributes.GetNamedItem("PrependLine2") != null)
                {
                    label.PrependLine2 = nodeList[0].Attributes.GetNamedItem("PrependLine2").InnerText;
                }

                label.extents = new MapWinGIS.ExtentsClass();
                label.extents.SetBounds(xMin, yMin, 0, xMax, yMax, 0);
                label.scale = this.ExtentsToScale(label.extents);
                label.Modified = false;
                label.LabelExtentsChanged = false;
                label.updateHeaderOnly = true;

                // Add all the points to this label
                Forms.Point p;
                XmlNode node;
                double x, y, rotation = 0;
                System.Collections.IEnumerator enumerator = nodeList[0].ChildNodes.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    node = (XmlNode)enumerator.Current;
                    x = double.Parse(node.Attributes.GetNamedItem("X").InnerText, CultureInfo.InvariantCulture);
                    y = double.Parse(node.Attributes.GetNamedItem("Y").InnerText, CultureInfo.InvariantCulture);

                    if (nodeList[0].Attributes.GetNamedItem("Rotation") != null)
                    {
                        rotation = double.Parse(node.Attributes.GetNamedItem("Rotation").InnerText, CultureInfo.InvariantCulture);
                    }

                    p = new Forms.Point();
                    p.x = x;
                    p.y = y;
                    p.rotation = rotation;

                    label.points.Add(p);
                }

                label.xml_LblFile = this.xmlDoc.InnerXml;
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Replace the label header
        /// </summary>
        /// <param name="labels">The label form</param>
        /// <param name="fileName">The file name</param>
        public void ReplaceHeader(ref Forms.Label labels, string fileName)
        {
            this.xmlDoc.LoadXml(labels.xml_LblFile);

            XmlElement root = this.xmlDoc.DocumentElement;

            XmlNode node = root.FirstChild.Attributes.GetNamedItem("Field");
            node.InnerText = labels.field.ToString();
            node = root.FirstChild.Attributes.GetNamedItem("Font");
            node.InnerText = labels.font.Name;
            node = root.FirstChild.Attributes.GetNamedItem("Size");
            node.InnerText = labels.font.Size.ToString();

            // Paul Meems 6/11/2009
            // Bug #913: Label setup ignores font style 
            try
            {
                if ((node = root.FirstChild.Attributes.GetNamedItem("Bold")) != null)
                {
                    node.InnerText = labels.font.Bold.ToString(); 
                }

                if ((node = root.FirstChild.Attributes.GetNamedItem("Italic")) != null)
                {
                    node.InnerText = labels.font.Italic.ToString();
                }

                if ((node = root.FirstChild.Attributes.GetNamedItem("Underline")) != null)
                {
                    node.InnerText = labels.font.Underline.ToString();
                }
            }
            catch
            {
                // Do nothing, older version of label file.
            }

            node = root.FirstChild.Attributes.GetNamedItem("Color");
            node.InnerText = labels.color.ToArgb().ToString();
            node = root.FirstChild.Attributes.GetNamedItem("Justification");
            node.InnerText = ((int)labels.alignment).ToString();
            node = root.FirstChild.Attributes.GetNamedItem("UseMinZoomLevel");
            node.InnerText = labels.UseMinExtents.ToString();

            this.AddXMLNodeInnerText(root, "Scaled", labels.Scaled.ToString());
            this.AddXMLNodeInnerText(root, "UseShadows", labels.UseShadows.ToString());
            this.AddXMLNodeInnerText(root, "ShadowColor", labels.shadowColor.ToArgb().ToString());
            this.AddXMLNodeInnerText(root, "Offset", labels.Offset.ToString());
            this.AddXMLNodeInnerText(root, "StandardViewWidth", labels.StandardViewWidth.ToString());
            this.AddXMLNodeInnerText(root, "UseLabelCollision", labels.UseLabelCollision.ToString());
            this.AddXMLNodeInnerText(root, "RemoveDuplicateLabels", labels.RemoveDuplicates.ToString());
            this.AddXMLNodeInnerText(root, "RotationField", labels.RotationField);

            if (labels.extents != null)
            {
                if (root.FirstChild.Attributes.GetNamedItem("Scale") != null)
                {
                    node = root.FirstChild.Attributes.GetNamedItem("Scale");
                    node.InnerText = labels.scale.ToString();
                }
                else
                {
                    node = root.FirstChild.Attributes.GetNamedItem("xMin");
                    node.InnerText = labels.extents.xMin.ToString();
                    node = root.FirstChild.Attributes.GetNamedItem("yMin");
                    node.InnerText = labels.extents.yMin.ToString();
                    node = root.FirstChild.Attributes.GetNamedItem("xMax");
                    node.InnerText = labels.extents.xMax.ToString();
                    node = root.FirstChild.Attributes.GetNamedItem("yMax");
                    node.InnerText = labels.extents.yMax.ToString();
                }
            }

            // Save .lbl file
            this.xmlDoc.Save(fileName);

            // Save the xml of the .lbl file in the label struct (label struct passed as 'ref')
            labels.xml_LblFile = this.xmlDoc.InnerXml;
        }

        /// <summary>
        /// Add points to the label file
        /// </summary>
        /// <param name="p">The point parameter</param>
        /// <param name="labelName">The label name parameter</param>
        /// <param name="parent">The parent XML element parameter</param>
        /// <param name="newVersion">Use new version settings parameter</param>
        private void AddPointLabel(Forms.Point p, string labelName, XmlElement parent, bool newVersion)
        {
            try
            {
                XmlElement label = this.xmlDoc.CreateElement("Label");
                XmlAttribute x = this.xmlDoc.CreateAttribute("X");
                XmlAttribute y = this.xmlDoc.CreateAttribute("Y");
                XmlAttribute rotation = this.xmlDoc.CreateAttribute("Rotation");
                XmlAttribute name = this.xmlDoc.CreateAttribute("Name");

                // Paul Meems 10 August 2009 Added
                // Bug 1379: Using commas vs points
                x.InnerText = p.x.ToString(CultureInfo.InvariantCulture);
                y.InnerText = p.y.ToString(CultureInfo.InvariantCulture);
                rotation.InnerText = p.rotation.ToString(CultureInfo.InvariantCulture);
                name.InnerText = labelName;

                label.Attributes.Append(x);
                label.Attributes.Append(y);
                if (newVersion == true)
                {
                    label.Attributes.Append(rotation);
                }

                label.Attributes.Append(name);

                parent.AppendChild(label);
            }
            catch (System.Exception ex)
            {
                this.mapWin.ShowErrorDialog(ex);
            }
        }

        /// <summary>
        /// Add inner text of XML node
        /// </summary>
        /// <param name="root">Root elememt</param>
        /// <param name="localName">Name of the node</param>
        /// <param name="new_value">Value of the node</param>
        private void AddXMLNodeInnerText(XmlElement root, string localName, string new_value)
        {
            XmlNode node = root.FirstChild.Attributes.GetNamedItem(localName);
            if (node == null)
            {
                System.Xml.XmlAttribute newAttribute = this.xmlDoc.CreateAttribute(localName);
                newAttribute.InnerText = new_value;
                root.FirstChild.Attributes.Append(newAttribute);
            }
            else
            {
                node.InnerText = new_value;
            }
        }

        /// <summary>
        /// Convert the extents to a scale
        /// </summary>
        /// <param name="ext">The extents parameter</param>
        /// <returns>The scale parameter</returns>
        private double ExtentsToScale(MapWinGIS.Extents ext)
        {
            AxMapWinGIS.AxMap mapMain = (AxMapWinGIS.AxMap)this.mapWin.GetOCX;
            return MapWinGeoProc.ScaleTools.CalcScale(ext, this.mapWin.Project.MapUnits, mapMain.Width, mapMain.Height);
        }

        /// <summary>
        /// Convert the scale to an extents 
        /// </summary>
        /// <param name="scale">The scale parameter</param>
        /// <param name="ext">The extents parameter</param>
        /// <returns>The new extents</returns>
        private MapWinGIS.Extents ScaleToExtents(double scale, MapWinGIS.Extents ext)
        {
            AxMapWinGIS.AxMap mapMain = (AxMapWinGIS.AxMap)this.mapWin.GetOCX;
            MapWinGIS.Point pt = new MapWinGIS.Point();            
            pt.x = (ext.xMin + ext.xMax) / 2;
            pt.y = (ext.yMin + ext.yMax) / 2;
            

            // Paul Meems, 14 October 2009
            // Possible fix for relabeling error:
            MapWinGIS.Extents extScaled = null;
            if (scale != 0.0)
            {
                extScaled =  MapWinGeoProc.ScaleTools.ExtentFromScale(Convert.ToInt32(scale), pt, this.mapWin.Project.MapUnits, mapMain.Width, mapMain.Height);
            }
            if (extScaled == null)
            {
                extScaled = new MapWinGIS.Extents();
                extScaled.SetBounds(pt.x, pt.y, 0, pt.x, pt.y, 0);
            }
            return extScaled;
        }
    }
}
