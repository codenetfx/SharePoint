// -----------------------------------------------------------------------
// <copyright file="CustomProperty.cs" company="AcmeCorp">
// AcmeCorp
// </copyright>
// -----------------------------------------------------------------------
namespace AcmeCorp.Engagements.ProjectDataWebPart
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Custom Property class
    /// </summary>
    public class CustomProperty
    {
        /// <summary>
        /// Gets or sets the property key.
        /// </summary>
        /// <value>
        /// The property key.
        /// </value>
        public string PropertyKey { get; set; }

        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        /// <value>
        /// The property value.
        /// </value>
        public object PropertyValue { get; set; }
    }
}
