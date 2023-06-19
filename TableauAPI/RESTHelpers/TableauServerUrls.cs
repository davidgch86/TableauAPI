﻿using System;
using TableauAPI.FilesLogging;
using TableauAPI.RESTRequests;
using TableauAPI.ServerData;

namespace TableauAPI.RESTHelpers
{
    /// <summary>
    /// Creates the set of server specific URLs
    /// </summary>
    public class TableauServerUrls : ITableauServerSiteInfo
    {
        /// <summary>
        /// What version of Server do we thing we are talking to? (URLs and APIs may differ)
        /// </summary>
        public ServerVersion ServerVersion { get; }

        /// <summary>
        /// Url for API login
        /// </summary>
        public readonly string UrlLogin;

        /// <summary>
        /// Template for URL to acess workbooks list
        /// </summary>
        private readonly string _urlListWorkbooksForUserTemplate;
        private readonly string _urlViewThumbnailTemplate;
        private readonly string _urlViewDataTemplate;
        private readonly string _urlViewsListForSiteTemplate;
        private readonly string _urlWorkbookTemplate;
        private readonly string _urlListViewsForWorkbookTemplate;
        private readonly string _urlListWorkbookConnectionsTemplate;
        private readonly string _urlListDatasourcesTemplate;
        private readonly string _urlListFlowsTemplate;
        private readonly string _urlListFlowRunTemplate;
        private readonly string _urlListProjectsTemplate;
        private readonly string _urlListGroupsTemplate;
        private readonly string _urlListUsersTemplate;
        private readonly string _urlListUsersInGroupTemplate;
        private readonly string _urlListGroupsForUserTemplate;
        private readonly string _urlDownloadWorkbookTemplate;
        private readonly string _urlDownloadDatasourceTemplate;
        private readonly string _urlDownloadFlowTemplate;
        private readonly string _urlDatasourceConnectionsTemplate;
        private readonly string _urlSiteInfoTemplate;
        private readonly string _urlInitiateUploadTemplate;
        private readonly string _urlAppendUploadChunkTemplate;
        private readonly string _urlFinalizeUploadDatasourceTemplate;
        private readonly string _urlFinalizeUploadWorkbookTemplate;
        private readonly string _urlCreateProjectTemplate;
        private readonly string _urlCreateUserTemplate;
        private readonly string _urlDeleteWorkbookTagTemplate;
        private readonly string _urlDeleteDatasourceTagTemplate;
        private readonly string _urlUpdateUserTemplate;
        private readonly string _urlViewImageTemplate;
        private readonly string _urlAddToFavorites;
        private readonly string _urlDeleteWorkbookFromFavorites;
        private readonly string _urlDeleteViewFromFavorites;
        private readonly string _urlGetFavoritesForUser;
        private readonly string _urlDownloadViewPDF;
        private readonly string _urlDownloadWorkbookPDF;
        private readonly string _urlViewFilter;
        private readonly string _urlOrderFavoritesForUser;
        private readonly string _urlQueryDataSource;
        private readonly string _urlQueryDataSources;
        private readonly string _urlGetSchedule;
        private readonly string _UrlQuerySchedules;
        private readonly string _urlExtractRefreshTaskBySite;
        private readonly string _urlGraphqlMetadata;

        /// <summary>
        /// Server url with protocol
        /// </summary>
        public readonly string ServerUrlWithProtocol;

        /// <summary>
        /// String representation of Server Protocol
        /// </summary>
        public readonly ServerProtocol ServerProtocol;

        /// <summary>
        /// Part of the URL that designates the site id
        /// </summary>
        public readonly string SiteUrlSegement;

        /// <summary>
        /// Server Name
        /// </summary>
        public readonly string ServerName;

        /// <summary>
        /// Page Size when dealing with large result sets
        /// </summary>
        public readonly int PageSize;

