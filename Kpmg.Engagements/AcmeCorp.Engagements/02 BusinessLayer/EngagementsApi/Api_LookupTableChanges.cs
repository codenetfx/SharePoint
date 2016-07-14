// -----------------------------------------------------------------------
// <copyright file="Api_Engagements.cs" company="AcmeCorp">
// AcmeCorp
// </copyright>
// -----------------------------------------------------------------------
namespace AcmeCorp.Engagements.EngagementsApi
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AcmeCorp.Engagements.EngagementsDomain;
    
    

    /// <summary>
    /// Initializes engagements API
    /// </summary>
    public partial class Api
    {
        //TODO - read from configuration
        protected string SPSiteURL = String.Empty;

        public void ProcessTermsSync(string tablename, string language)
        {
            this.dataLayer.ProcessTermsSync(tablename, language);
        }

        public void ProcessTermsAsync(string tablename, string language)
        {
            this.dataLayer.ProcessTermsAsync(tablename, language);
        }


        CustomTableQueryResponseRows[] GetDataFromLookupTable(string tablename, string language)
        {
            //TODO
            CustomTableQueryResponseRows[] result = null;
            this.dataLayer.GetDataFromLookupTable(tablename, language);

            return (result);
        }

        public void AddTerm(string tablename, string term, string language)
        {
            try
            {
                this.dataLayer.AddTerm(tablename, term, language, SPSiteURL);
            }
            catch (Exception ex)
            {
                Acme.Core.DiagnosticSystem.ExceptionManager.ExceptionManager.Manager.CatchException(Acme.Core.DiagnosticSystem.Enums.Enums.ExceptionCode.GenericSharePointCoreMmsSettings, "Could not add term - " + ex.Message + " - " + ex.StackTrace, ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical, Acme.Core.DiagnosticSystem.Enums.Enums.ExceptionType.AcmeSharepointCoreException);
            }
        }

        public void EditTerm(string tablename, string term, string language)
        {
            try
            {
                this.dataLayer.EditTerm(tablename, term, language, SPSiteURL);
            }
            catch (Exception ex)
            {
                Acme.Core.DiagnosticSystem.ExceptionManager.ExceptionManager.Manager.CatchException(Acme.Core.DiagnosticSystem.Enums.Enums.ExceptionCode.GenericSharePointCoreMmsSettings, "Could not edit term - " + ex.Message + " - " + ex.StackTrace, ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical, Acme.Core.DiagnosticSystem.Enums.Enums.ExceptionType.AcmeSharepointCoreException);
            }
        }

        public void DeprecateTerm(string tablename, string term)
        {
            try
            {
                this.dataLayer.DeprecateTerm(tablename, term, SPSiteURL);
            }
            catch (Exception ex)
            {
                Acme.Core.DiagnosticSystem.ExceptionManager.ExceptionManager.Manager.CatchException(Acme.Core.DiagnosticSystem.Enums.Enums.ExceptionCode.GenericSharePointCoreMmsSettings, "Could not deprecate term - " + ex.Message + " - " + ex.StackTrace, ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical, Acme.Core.DiagnosticSystem.Enums.Enums.ExceptionType.AcmeSharepointCoreException);
            }
        }


        
    }



    
}
