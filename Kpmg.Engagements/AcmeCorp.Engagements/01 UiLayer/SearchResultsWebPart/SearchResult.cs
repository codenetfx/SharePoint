// -----------------------------------------------------------------------
// <copyright file="SearchResult.cs" company="AcmeCorp">
// AcmeCorp
// </copyright>
// -----------------------------------------------------------------------
namespace SearchResultsWebPart.MyEngagementsWebPart
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// Search result entity
    /// </summary>
    public class SearchResult
    {
        /// <summary>
        /// Gets or sets the site title.
        /// </summary>
        /// <value>
        /// The site title.
        /// </value>
        public string SiteTitle { get; set; }

        /// <summary>
        /// Gets or sets the site URL.
        /// </summary>
        /// <value>
        /// The site URL.
        /// </value>
        public string SiteUrl { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the status date.
        /// </summary>
        /// <value>
        /// The status date.
        /// </value>
        public DateTime? StatusDate { get; set; }

        /// <summary>
        /// Gets or sets the name mandaten.
        /// </summary>
        /// <value>
        /// The name mandaten.
        /// </value>
        public string NameMandaten { get; set; }

        /// <summary>
        /// Gets or sets the opportunity nr.
        /// </summary>
        /// <value>
        /// The opportunity nr.
        /// </value>
        public int? OpportunityNr { get; set; }

        /// <summary>
        /// Gets or sets the account.
        /// </summary>
        /// <value>
        /// The account.
        /// </value>
        public string Account { get; set; }

        /// <summary>
        /// Gets or sets the concurring partner.
        /// </summary>
        /// <value>
        /// The concurring partner.
        /// </value>
        public string ConcurringPartner { get; set; }

        /// <summary>
        /// Gets or sets the niederlassung.
        /// </summary>
        /// <value>
        /// The niederlassung.
        /// </value>
        public string Niederlassung { get; set; }

        /// <summary>
        /// Gets or sets the bezeichnung.
        /// </summary>
        /// <value>
        /// The bezeichnung.
        /// </value>
        public string Bezeichnung { get; set; }

        /// <summary>
        /// Gets or sets the wb auftrags nr.
        /// </summary>
        /// <value>
        /// The wb auftrags nr.
        /// </value>
        public int? WbAuftragsNr { get; set; }

        /// <summary>
        /// Gets or sets the engagement partner.
        /// </summary>
        /// <value>
        /// The engagement partner.
        /// </value>
        public string EngPartner { get; set; }

        /// <summary>
        /// Gets or sets the engagement manager.
        /// </summary>
        /// <value>
        /// The engagement manager.
        /// </value>
        public string EngManager { get; set; }

        /// <summary>
        /// Gets or sets the wb auftrags status.
        /// </summary>
        /// <value>
        /// The wb auftrags status.
        /// </value>
        public string WbAuftragsStatus { get; set; }

        /// <summary>
        /// Gets or sets the wb auftrags status datum.
        /// </summary>
        /// <value>
        /// The wb auftrags status datum.
        /// </value>
        public DateTime? WbAuftragsStatusDatum { get; set; }
    }
}