        /// <summary>
        /// Size of chunks uploaded to Tableau server
        /// </summary>
        public const int UploadFileChunkSize = 8000000; //8MB

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serverName">Server IP, hostname or FQDN</param>
        /// <param name="siteName">Tableau Site Name</param>
        /// <param name="protocol">HTTP protocol</param>
        /// <param name="pageSize">Page size, defaults to 1000</param>
        /// <param name="serverVersion">Tableau Server version</param>
        public TableauServerUrls(ServerProtocol protocol, string serverName, string siteName, int pageSize = 1000, ServerVersion serverVersion = ServerVersion.Server9)
        {
            PageSize = 1000;
            ServerProtocol = protocol;

            PageSize = pageSize;
            var serverNameWithProtocol = (protocol == ServerProtocol.Http ? "http://" : "https://") + serverName;
            ServerVersion = serverVersion;
            SiteUrlSegement = siteName;
            ServerName = serverName;
            ServerUrlWithProtocol = serverNameWithProtocol;
            var apiVersion = ServerVersionLookup.APIVersion(serverVersion);
            UrlLogin = serverNameWithProtocol + $"/api/{apiVersion}/auth/signin";
            _urlListWorkbooksForUserTemplate = serverNameWithProtocol + $"/api/{apiVersion}/sites/%%iwsSiteId%%/users/%%iwsUserId%%/workbooks?pageSize=%%iwsPageSize%%&pageNumber=%%iwsPageNumber%%";
            _urlViewsListForSiteTemplate = serverNameWithProtocol + $"/api/{apiVersion}/sites/%%iwsSiteId%%/views?pageSize=%%iwsPageSize%%&pageNumber=%%iwsPageNumber%%";
            _urlViewThumbnailTemplate = serverNameWithProtocol + $"/api/{apiVersion}/sites/%%iwsSiteId%%/workbooks/%%iwsWorkbookId%%/views/%%iwsViewId%%/previewImage";
            _urlViewDataTemplate = serverNameWithProtocol + $"/api/{apiVersion}/sites/%%iwsSiteId%%/views/%%iwsViewId%%/data?%%iwsFilterValue%%";
            _urlListViewsForWorkbookTemplate = serverNameWithProtocol + $"/api/{apiVersion}/sites/%%iwsSiteId%%/workbooks/%%iwsWorkbookId%%/views";
            _urlListWorkbookConnectionsTemplate = serverNameWithProtocol + $"/api/{apiVersion}/sites/%%iwsSiteId%%/workbooks/%%iwsWorkbookId%%/connections";
            _urlWorkbookTemplate = serverNameWithProtocol + $"/api/{apiVersion}/sites/%%iwsSiteId%%/workbooks/%%iwsWorkbookId%%";
            _urlListDatasourcesTemplate = serverNameWithProtocol + $"/api/{apiVersion}/sites/%%iwsSiteId%%/datasources?pageSize=%%iwsPageSize%%&pageNumber=%%iwsPageNumber%%";
            _urlListFlowsTemplate = serverNameWithProtocol + $"/api/{apiVersion}/sites/%%iwsSiteId%%/flows?pageSize=%%iwsPageSize%%&pageNumber=%%iwsPageNumber%%";
            _urlListFlowRunTemplate = serverNameWithProtocol + $"/api/{apiVersion}/sites/%%iwsSiteId%%/flows/runs?filter=startedAt:gt:%%iwsStartedAt%%";
            _urlListProjectsTemplate = serverNameWithProtocol + $"/api/{apiVersion}/sites/%%iwsSiteId%%/projects?pageSize=%%iwsPageSize%%&pageNumber=%%iwsPageNumber%%";
            _urlListGroupsTemplate = serverNameWithProtocol + $"/api/{apiVersion}/sites/%%iwsSiteId%%/groups?pageSize=%%iwsPageSize%%&pageNumber=%%iwsPageNumber%%";
            _urlListGroupsForUserTemplate = serverNameWithProtocol + $"/api/{apiVersion}/sites/%%iwsSiteId%%/users/%%iwsUserId%%/groups";
            _urlListUsersTemplate = serverNameWithProtocol + $"/api/{apiVersion}/sites/%%iwsSiteId%%/users?pageSize=%%iwsPageSize%%&pageNumber=%%iwsPageNumber%%";
            _urlListUsersInGroupTemplate = serverNameWithProtocol + $"/api/{apiVersion}/sites/%%iwsSiteId%%/groups/%%iwsGroupId%%/users?pageSize=%%iwsPageSize%%&pageNumber=%%iwsPageNumber%%";
            _urlDownloadDatasourceTemplate = serverNameWithProtocol + $"/api/{apiVersion}/sites/%%iwsSiteId%%/datasources/%%iwsRepositoryId%%/content";
            _urlDatasourceConnectionsTemplate = serverNameWithProtocol + $"/api/{apiVersion}/sites/%%iwsSiteId%%/datasources/%%iwsRepositoryId%%/connections";
            _urlDownloadWorkbookTemplate = serverNameWithProtocol + $"/api/{apiVersion}/sites/%%iwsSiteId%%/workbooks/%%iwsRepositoryId%%/content";
            _urlDownloadFlowTemplate = serverNameWithProtocol + $"/api/{apiVersion}/sites/%%iwsSiteId%%/flows/%%iwsRepositoryId%%/content";
            _urlSiteInfoTemplate = serverNameWithProtocol + $"/api/{apiVersion}/sites/%%iwsSiteId%%";
            _urlInitiateUploadTemplate = serverNameWithProtocol + $"/api/{apiVersion}/sites/%%iwsSiteId%%/fileUploads";
            _urlAppendUploadChunkTemplate = serverNameWithProtocol + $"/api/{apiVersion}/sites/%%iwsSiteId%%/fileUploads/%%iwsUploadSession%%";
            _urlFinalizeUploadDatasourceTemplate = serverNameWithProtocol + $"/api/{apiVersion}/sites/%%iwsSiteId%%/datasources?uploadSessionId=%%iwsUploadSession%%&datasourceType=%%iwsDatasourceType%%&overwrite=true";
            _urlFinalizeUploadWorkbookTemplate = serverNameWithProtocol + $"/api/{apiVersion}/sites/%%iwsSiteId%%/workbooks?uploadSessionId=%%iwsUploadSession%%&workbookType=%%iwsWorkbookType%%&overwrite=true";
            _urlCreateProjectTemplate = serverNameWithProtocol + $"/api/{apiVersion}/sites/%%iwsSiteId%%/projects";
            _urlCreateUserTemplate = serverNameWithProtocol + $"/api/{apiVersion}/sites/%%iwsSiteId%%/users";
            _urlDeleteWorkbookTagTemplate = serverNameWithProtocol + $"/api/{apiVersion}/sites/%%iwsSiteId%%/workbooks/%%iwsWorkbookId%%/tags/%%iwsTagText%%";
            _urlDeleteDatasourceTagTemplate = serverNameWithProtocol + $"/api/{apiVersion}/sites/%%iwsSiteId%%/datasources/%%iwsDatasourceId%%/tags/%%iwsTagText%%";
            _urlUpdateUserTemplate = serverNameWithProtocol + $"/api/{apiVersion}/sites/%%iwsSiteId%%/users/iwsUserId";
            _urlViewImageTemplate = serverNameWithProtocol + $"/api/{apiVersion}/sites/%%iwsSiteId%%/views/%%iwsViewId%%/image?maxAge=%%iwsMaxAge%%&%%iwsFilterValue%%";
            _urlAddToFavorites = serverNameWithProtocol + $"/api/{apiVersion}/sites/%%iwsSiteId%%/favorites/%%iwsUserId%%";
            _urlDeleteWorkbookFromFavorites = serverNameWithProtocol + $"/api/{apiVersion}/sites/%%iwsSiteId%%/favorites/%%iwsUserId%%/workbooks/%%iwsWorkbookId%%";
            _urlDeleteViewFromFavorites = serverNameWithProtocol + $"/api/{apiVersion}/sites/%%iwsSiteId%%/favorites/%%iwsUserId%%/views/%%iwsViewId%%";
            _urlGetFavoritesForUser = serverNameWithProtocol + $"/api/{apiVersion}/sites/%%iwsSiteId%%/favorites/%%iwsUserId%%";
            _urlDownloadViewPDF = serverNameWithProtocol + $"/api/{apiVersion}/sites/%%iwsSiteId%%/views/%%iwsViewId%%/pdf?type=%%iwsPageType%%&orientation=%%iwsPageOrientation%%";
            _urlDownloadWorkbookPDF = serverNameWithProtocol + $"/api/{apiVersion}/sites/%%iwsSiteId%%/workbooks/%%iwsWorkbookId%%/pdf?type=%%iwsPageType%%&orientation=%%iwsPageOrientation%%";
            _urlViewFilter = $"vf_%%iwsFieldName%%=%%iwsFieldValue%%";
            _urlOrderFavoritesForUser = serverNameWithProtocol + $"/api/{apiVersion}/sites/%%iwsSiteId%%/orderFavorites/%%iwsUserId%%";
            _urlQueryDataSource = serverNameWithProtocol + $"/api/{apiVersion}/sites/%%iwsSiteId%%/datasources/%%iwsDatasourceId%%";
            _urlQueryDataSources = serverNameWithProtocol + $"/api/{apiVersion}/sites/%%iwsSiteId%%/datasources";
            _urlGetSchedule = serverNameWithProtocol + $"/api/{apiVersion}/schedules/%%iwsScheduleId%%";
            _UrlQuerySchedules = serverNameWithProtocol + $"/api/{apiVersion}/schedules";
            _urlExtractRefreshTaskBySite = serverNameWithProtocol + $"/api/{apiVersion}/sites/%%iwsSiteId%%/tasks/extractRefreshes";
            _urlGraphqlMetadata = serverNameWithProtocol + "/api/metadata/graphql";
        }

