using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Shared
{
    /// <summary>
    /// List node generic.
    /// </summary>
    [RequireComponent(typeof(LayoutElement))]
    public class ListNodeGeneric<M> : MonoBehaviour
    {
        /// <summary>
        /// The data.
        /// </summary>
        M data;

        #region Properties

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        virtual public M Data
        {
            get
            {
                return data;
            }

            set
            {
                data = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:ListNodeGeneric`1"/> is active.
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        public bool Active
        {
            get
            {
                return gameObject.activeSelf;
            }

            set
            {
                gameObject.SetActive(value);
            }
        }
        #endregion Properties
    }
}