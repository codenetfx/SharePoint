using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using TestEngagementsService.EngagementsService;
using System.IO;
using Microsoft.SharePoint;
using System.Configuration;
using Microsoft.Office.Server.UserProfiles;

namespace TestEngagementsService
{
    class Program
    {
        private static TestEngagementsService.EngagementsService.EngagementsServiceClient client;

        static void Main(string[] args)
        {
            client = new TestEngagementsService.EngagementsService.EngagementsServiceClient();
            client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;
            client.ChannelFactory.Credentials.Windows.ClientCredential = System.Net.CredentialCache.DefaultNetworkCredentials;
            //client.ChannelFactory.Credentials.Windows.ClientCredential = new System.Net.NetworkCredential("administrator", "pass@word1", "spfarm");


            string name = System.Threading.Thread.CurrentPrincipal.Identity.Name;

            while (1 > 0)
            {

                Console.WriteLine("1 - Get Data");
                Console.WriteLine("2 - Get Engagement Status");
                Console.WriteLine("3 - Close Engagement");
                Console.WriteLine("4 - Reopen Engagement");
                Console.WriteLine("5 - Create Engagement Site");
                Console.WriteLine("6 - Update Engagement Site Properties");
                Console.WriteLine("7 - Create Opportunity Site");
                Console.WriteLine("8 - Update Opportunity Site Properties");
                Console.WriteLine("9 - Convert Engagement To Opportunity");
                //Console.WriteLine("5 - Create folder structure");
                //Console.WriteLine("6 - Read folder structure");
                //Console.WriteLine("7 - Delete Folder");
                //Console.WriteLine("8 - Upload file to folder");
                //Console.WriteLine("9 - Delete File");
                //Console.WriteLine("10 - Get files from folder");
                //Console.WriteLine("11 - Update file properties");
                //Console.WriteLine("12 - Add principal to project");
                //Console.WriteLine("13 - Read folder structures");
                //Console.WriteLine("14 - Apply folder structure to project");
                //Console.WriteLine("15 - Copy folder structure");
                //Console.WriteLine("16 - Initial sorting");
                Console.WriteLine("99 - Exit");

                string action = Console.ReadLine();

                bool end = false;

                switch (action)
                {
                    case "1":
                        GetData();
                        break;
                    case "2":
                        GetEngagementStatus();
                        break;
                    case "3":
                        CloseEngagement();
                        break;
                    case "4":
                        ReopenEngagement();
                        break;
                    case "5":
                        CreateEngagement();
                        break;
                    case "6":
                        UpdateEngagementSite();
                        break;
                    case "7":
                        CreateOpportunity();
                        break;
                    case "8":
                        UpdateOpportunitySite();
                        break;

                    case "9":
                        ConvertEngagementToOpportunity();
                        break;
                    //    //TestWriteMms(); 
                    //    TestReadMms();
                    //    break;
                    //case "3a":
                    //    TestWriteMms();
                    //    //TestReadMms();
                    //    break;
                    //case "4":
                    //    TestCreateSite();
                    //    break;
                    //case "5":
                    //    CreateFolderStructure();
                    //    break;
                    //case "6":
                    //    ReadFolderStructure();
                    //    break;
                    //case "6a":
                    //    UpdateFolder();
                    //    break;
                    //case "7":
                    //    DeleteFolder();
                    //    break;
                    //case "8":
                    //    UploadFileToFolder();
                    //    break;
                    //case "9":
                    //    DeleteFile();
                    //    break;
                    //case "10":
                    //    GetFilesInFolder();
                    //    break;
                    //case "11":
                    //    UpdateFileProperties();
                    //    break;
                    //case "12":
                    //    AddPermissions();
                    //    break;
                    //case "13":
                    //    ReadMmsFolderStructureTemplates();
                    //    break;
                    //case "14":
                    //    ApplyFolderStructureToProject();
                    //    break;
                    //case "15":
                    //    CopyFolderStructure();
                    //    break;
                    //case "16":
                    //    InitialSorting();
                    //    break;
                    case "99":
                    case "X":
                    case "x":
                        end = true;
                        break;
                    default:
                        break;
                }

                if (end)
                {
                    break;
                }

                Console.ReadLine();
            }

        }

        private static void UpdateEngagementSite()
        {
            LoadFields();
            Console.WriteLine("Please enter site (10000-99999):");
            string siteId = Console.ReadLine();
            wbNumber = Convert.ToInt32(siteId);
            UpdateEngagementSiteProperties();
        }

        private static void UpdateOpportunitySite()
        {
            LoadFields();
            Console.WriteLine("Please enter site (10000-99999):");
            string siteId = Console.ReadLine();
            wbNumber = Convert.ToInt32(siteId);
            UpdateOpportunitySiteProperties();
        }