        private static ServerProtocol _GetProtocolFromUrl(string url)
        {
            const string protocolIndicator = "://";
            int idxProtocol = url.IndexOf(protocolIndicator, StringComparison.Ordinal);
            if (idxProtocol < 1)
            {
                throw new Exception("No protocol found in " + url);
            }

            string protocol = url.Substring(0, idxProtocol + protocolIndicator.Length);

            return protocol.ToLower().Equals("https") ? ServerProtocol.Https : ServerProtocol.Http;
        }

        /// <summary>
        /// Parse out the server-user and site name from the content URL
        /// </summary>
        /// <param name="userContentUrl">e.g. https://online.tableausoftware.com/t/tableausupport/workbooks </param>
        /// <param name="pageSize">Size of page to use when interacting with the Tableau Server</param>
        /// <returns></returns>
        public static TableauServerUrls FromContentUrl(string userContentUrl, int pageSize)
        {
            userContentUrl = userContentUrl.Trim();
            var foundProtocol = _GetProtocolFromUrl(userContentUrl);

            //Find where the server name ends
            string urlAfterProtocol = userContentUrl.Substring(userContentUrl.IndexOf("://", StringComparison.Ordinal) + 3);
            var urlParts = urlAfterProtocol.Split('/');
            string serverName = urlParts[0];

            string siteUrlSegment;
            ServerVersion serverVersion;
            //Check for the site specifier.  Infer the server version based on this URL
            if ((urlParts[1] == "#") && (urlParts[2] == "site"))
            {
                siteUrlSegment = urlParts[3];
                serverVersion = ServerVersion.Server9;
            }
            else if (urlParts[1] == "#")
            {
                siteUrlSegment = ""; //Default site
                serverVersion = ServerVersion.Server9;
            }
            else
            {
                throw new Exception("Could not infer version of Tableau Server.");
            }

            return new TableauServerUrls(foundProtocol, serverName, siteUrlSegment, pageSize, serverVersion);
        }

        /// <summary>
        /// The URL to get site info
        /// </summary>
        /// <param name="logInInfo">Tableau Sign In Information</param>
        /// <returns></returns>
        public string Url_SiteInfo(TableauServerSignIn logInInfo)
        {
            string workingText = _urlSiteInfoTemplate.Replace("%%iwsSiteId%%", logInInfo.SiteId);
            _ValidateTemplateReplaceComplete(workingText);

            return workingText;
        }

        /// <summary>
        /// The URL to start an upload
        /// </summary>
        /// <param name="logInInfo">Tableau Sign In Information</param>
        /// <returns></returns>
        public string Url_InitiateFileUpload(TableauServerSignIn logInInfo)
        {
            string workingText = _urlInitiateUploadTemplate.Replace("%%iwsSiteId%%", logInInfo.SiteId);
            _ValidateTemplateReplaceComplete(workingText);

            return workingText;
        }

        /// <summary>
        /// The URL to continue an upload
        /// </summary>
        /// <param name="logInInfo">Tableau Sign In Information</param>
        /// <param name="uploadSessionId">ID for the upload session</param>
        /// <returns></returns>
        public string Url_AppendFileUploadChunk(TableauServerSignIn logInInfo, string uploadSessionId)
        {
            string workingText = _urlAppendUploadChunkTemplate.Replace("%%iwsSiteId%%", logInInfo.SiteId);
            workingText = workingText.Replace("%%iwsUploadSession%%", uploadSessionId);
            _ValidateTemplateReplaceComplete(workingText);

            return workingText;
        }


