using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections;
using System.Runtime.Serialization;

namespace Tiveria.Common.Controls.Annotations
{
    public class ShapeCollection : Collection<IShape>, ICollection, ICloneable
    {
        #region Events
        public event EventHandler<ShapeChangedEventArgs> ItemInserted;
        public event EventHandler<ShapeChangedEventArgs> ItemRemoved;

        protected virtual void OnItemInserted(object sender, ShapeChangedEventArgs e)
        {
            EventHandler<ShapeChangedEventArgs> handler = ItemInserted;
            if (handler != null)
                handler(sender, e);
        }
        protected virtual void OnItemRemoved(object sender, ShapeChangedEventArgs e)
        {
            EventHandler<ShapeChangedEventArgs> handler = ItemRemoved;
            if (handler != null)
                handler(sender, e);
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ShapeCollection()
        {
        }

        #endregion

        #region ICloneable Interface

        /// <summary>
        /// Clones the collection and its content.
        /// </summary>
        /// <returns>Cloned object.</returns>
        virtual public object Clone()
        {
            ShapeCollection clonedCollection = new ShapeCollection();
            foreach (IShape shape in this)
                clonedCollection.Add(shape.Clone() as IShape);

            return clonedCollection;
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Add a ShapeCollection in the current collection.
        /// </summary>
        /// <param name="shapes">ShapeCollection to add.</param>
        public void AddRange(ShapeCollection shapes)
        {
            foreach (IShape shape in shapes)
                Add(shape);
        }

        /// <summary>
        /// Brings to front the shape.
        /// </summary>
        /// <param name="shape">Shape to move.</param>
        public void BringToFront(IShape shape)
        {
            if (Remove(shape))
                Add(shape);
        }

        /// <summary>
        /// Sends to back the shape.
        /// </summary>
        /// <param name="shape">Shape to move.</param>
        public void SendToBack(IShape shape)
        {
            if (Remove(shape))
                Insert(0, shape);
        }

        /// <summary>
        /// Transforms all internal shapes into an array of objects.
        /// </summary>
        /// <param name="shapes">Collection to transform.</param>
        /// <returns>Array of object.</returns>
        public static object[] ToObjects(ShapeCollection shapes)
        {
            if (shapes.Count == 0)
                return null;

            object[] objectShapes = new object[shapes.Count];
            for (int i = 0; i < shapes.Count; i++)
                objectShapes[i] = shapes[i];

            return objectShapes;
        }

        #endregion

        #region Protected Functions

        /// <summary>
        /// Called when a shape is inserted.
        /// </summary>
        /// <param name="index">Inserting index.</param>
        /// <param name="item">Shape inserted.</param>
        protected override void InsertItem(int index, IShape item)
        {
            //if (item.Parent != null)
            //    throw new ApplicationException("Must first removes item from Shape Container!");

            base.InsertItem(index, item);

            OnItemInserted(this, new ShapeChangedEventArgs(item, index));
        }

        /// <summary>
        /// Called when a shape is removed.
        /// </summary>
        /// <param name="index">Removing shape.</param>
        protected override void RemoveItem(int index)
        {
            IShape item = this[index];

            base.RemoveItem(index);

            OnItemRemoved(this, new ShapeChangedEventArgs(item, index));
        }
        #endregion
    }

    public class ShapeChangedEventArgs : EventArgs
    {
        public IShape Shape { get; private set; }
        public int Index { get; private set; }
        public ShapeChangedEventArgs(IShape shape, int index)
        {
            Shape = shape;
            Index = index;
        }
    }
}
