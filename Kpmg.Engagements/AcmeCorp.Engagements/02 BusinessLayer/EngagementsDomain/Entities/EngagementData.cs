// -----------------------------------------------------------------------
// <copyright file="EngagementData.cs" company="AcmeCorp">
// AcmeCorp
// </copyright>
// -----------------------------------------------------------------------
namespace AcmeCorp.Engagements.EngagementsDomain.Entities
{
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    /// <summary>
    /// Engagement Data class
    /// </summary>
    public class EngagementData
    {

        /// <summary>
        /// Gets or sets the SAP table name
        /// </summary>
        /// <value>
        /// The SAP table name.
        /// </value>
        public string tableName { get; set; }


        /// <summary>
        /// Gets or sets the term.
        /// </summary>
        /// <value>
        /// The term.
        /// </value>
        public string term { get; set; }

        /// <summary>
        /// Gets or sets the SAP language.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        public string language { get; set; }


        /// <summary>
        /// Gets or sets the engagement id.
        /// </summary>
        /// <value>
        /// The engagement id.
        /// </value>
        public long EngagementId { get; set; }

        /// <summary>
        /// Gets or sets the type of the engagement folders.
        /// </summary>
        /// <value>
        /// The type of the engagement folders.
        /// </value>
        public int? EngagementFoldersType { get; set; }

        /// <summary>
        /// Gets or sets the managers.
        /// </summary>
        /// <value>
        /// The managers.
        /// </value>
        public List<string> Managers { get; set; }

        /// <summary>
        /// Gets or sets the partners.
        /// </summary>
        /// <value>
        /// The partners.
        /// </value>
        public List<string> Partners { get; set; }

        /// <summary>
        /// Gets or sets the staff.
        /// </summary>
        /// <value>
        /// The staff.
        /// </value>
        public List<string> Staff { get; set; }

        /// <summary>
        /// Gets or sets the engagement properties.
        /// </summary>
        /// <value>
        /// The engagement properties.
        /// </value>
        public Dictionary<string, object> EngagementProperties { get; set; }
    }
}