        /// <summary>
        /// URL to finish publishing a datasource
        /// </summary>
        /// <param name="logInInfo">Tableau Sign In Information</param>
        /// <param name="uploadSessionId">ID for the upload session</param>
        /// <param name="datasourceType">Data Source Type: one of tds, tdsx, or tde</param>
        /// <returns></returns>
        public string Url_FinalizeDataSourcePublish(TableauServerSignIn logInInfo, string uploadSessionId, string datasourceType)
        {

            string workingText = _urlFinalizeUploadDatasourceTemplate.Replace("%%iwsSiteId%%", logInInfo.SiteId);
            workingText = workingText.Replace("%%iwsUploadSession%%", uploadSessionId);
            workingText = workingText.Replace("%%iwsDatasourceType%%", datasourceType);
            _ValidateTemplateReplaceComplete(workingText);

            return workingText;
        }

        /// <summary>
        /// URL to finish publishing a workbook
        /// </summary>
        /// <param name="logInInfo">Tableau Sign In Information</param>
        /// <param name="uploadSessionId">ID for the upload session</param>
        /// <param name="workbookType">Workbook Type: one of twb or twbx</param>
        /// <returns></returns>
        public string Url_FinalizeWorkbookPublish(TableauServerSignIn logInInfo, string uploadSessionId, string workbookType)
        {

            string workingText = _urlFinalizeUploadWorkbookTemplate.Replace("%%iwsSiteId%%", logInInfo.SiteId);
            workingText = workingText.Replace("%%iwsUploadSession%%", uploadSessionId);
            workingText = workingText.Replace("%%iwsWorkbookType%%", workbookType);
            _ValidateTemplateReplaceComplete(workingText);

            return workingText;
        }

        /// <summary>
        /// URL for the View thumbnail.
        /// </summary>
        /// <param name="workbookId">Workbook ID</param>
        /// <param name="viewId">View ID</param>
        /// <param name="logInInfo">Tableau Sign In Information</param>
        /// <returns></returns>
        public string Url_ViewThumbnail(string workbookId, string viewId, TableauServerSignIn logInInfo)
        {
            var workingText = _urlViewThumbnailTemplate.Replace("%%iwsSiteId%%", logInInfo.SiteId);
            workingText = workingText.Replace("%%iwsWorkbookId%%", workbookId);
            workingText = workingText.Replace("%%iwsViewId%%", viewId);
            _ValidateTemplateReplaceComplete(workingText);
            return workingText;
        }

        /// <summary>
        /// URL for the View Image.
        /// </summary>
        /// <param name="workbookId">Workbook ID</param>
        /// <param name="viewId">View ID</param>
        /// <param name="logInInfo">Tableau Sign In Information</param>
        /// <returns></returns>
        public string Url_ViewImage(string workbookId, string viewId, string filterName, string filterValue, int maxAge, TableauServerSignIn logInInfo)
        {
            var filterText = string.Empty;
            filterText = _urlViewFilter.Replace("%%iwsFieldName%%", filterName);
            filterText = filterText.Replace("%%iwsFieldValue%%", filterValue);
            var workingText = _urlViewImageTemplate.Replace("%%iwsSiteId%%", logInInfo.SiteId);
            workingText = workingText.Replace("%%iwsViewId%%", viewId);
            workingText = workingText.Replace("%%iwsMaxAge%%", maxAge.ToString());
            workingText = workingText.Replace("%%iwsFilterValue%%", filterText);
            _ValidateTemplateReplaceComplete(workingText);
            return workingText;
        }

        /// <summary>
        /// URL for the View Data.
        /// </summary>
        /// <param name="viewId">View ID</param>
        /// <param name="logInInfo">Tableau Sign In Information</param>
        /// <returns></returns>
        public string Url_ViewData(string viewId, string filterName, string filterValue, TableauServerSignIn logInInfo)
        {
            var filterText = string.Empty;
            filterText = _urlViewFilter.Replace("%%iwsFieldName%%", filterName);
            filterText = filterText.Replace("%%iwsFieldValue%%", filterValue);
            var workingText = _urlViewDataTemplate.Replace("%%iwsSiteId%%", logInInfo.SiteId);
            workingText = workingText.Replace("%%iwsViewId%%", viewId);
            workingText = workingText.Replace("%%iwsFilterValue%%", filterText);
            _ValidateTemplateReplaceComplete(workingText);
            return workingText;
        }

        /// <summary>
        /// URL for the Views list
        /// </summary>
        /// <param name="workbookId">Workbook ID</param>
        /// <param name="logInInfo">Tableau Sign In Information</param>
        /// <returns></returns>
        public string Url_ViewsListForWorkbook(string workbookId, TableauServerSignIn logInInfo)
        {
            var workingText = _urlListViewsForWorkbookTemplate;
            workingText = workingText.Replace("%%iwsSiteId%%", logInInfo.SiteId);
            workingText = workingText.Replace("%%iwsWorkbookId%%", workbookId);
            _ValidateTemplateReplaceComplete(workingText);

            return workingText;
        }


