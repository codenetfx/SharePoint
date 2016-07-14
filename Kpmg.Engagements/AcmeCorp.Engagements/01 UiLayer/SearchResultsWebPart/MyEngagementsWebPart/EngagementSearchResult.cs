// -----------------------------------------------------------------------
// <copyright file="EngagementSearchResult.cs" company="AcmeCorp">
// AcmeCorp
// </copyright>
// -----------------------------------------------------------------------
namespace SearchResultsWebPart.MyEngagementsWebPart
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Engagement Search result definition
    /// </summary>
    public class EngagementSearchResult
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the Engagement Status.
        /// </summary>
        /// <value>
        /// Engagement Status.
        /// </value>
        public string KEstatus { get; set; }

        /// <summary>
        /// Gets or sets the Engagement Status Date.
        /// </summary>
        /// <value>
        /// The Engagement Status Date.
        /// </value>
        public DateTime? KEstatusdate { get; set; }

        /// <summary>
        /// Gets or sets the Engagement name des mandanten.
        /// </summary>
        /// <value>
        /// The Engagement name des mandanten
        /// </value>
        public string KENamedesMandanten { get; set; }

        /// <summary>
        /// Gets or sets the Engagement Opportunity nr.
        /// </summary>
        /// <value>
        /// The Engagement Opportunity nr.
        /// </value>
        public int? KEOpportunityNr { get; set; }

        /// <summary>
        /// Gets or sets the Engagement Account.
        /// </summary>
        /// <value>
        /// The Engagement Account.
        /// </value>
        public string KEAccount { get; set; }

        /// <summary>
        /// Gets or sets the Concurring partner for the engagement.
        /// </summary>
        /// <value>
        /// The Concurring partner for the engagement.
        /// </value>
        public string KEConcurringPartner { get; set; }

        /// <summary>
        /// Gets or sets the KE niederlassung.
        /// </summary>
        /// <value>
        /// The KE niederlassung.
        /// </value>
        public string KENiederlassung { get; set; }

        /// <summary>
        /// Gets or sets the KE bezeichnung.
        /// </summary>
        /// <value>
        /// The KE bezeichnung.
        /// </value>
        public string KEBezeichnung { get; set; }

        /// <summary>
        /// Gets or sets the KEWB auftrags nr.
        /// </summary>
        /// <value>
        /// The KEWB auftrags nr.
        /// </value>
        public int? KEWBAuftragsNr { get; set; }

        /// <summary>
        /// Gets or sets the Engagement engineering partner.
        /// </summary>
        /// <value>
        /// The Engagement engineering partner.
        /// </value>
        public string KEEngPartner { get; set; }

        /// <summary>
        /// Gets or sets the Engagement engineering manager.
        /// </summary>
        /// <value>
        /// The Engagement engineering manager.
        /// </value>
        public string KEEngManager { get; set; }

        /// <summary>
        /// Gets or sets the KEWB auftrag status.
        /// </summary>
        /// <value>
        /// The KEWB auftrag status.
        /// </value>
        public string KEWBAuftragStatus { get; set; }

        /// <summary>
        /// Gets or sets the KEWB auftrag status datum.
        /// </summary>
        /// <value>
        /// The KEWB auftrag status datum.
        /// </value>
        public DateTime? KEWBAuftragStatusDatum { get; set; }
    }
}
