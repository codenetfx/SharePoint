// -----------------------------------------------------------------------
// <copyright file="EngagementProperty.cs" company="AcmeCorp">
// AcmeCorp
// </copyright>
// -----------------------------------------------------------------------
namespace SearchResultsWebPart
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Engagement Property definitions
    /// </summary>
    public class EngagementProperty
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