        /// <summary>
        /// URL for the Views list
        /// </summary>
        /// <param name="logInInfo">Tableau Sign In Information</param>
        /// <param name="pageSize">Page size to use when retrieving results from Tableau server</param>
        /// <param name="pageNumber">Which page of the results to return. Defaults to 1.</param>
        /// <returns></returns>
        public string Url_ViewsListForSite(TableauServerSignIn logInInfo, int pageSize, int pageNumber = 1)
        {
            string workingText = _urlViewsListForSiteTemplate;
            workingText = workingText.Replace("%%iwsSiteId%%", logInInfo.SiteId);
            workingText = workingText.Replace("%%iwsPageSize%%", pageSize.ToString());
            workingText = workingText.Replace("%%iwsPageNumber%%", pageNumber.ToString());
            _ValidateTemplateReplaceComplete(workingText);

            return workingText;
        }

        /// <summary>
        /// URL for the Workbooks list
        /// </summary>
        /// <param name="logInInfo">Tableau Sign In Information</param>
        /// <param name="userId">User ID who's workbooks we should retrieve</param>
        /// <param name="pageSize">Size of result set to retrieve from Tableau Server</param>
        /// <param name="pageNumber">Which page of the results to return. Defaults to 1.</param>
        /// <returns></returns>
        public string Url_WorkbooksListForUser(TableauServerSignIn logInInfo, string userId, int pageSize, int pageNumber = 1)
        {
            string workingText = _urlListWorkbooksForUserTemplate;
            workingText = workingText.Replace("%%iwsSiteId%%", logInInfo.SiteId);
            workingText = workingText.Replace("%%iwsUserId%%", userId);
            workingText = workingText.Replace("%%iwsPageSize%%", pageSize.ToString());
            workingText = workingText.Replace("%%iwsPageNumber%%", pageNumber.ToString());
            _ValidateTemplateReplaceComplete(workingText);

            return workingText;
        }

        /// <summary>
        /// URL for the Workbook's data source connections list
        /// </summary>
        /// <param name="logInInfo">Tableau Sign In Information</param>
        /// <param name="workbookId">Workbook ID</param>
        /// <returns></returns>
        public string Url_Workbook(TableauServerSignIn logInInfo, string workbookId)
        {
            string workingText = _urlWorkbookTemplate;
            workingText = workingText.Replace("%%iwsSiteId%%", logInInfo.SiteId);
            workingText = workingText.Replace("%%iwsWorkbookId%%", workbookId);
            _ValidateTemplateReplaceComplete(workingText);

            return workingText;
        }

        /// <summary>
        /// URL for the Workbook's data source connections list
        /// </summary>
        /// <param name="logInInfo">Tableau Sign In Information</param>
        /// <param name="workbookId">Workbook ID</param>
        /// <returns></returns>
        public string Url_WorkbookConnectionsList(TableauServerSignIn logInInfo, string workbookId)
        {
            string workingText = _urlListWorkbookConnectionsTemplate;
            workingText = workingText.Replace("%%iwsSiteId%%", logInInfo.SiteId);
            workingText = workingText.Replace("%%iwsWorkbookId%%", workbookId);
            _ValidateTemplateReplaceComplete(workingText);

            return workingText;
        }

        /// <summary>
        /// URL for the Workbook's data source connections list
        /// </summary>
        /// <param name="logInInfo">Tableau Sign In Information</param>
        /// <param name="datasourceId">Datasource ID</param>
        /// <returns></returns>
        public string Url_DatasourceConnectionsList(TableauServerSignIn logInInfo, string datasourceId)
        {
            string workingText = _urlDatasourceConnectionsTemplate;
            workingText = workingText.Replace("%%iwsSiteId%%", logInInfo.SiteId);
            workingText = workingText.Replace("%%iwsRepositoryId%%", datasourceId);
            _ValidateTemplateReplaceComplete(workingText);

            return workingText;
        }

        /// <summary>
        /// URL for the Datasources list
        /// </summary>
        /// <param name="logInInfo">Tableau Sign In Information</param>
        /// <param name="pageSize">Page size to use when retrieving results from Tableau server</param>
        /// <param name="pageNumber">Which page of the results to return. Defaults to 1.</param>
        /// <returns></returns>
        public string Url_DatasourcesList(TableauServerSignIn logInInfo, int pageSize, int pageNumber = 1)
        {
            string workingText = _urlListDatasourcesTemplate;
            workingText = workingText.Replace("%%iwsSiteId%%", logInInfo.SiteId);
            workingText = workingText.Replace("%%iwsPageSize%%", pageSize.ToString());
            workingText = workingText.Replace("%%iwsPageNumber%%", pageNumber.ToString());
            _ValidateTemplateReplaceComplete(workingText);

            return workingText;
        }

        public string Url_QueryDataSource(TableauServerSignIn logInInfo, string id)
        {
            string workingText = _urlQueryDataSource;
            workingText = workingText.Replace("%%iwsSiteId%%", logInInfo.SiteId);
            workingText = workingText.Replace("%%iwsDatasourceId%%", id);
            _ValidateTemplateReplaceComplete(workingText);
          
            return workingText;
        }

        public string Url_QueryDataSources(TableauServerSignIn logInInfo) {
            string workingText = _urlQueryDataSources;
            workingText = workingText.Replace("%%iwsSiteId%%", logInInfo.SiteId);
            _ValidateTemplateReplaceComplete(workingText);
            return workingText;
        }


        /// <summary>
        /// URL for the Flows list
        /// </summary>
        /// <param name="logInInfo">Tableau Sign In Information</param>
        /// <param name="pageSize">Page size to use when retrieving results from Tableau server</param>
        /// <param name="pageNumber">Which page of the results to return. Defaults to 1.</param>
        /// <returns></returns>
        public string Url_FlowsList(TableauServerSignIn logInInfo, int pageSize, int pageNumber = 1)
        {
            string workingText = _urlListFlowsTemplate;
            workingText = workingText.Replace("%%iwsSiteId%%", logInInfo.SiteId);
            workingText = workingText.Replace("%%iwsPageSize%%", pageSize.ToString());
            workingText = workingText.Replace("%%iwsPageNumber%%", pageNumber.ToString());
            _ValidateTemplateReplaceComplete(workingText);

            return workingText;
        }

