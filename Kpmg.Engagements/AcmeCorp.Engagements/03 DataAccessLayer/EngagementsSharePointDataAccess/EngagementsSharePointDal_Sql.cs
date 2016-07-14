// -----------------------------------------------------------------------
// <copyright file="EngagementsSharePointDal_Sql.cs" company="AcmeCorp">
// AcmeCorp
// </copyright>
// -----------------------------------------------------------------------
namespace AcmeCorp.Engagements.EngagementsSharePointDataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Administration;
    using Acme.SharePointCore.Helpers;

    /// <summary>
    /// SharePoint DataAccess Layer for AcmeCorp Engagements
    /// </summary>
    public partial class EngagementsSharePointDal : AcmeCorp.Engagements.EngagementsDomain.IAcmeCorpEngagementsDalInterface
    {
        /// <summary>
        /// Creates a staff row.
        /// </summary>
        /// <param name="engagementWbId">The engagement wb id.</param>
        /// <param name="dataSet">The ds engagements.</param>
        /// <param name="login">The login.</param>
        /// <param name="role">The role.</param>
        private static void CreateStaffRow(long engagementWbId, DataSet dataSet, string login, string role)
        {
            DataRow staffRow = dataSet.Tables[0].NewRow();
            staffRow.BeginEdit();
            staffRow["engagement"] = 0;
            staffRow["engagementwbid"] = engagementWbId;
            staffRow["role"] = role;
            staffRow["login"] = login;
            staffRow.EndEdit();

            dataSet.Tables[0].Rows.Add(staffRow);
        }

        /// <summary>
        /// Saves the engagement into database.
        /// </summary>
        /// <param name="siteCol">The site collection</param>
        /// <param name="engagementOwners">The engagement owners.</param>
        /// <param name="engagementPartners">The engagement partners.</param>
        /// <param name="engagementStaff">The engagement staff.</param>
        /// <param name="engagementProperties">The engagement properties.</param>
        /// <exception cref="Acme">Exception that will be thrown</exception>
        private void SaveEngagementIntoDb(SPSite siteCol, string[] engagementOwners, string[] engagementPartners, string[] engagementStaff, Dictionary<string, object> engagementProperties)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(this.engagementsDbConnectionString))
                {
                    SqlDataAdapter engAdapter = this.GetAdapter("tEngagements");

                    DataSet dataSetEngagements = new DataSet("Engagements");
                    engAdapter.Fill(dataSetEngagements, "tEngagements");

                    DataRow dataRowNewEngagement = dataSetEngagements.Tables[0].NewRow();
                    dataRowNewEngagement.BeginEdit();

                    // TODO: VALIDATION
                    long engagementWbId = Convert.ToInt64(engagementProperties["WB-Auftrags-Nr"]);

                    dataRowNewEngagement["path"] = siteCol.Url;
                    dataRowNewEngagement["sitecolid"] = 0;
                    dataRowNewEngagement["sitecolguid"] = siteCol.ID;
                    dataRowNewEngagement["wbnummer"] = engagementWbId;
                    dataRowNewEngagement["mandantenname"] = engagementProperties["Name des Mandanten"].ToString();
                    dataRowNewEngagement["opportunitynr"] = engagementProperties["Opportunity Nr"].ToString();
                    dataRowNewEngagement["account"] = engagementProperties["Account"].ToString();
                    dataRowNewEngagement["concurringpartner"] = engagementProperties["Concurring Partner"].ToString();
                    dataRowNewEngagement["partner"] = engagementProperties["engpartner"].ToString();
                    dataRowNewEngagement["manager"] = engagementProperties["engmanager"].ToString();
                    dataRowNewEngagement["niederlassung"] = engagementProperties["Niederlassung"].ToString();
                    dataRowNewEngagement["bezeichnung"] = engagementProperties["Bezeichnung"].ToString();
                    dataRowNewEngagement["wbauftragstatus"] = engagementProperties["WB-Auftrag Status"].ToString();
                    dataRowNewEngagement["wbauftragstatusdatum"] = (DateTime)engagementProperties["WB-Auftrag Status Datum"];

                    dataRowNewEngagement["status"] = engagementProperties["status"].ToString();
                    dataRowNewEngagement["statusdate"] = (DateTime)engagementProperties["statusdate"];
                    dataRowNewEngagement["created"] = (DateTime)engagementProperties["created"];

                    dataRowNewEngagement.EndEdit();

                    dataSetEngagements.Tables[0].Rows.Add(dataRowNewEngagement);

                    // dsEngagements.Tables[0].AcceptChanges();
                    if (dataSetEngagements.HasChanges())
                    {
                        engAdapter.Update(dataSetEngagements.GetChanges(), "tEngagements");
                    }

                    this.StoreEngagementStaffIntoDb(engagementWbId, engagementOwners, engagementPartners, engagementStaff);
                }
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeDalException(1000, "Error in storing engagement data into db", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }

        /// <summary>
        /// Stores the engagement staff into database.
        /// </summary>
        /// <param name="engagementWbId">The engagement wb id.</param>
        /// <param name="engagementOwners">The engagement owners.</param>
        /// <param name="engagementPartners">The engagement partners.</param>
        /// <param name="engagementStaff">The engagement staff.</param>
        /// <exception cref="Acme">Exception that will be thrown</exception>
        private void StoreEngagementStaffIntoDb(long engagementWbId, string[] engagementOwners, string[] engagementPartners, string[] engagementStaff)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(this.engagementsDbConnectionString))
                {
                    SqlDataAdapter engAdapter = this.GetAdapter("tEngagementStaff");

                    // delete existing info...
                    engAdapter.SelectCommand.Connection.Open();
                    SqlCommand deleteCommand = new SqlCommand("DELETE FROM tEngagementStaff WHERE engagementwbid=" + engagementWbId.ToString(), engAdapter.SelectCommand.Connection);
                    deleteCommand.ExecuteNonQuery();
                    engAdapter.SelectCommand.Connection.Close();

                    DataSet dataSetEngagements = new DataSet("EngagementStaff");
                    engAdapter.Fill(dataSetEngagements, "tEngagementStaff");

                    foreach (string login in engagementOwners)
                    {
                        CreateStaffRow(engagementWbId, dataSetEngagements, login, "manager");
                    }

                    foreach (string login in engagementPartners)
                    {
                        CreateStaffRow(engagementWbId, dataSetEngagements, login, "partner");
                    }

                    foreach (string login in engagementStaff)
                    {
                        CreateStaffRow(engagementWbId, dataSetEngagements, login, "staff");
                    }

                    if (dataSetEngagements.HasChanges())
                    {
                        engAdapter.Update(dataSetEngagements.GetChanges(), "tEngagementStaff");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Acme.Core.DiagnosticSystem.ExceptionEntities.AcmeDalException(1000, "Error in storing engagement staff into db", ex, Acme.Core.Logger.Enums.EventServerity.ErrorCritical);
            }
        }

        /// <summary>
        /// Creates the data adapter for requested table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>SQL Data Adapter</returns>
        private SqlDataAdapter GetAdapter(string tableName)
        {
            SqlConnection dataBaseConnection = new SqlConnection(this.engagementsDbConnectionString);

            // store engagement information
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM " + tableName, dataBaseConnection);
            SqlCommandBuilder engCb = new SqlCommandBuilder(adapter);

            SqlCommand engInsertCommand = engCb.GetInsertCommand();
            SqlCommand engUpdateCommand = engCb.GetUpdateCommand();
            SqlCommand engDeleteCommand = engCb.GetDeleteCommand();

            adapter.InsertCommand = engInsertCommand;
            adapter.UpdateCommand = engUpdateCommand;
            adapter.DeleteCommand = engDeleteCommand;

            return adapter;
        }
    }
}