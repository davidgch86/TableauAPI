﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using TableauAPI.FilesLogging;

namespace TableauAPI.ServerData
{
    /// <summary>
    /// Information about a Datasource in a Server's site
    /// </summary>
    public class SiteDatasource : SiteDocumentBase, IEditDataConnectionsSet
    {
        /// <summary>
        /// The underlying source of the data (e.g. SQL Server, MySQL, Excel, CSV)
        /// </summary>
        public readonly string Type;

        private List<SiteConnection> _dataConnections;
        /// <summary>
        /// Return a set of data connections (if they were downloaded)
        /// </summary>
        public ReadOnlyCollection<SiteConnection> DataConnections
        {
            get
            {
                var dataConnections = _dataConnections;
                return dataConnections?.AsReadOnly();
            }
        }

        /// <summary>
        /// Creates an instance of a Datasource from XML returned by the Tableau server
        /// </summary>
        /// <param name="datasourceNode"></param>
        public SiteDatasource(XmlNode datasourceNode) : base(datasourceNode)
        {
            if (datasourceNode.Name.ToLower() != "datasource")
            {
                AppDiagnostics.Assert(false, "Not a datasource");
                throw new Exception("Unexpected content - not datasource");
            }
            //Get the underlying data source type
            Type = datasourceNode.Attributes?["type"].Value;

        }

        /// <summary>
        /// Datasource description
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Datasource: " + Name + "/" + Type + "/" + Id;
        }

        /// <summary>
        /// Interface for inserting the set of data connections associated with this content
        /// </summary>
        /// <param name="connections"></param>
        void IEditDataConnectionsSet.SetDataConnections(IEnumerable<SiteConnection> connections)
        {
            _dataConnections = connections == null ? null : new List<SiteConnection>(connections);
        }
    }
}