        public string Url_FlowRunsList(TableauServerSignIn logInInfo,DateTime startedAt) {
            string workingText = _urlListFlowRunTemplate;
            workingText = workingText.Replace("%%iwsSiteId%%", logInInfo.SiteId);
            workingText = workingText.Replace("%%iwsStartedAt%%", startedAt.ToString("o"));
            return workingText;
        }

        /// <summary>
        /// URL for creating a project
        /// </summary>
        /// <param name="logInInfo">Tableau Sign In Information</param>
        /// <returns></returns>
        public string Url_CreateProject(TableauServerSignIn logInInfo)
        {
            string workingText = _urlCreateProjectTemplate;
            workingText = workingText.Replace("%%iwsSiteId%%", logInInfo.SiteId);
            _ValidateTemplateReplaceComplete(workingText);

            return workingText;
        }

        /// <summary>
        /// URL for creating a user
        /// </summary>
        /// <param name="logInInfo">Tableau Sign In Information</param>
        /// <returns></returns>
        public string Url_CreateUser(TableauServerSignIn logInInfo)
        {
            string workingText = _urlCreateUserTemplate;
            workingText = workingText.Replace("%%iwsSiteId%%", logInInfo.SiteId);
            _ValidateTemplateReplaceComplete(workingText);

            return workingText;
        }

        /// <summary>
        /// URL for deleting a tag from a workbook
        /// </summary>
        /// <param name="logInInfo">Tableau Sign In Information</param>
        /// <param name="workbookId">Workbook ID</param>
        /// <param name="tagText">Tag we want to delete</param>
        /// <returns></returns>
        public string Url_DeleteWorkbookTag(TableauServerSignIn logInInfo, string workbookId, string tagText)
        {
            string workingText = _urlDeleteWorkbookTagTemplate;
            workingText = workingText.Replace("%%iwsSiteId%%", logInInfo.SiteId);
            workingText = workingText.Replace("%%iwsWorkbookId%%", workbookId);
            workingText = workingText.Replace("%%iwsTagText%%", tagText);
            _ValidateTemplateReplaceComplete(workingText);

            return workingText;
        }

        /// <summary>
        /// URL for deleting a tag from a datasource
        /// </summary>
        /// <param name="logInInfo">Tableau Sign In Information</param>
        /// <param name="datasourceId">Data Source ID</param>
        /// <param name="tagText">Tag we want to delete</param>
        /// <returns></returns>
        public string Url_DeleteDatasourceTag(TableauServerSignIn logInInfo, string datasourceId, string tagText)
        {
            string workingText = _urlDeleteDatasourceTagTemplate;
            workingText = workingText.Replace("%%iwsSiteId%%", logInInfo.SiteId);
            workingText = workingText.Replace("%%iwsDatasourceId%%", datasourceId);
            workingText = workingText.Replace("%%iwsTagText%%", tagText);
            _ValidateTemplateReplaceComplete(workingText);

            return workingText;
        }


        /// <summary>
        /// URL for the Projects list
        /// </summary>
        /// <param name="logInInfo">Tableau Sign In Information</param>
        /// <param name="pageSize">Page size to use when retrieving results from Tableau server</param>
        /// <param name="pageNumber">Which page of the results to return. Defaults to 1.</param>
        /// <returns></returns>
        public string Url_ProjectsList(TableauServerSignIn logInInfo, int pageSize, int pageNumber = 1)
        {
            string workingText = _urlListProjectsTemplate;
            workingText = workingText.Replace("%%iwsSiteId%%", logInInfo.SiteId);
            workingText = workingText.Replace("%%iwsPageSize%%", pageSize.ToString());
            workingText = workingText.Replace("%%iwsPageNumber%%", pageNumber.ToString());
            _ValidateTemplateReplaceComplete(workingText);

            return workingText;
        }

        /// <summary>
        /// URL for the Groups list
        /// </summary>
        /// <param name="logInInfo">Tableau Sign In Information</param>
        /// <param name="pageSize">Page size to use when retrieving results from Tableau server</param>
        /// <param name="pageNumber">Which page of the results to return. Defaults to 1.</param>
        /// <returns></returns>
        public string Url_GroupsList(TableauServerSignIn logInInfo, int pageSize, int pageNumber = 1)
        {
            string workingText = _urlListGroupsTemplate;
            workingText = workingText.Replace("%%iwsSiteId%%", logInInfo.SiteId);
            workingText = workingText.Replace("%%iwsPageSize%%", pageSize.ToString());
            workingText = workingText.Replace("%%iwsPageNumber%%", pageNumber.ToString());
            _ValidateTemplateReplaceComplete(workingText);

            return workingText;
        }


        /// <summary>
        /// URL for the Users list
        /// </summary>
        /// <param name="logInInfo">Tableau Sign In Information</param>
        /// <param name="pageSize">Page size to use when retrieving results from Tableau server</param>
        /// <param name="pageNumber">Which page of the results to return. Defaults to 1.</param>/// <returns></returns>
        public string Url_UsersList(TableauServerSignIn logInInfo, int pageSize, int pageNumber = 1)
        {
            string workingText = _urlListUsersTemplate.Replace("%%iwsSiteId%%", logInInfo.SiteId);
            workingText = workingText.Replace("%%iwsPageSize%%", pageSize.ToString());
            workingText = workingText.Replace("%%iwsPageNumber%%", pageNumber.ToString());
            _ValidateTemplateReplaceComplete(workingText);

            return workingText;
        }