        private static void UpdateEngagementSiteProperties()
        {
            string newSiteUrl = string.Empty;
            try
            {
                Console.WriteLine("Creating new site with Engagement ID: " + wbNumber);
                //string engagementIDString = Console.ReadLine();
                //long engagementId = Convert.ToInt64(engagementIDString);

                string domain = ConfigurationManager.AppSettings["DefaultDomain"];

                List<string> partners = new List<string>();
                partners.Add(domain + engagementPartner);

                List<string> managers = new List<string>();
                managers.Add(domain + engagementManager);

                List<string> staff = new List<string>();
                foreach (string staffName in staffMembers.Split(';'))
                {
                    staff.Add(domain + staffName);
                }

                Dictionary<string, object> engagementProperties = new Dictionary<string, object>();

                engagementProperties.Add("Name des Mandanten", "Some Company");
                engagementProperties.Add("Opportunity Nr", "111111");
                engagementProperties.Add("Account", "Company AG");
                engagementProperties.Add("Concurring Partner", domain + concurringPartner);
                engagementProperties.Add("Niederlassung", "Frankfurt");
                engagementProperties.Add("Bezeichnung", "My engagement's description");
                engagementProperties.Add("WB-Auftrags-Nr", wbNumber);
                engagementProperties.Add("Eng.Partner", domain + engagementPartner);
                engagementProperties.Add("Eng.Manager", domain + engagementManager);
                engagementProperties.Add("WB-Auftrag Status", "open");
                engagementProperties.Add("WB-Auftrag Status Datum", auftragStatusDatum.Date);


                //TODO:
                //to be used later for partner and manager groups
                engagementProperties.Add("partnergroup", string.Empty);
                engagementProperties.Add("managergroup", string.Empty);

                newSiteUrl = client.UpdateEngagementSiteProperties(Convert.ToInt64(wbNumber), 1, managers.ToArray(), partners.ToArray(), staff.ToArray(), engagementProperties);
                Console.WriteLine("Site is created at URL: " + newSiteUrl);

            }
            catch (FaultException fex)
            {
                Console.WriteLine("Service Fault:");

                Console.WriteLine(fex.Message);
                Console.WriteLine(fex.InnerException);
                Console.WriteLine(fex.StackTrace);
                Console.WriteLine(fex.Code.Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:");

                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                Console.WriteLine("Finished");
            }
        }


        private static void UpdateOpportunitySiteProperties()
        {
            string newSiteUrl = string.Empty;
            try
            {
                Console.WriteLine("Updating Opportunity site with  ID: " + wbNumber);
                //string engagementIDString = Console.ReadLine();
                //long engagementId = Convert.ToInt64(engagementIDString);

                string domain = ConfigurationManager.AppSettings["DefaultDomain"];

                List<string> partners = new List<string>();
                partners.Add(domain + engagementPartner);

                List<string> managers = new List<string>();
                managers.Add(domain + engagementManager);

                List<string> staff = new List<string>();
                foreach (string staffName in staffMembers.Split(';'))
                {
                    staff.Add(domain + staffName);
                }

                Dictionary<string, object> engagementProperties = new Dictionary<string, object>();

                engagementProperties.Add("Name des Mandanten", "Some Company");
                engagementProperties.Add("Opportunity Nr", "111111");
                engagementProperties.Add("Account", "Company AG");
                engagementProperties.Add("Concurring Partner", domain + concurringPartner);
                engagementProperties.Add("Niederlassung", "Frankfurt");
                engagementProperties.Add("Bezeichnung", "My engagement's description");
                engagementProperties.Add("WB-Auftrags-Nr", wbNumber);
                engagementProperties.Add("Eng.Partner", domain + engagementPartner);
                engagementProperties.Add("Eng.Manager", domain + engagementManager);
                engagementProperties.Add("WB-Auftrag Status", "open");
                engagementProperties.Add("WB-Auftrag Status Datum", auftragStatusDatum.Date);


                //TODO:
                //to be used later for partner and manager groups
                engagementProperties.Add("partnergroup", string.Empty);
                engagementProperties.Add("managergroup", string.Empty);

                newSiteUrl = client.UpdateOpportunitySiteProperties(Convert.ToInt64(wbNumber), managers.ToArray(), partners.ToArray(), staff.ToArray(), engagementProperties);
                Console.WriteLine("Site is updated.");

            }
            catch (FaultException fex)
            {
                Console.WriteLine("Service Fault:");

                Console.WriteLine(fex.Message);
                Console.WriteLine(fex.InnerException);
                Console.WriteLine(fex.StackTrace);
                Console.WriteLine(fex.Code.Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:");

                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                Console.WriteLine("Finished");
            }
        }

        private static void CreateEngagement()
        {
            LoadFields();
            CreateNewEngagementSite();
        }

        private static void CreateOpportunity()
        {
            LoadFields();
            CreateNewOpportunitySite();
        }


        private static void ReadMmsFolderStructureTemplates()
        {
            try
            {
                Console.WriteLine("Please enter the project id:");
                string pid = Console.ReadLine();

                Console.WriteLine("Please enter user login (with domain):");
                string login = Console.ReadLine();

                //client. .AddPermissionForPrincipal(Convert.ToInt32(pid), login, ArgoProjectsEnumsArgoPermissionLevels.Contribute);
            }
            catch (FaultException fex)
            {
                Console.WriteLine("Service Fault by updating file metada:");

                Console.WriteLine(fex.Message);
                Console.WriteLine(fex.InnerException);
                Console.WriteLine(fex.StackTrace);
                Console.WriteLine(fex.Code.Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in updating file metadata:");

                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                Console.WriteLine("Finished");
            }
        }

        private static void ApplyFolderStructureToProject()
        {
            throw new NotImplementedException();
        }

        static int wbNumber;
        static string engagementManager = string.Empty;
        static string engagementPartner = string.Empty;
        static string concurringPartner = string.Empty;
        static DateTime auftragStatusDatum;
        static string staffMembers = string.Empty;

        private static void LoadFields()
        {
            try
            {
                Helpers.DummyUsers dummyUsers = new Helpers.DummyUsers();

                Console.WriteLine( "Getting initial values... (users)");
                        wbNumber = (int)(new Random().Next(10000, 99999));
                //this.Cursor = Cursors.WaitCursor;

                //this.txtStaff.Clear();

                using (SPSite spSite = new SPSite(ConfigurationManager.AppSettings["RootSite"]))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {

                        SPUserCollection allUsers = spWeb.SiteUsers;
                        SPUser mUser = allUsers[1];


                        SPUser managerUser = spWeb.SiteUsers[GetFromSiteUsers(dummyUsers)];
                        engagementManager = managerUser.LoginName.Split('\\')[1];

                        SPUser partnerUser = spWeb.SiteUsers[GetFromSiteUsers(dummyUsers)];
                        engagementPartner = partnerUser.LoginName.Split('\\')[1];

                        SPUser concurentPartner = spWeb.SiteUsers[GetFromSiteUsers(dummyUsers)];
                        concurringPartner = concurentPartner.LoginName.Split('\\')[1];

                        auftragStatusDatum = DateTime.Now.Date;

                        int staffNum = new Random().Next(5, 9);

                        List<string> staffList = new List<string>();

                        for (int i = 0; i < staffNum; i++)
                        {
                            SPUser staffUser = spWeb.SiteUsers[GetFromSiteUsers(dummyUsers)];
                            string staffLogin = staffUser.LoginName.Split('\\')[1];
                            if (staffMembers.Length >1)
                            {
                                if (!staffMembers.EndsWith(";"))
                                {
                                    staffMembers += ";";
                                }
                            }
                            staffList.Add(staffLogin);
                            
                            staffMembers += staffLogin;
                            if (i < staffNum - 1)
                            {
                                staffMembers += ";";
                            }
                        }

                    }
                }

                string bla = string.Empty;
            }
            catch (Exception ex)
            {
                
            }
        }

        private static string GetFromSiteUsers(Helpers.DummyUsers dummyUsers)
        {
            string username = dummyUsers.GetNextUser();
            string domain = ConfigurationManager.AppSettings["DefaultDomain"];
            return domain + username;

        }

        private static void CreateNewEngagementSite()
        {
            string newSiteUrl = string.Empty;
            try
            {
                Console.WriteLine("Creating new site with Engagement ID: " + wbNumber);
                //string engagementIDString = Console.ReadLine();
                //long engagementId = Convert.ToInt64(engagementIDString);

                string domain = ConfigurationManager.AppSettings["DefaultDomain"];

                List<string> partners = new List<string>();
                partners.Add(domain + engagementPartner);

                List<string> managers = new List<string>();
                managers.Add(domain + engagementManager);

                List<string> staff = new List<string>();
                foreach (string staffName in staffMembers.Split(';'))
                {
                    staff.Add(domain + staffName);
                }

                Dictionary<string, object> engagementProperties = new Dictionary<string, object>();

                engagementProperties.Add("Name des Mandanten", "Some Company");
                engagementProperties.Add("Opportunity Nr", "111111");
                engagementProperties.Add("Account", "Company AG");
                engagementProperties.Add("Concurring Partner", domain + concurringPartner);
                engagementProperties.Add("Niederlassung", "Frankfurt");
                engagementProperties.Add("Bezeichnung", "My engagement's description");
                engagementProperties.Add("WB-Auftrags-Nr", wbNumber);
                engagementProperties.Add("Eng.Partner", domain + engagementPartner);
                engagementProperties.Add("Eng.Manager", domain + engagementManager);
                engagementProperties.Add("WB-Auftrag Status", "open");
                engagementProperties.Add("WB-Auftrag Status Datum", auftragStatusDatum.Date);

                
                //TODO:
                //to be used later for partner and manager groups
                engagementProperties.Add("partnergroup", string.Empty);
                engagementProperties.Add("managergroup", string.Empty);

                newSiteUrl = client.CreateNewEngagementSite(Convert.ToInt64(wbNumber), 1, managers.ToArray(), partners.ToArray(), staff.ToArray(), engagementProperties);
                Console.WriteLine("Site is created at URL: " + newSiteUrl);
            
            }
            catch (FaultException fex)
            {
                Console.WriteLine("Service Fault:");

                Console.WriteLine(fex.Message);
                Console.WriteLine(fex.InnerException);
                Console.WriteLine(fex.StackTrace);
                Console.WriteLine(fex.Code.Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:");

                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                Console.WriteLine("Finished");
            }

        }

        private static void CreateNewOpportunitySite()
        {
            string newSiteUrl = string.Empty;
            try
            {
                Console.WriteLine("Creating new Opportunity site with ID: " + wbNumber);
                //string engagementIDString = Console.ReadLine();
                //long engagementId = Convert.ToInt64(engagementIDString);

                string domain = ConfigurationManager.AppSettings["DefaultDomain"];

                List<string> partners = new List<string>();
                partners.Add(domain + engagementPartner);

                List<string> managers = new List<string>();
                managers.Add(domain + engagementManager);

                List<string> staff = new List<string>();
                foreach (string staffName in staffMembers.Split(';'))
                {
                    staff.Add(domain + staffName);
                }

                Dictionary<string, object> engagementProperties = new Dictionary<string, object>();

                engagementProperties.Add("Name des Mandanten", "Some Company");
                engagementProperties.Add("Opportunity Nr", "111111");
                engagementProperties.Add("Account", "Company AG");
                engagementProperties.Add("Concurring Partner", domain + concurringPartner);
                engagementProperties.Add("Niederlassung", "Frankfurt");
                engagementProperties.Add("Bezeichnung", "My engagement's description");
                engagementProperties.Add("WB-Auftrags-Nr", wbNumber);
                engagementProperties.Add("Eng.Partner", domain + engagementPartner);
                engagementProperties.Add("Eng.Manager", domain + engagementManager);
                engagementProperties.Add("WB-Auftrag Status", "open");
                engagementProperties.Add("WB-Auftrag Status Datum", auftragStatusDatum.Date);


                //TODO:
                //to be used later for partner and manager groups
                engagementProperties.Add("partnergroup", string.Empty);
                engagementProperties.Add("managergroup", string.Empty);

                newSiteUrl = client.CreateNewOpportunitySite(Convert.ToInt64(wbNumber), managers.ToArray(), partners.ToArray(), staff.ToArray(), engagementProperties);
                Console.WriteLine("Opportunity Site is created.");

            }
            catch (FaultException fex)
            {
                Console.WriteLine("Service Fault:");

                Console.WriteLine(fex.Message);
                Console.WriteLine(fex.InnerException);
                Console.WriteLine(fex.StackTrace);
                Console.WriteLine(fex.Code.Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:");

                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                Console.WriteLine("Finished");
            }

        }

        private static void GetEngagementStatus()
        {
            try
            {
                Console.WriteLine("Please enter engagementID (valid number 10000-99999):");
                string engagementIDString = Console.ReadLine();
                long engagementId = Convert.ToInt64(engagementIDString);

                string retVal = client.GetEngagementStatus(engagementId);
                Console.WriteLine(retVal);
            }
            catch (FaultException fex)
            {
                Console.WriteLine("Service Fault:");

                Console.WriteLine(fex.Message);
                Console.WriteLine(fex.InnerException);
                Console.WriteLine(fex.StackTrace);
                Console.WriteLine(fex.Code.Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:");

                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                Console.WriteLine("Finished");
            }
        }

        private static void CloseEngagement()
        {
            try
            {
                Console.WriteLine("Please enter engagementID (valid number 10000-99999):");
                string engagementIDString = Console.ReadLine();
                long engagementId = Convert.ToInt64(engagementIDString);


                string retVal = client.CloseEngagement(engagementId);
                Console.WriteLine(retVal);
            }
            catch (FaultException fex)
            {
                Console.WriteLine("Service Fault:");

                Console.WriteLine(fex.Message);
                Console.WriteLine(fex.InnerException);
                Console.WriteLine(fex.StackTrace);
                Console.WriteLine(fex.Code.Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:");

                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                Console.WriteLine("Finished");
            }
        }

        private static void ConvertEngagementToOpportunity()
        {
            try
            {
                Console.WriteLine("Please enter engagementID that you want to convert to opportunity(valid number 10000-99999):");
                string engagementIDString = Console.ReadLine();
                long engagementId = Convert.ToInt64(engagementIDString);


                string retVal = client.CreateEngagementFromOpportunity(engagementId);
                Console.WriteLine(retVal);
            }
            catch (FaultException fex)
            {
                Console.WriteLine("Service Fault:");

                Console.WriteLine(fex.Message);
                Console.WriteLine(fex.InnerException);
                Console.WriteLine(fex.StackTrace);
                Console.WriteLine(fex.Code.Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:");

                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                Console.WriteLine("Finished");
            }
        }

        private static void ReopenEngagement()
        {
            try
            {
                Console.WriteLine("Please enter engagementID (valid number 10000-99999):");
                string engagementIDString = Console.ReadLine();
                long engagementId = Convert.ToInt64(engagementIDString);


                string retVal = client.ReopenEngagement(engagementId);
                Console.WriteLine(retVal);
            }
            catch (FaultException fex)
            {
                Console.WriteLine("Service Fault:");

                Console.WriteLine(fex.Message);
                Console.WriteLine(fex.InnerException);
                Console.WriteLine(fex.StackTrace);
                Console.WriteLine(fex.Code.Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:");

                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                Console.WriteLine("Finished");
            }
        }

        private static void GetData()
        {
            try
            {
                 Console.WriteLine("THIS METHOD IS TESTING WHETHER WEB SERVICE WORKS AT ALL\nPlease enter the data to send:");
                string dataSend = Console.ReadLine();
                //int dataSendInt = Convert.ToInt32(dataSend);


                string retVal = client.GetData(dataSend);
                Console.WriteLine(retVal);
            }
            catch (FaultException fex)
            {
                Console.WriteLine("Service Fault:");

                Console.WriteLine(fex.Message);
                Console.WriteLine(fex.InnerException);
                Console.WriteLine(fex.StackTrace);
                Console.WriteLine(fex.Code.Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:");

                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.InnerException);
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                Console.WriteLine("Finished");
            }
        }

        //private static void AddPermissions()
        //{
        //    try
        //    {
        //        Console.WriteLine("Please enter the project id:");
        //        string pid = Console.ReadLine();

        //        Console.WriteLine("Please enter user login (with domain):");
        //        string login = Console.ReadLine();

        //        client.AddPermissionForPrincipal(Convert.ToInt32(pid), login, ArgoProjectsEnumsArgoPermissionLevels.Contribute);
        //    }
        //    catch (FaultException fex)
        //    {
        //        Console.WriteLine("Service Fault by updating file metada:");

        //        Console.WriteLine(fex.Message);
        //        Console.WriteLine(fex.InnerException);
        //        Console.WriteLine(fex.StackTrace);
        //        Console.WriteLine(fex.Code.Name);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Exception in updating file metadata:");

        //        Console.WriteLine(ex.Message);
        //        Console.WriteLine(ex.InnerException);
        //        Console.WriteLine(ex.StackTrace);
        //    }
        //    finally
        //    {
        //        Console.WriteLine("Finished");
        //    }
        //}

        //private static void UpdateFileProperties()
        //{
        //    try
        //    {

        //        Console.WriteLine("Please enter the project id:");
        //        string pid = Console.ReadLine();

        //        Console.WriteLine("Please enter the file guid:");
        //        string fguid = Console.ReadLine();
        //        Guid guid = new Guid(fguid);

        //        ProjectFile file = client.GetFileById(Convert.ToInt32(pid), guid);

        //        Console.WriteLine("Please enter the new parent folder guid:");
        //        string nfguids = Console.ReadLine();
        //        Guid nfguid = new Guid(nfguids);

        //        ProjectFolder newFolder = client.GetFolderById(Convert.ToInt32(pid), nfguid);

        //        Console.WriteLine("Please enter the new file title:");
        //        string title = Console.ReadLine();

        //        Console.WriteLine("Please enter the new Argo Activity Id:");
        //        string aaid = Console.ReadLine();

        //        Console.WriteLine("Please enter the new File Type Id:");
        //        string ftid = Console.ReadLine();

        //        client.UploadFileProperties(Convert.ToInt32(pid), file, title, newFolder, Convert.ToInt32(aaid), Convert.ToInt32(ftid), ArgoProjectsEnumsIncreaseVersion.NewMajorVersion);

        //    }
        //    catch (FaultException fex)
        //    {
        //        Console.WriteLine("Service Fault by adding a principal:");

        //        Console.WriteLine(fex.Message);
        //        Console.WriteLine(fex.InnerException);
        //        Console.WriteLine(fex.StackTrace);
        //        Console.WriteLine(fex.Code.Name);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Exception in adding a principal:");

        //        Console.WriteLine(ex.Message);
        //        Console.WriteLine(ex.InnerException);
        //        Console.WriteLine(ex.StackTrace);
        //    }
        //    finally
        //    {
        //        Console.WriteLine("Finished");
        //    }
        //}

        //private static void GetFilesInFolder()
        //{
        //    try
        //    {
        //        Console.WriteLine("Please enter the project id:");
        //        string pid = Console.ReadLine();

        //        Console.WriteLine("Please enter the folder guid:");
        //        string fguid = Console.ReadLine();
        //        Guid guid = new Guid(fguid);

        //        ProjectFile[] files = client.GetFilesInFolder(Convert.ToInt32(pid), guid);

        //        foreach (ProjectFile pf in files)
        //        {
        //            Console.WriteLine(string.Format("File: {0} / {1} / Url: {2} / Name: {3} / ArgoAcitivityId: {4}", pf.UniqueId, pf.Title, pf.FullUrl, pf.FileName, pf.ArgoActivityId));
        //        }

        //    }
        //    catch (FaultException fex)
        //    {
        //        Console.WriteLine("Service Fault by listing files:");

        //        Console.WriteLine(fex.Message);
        //        Console.WriteLine(fex.InnerException);
        //        Console.WriteLine(fex.StackTrace);
        //        Console.WriteLine(fex.Code.Name);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Exception in listing files:");

        //        Console.WriteLine(ex.Message);
        //        Console.WriteLine(ex.InnerException);
        //        Console.WriteLine(ex.StackTrace);
        //    }
        //    finally
        //    {
        //        Console.WriteLine("Finished");
        //    }
        //}

        //private static void DeleteFile()
        //{
        //    try
        //    {
        //        Console.WriteLine("Please enter the project id:");
        //        string pid = Console.ReadLine();

        //        Console.WriteLine("Please enter the file guid:");
        //        string fguid = Console.ReadLine();
        //        Guid guid = new Guid(fguid);

        //        bool deleted = client.DeleteFile(Convert.ToInt32(pid), guid);

        //        Console.WriteLine("File deleted: " + deleted.ToString());

        //    }
        //    catch (FaultException fex)
        //    {
        //        Console.WriteLine("Service Fault by deleting file:");

        //        Console.WriteLine(fex.Message);
        //        Console.WriteLine(fex.InnerException);
        //        Console.WriteLine(fex.StackTrace);
        //        Console.WriteLine(fex.Code.Name);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Exception in deleting file:");

        //        Console.WriteLine(ex.Message);
        //        Console.WriteLine(ex.InnerException);
        //        Console.WriteLine(ex.StackTrace);
        //    }
        //    finally
        //    {
        //        Console.WriteLine("Finished");
        //    }
        //}

        //private static void UploadFileToFolder()
        //{
        //    try
        //    {
        //        Console.WriteLine("Please enter the project id:");
        //        string pid = Console.ReadLine();

        //        Console.WriteLine("Please enter the folder guid:");
        //        string fguid = Console.ReadLine();
        //        Guid guid = new Guid(fguid);

        //        Console.WriteLine("Please enter the file path:");
        //        string filepath = Console.ReadLine();
        //        byte[] file1ToUpload = File.ReadAllBytes(filepath);

        //        Console.WriteLine("Please enter the file title:");
        //        string filetitle = Console.ReadLine();

        //        ProjectFile file = client.UploadFileToFolder(Convert.ToInt32(pid), filetitle, filepath, guid, 11, 12, file1ToUpload, ArgoProjectsEnumsIncreaseVersion.NewMajorVersion);

        //        //bool deleted = client.DeleteFolder(Convert.ToInt32(pid), guid);

        //        //Console.WriteLine("Folder deleted: " + deleted.ToString());

        //        if (file != null)
        //        {
        //            Console.WriteLine("File uploaded: " + file.FileName);
        //        }
        //        else
        //        {
        //            Console.WriteLine("Someting is wrong, file is null!");
        //        }


        //    }
        //    catch (FaultException fex)
        //    {
        //        Console.WriteLine("Service Fault by uploading file:");

        //        Console.WriteLine(fex.Message);
        //        Console.WriteLine(fex.InnerException);
        //        Console.WriteLine(fex.StackTrace);
        //        Console.WriteLine(fex.Code.Name);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Exception in uploading file:");

        //        Console.WriteLine(ex.Message);
        //        Console.WriteLine(ex.InnerException);
        //        Console.WriteLine(ex.StackTrace);
        //    }
        //    finally
        //    {
        //        Console.WriteLine("Finished");
        //    }
        //}



        //private static void TestCreateSite()
        //{
        //    try
        //    {
        //        Console.WriteLine("Project id:");
        //        string projid = Console.ReadLine();

        //        int pid = Convert.ToInt32(projid);

        //        bool siteCreated = client.ExampleCreateProject(pid, "TestProject1", "Hi Marco");

        //        if (siteCreated == true)
        //        {
        //            Console.WriteLine("Site created");
        //        }
        //        else
        //        {
        //            Console.WriteLine("Site NOT created");
        //        }
        //    }
        //    catch (FaultException fex)
        //    {
        //        Console.WriteLine("Service Fault by Creating site:");

        //        Console.WriteLine(fex.Message);
        //        Console.WriteLine(fex.InnerException);
        //        Console.WriteLine(fex.StackTrace);
        //        Console.WriteLine(fex.Code.Name);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Exception by Creating site:");

        //        Console.WriteLine(ex.Message);
        //        Console.WriteLine(ex.InnerException);
        //        Console.WriteLine(ex.StackTrace);
        //    }
        //    finally
        //    {
        //        Console.WriteLine("Finished");
        //    }
        //}

        //private static void TestLogMessage()
        //{
        //    client.TestWriteUlsLog();
        //}

        //private static void TestWriteMms()
        //{
        //    try
        //    {
        //        Console.WriteLine("Key: ");
        //        string key = Console.ReadLine();

        //        Console.WriteLine("Value: ");
        //        string val = Console.ReadLine();

        //        client.TestWriteSetting("WcfTestArea", key, val);
        //    }
        //    catch (FaultException fex)
        //    {
        //        Console.WriteLine("Service Fault by MMS:");

        //        Console.WriteLine(fex.Message);
        //        Console.WriteLine(fex.InnerException);
        //        Console.WriteLine(fex.StackTrace);
        //        Console.WriteLine(fex.Code.Name);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Exception Fault by MMS:");

        //        Console.WriteLine(ex.Message);
        //        Console.WriteLine(ex.InnerException);
        //        Console.WriteLine(ex.StackTrace);
        //    }
        //    finally
        //    {
        //        Console.WriteLine("Finished");
        //    }
        //}

        //private static void TestReadMms()
        //{
        //    try
        //    {
        //        Dictionary<string, string> settings = client.TestReadAllSettingsSetting();

        //        Console.WriteLine("Settings:");

        //        foreach (KeyValuePair<string, string> setting in settings)
        //        {
        //            Console.WriteLine(setting.Key + " ==== " + setting.Value);
        //        }
        //    }
        //    catch (FaultException fex)
        //    {
        //        Console.WriteLine("Service Fault by MMS:");

        //        Console.WriteLine(fex.Message);
        //        Console.WriteLine(fex.InnerException);
        //        Console.WriteLine(fex.StackTrace);
        //        Console.WriteLine(fex.Code.Name);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Exception Fault by MMS:");

        //        Console.WriteLine(ex.Message);
        //        Console.WriteLine(ex.InnerException);
        //        Console.WriteLine(ex.StackTrace);
        //    }

        //}

        //private static void TestCachingSave()
        //{
        //    client.TestWriteCache("JaneTest", "JaneValue");
        //}

        //private static void TestCachingRead()
        //{
        //    Console.WriteLine("Cached value: " + client.TestReadCache("JaneTest"));
        //}

        //private static void SearchData(string searchQuery)
        //{

        //    try
        //    {
        //        TestEngagementsService.ArgoProjectsService.ArgoSearchResult[] results = client.SearchRepository(searchQuery);

        //        foreach (TestEngagementsService.ArgoProjectsService.ArgoSearchResult result in results)
        //        {
        //            Console.WriteLine(result.FileName + " --- " + result.FileAbstract);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }

        //}

        //private static void ReadFolderStructure()
        //{
        //    try
        //    {
        //        Console.WriteLine("Please enter the project id:");
        //        string pid = Console.ReadLine();

        //        ProjectFolder[] folders = client.GetFolderStructure(Convert.ToInt32(pid));

        //        foreach (ProjectFolder folder in folders)
        //        {
        //            Console.WriteLine(string.Format("Folder: {0} / {1} / Parentid: {2} / Foldertype: {3} / Busienessunit: {4} / Childrencout: {5} / SortOrder: {6}", folder.UniqueId, folder.Name, folder.ParentFolderId, folder.FolderTypeId, folder.BusinessUnitId, folder.Children.Count(), folder.SortingOrder));
        //        }
        //    }
        //    catch (FaultException fex)
        //    {
        //        Console.WriteLine("Service Fault by reading folder structure:");

        //        Console.WriteLine(fex.Message);
        //        Console.WriteLine(fex.InnerException);
        //        Console.WriteLine(fex.StackTrace);
        //        Console.WriteLine(fex.Code.Name);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Exception in reading folder structure:");

        //        Console.WriteLine(ex.Message);
        //        Console.WriteLine(ex.InnerException);
        //        Console.WriteLine(ex.StackTrace);
        //    }
        //    finally
        //    {
        //        Console.WriteLine("Finished");
        //    }

        //}

        //private static void DeleteFolder()
        //{
        //    try
        //    {
        //        Console.WriteLine("Please enter the project id:");
        //        string pid = Console.ReadLine();

        //        Console.WriteLine("Please enter the folder guid:");
        //        string fguid = Console.ReadLine();

        //        Guid guid = new Guid(fguid);

        //        bool deleted = client.DeleteFolder(Convert.ToInt32(pid), guid);

        //        Console.WriteLine("Folder deleted: " + deleted.ToString());

        //    }
        //    catch (FaultException fex)
        //    {
        //        Console.WriteLine("Service Fault by deleting folder:");

        //        Console.WriteLine(fex.Message);
        //        Console.WriteLine(fex.InnerException);
        //        Console.WriteLine(fex.StackTrace);
        //        Console.WriteLine(fex.Code.Name);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Exception in deleting folder:");

        //        Console.WriteLine(ex.Message);
        //        Console.WriteLine(ex.InnerException);
        //        Console.WriteLine(ex.StackTrace);
        //    }
        //    finally
        //    {
        //        Console.WriteLine("Finished");
        //    }
        //}

        //private static void CreateFolderStructure()
        //{
        //    try
        //    {
        //        Console.WriteLine("Please enter the project id:");
        //        string pid = Console.ReadLine();

        //        ProjectFolder myFolder1 = new ProjectFolder();
        //        ProjectFolder myFolder2 = new ProjectFolder();
        //        ProjectFolder myFolder3 = new ProjectFolder();
        //        myFolder1 = createFolder("test 1", null, Convert.ToInt32(pid), 1, 2, 10);
        //        myFolder2 = createFolder("test 2", null, Convert.ToInt32(pid), 3, 4, 20);
        //        myFolder3 = createFolder("test 11", myFolder2.UniqueId, Convert.ToInt32(pid), 11, 22, 30);
        //    }
        //    catch (FaultException fex)
        //    {
        //        Console.WriteLine("Service Fault by creating folder structure:");

        //        Console.WriteLine(fex.Message);
        //        Console.WriteLine(fex.InnerException);
        //        Console.WriteLine(fex.StackTrace);
        //        Console.WriteLine(fex.Code.Name);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Exception in creating folder structure:");

        //        Console.WriteLine(ex.Message);
        //        Console.WriteLine(ex.InnerException);
        //        Console.WriteLine(ex.StackTrace);
        //    }
        //    finally
        //    {
        //        Console.WriteLine("Finished");
        //    }
        //}

        //private static ProjectFolder createFolder(string name, Guid? parentFolderId, int projectId, int businessUnitId, int folderTypeId, int sortingOrder)
        //{
        //    ProjectFolder newFolder = new ProjectFolder();
        //    newFolder = client.CreateFolder(name, parentFolderId, projectId, businessUnitId, folderTypeId, sortingOrder);
        //    return newFolder;
        //}

    //    private static void UpdateFolder()
    //    {
    //        try
    //        {
    //            Console.WriteLine("Please enter the project id:");
    //            string pid = Console.ReadLine();

    //            Console.WriteLine("Please enter the folder guid:");
    //            string fguid = Console.ReadLine();
    //            Guid folderguid = new Guid(fguid);

    //            ProjectFolder folder = client.GetFolderById(Convert.ToInt32(pid), folderguid);

    //            //ProjectId   BusinessUnitId   FolderTypeId

    //            Console.WriteLine("Please enter the new name:");
    //            string newName = Console.ReadLine();

    //            Console.WriteLine("Please enter the new parent folder id:");
    //            string pfid = Console.ReadLine();
    //            Guid? newParentFolderId = null;
    //            if (pfid.Trim() != String.Empty)
    //            {
    //                newParentFolderId = new Guid(pfid);
    //            }

    //            Console.WriteLine("Please enter the new folder type id:");
    //            string newfolderTypeId = Console.ReadLine();

    //            Console.WriteLine("Please enter the new business id:");
    //            string newBusinessUnitId = Console.ReadLine();

    //            Console.WriteLine("Please enter the new project id:");
    //            string newProjectId = Console.ReadLine();

    //            Console.WriteLine("Please enter the new sorting order:");
    //            string newSortingOrder = Console.ReadLine();

    //            client.UpdateFolder(
    //                folderguid, newName, 
    //                newParentFolderId, 
    //                folder.ProjectId, 
    //                Convert.ToInt32(newProjectId),
    //                Convert.ToInt32(newBusinessUnitId), 
    //                Convert.ToInt32(newfolderTypeId), 
    //                Convert.ToInt32(newSortingOrder));
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine("Exception in updating folder:");

    //            Console.WriteLine(ex.Message);
    //            Console.WriteLine(ex.InnerException);
    //            Console.WriteLine(ex.StackTrace);
    //        }
    //        finally
    //        {
    //            Console.WriteLine("Finished");
    //        }

    //    }


    //    private static void CopyFolderStructure()
    //    {
    //        try
    //        {
    //            Console.WriteLine("Please enter the project id:");
    //            string pid = Console.ReadLine();

    //            Console.WriteLine("Please enter structure template name:");
    //            string name = Console.ReadLine();

    //            client.CreateProjectStructureFromTemplate(Convert.ToInt32(pid), name);
    //        }
    //        catch (FaultException fex)
    //        {
    //            Console.WriteLine("Service Fault by copying folder structure:");

    //            Console.WriteLine(fex.Message);
    //            Console.WriteLine(fex.InnerException);
    //            Console.WriteLine(fex.StackTrace);
    //            Console.WriteLine(fex.Code.Name);
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine("Exception in copying folder structure:");

    //            Console.WriteLine(ex.Message);
    //            Console.WriteLine(ex.InnerException);
    //            Console.WriteLine(ex.StackTrace);
    //        }
    //        finally
    //        {
    //            Console.WriteLine("Finished");
    //        }
    //    }


    //    private static void InitialSorting()
    //    {
    //        try
    //        {
    //            Console.WriteLine("Please enter the project id:");
    //            string pid = Console.ReadLine();

    //            ProjectFolder[] projectFolders = client.GetFolderStructure(Convert.ToInt32(pid)); // alle Ordner holen
    //            foreach (ProjectFolder folderItem in projectFolders)
    //            {
    //                Guid folderId = folderItem.UniqueId;
    //                Guid? newParentFolderId = folderItem.ParentFolderId;
    //                ProjectFolder newParentFolder = null;
    //                ProjectFolder folder = null;
    //                int newSortingOrder = 0;

    //                bool result = Int32.TryParse(folderItem.Name.Substring(0, System.Text.RegularExpressions.Regex.Match(folderItem.Name.Replace(".", "0"), "[^0-9]").Index - 1), out newSortingOrder);

    //                // Get the folder from its ID
    //                if (newParentFolderId != null)
    //                {
    //                    newParentFolder = client.GetFolderById(Convert.ToInt32(pid), (Guid)newParentFolderId);
    //                }

    //                folder = client.GetFolderById(Convert.ToInt32(pid), folderId);

    //                client.UpdateFolder(folderItem.UniqueId, folderItem.Name, newParentFolderId, Convert.ToInt32(pid), Convert.ToInt32(pid), folderItem.BusinessUnitId, folderItem.FolderTypeId, newSortingOrder);
    //            }
    //        }
    //        catch (Exception error)
    //        {
    //            string message = error.Message;
    //        }

    //}

    }
}
