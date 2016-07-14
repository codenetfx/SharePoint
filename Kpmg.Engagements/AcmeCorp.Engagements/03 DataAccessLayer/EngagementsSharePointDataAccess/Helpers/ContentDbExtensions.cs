// -----------------------------------------------------------------------
// <copyright file="ContentDbExtensions.cs" company="Acme.">
// TODO: Acme.
// </copyright>
// -----------------------------------------------------------------------

namespace AcmeCorp.Engagements.EngagementsSharePointDataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.SharePoint.Administration;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class ContentDbExtensions
    {
        /// <summary>
        /// Gets the database index as string for context database (reads it from the property bag)
        /// </summary>
        /// <param name="x">SharePoint content database</param>
        /// <returns>Database index as string</returns>
        public static string GetDbIndexAsString(this SPContentDatabase x)
        {
            string index = "000";
            if (x.Properties.ContainsKey("StringIndexCounter"))
            {
                index = x.Properties["StringIndexCounter"].ToString();
            }

            return index;
        }

        /// <summary>
        /// Gets the database index as integer for context database (reads it from the property bag)
        /// </summary>
        /// <param name="x">SharePoint content database</param>
        /// <returns>Database index as integer</returns>
        public static int GetDbIndexAsInt(this SPContentDatabase x)
        {
            int index = 0;
            if (x.Properties.ContainsKey("IntIndexCounter"))
            {
                index = (int)x.Properties["IntIndexCounter"];
            }

            return index;
        }
    }
}