        /// <summary>
        /// URL to get the list of Users in a Group
        /// </summary>
        /// <param name="logInInfo">Tableau Sign In Information</param>
        /// <param name="groupId">Group ID</param>
        /// <param name="pageSize">Page size to use when retrieving results from Tableau server</param>
        /// <param name="pageNumber">Which page of the results to return. Defaults to 1.</param>/// <returns></returns>
        public string Url_UsersListInGroup(TableauServerSignIn logInInfo, string groupId, int pageSize, int pageNumber = 1)
        {
            string workingText = _urlListUsersInGroupTemplate.Replace("%%iwsSiteId%%", logInInfo.SiteId);
            workingText = workingText.Replace("%%iwsGroupId%%", groupId);
            workingText = workingText.Replace("%%iwsPageSize%%", pageSize.ToString());
            workingText = workingText.Replace("%%iwsPageNumber%%", pageNumber.ToString());
            _ValidateTemplateReplaceComplete(workingText);

            return workingText;
        }

        /// <summary>
        /// URL for a User's list of groups
        /// </summary>
        /// <param name="logInInfo">Tableau Sign In Information</param>
        /// <param name="pageSize">Page size to use when retrieving results from Tableau server</param>
        /// <param name="pageNumber">Which page of the results to return. Defaults to 1.</param>
        /// <param name="userId">User ID who's groups we should retrieve</param>
        /// <returns></returns>
        public string Url_GroupsListForUser(TableauServerSignIn logInInfo, string userId, int pageSize, int pageNumber = 1)
        {
            string workingText = _urlListGroupsForUserTemplate;
            workingText = workingText.Replace("%%iwsSiteId%%", logInInfo.SiteId);
            workingText = workingText.Replace("%%iwsUserId%%", userId);
            workingText = workingText.Replace("%%iwsPageSize%%", pageSize.ToString());
            workingText = workingText.Replace("%%iwsPageNumber%%", pageNumber.ToString());
            _ValidateTemplateReplaceComplete(workingText);

            return workingText;
        }

        /// <summary>
        /// URL to get update a user
        /// </summary>
        /// <param name="logInInfo">Tableau Sign In Information</param>
        /// <param name="userId">User ID</param>
        public string Url_UpdateUser(TableauServerSignIn logInInfo, string userId)
        {
            string workingText = _urlUpdateUserTemplate.Replace("%%iwsSiteId%%", logInInfo.SiteId);
            workingText = workingText.Replace("iwsUserId", userId);
            _ValidateTemplateReplaceComplete(workingText);

            return workingText;
        }

        /// <summary>
        /// URL to download a workbook
        /// </summary>
        /// <param name="logInInfo">Tableau Sign In Information</param>
        /// <param name="workbook">Tableau Workbook</param>
        /// <returns></returns>
        public string Url_WorkbookDownload(TableauServerSignIn logInInfo, SiteWorkbook workbook)
        {
            string workingText = _urlDownloadWorkbookTemplate;
            workingText = workingText.Replace("%%iwsSiteId%%", logInInfo.SiteId);
            workingText = workingText.Replace("%%iwsRepositoryId%%", workbook.Id);

            _ValidateTemplateReplaceComplete(workingText);
            return workingText;
        }

        /// <summary>
        /// URL to download a datasource
        /// </summary>
        /// <param name="logInInfo">Tableau Sign In Information</param>
        /// <param name="datasource">Tableau Data Source</param>
        /// <returns></returns>
        public string Url_DatasourceDownload(TableauServerSignIn logInInfo, SiteDatasource datasource)
        {
            string workingText = _urlDownloadDatasourceTemplate;
            workingText = workingText.Replace("%%iwsSiteId%%", logInInfo.SiteId);
            workingText = workingText.Replace("%%iwsRepositoryId%%", datasource.Id);

            _ValidateTemplateReplaceComplete(workingText);
            return workingText;
        }

        /// <summary>
        /// URL to download a Flow
        /// </summary>
        /// <param name="logInInfo">Tableau Sign In Information</param>
        /// <param name="flow">Tableau Data Source</param>
        /// <returns></returns>
        public string Url_FlowDownload(TableauServerSignIn logInInfo, SiteFlow flow)
        {
            string workingText = _urlDownloadFlowTemplate;
            workingText = workingText.Replace("%%iwsSiteId%%", logInInfo.SiteId);
            workingText = workingText.Replace("%%iwsRepositoryId%%", flow.Id);

            _ValidateTemplateReplaceComplete(workingText);
            return workingText;
        }

        /// <summary>
        /// Adds the specified workbook to a user's favorites.
        /// </summary>
        /// <param name="logInInfo">Tableau Sign In Information</param>
        /// <returns></returns>
        public string Url_AddToFavorites(string userId, TableauServerSignIn logInInfo)
        {
            string workingText = _urlAddToFavorites;
            workingText = workingText.Replace("%%iwsSiteId%%", logInInfo.SiteId);
            workingText = workingText.Replace("%%iwsUserId%%", userId);
            _ValidateTemplateReplaceComplete(workingText);
            return workingText;
        }

        /// <summary>
        /// Deletes a workbook from a user's favorites. If the specified workbook is not a favorite of the specified user, this call has no effect.
        /// </summary>
        /// <param name="workbookId">The ID of the workbook to remove from the user's favorites.</param>
        /// <param name="logInInfo">Tableau Sign In Information</param>
        /// <returns></returns>
        public string Url_DeleteWorkbookFromFavorites(string workbookId, string userId, TableauServerSignIn logInInfo)
        {
            string workingText = _urlDeleteWorkbookFromFavorites;
            workingText = workingText.Replace("%%iwsSiteId%%", logInInfo.SiteId);
            workingText = workingText.Replace("%%iwsUserId%%", userId);
            workingText = workingText.Replace("%%iwsWorkbookId%%", workbookId);
            _ValidateTemplateReplaceComplete(workingText);
            return workingText;
        }


