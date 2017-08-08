
using Antlr.Runtime.Tree;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(App.Web.MVCGridConfig), "RegisterGrids")]

namespace App.Web
{
    using System.Linq;
    using MVCGrid.Models;
    using MVCGrid.Web;
    using Entity.Models;
    using Context;
    using Helper;

    public static class MVCGridConfig 
    {
        public static void RegisterGrids()
        {
            var defaults = new GridDefaults()
            {
                Paging = true,
                ItemsPerPage = 10,
                Sorting = true,
                NoResultsMessage = "Sorry, no results were found."
            };

            // Branch Table
            MVCGridDefinitionTable.Add("branchTable", new MVCGridBuilder<BranchInfo>(defaults)
                .WithAuthorizationType(AuthorizationType.Authorized)
                .AddColumns(cols =>
                {
                    cols.Add("BranchId").WithHeaderText("Branch Id").WithValueExpression(p => p.BranchId).WithSorting(true);
                    cols.Add("BranchName").WithHeaderText("Branch Name").WithValueExpression(p => p.BranchName).WithSorting(true);
                    cols.Add("BranchCode").WithHeaderText("Branch Code").WithValueExpression(p => p.BranchCode).WithSorting(true);
                    cols.Add("Status").WithHeaderText("Status").WithValueExpression(p => p.Status > 0?"Active":"Inactive");
                    cols.Add("DelStatus").WithHeaderText("Delete Status").WithValueExpression(p => p.DelStatus.ToString());
                    cols.Add("EntryDate").WithHeaderText("Entry Date").WithValueExpression(p => p.EntryDate.ToString()).WithSorting(true);
                    cols.Add("EntryBy").WithHeaderText("Entry By").WithValueExpression(p => p.EntryBy.GetUserName()).WithSorting(true);
                    cols.Add("ViewLink").WithSorting(false).WithHeaderText("Action").WithHtmlEncoding(false)
                        .WithValueExpression(p=>p.Id.ToString()).WithValueTemplate(
                        "<a class='btn btn-sm btn-outline-primary' href='/Branch/Edit/{Value}'>Edit</a> " +
                        //"<a class='btn btn-sm btn-outline-info' href='/Branch/Details/{Value}'>Details</a> " +
                        "<button class='btn btn-sm btn-outline-danger delete' data-id='{Value}'>Delete</button>"
                     );
                })
                .WithSorting(true, "BranchId")
                .WithPaging(true,10,true,100)
                .WithAdditionalQueryOptionNames("Search")
                .WithAdditionalSetting("RenderLoadingDiv", false)
                .WithRetrieveDataMethod((context) =>
                {
                    var options = context.QueryOptions;
                    var result = new QueryResult<BranchInfo>();
                    using (var db = new CrmDbContext())
                    {
                        var query = db.BranchInfos.AsQueryable();
                        
                        var globalSearch = options.GetAdditionalQueryOptionString("Search");
                        if (!string.IsNullOrWhiteSpace(globalSearch))
                        {
                            query = query.Where(x =>
                                    x.BranchId.Contains(globalSearch)||
                                    x.BranchName.Contains(globalSearch) ||
                                    x.BranchCode.Contains(globalSearch) ||
                                    x.EntryDate.ToString().Contains(globalSearch)
                            );
                        }

                        if (!string.IsNullOrWhiteSpace(options.SortColumnName))
                        {
                            var direction = options.SortDirection;
                            switch (options.SortColumnName.ToLower())
                            {
                                case "branchid":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.BranchId) : query.OrderBy(p => p.BranchId);
                                    break;
                                case "branchname":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.BranchId) : query.OrderBy(p => p.BranchId);
                                    break;
                                case "branchcode":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.BranchCode) : query.OrderBy(p => p.BranchCode);
                                    break;
                                //case "status":
                                //    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.Status) : query.OrderBy(p => p.Status);
                                //    break;
                                case "entrydate":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.EntryDate) : query.OrderBy(p => p.EntryDate);
                                    break;
                                case "entryby":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.EntryBy) : query.OrderBy(p => p.EntryBy);
                                    break;
                            }
                        }
                        if (options.GetLimitOffset().HasValue && query.Count() != 0)
                        {
                            query = query.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value);
                        }
                        result.Items = query.ToList();
                        result.TotalRecords = query.Count();
                    }
                    return result;
                })
            );

            //Designation Table
            MVCGridDefinitionTable.Add("designationTable", new MVCGridBuilder<EmployeeDesignation>(defaults)
                .WithAuthorizationType(AuthorizationType.Authorized)
                .AddColumns(cols =>
                {
                    cols.Add("DesignationId").WithHeaderText("Designation Id").WithValueExpression(p => p.DesignationId).WithSorting(true);
                    cols.Add("DesignationTitleBn").WithHeaderText("Designation Name (BN)").WithValueExpression(p => p.DesignationTitleBn);
                    cols.Add("DesignationTitleEn").WithHeaderText("Designation Name (EN)").WithValueExpression(p => p.DesignationTitleEn).WithSorting(true);
                    //cols.Add("DesignationDepertment").WithHeaderText("Designation Depertment").WithValueExpression(p => p.DesignationDepertment).WithSorting(true);
                    cols.Add("Status").WithHeaderText("Status").WithValueExpression(p => p.Status > 0 ? "Active" : "Inactive");
                    //cols.Add("DelStatus").WithHeaderText("Delete Status").WithValueExpression(p => p.DelStatus.ToString());
                    //cols.Add("EntryDate").WithHeaderText("Entry Date").WithValueExpression(p => p.EntryDate.ToString()).WithSorting(true);
                    //cols.Add("EntryBy").WithHeaderText("Entry By").WithValueExpression(p => p.EntryBy.GetUserName()).WithSorting(true);
                    cols.Add("ViewLink").WithSorting(false).WithHeaderText("Action").WithHtmlEncoding(false)
                        .WithValueExpression(p => p.Id.ToString()).WithValueTemplate(
                        "<a class='btn btn-sm btn-outline-primary' href='/Designations/Edit/{Value}'>Edit</a> " +
                        "<button class='btn btn-sm btn-outline-danger delete' data-id='{Value}'>Delete</button>"
                     );
                })
                .WithSorting(true, "DesignationId")
                .WithPaging(true, 10, true, 100)
                .WithAdditionalQueryOptionNames("Search")
                .WithAdditionalSetting("RenderLoadingDiv", false)
                .WithRetrieveDataMethod((context) =>
                {
                    var options = context.QueryOptions;
                    var result = new QueryResult<EmployeeDesignation>();
                    using (var db = new CrmDbContext())
                    {
                        var query = db.EmployeeDesignations.AsQueryable();

                        var globalSearch = options.GetAdditionalQueryOptionString("Search");
                        if (!string.IsNullOrWhiteSpace(globalSearch))
                        {
                            query = query.Where(x =>
                                    x.DesignationId.Contains(globalSearch) 
                                    || x.DesignationTitleEn.Contains(globalSearch) 
                                    //|| x.DesignationDepertment.Contains(globalSearch) 
                                    //|| x.EntryDate.ToString().Contains(globalSearch)
                            );
                        }

                        if (!string.IsNullOrWhiteSpace(options.SortColumnName))
                        {
                            var direction = options.SortDirection;
                            switch (options.SortColumnName.ToLower())
                            {
                                case "designationid":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.DesignationId) : query.OrderBy(p => p.DesignationId);
                                    break;
                                case "designationtitlebn":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.DesignationTitleEn) : query.OrderBy(p => p.DesignationTitleEn);
                                    break;
                                case "designationdepertment":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.DesignationDepertment) : query.OrderBy(p => p.DesignationDepertment);
                                    break;
                                //case "entrydate":
                                //    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.EntryDate) : query.OrderBy(p => p.EntryDate);
                                //    break;
                                //case "entryby":
                                //    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.EntryBy) : query.OrderBy(p => p.EntryBy);
                                //    break;
                            }
                        }
                        if (options.GetLimitOffset().HasValue && query.Count()!=0)
                        {
                            query = query.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value);
                        }
                        result.Items = query.ToList();
                        result.TotalRecords = query.Count();
                    }
                    return result;
                })
            );

            //Service Table
            MVCGridDefinitionTable.Add("serviceTable", new MVCGridBuilder<ServiceInfo>(defaults)
                .WithAuthorizationType(AuthorizationType.Authorized)
                .AddColumns(cols =>
                {
                    cols.Add("ServiceId").WithHeaderText("Service Id").WithValueExpression(p => p.ServiceId).WithSorting(true);
                    cols.Add("ServiceName").WithHeaderText("Service Name").WithValueExpression(p => p.ServiceName).WithSorting(true);
                    cols.Add("Description").WithHeaderText("Description").WithValueExpression(p => p.Description);
                    cols.Add("Status").WithHeaderText("Status").WithValueExpression(p => p.Status > 0 ? "Active" : "Inactive");
                    cols.Add("ViewLink").WithSorting(false).WithHeaderText("Action").WithHtmlEncoding(false)
                        .WithValueExpression(p => p.Id.ToString()).WithValueTemplate(
                        "<a class='btn btn-sm btn-outline-primary' href='/Services/Edit/{Value}'>Edit</a> " +
                        "<button class='btn btn-sm btn-outline-danger delete' data-id='{Value}'>Delete</button>"
                     );
                })
                .WithSorting(true, "ServiceId")
                .WithPaging(true, 10, true, 100)
                .WithAdditionalQueryOptionNames("Search")
                .WithAdditionalSetting("RenderLoadingDiv", false)
                .WithRetrieveDataMethod((context) =>
                {
                    var options = context.QueryOptions;
                    var result = new QueryResult<ServiceInfo>();
                    using (var db = new CrmDbContext())
                    {
                        var query = db.ServiceInfos.AsQueryable();

                        var globalSearch = options.GetAdditionalQueryOptionString("Search");
                        if (!string.IsNullOrWhiteSpace(globalSearch))
                        {
                            query = query.Where(x =>
                                    x.ServiceId.Contains(globalSearch)
                                    || x.ServiceName.Contains(globalSearch)
                            );
                        }

                        if (!string.IsNullOrWhiteSpace(options.SortColumnName))
                        {
                            var direction = options.SortDirection;
                            switch (options.SortColumnName.ToLower())
                            {
                                case "serviceid":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.ServiceId) : query.OrderBy(p => p.ServiceId);
                                    break;
                                case "servicename":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.ServiceName) : query.OrderBy(p => p.ServiceName);
                                    break;
                            }
                        }
                        if (options.GetLimitOffset().HasValue && query.Count() != 0)
                        {
                            query = query.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value);
                        }
                        result.Items = query.ToList();
                        result.TotalRecords = query.Count();
                    }
                    return result;
                })
            );

        }
    }
}