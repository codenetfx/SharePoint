// -----------------------------------------------------------------------
// <copyright file="EngagementsListEventReceivers.cs" company="AcmeCorp">
// AcmeCorp
// </copyright>
// -----------------------------------------------------------------------
namespace AcmeCorp.Engagements.EngagementsListEventReceivers
{
    using System;
    using System.Diagnostics;
    using System.Security.Permissions;
    using System.Text;

    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Utilities;
    using Microsoft.SharePoint.Workflow;

    /// <summary>
    /// List Item Events
    /// </summary>
    public class EngagementsListEventReceivers : SPItemEventReceiver
    {
        /// <summary>
        /// An item is being added.
        /// </summary>
        /// <param name="properties">Item event properties</param>
        public override void ItemAdding(SPItemEventProperties properties)
        {
            Debug.Print("ItemAdding event is fired!");

            // base.ItemAdding(properties);
        }

        /// <summary>
        /// An item is being updated.
        /// </summary>
        /// <param name="properties">Item event properties</param>
        public override void ItemUpdating(SPItemEventProperties properties)
        {
            Debug.Print("ItemUpdating event is fired!");

            // base.ItemUpdating(properties);
        }

        /// <summary>
        /// An item is being deleted.
        /// </summary>
        /// <param name="properties">Item event properties</param>
        public override void ItemDeleting(SPItemEventProperties properties)
        {
            try
            {
                StringBuilder output = new StringBuilder();
                SPFolder folder = properties.ListItem.Folder;

                if (folder != null)
                {
                    if (folder.Files.Count > 0 || folder.SubFolders.Count > 0)
                    {
                        // Trying to delete a folder
                        output.AppendFormat("Prevented deletion of the folder {0}\n", folder.Name);
                        output.AppendFormat(" - Folder url: {0}\n", folder.Url);
                        output.AppendFormat(" - Folder contains {0} files\n", folder.Files.Count);
                        output.AppendFormat(" - Folder contains {0} subfolders", folder.SubFolders.Count);

                        // Prevent deletion
                        properties.ErrorMessage = output.ToString();
                        properties.Status = SPEventReceiverStatus.CancelWithError;
                        return;
                    }

                    SPFolder parentFolder = folder.ParentFolder;
                    SPFolder rootFolder = folder.DocumentLibrary.RootFolder;
                    if (parentFolder.UniqueId == rootFolder.UniqueId)
                    {
                        // Trying to delete a folder
                        output.AppendFormat("Prevented deletion of the folder {0}\n", folder.Name);
                        output.AppendFormat(" - Folder url: {0}\n", folder.Url);
                        output.AppendFormat(" - Folder is in first level under document library {0}.", folder.DocumentLibrary.Title);

                        // Prevent deletion
                        properties.ErrorMessage = output.ToString();
                        properties.Status = SPEventReceiverStatus.CancelWithError;
                    }
                }
                else
                {
                    base.ItemDeleting(properties);
                }
            }
            catch (Exception)
            {
                // TODO: Exception handling 
                throw;
            }
        }

        /// <summary>
        /// An item was added.
        /// </summary>
        /// <param name="properties">Item event properties</param>
        public override void ItemAdded(SPItemEventProperties properties)
        {
            Debug.Print("ItemAdded event is fired!");

            // base.ItemAdded(properties);
        }

        /// <summary>
        /// An item was updated.
        /// </summary>
        /// <param name="properties">Item event properties</param>
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            Debug.Print("ItemUpdated event is fired!");

            // base.ItemUpdated(properties);
        }

        /// <summary>
        /// An item was deleted.
        /// </summary>
        /// <param name="properties">Item event properties</param>
        public override void ItemDeleted(SPItemEventProperties properties)
        {
            Debug.Print("ItemDeleted event is fired!");

            // base.ItemDeleted(properties);
        }
    }
}