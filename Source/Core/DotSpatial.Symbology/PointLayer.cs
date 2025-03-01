// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using System.ComponentModel;
using DotSpatial.Data;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// A layer with drawing characteristics for points.
    /// </summary>
    public class PointLayer : FeatureLayer, IPointLayer
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PointLayer"/> class with an empty dataset configured to the point feature type.
        /// </summary>
        public PointLayer()
            : this(new FeatureSet(FeatureType.Point), null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointLayer"/> class.
        /// </summary>
        /// <param name="inFeatureSet">The IFeatureLayer of data values to turn into a graphical PointLayer.</param>
        /// <exception cref="PointFeatureTypeException">Thrown if the featureSet FeatureType is not point or multi-point.</exception>
        public PointLayer(IFeatureSet inFeatureSet)
            : this(inFeatureSet, null)
        {
            // this simply handles the default case where no status messages are requested
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointLayer"/> class.
        /// </summary>
        /// <param name="inFeatureSet">Any implementation of an IFeatureLayer.</param>
        /// <param name="progressHandler">A valid implementation of the IProgressHandler interface.</param>
        /// <exception cref="PointFeatureTypeException">Thrown if the featureSet FeatureType is not point or multi-point.</exception>
        public PointLayer(IFeatureSet inFeatureSet, IProgressHandler progressHandler)
            : base(inFeatureSet, null, progressHandler)
        {
            Configure(inFeatureSet);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointLayer"/> class.
        /// </summary>
        /// <param name="inFeatureSet">Any implementation of an IFeatureLayer.</param>
        /// <param name="container">An IContainer to contain this layer.</param>
        /// <param name="progressHandler">A valid implementation of the IProgressHandler interface.</param>
        /// <exception cref="PointFeatureTypeException">Thrown if the featureSet FeatureType is
        ///  not point or multi-point.</exception>
        public PointLayer(IFeatureSet inFeatureSet, ICollection<ILayer> container, IProgressHandler progressHandler)
            : base(inFeatureSet, container, progressHandler)
        {
            Configure(inFeatureSet);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the pointSymbolizer characteristics to use for the selected features.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets the symbolic characteristics to use for the selected features.")]
        [ShallowCopy]
        public new IPointSymbolizer SelectionSymbolizer
        {
            get
            {
                return base.SelectionSymbolizer as IPointSymbolizer;
            }

            set
            {
                base.SelectionSymbolizer = value;
            }
        }

        /// <summary>
        /// Gets or sets the symbolic characteristics for this layer.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets the symbolic characteristics for this layer.")]
        [ShallowCopy]
        public new IPointSymbolizer Symbolizer
        {
            get
            {
                return base.Symbolizer as IPointSymbolizer;
            }

            set
            {
                base.Symbolizer = value;
            }
        }

        /// <summary>
        /// Gets or sets the currently applied scheme. Because setting the scheme requires a processor intensive
        /// method, we use the ApplyScheme method for assigning a new scheme. This allows access
        /// to editing the members of an existing scheme directly, however.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets the currently applied scheme.")]
        [Serialize("Symbology")]
        public new IPointScheme Symbology
        {
            get
            {
                return base.Symbology as IPointScheme;
            }

            set
            {
                base.Symbology = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Attempts to create a new PointLayer using the specified file. If the filetype is not
        /// does not generate a point layer, an exception will be thrown.
        /// </summary>
        /// <param name="fileName">A string fileName to create a point layer for.</param>
        /// <param name="progressHandler">Any valid implementation of IProgressHandler for receiving progress messages.</param>
        /// <returns>A PointLayer created from the specified fileName.</returns>
        public static new IPointLayer OpenFile(string fileName, IProgressHandler progressHandler)
        {
            ILayer fl = LayerManager.DefaultLayerManager.OpenLayer(fileName, progressHandler);
            return fl as PointLayer;
        }

        /// <summary>
        /// Attempts to create a new PointLayer using the specified file. If the filetype is not
        /// does not generate a point layer, an exception will be thrown.
        /// </summary>
        /// <param name="fileName">A string fileName to create a point layer for.</param>
        /// <returns>A PointLayer created from the specified fileName.</returns>
        public static new IPointLayer OpenFile(string fileName)
        {
            IFeatureLayer fl = LayerManager.DefaultLayerManager.OpenVectorLayer(fileName);
            return fl as PointLayer;
        }

        private void Configure(IFeatureSet inFeatureSet)
        {
            FeatureType ft = inFeatureSet.FeatureType;
            if (ft != FeatureType.Point && ft != FeatureType.MultiPoint && ft != FeatureType.Unspecified)
            {
                throw new PointFeatureTypeException();
            }

            if (inFeatureSet.NumRows() == 0)
            {
                MyExtent = new Extent();
            }
            else if (inFeatureSet.NumRows() == 1)
            {
                MyExtent = inFeatureSet.Extent.Copy();
                MyExtent.ExpandBy(10, 10);
            }
            else
            {
                MyExtent = inFeatureSet.Extent.Copy();
            }

            Symbology = new PointScheme();
        }

        #endregion
    }
}