        /// <summary>
        /// Deletes a view from a user's favorites. If the specified view is not a favorite of the specified user, this call has no effect.
        /// </summary>
        /// <param name="viewId">The ID of the workbook to remove from the user's favorites.</param>
        /// <param name="logInInfo">Tableau Sign In Information</param>
        /// <returns></returns>
        public string Url_DeleteViewFromFavorites(string viewId, string userId, TableauServerSignIn logInInfo)
        {
            string workingText = _urlDeleteViewFromFavorites;
            workingText = workingText.Replace("%%iwsSiteId%%", logInInfo.SiteId);
            workingText = workingText.Replace("%%iwsUserId%%", userId);
            workingText = workingText.Replace("%%iwsViewId%%", viewId);
            _ValidateTemplateReplaceComplete(workingText);
            return workingText;
        }


        /// <summary>
        /// Returns a list of favorite projects, data sources, views, workbooks, and flows for a user.
        /// </summary>
        /// <param name="logInInfo">Tableau Sign In Information</param>
        /// <returns></returns>
        public string Url_GetFavoritesForUser(string userId, TableauServerSignIn logInInfo)
        {
            string workingText = _urlGetFavoritesForUser;
            workingText = workingText.Replace("%%iwsSiteId%%", logInInfo.SiteId);
            workingText = workingText.Replace("%%iwsUserId%%", userId);
            _ValidateTemplateReplaceComplete(workingText);
            return workingText;
        }

        /// <summary>
        /// URL to download a PDF for a view
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <param name="workbookId"></param>
        /// <param name="pageType"></param>
        /// <param name="pageOrientation"></param>
        /// <returns></returns>
        public string Url_DownloadViewPDF(TableauServerSignIn loginInfo, string workbookId, PageType pageType, PageOrientation pageOrientation)
        {
            string workingText = _urlDownloadViewPDF;
            workingText = workingText.Replace("%%iwsSiteId%%", loginInfo.SiteId);
            workingText = workingText.Replace("%%iwsViewId%%", workbookId);
            workingText = workingText.Replace("%%iwsPageType%%", pageType.ToString());
            workingText = workingText.Replace("%%iwsPageOrientation%%", pageOrientation.ToString());
            _ValidateTemplateReplaceComplete(workingText);
            return workingText;
        }

        /// <summary>
        /// URL to download a PDF for a workbook
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <param name="workbookId"></param>
        /// <param name="pageType"></param>
        /// <param name="pageOrientation"></param>
        /// <returns></returns>
        public string Url_DownloadWorkbookPDF(TableauServerSignIn loginInfo, string workbookId, PageType pageType, PageOrientation pageOrientation)
        {
            string workingText = _urlDownloadWorkbookPDF;
            workingText = workingText.Replace("%%iwsSiteId%%", loginInfo.SiteId);
            workingText = workingText.Replace("%%iwsWorkbookId%%", workbookId);
            workingText = workingText.Replace("%%iwsPageType%%", pageType.ToString());
            workingText = workingText.Replace("%%iwsPageOrientation%%", pageOrientation.ToString());
            _ValidateTemplateReplaceComplete(workingText);
            return workingText;
        }

        public string Url_GetSchedule(string scheduleId) {
            string workingText = _urlGetSchedule;
            workingText = workingText.Replace("%%iwsScheduleId%%",scheduleId);
            _ValidateTemplateReplaceComplete(workingText);
            return workingText;
        }

        public string Url__QuerySchedules() {
            string workingText = _UrlQuerySchedules;
            _ValidateTemplateReplaceComplete(workingText);
            return workingText;
        }

        /// Submits a change request for the User's Favorite Workbooks and Views sort order
        /// </summary>
        /// <param name="logInInfo">Tableau Sign In Information</param>
        /// <returns></returns>
        public string Url_OrderFavoritesForUser(string userId, TableauServerSignIn logInInfo)
        {
            string workingText = _urlOrderFavoritesForUser;
            workingText = workingText.Replace("%%iwsSiteId%%", logInInfo.SiteId);
            workingText = workingText.Replace("%%iwsUserId%%", userId);
            _ValidateTemplateReplaceComplete(workingText);
            return workingText;
        }

        public string Url_ExtractRefreshTaskBySiteId(TableauServerSignIn logInInfo) {
            string workingText = _urlExtractRefreshTaskBySite;
            workingText = workingText.Replace("%%iwsSiteId%%",logInInfo.SiteId);
            _ValidateTemplateReplaceComplete(workingText);
            return workingText;
        }

        public string Url_QueryGraphqlMetadata()
        {
            return _urlGraphqlMetadata;
        }


        string ITableauServerSiteInfo.ServerName => ServerName;

        string ITableauServerSiteInfo.SiteId => SiteUrlSegement;

        string ITableauServerSiteInfo.ServerNameWithProtocol => ServerUrlWithProtocol;

        #region Private Methods

        /// <summary>
        /// Returns true if the all required parameters are filled in; false otherwise.
        /// </summary>
        /// <param name="str">URL string</param>
        /// <returns></returns>
        private static void _ValidateTemplateReplaceComplete(string str)
        {
            if (str.Contains("%%iws"))
            {
                throw new ApplicationException($"Template replacement was incomplete: {str}");
            }
        }

        #endregion

    }
}
