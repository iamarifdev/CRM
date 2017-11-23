using System.Data.Entity;
using System.Linq;
using MVCGrid.Models;
using MVCGrid.Web;
using App.Entity.Models;
using App.Web.Context;
using App.Web.Helper;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(App.Web.MVCGridConfig), "RegisterGrids")]

namespace App.Web
{
    public static class MVCGridConfig
    {
        public static void RegisterGrids()
        {
            var crmDb = new CrmDbContext();
            var defaults = new GridDefaults()
            {
                Paging = true,
                ItemsPerPage = 10,
                Sorting = true,
                NoResultsMessage = "Sorry, no results were found."
            };

            // Menu Table
            MVCGridDefinitionTable.Add("navigationTable", new MVCGridBuilder<Menu>(defaults)
                .WithAuthorizationType(AuthorizationType.Authorized)
                .AddColumns(cols =>
                {
                    cols.Add("MenuId").WithHeaderText("Menu Id").WithValueExpression(p => p.MenuId.ToString()).WithSorting(true);
                    cols.Add("ModuleName").WithHeaderText("Module Name").WithValueExpression(p => Common.GetDescription(p.ModuleName)).WithSorting(true);
                    cols.Add("ControllerName").WithHeaderText("Controller Name").WithValueExpression(p => p.ControllerName).WithSorting(true);
                    cols.Add("ActionName").WithHeaderText("Action Name").WithValueExpression(p => p.ActionName).WithSorting(true);
                    cols.Add("Status").WithHtmlEncoding(false)
                        .WithHeaderText("Status")
                        .WithValueExpression((p, c) => p.Status == Status.Active ? "btn-success" : "btn-danger")
                        .WithValueTemplate("<button class='btn btn-sm m-b-0-25 {Value}' onclick='InactiveOrDeactive({Model.MenuId})'>{Model.Status}</button>")
                        .WithSorting(true);
                    cols.Add("ViewLink").WithSorting(false).WithHeaderText("Action").WithHtmlEncoding(false)
                        .WithValueExpression(p => p.MenuId.ToString()).WithValueTemplate(
                        "<a class='btn btn-sm m-b-0-25 btn-outline-primary' href='/Navigation/Edit/{Value}'>Edit</a> "
                        +"<button class='btn btn-sm m-b-0-25 btn-outline-danger delete' data-id='{Value}'>Delete</button>"
                     );
                })
                .WithSorting(true, "MenuId")
                .WithPaging(true, 10, true, 100)
                .WithAdditionalQueryOptionNames("Search")
                .WithAdditionalSetting("RenderLoadingDiv", false)
                .WithRetrieveDataMethod((context) =>
                {
                    var options = context.QueryOptions;
                    var result = new QueryResult<Menu>();
                    using (var db = new CrmDbContext())
                    {
                        var query = db.Menus.AsQueryable();

                        var globalSearch = options.GetAdditionalQueryOptionString("Search");
                        if (!string.IsNullOrWhiteSpace(globalSearch))
                        {
                            query = query.Where(x =>
                                    x.MenuId.ToString().Contains(globalSearch) ||
                                    //Common.GetDescription(x.ModuleName).Contains(globalSearch) ||
                                    x.ControllerName.Contains(globalSearch) ||
                                    x.ActionName.Contains(globalSearch) ||
                                    x.Status.ToString().Contains(globalSearch)
                            );
                        }

                        if (!string.IsNullOrWhiteSpace(options.SortColumnName))
                        {
                            var direction = options.SortDirection;
                            switch (options.SortColumnName.ToLower())
                            {
                                case "menuid":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.MenuId) : query.OrderBy(p => p.MenuId);
                                    break;
                                case "modulename":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.ModuleName) : query.OrderBy(p => p.ModuleName);
                                    break;
                                case "controllername":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.ControllerName) : query.OrderBy(p => p.ControllerName);
                                    break;
                                case "actionname":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.ActionName) : query.OrderBy(p => p.ActionName);
                                    break;
                                case "status":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.Status) : query.OrderBy(p => p.Status);
                                    break;
                            }
                        }
                        result.TotalRecords = query.Count();
                        if (options.GetLimitOffset().HasValue && query.Count() != 0)
                        {
                            query = query.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value);
                        }
                        result.Items = query.ToList();
                    }
                    return result;
                })
            );

            // User Table
            MVCGridDefinitionTable.Add("userTable", new MVCGridBuilder<User>(defaults)
                .WithAuthorizationType(AuthorizationType.Authorized)
                .AddColumns(cols =>
                {
                    cols.Add("Id").WithHeaderText("Id").WithValueExpression(p => p.Id.ToString()).WithSorting(true);
                    cols.Add("UserName").WithHeaderText("User Name").WithValueExpression(p => p.UserName).WithSorting(true);
                    cols.Add("BranchName").WithHeaderText("Branch Name")
                        .WithValueExpression(p => crmDb.BranchInfos.Where(x => x.Id == p.BranchId).Select(x => x.BranchName).FirstOrDefault())
                        .WithSorting(true);
                    cols.Add("EmployeeName").WithHeaderText("Employee Name")
                        .WithValueExpression(p => crmDb.EmployeeBasicInfos.Where(x => x.Id == p.EmployeeId).Select(x => x.EmployeeName).FirstOrDefault())
                        .WithSorting(true);
                    cols.Add("UserGroup").WithHeaderText("User Group").WithValueExpression(p => p.Group.Name).WithSorting(true);
                    cols.Add("UserLevel").WithHeaderText("User Level").WithValueExpression(p => Common.GetDescription(p.Level)).WithSorting(true);
                    cols.Add("Status").WithHtmlEncoding(false)
                        .WithHeaderText("Status")
                        .WithValueExpression((p, c) => p.Status == Status.Active ? "btn-success" : "btn-danger")
                        .WithValueTemplate("<button class='btn btn-sm m-b-0-25 {Value}' onclick='InactiveOrDeactive({Model.Id})'>{Model.Status}</button>")
                        .WithSorting(true);
                    cols.Add("ViewLink")
                        .WithSorting(false)
                        .WithHeaderText("Action")
                        .WithHtmlEncoding(false)
                        .WithValueExpression(p => p.Id.ToString()).WithValueTemplate(
                        "<a class='btn btn-sm m-b-0-25 btn-outline-primary' href='/Users/Edit/{Value}'>Edit</a> "
                        + "<a class='btn btn-sm m-b-0-25 btn-outline-info' href='/Users/Details/{Value}'>Details</a> "
                        + "<button class='btn btn-sm m-b-0-25 btn-outline-danger delete' data-id='{Value}'>Delete</button>"
                     );
                })
                .WithSorting(true, "Id")
                .WithPaging(true, 10, true, 100)
                .WithAdditionalQueryOptionNames("Search")
                .WithAdditionalSetting("RenderLoadingDiv", false)
                .WithRetrieveDataMethod((context) =>
                {
                    var options = context.QueryOptions;
                    var result = new QueryResult<User>();
                    using (var db = new CrmDbContext())
                    {
                        var query = db.Users.AsQueryable().Include(x=>x.Group);

                        var globalSearch = options.GetAdditionalQueryOptionString("Search");
                        if (!string.IsNullOrWhiteSpace(globalSearch))
                        {
                            query = query.Where(x =>
                                    x.Id.ToString().Contains(globalSearch) ||
                                    x.UserName.Contains(globalSearch) ||
                                    x.Group.Name.Contains(globalSearch)
                                //|| x.EntryDate.ToString().Contains(globalSearch)
                            );
                        }

                        if (!string.IsNullOrWhiteSpace(options.SortColumnName))
                        {
                            var direction = options.SortDirection;
                            switch (options.SortColumnName.ToLower())
                            {
                                case "id":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.Id) : query.OrderBy(p => p.Id);
                                    break;
                                case "username":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.UserName) : query.OrderBy(p => p.UserName);
                                    break;
                                case "branchname":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.BranchId) : query.OrderBy(p => p.BranchId);
                                    break;
                                case "employeename":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.EmployeeId) : query.OrderBy(p => p.EmployeeId);
                                    break;
                                case "usergroup":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.Group.Name) : query.OrderBy(p => p.Group.Name);
                                    break;
                                case "userlevel":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.Level) : query.OrderBy(p => p.Level);
                                    break;
                                case "status":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.Status) : query.OrderBy(p => p.Status);
                                    break;
                            }
                        }
                        result.TotalRecords = query.Count();
                        if (options.GetLimitOffset().HasValue && query.Count() != 0)
                        {
                            query = query.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value);
                        }
                        result.Items = query.ToList();
                    }
                    return result;
                })
            );

            // Group Table
            MVCGridDefinitionTable.Add("groupTable", new MVCGridBuilder<Group>(defaults)
                .WithAuthorizationType(AuthorizationType.Authorized)
                .AddColumns(cols =>
                {
                    cols.Add("Id").WithHeaderText("Id").WithValueExpression(p => p.Id.ToString()).WithSorting(true);
                    cols.Add("Name").WithHeaderText("Group Name").WithValueExpression(p => p.Name).WithSorting(true);
                    cols.Add("Description").WithHeaderText("Description").WithValueExpression(p => p.Description).WithSorting(true);
                    cols.Add("Crm").WithHeaderText("Crm").WithValueExpression(p => p.Crm ? "YES" : "NO").WithSorting(true);
                    cols.Add("Billing").WithHeaderText("Billing").WithValueExpression(p => p.Billing ? "YES" : "NO").WithSorting(true);
                    cols.Add("Account").WithHeaderText("Accounts").WithValueExpression(p => p.Account ? "YES" : "NO").WithSorting(true);
                    cols.Add("Report").WithHeaderText("Report").WithValueExpression(p => p.Report ? "YES" : "NO").WithSorting(true);
                    cols.Add("Hrm").WithHeaderText("Hrm").WithValueExpression(p => p.Hrm ? "YES" : "NO").WithSorting(true);
                    cols.Add("Setup").WithHeaderText("Setup").WithValueExpression(p => p.Setup ? "YES" : "NO").WithSorting(true);
                    cols.Add("ViewLink").WithSorting(false).WithHeaderText("Action").WithHtmlEncoding(false)
                        .WithValueExpression(p => p.Id.ToString()).WithValueTemplate(
                        "<a class='btn btn-sm m-b-0-25 btn-outline-primary' href='/Groups/Edit/{Value}'>Edit</a>"
                     );
                })
                .WithSorting(true, "Name")
                .WithPaging(true, 10, true, 100)
                .WithAdditionalQueryOptionNames("Search")
                .WithAdditionalSetting("RenderLoadingDiv", false)
                .WithRetrieveDataMethod((context) =>
                {
                    var options = context.QueryOptions;
                    var result = new QueryResult<Group>();
                    using (var db = new CrmDbContext())
                    {
                        var query = db.Groups.AsQueryable();

                        var globalSearch = options.GetAdditionalQueryOptionString("Search");
                        if (!string.IsNullOrWhiteSpace(globalSearch))
                        {
                            query = query.Where(x =>
                                    x.Id.ToString().Contains(globalSearch) ||
                                    x.Name.Contains(globalSearch) ||
                                    x.Description.Contains(globalSearch)
                            );
                        }

                        if (!string.IsNullOrWhiteSpace(options.SortColumnName))
                        {
                            var direction = options.SortDirection;
                            switch (options.SortColumnName.ToLower())
                            {
                                case "id":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.Id) : query.OrderBy(p => p.Id);
                                    break;
                                case "name":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name);
                                    break;
                                case "description":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.Description) : query.OrderBy(p => p.Description);
                                    break;
                                case "crm":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.Crm) : query.OrderBy(p => p.Crm);
                                    break;
                                case "billing":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.Billing) : query.OrderBy(p => p.Billing);
                                    break;
                                case "account":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.Account) : query.OrderBy(p => p.Account);
                                    break;
                                case "report":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.Report) : query.OrderBy(p => p.Report);
                                    break;
                                case "hrm":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.Hrm) : query.OrderBy(p => p.Hrm);
                                    break;
                                case "setup":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.Setup) : query.OrderBy(p => p.Setup);
                                    break;
                            }
                        }
                        result.TotalRecords = query.Count();
                        if (options.GetLimitOffset().HasValue && query.Count() != 0)
                        {
                            query = query.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value);
                        }
                        result.Items = query.ToList();
                    }
                    return result;
                })
            );

            // Branch Table
            MVCGridDefinitionTable.Add("branchTable", new MVCGridBuilder<BranchInfo>(defaults)
                .WithAuthorizationType(AuthorizationType.Authorized)
                .AddColumns(cols =>
                {
                    cols.Add("BranchId").WithHeaderText("Branch Id").WithValueExpression(p => p.BranchId).WithSorting(true);
                    cols.Add("BranchName").WithHeaderText("Branch Name").WithValueExpression(p => p.BranchName).WithSorting(true);
                    cols.Add("BranchCode").WithHeaderText("Branch Code").WithValueExpression(p => p.BranchCode).WithSorting(true);
                    cols.Add("Status").WithHeaderText("Status").WithValueExpression(p => p.Status == Status.Active ? "Active" : "Inactive");
                    //cols.Add("DelStatus").WithHeaderText("Delete Status").WithValueExpression(p => p.DelStatus.ToString());
                    //cols.Add("EntryDate").WithHeaderText("Entry Date").WithValueExpression(p => p.EntryDate.ToString()).WithSorting(true);
                    //cols.Add("EntryBy").WithHeaderText("Entry By").WithValueExpression(p => p.EntryBy.GetUserName()).WithSorting(true);
                    cols.Add("ViewLink").WithSorting(false).WithHeaderText("Action").WithHtmlEncoding(false)
                        .WithValueExpression(p => p.Id.ToString()).WithValueTemplate(
                        "<a class='btn btn-sm m-b-0-25 btn-outline-primary' href='/Branch/Edit/{Value}'>Edit</a> "
                        + "<a class='btn btn-sm m-b-0-25 btn-outline-info' href='/Branch/Details/{Value}'>Details</a> "
                        + "<button class='btn btn-sm m-b-0-25 btn-outline-danger delete' data-id='{Value}'>Delete</button>"
                     );
                })
                .WithSorting(true, "BranchId")
                .WithPaging(true, 10, true, 100)
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
                                    x.BranchId.Contains(globalSearch) ||
                                    x.BranchName.Contains(globalSearch) ||
                                    x.BranchCode.Contains(globalSearch) 
                                    //|| x.EntryDate.ToString().Contains(globalSearch)
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
                                //case "entrydate":
                                //    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.EntryDate) : query.OrderBy(p => p.EntryDate);
                                //    break;
                                //case "entryby":
                                //    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.EntryBy) : query.OrderBy(p => p.EntryBy);
                                //    break;
                            }
                        }
                        result.TotalRecords = query.Count();
                        if (options.GetLimitOffset().HasValue && query.Count() != 0)
                        {
                            query = query.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value);
                        }
                        result.Items = query.ToList();
                    }
                    return result;
                })
            );

            // Country Table
            MVCGridDefinitionTable.Add("countryTable", new MVCGridBuilder<Country>(defaults)
                .WithAuthorizationType(AuthorizationType.Authorized)
                .AddColumns(cols =>
                {
                    cols.Add("CountryName").WithHeaderText("Country Name").WithValueExpression(p => p.CountryName).WithSorting(true);
                    cols.Add("CountryCode").WithHeaderText("Country Code").WithValueExpression(p => p.CountryCode).WithSorting(true);
                    cols.Add("Status").WithHeaderText("Deleted").WithValueExpression(p => p.DelStatus ? "Yes" : "No");
                    cols.Add("ViewLink").WithSorting(false).WithHeaderText("Action").WithHtmlEncoding(false)
                        .WithValueExpression(p => p.CountryId.ToString()).WithValueTemplate(
                        "<a class='btn btn-sm m-b-0-25 btn-outline-primary' href='/Coutry/Edit/{Value}'>Edit</a> "
                        + "<a class='btn btn-sm m-b-0-25 btn-outline-info' href='/Coutry/Details/{Value}'>Details</a> "
                        + "<button class='btn btn-sm m-b-0-25 btn-outline-danger delete' data-id='{Value}'>Delete</button>"
                     );
                })
                .WithSorting(true, "CountryName")
                .WithPaging(true, 10, true, 100)
                .WithAdditionalQueryOptionNames("Search")
                .WithAdditionalSetting("RenderLoadingDiv", false)
                .WithRetrieveDataMethod((context) =>
                {
                    var options = context.QueryOptions;
                    var result = new QueryResult<Country>();
                    using (var db = new CrmDbContext())
                    {
                        var query = db.Countries.AsQueryable();

                        var globalSearch = options.GetAdditionalQueryOptionString("Search");
                        if (!string.IsNullOrWhiteSpace(globalSearch))
                        {
                            query = query.Where(x =>
                                    x.CountryName.Contains(globalSearch) ||
                                    x.CountryCode.Contains(globalSearch)
                            );
                        }

                        if (!string.IsNullOrWhiteSpace(options.SortColumnName))
                        {
                            var direction = options.SortDirection;
                            switch (options.SortColumnName.ToLower())
                            {
                                case "countryname":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.CountryName) : query.OrderBy(p => p.CountryName);
                                    break;
                                case "countrycode":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.CountryCode) : query.OrderBy(p => p.CountryCode);
                                    break;
                                case "status":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.DelStatus) : query.OrderBy(p => p.DelStatus);
                                    break;
                            }
                        }
                        result.TotalRecords = query.Count();
                        if (options.GetLimitOffset().HasValue && query.Count() != 0)
                        {
                            query = query.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value);
                        }
                        result.Items = query.ToList();
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
                    cols.Add("Status").WithHeaderText("Status").WithValueExpression(p => p.Status == Status.Active ? "Active" : "Inactive");
                    cols.Add("ViewLink").WithSorting(false).WithHeaderText("Action").WithHtmlEncoding(false)
                        .WithValueExpression(p => p.Id.ToString()).WithValueTemplate(
                        "<a class='btn btn-sm m-b-0-25 btn-outline-primary' href='/Designations/Edit/{Value}'>Edit</a> "
                        + "<a class='btn btn-sm m-b-0-25 btn-outline-info' href='/Designations/Details/{Value}'>Details</a> "
                        + "<button class='btn btn-sm m-b-0-25 btn-outline-danger delete' data-id='{Value}'>Delete</button>"
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
                                case "designationtitleen":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.DesignationTitleEn) : query.OrderBy(p => p.DesignationTitleEn);
                                    break;
                                case "designationdepertment":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.DesignationDepertment) : query.OrderBy(p => p.DesignationDepertment);
                                    break;
                            }
                        }
                        result.TotalRecords = query.Count();
                        if (options.GetLimitOffset().HasValue && query.Count() != 0)
                        {
                            query = query.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value);
                        }
                        result.Items = query.ToList();
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
                    cols.Add("Status").WithHeaderText("Status").WithValueExpression(p => p.Status > 0 ? "Active" : "Inactive");
                    cols.Add("ViewLink").WithSorting(false).WithHeaderText("Action").WithHtmlEncoding(false)
                        .WithValueExpression(p => p.Id.ToString()).WithValueTemplate(
                        "<a class='btn btn-sm m-b-0-25 btn-outline-primary' href='/Services/Edit/{Value}'>Edit</a> "
                        + "<a class='btn btn-sm m-b-0-25 btn-outline-info' href='/Services/Details/{Value}'>Details</a> "
                        + "<button class='btn btn-sm m-b-0-25 btn-outline-danger delete' data-id='{Value}'>Delete</button>"
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
                        result.TotalRecords = query.Count();
                        if (options.GetLimitOffset().HasValue && query.Count() != 0)
                        {
                            query = query.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value);
                        }
                        result.Items = query.ToList();
                    }
                    return result;
                })
            );

            //Sector Table
            MVCGridDefinitionTable.Add("sectorTable", new MVCGridBuilder<SectorInfo>(defaults)
                .WithAuthorizationType(AuthorizationType.Authorized)
                .AddColumns(cols =>
                {
                    cols.Add("SectorId").WithHeaderText("Airport Id").WithValueExpression(p => p.SectorId).WithSorting(true);
                    cols.Add("SectorName").WithHeaderText("Airport Name").WithValueExpression(p => p.SectorName).WithSorting(true);
                    cols.Add("SectorCode").WithHeaderText("Airport Code").WithValueExpression(p => p.SectorCode).WithSorting(true);
                    cols.Add("Status").WithHeaderText("Status").WithValueExpression(p => p.Status > 0 ? "Active" : "Inactive").WithSorting(true);
                    cols.Add("ViewLink").WithSorting(false).WithHeaderText("Action").WithHtmlEncoding(false)
                        .WithValueExpression(p => p.Id.ToString()).WithValueTemplate(
                        "<a class='btn btn-sm m-b-0-25 btn-outline-primary' href='/Sectors/Edit/{Value}'>Edit</a> "
                        + "<a class='btn btn-sm m-b-0-25 btn-outline-info' href='/Sectors/Details/{Value}'>Details</a> "
                        + "<button class='btn btn-sm m-b-0-25 btn-outline-danger delete' data-id='{Value}'>Delete</button>"
                     );
                })
                .WithSorting(true, "SectorId")
                .WithPaging(true, 10, true, 100)
                .WithAdditionalQueryOptionNames("Search")
                .WithAdditionalSetting("RenderLoadingDiv", false)
                .WithRetrieveDataMethod((context) =>
                {
                    var options = context.QueryOptions;
                    var result = new QueryResult<SectorInfo>();
                    using (var db = new CrmDbContext())
                    {
                        var query = db.SectorInfos.AsQueryable();

                        var globalSearch = options.GetAdditionalQueryOptionString("Search");
                        if (!string.IsNullOrWhiteSpace(globalSearch))
                        {
                            query = query.Where(x =>
                                    x.SectorId.Contains(globalSearch)
                                    || x.SectorName.Contains(globalSearch)
                                    || x.SectorCode.Contains(globalSearch)
                            );
                        }

                        if (!string.IsNullOrWhiteSpace(options.SortColumnName))
                        {
                            var direction = options.SortDirection;
                            switch (options.SortColumnName.ToLower())
                            {
                                case "sectorid":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.SectorId) : query.OrderBy(p => p.SectorId);
                                    break;
                                case "sectorname":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.SectorName) : query.OrderBy(p => p.SectorName);
                                    break;
                                case "sectorcode":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.SectorCode) : query.OrderBy(p => p.SectorCode);
                                    break;
                                case "status":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.Status) : query.OrderBy(p => p.Status);
                                    break;
                            }
                        }
                        result.TotalRecords = query.Count();
                        if (options.GetLimitOffset().HasValue && query.Count() != 0)
                        {
                            query = query.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value);
                        }
                        result.Items = query.ToList();
                    }
                    return result;
                })
            );

            //Airlines Table
            MVCGridDefinitionTable.Add("airLineTable", new MVCGridBuilder<AirLineInfo>(defaults)
                .WithAuthorizationType(AuthorizationType.Authorized)
                .AddColumns(cols =>
                {
                    cols.Add("AirLineId").WithHeaderText("AirLine Id").WithValueExpression(p => p.AirLineId).WithSorting(true);
                    cols.Add("AirLineName").WithHeaderText("AirLine Name").WithValueExpression(p => p.AirLineName).WithSorting(true);
                    cols.Add("Description").WithHeaderText("Description").WithValueExpression(p => p.Description);
                    cols.Add("Status").WithHeaderText("Status").WithValueExpression(p => p.Status > 0 ? "Active" : "Inactive").WithSorting(true);
                    cols.Add("ViewLink").WithSorting(false).WithHeaderText("Action").WithHtmlEncoding(false)
                        .WithValueExpression(p => p.Id.ToString()).WithValueTemplate(
                        "<a class='btn btn-sm m-b-0-25 btn-outline-primary' href='/Airlines/Edit/{Value}'>Edit</a> "
                        + "<a class='btn btn-sm m-b-0-25 btn-outline-info' href='/Airlines/Details/{Value}'>Details</a> "
                        + "<button class='btn btn-sm m-b-0-25 btn-outline-danger delete' data-id='{Value}'>Delete</button>"
                     );
                })
                .WithSorting(true, "AirLineId")
                .WithPaging(true, 10, true, 100)
                .WithAdditionalQueryOptionNames("Search")
                .WithAdditionalSetting("RenderLoadingDiv", false)
                .WithRetrieveDataMethod((context) =>
                {
                    var options = context.QueryOptions;
                    var result = new QueryResult<AirLineInfo>();
                    using (var db = new CrmDbContext())
                    {
                        var query = db.AirLineInfos.AsQueryable();

                        var globalSearch = options.GetAdditionalQueryOptionString("Search");
                        if (!string.IsNullOrWhiteSpace(globalSearch))
                        {
                            query = query.Where(x =>
                                    x.AirLineId.Contains(globalSearch)
                                    || x.AirLineName.Contains(globalSearch)
                            );
                        }

                        if (!string.IsNullOrWhiteSpace(options.SortColumnName))
                        {
                            var direction = options.SortDirection;
                            switch (options.SortColumnName.ToLower())
                            {
                                case "airlineid":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.AirLineId) : query.OrderBy(p => p.AirLineId);
                                    break;
                                case "airlinename":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.AirLineName) : query.OrderBy(p => p.AirLineName);
                                    break;
                                case "status":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.Status) : query.OrderBy(p => p.Status);
                                    break;
                            }
                        }
                        result.TotalRecords = query.Count();
                        if (options.GetLimitOffset().HasValue && query.Count() != 0)
                        {
                            query = query.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value);
                        }
                        result.Items = query.ToList();
                    }
                    return result;
                })
            );

            //Suppliers Table
            MVCGridDefinitionTable.Add("supplierTable", new MVCGridBuilder<SuppliersInfo>(defaults)
                .WithAuthorizationType(AuthorizationType.Authorized)
                .AddColumns(cols =>
                {
                    cols.Add("SupplierId").WithHeaderText("Supplier Id").WithValueExpression(p => p.SupplierId).WithSorting(true);
                    cols.Add("SupplierName").WithHeaderText("Name").WithValueExpression(p => p.SupplierName).WithSorting(true);
                    cols.Add("SupplierEmail").WithHeaderText("Email").WithValueExpression(p => p.SupplierEmail).WithSorting(true);
                    cols.Add("SupplierMobileNo").WithHeaderText("Mobile No.").WithValueExpression(p => p.SupplierMobileNo).WithSorting(true);
                    cols.Add("ViewLink").WithSorting(false).WithHeaderText("Action").WithHtmlEncoding(false)
                        .WithValueExpression(p => p.Id.ToString()).WithValueTemplate(
                        "<a class='btn btn-sm m-b-0-25 btn-outline-primary' href='/Suppliers/Edit/{Value}'>Edit</a> "
                        + "<a class='btn btn-sm m-b-0-25 btn-outline-info' href='/Suppliers/Details/{Value}'>Details</a> "
                        + "<button class='btn btn-sm m-b-0-25 btn-outline-danger delete' data-id='{Value}'>Delete</button>"
                     );
                })
                .WithSorting(true, "SupplierId")
                .WithPaging(true, 10, true, 100)
                .WithAdditionalQueryOptionNames("Search")
                .WithAdditionalSetting("RenderLoadingDiv", false)
                .WithRetrieveDataMethod((context) =>
                {
                    var options = context.QueryOptions;
                    var result = new QueryResult<SuppliersInfo>();
                    using (var db = new CrmDbContext())
                    {
                        var query = db.SuppliersInfos.AsQueryable();

                        var globalSearch = options.GetAdditionalQueryOptionString("Search");
                        if (!string.IsNullOrWhiteSpace(globalSearch))
                        {
                            query = query.Where(x =>
                                    x.SupplierId.Contains(globalSearch)
                                    || x.SupplierEmail.Contains(globalSearch)
                                    || x.SupplierName.Contains(globalSearch)
                                    || x.SupplierMobileNo.Contains(globalSearch)
                            );
                        }

                        if (!string.IsNullOrWhiteSpace(options.SortColumnName))
                        {
                            var direction = options.SortDirection;
                            switch (options.SortColumnName.ToLower())
                            {
                                case "supplierid":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.SupplierId) : query.OrderBy(p => p.SupplierId);
                                    break;
                                case "suppliername":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.SupplierName) : query.OrderBy(p => p.SupplierName);
                                    break;
                                case "supplieremail":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.SupplierEmail) : query.OrderBy(p => p.SupplierEmail);
                                    break;
                                case "suppliermobileno":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.SupplierMobileNo) : query.OrderBy(p => p.SupplierMobileNo);
                                    break;
                            }
                        }
                        result.TotalRecords = query.Count();
                        if (options.GetLimitOffset().HasValue && query.Count() != 0)
                        {
                            query = query.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value);
                        }
                        result.Items = query.ToList();
                    }
                    return result;
                })
            );

            //Payment Methods Table
            MVCGridDefinitionTable.Add("paymentMethodTable", new MVCGridBuilder<PaymentMethod>(defaults)
                .WithAuthorizationType(AuthorizationType.Authorized)
                .AddColumns(cols =>
                {
                    cols.Add("MethodId").WithHeaderText("Id").WithValueExpression(p => p.MethodId).WithSorting(true);
                    cols.Add("MethodName").WithHeaderText("Payment Method").WithValueExpression(p => p.MethodName).WithSorting(true);
                    cols.Add("ViewLink").WithSorting(false).WithHeaderText("Action").WithHtmlEncoding(false)
                        .WithValueExpression(p => p.Id.ToString()).WithValueTemplate(
                        "<a class='btn btn-sm m-b-0-25 btn-outline-primary' href='/PaymentMethods/Edit/{Value}'>Edit</a> "
                        + "<a class='btn btn-sm m-b-0-25 btn-outline-info' href='/PaymentMethods/Details/{Value}'>Details</a> "
                        + "<button class='btn btn-sm m-b-0-25 btn-outline-danger delete' data-id='{Value}'>Delete</button>"
                     );
                })
                .WithSorting(true, "MethodId")
                .WithPaging(true, 10, true, 100)
                .WithAdditionalQueryOptionNames("Search")
                .WithAdditionalSetting("RenderLoadingDiv", false)
                .WithRetrieveDataMethod((context) =>
                {
                    var options = context.QueryOptions;
                    var result = new QueryResult<PaymentMethod>();
                    using (var db = new CrmDbContext())
                    {
                        var query = db.PaymentMethods.AsQueryable();

                        var globalSearch = options.GetAdditionalQueryOptionString("Search");
                        if (!string.IsNullOrWhiteSpace(globalSearch))
                        {
                            query = query.Where(x =>
                                    x.MethodId.Contains(globalSearch)
                                    || x.MethodName.Contains(globalSearch)
                            );
                        }

                        if (!string.IsNullOrWhiteSpace(options.SortColumnName))
                        {
                            var direction = options.SortDirection;
                            switch (options.SortColumnName.ToLower())
                            {
                                case "methodid":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.MethodId) : query.OrderBy(p => p.MethodId);
                                    break;
                                case "methodname":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.MethodName) : query.OrderBy(p => p.MethodName);
                                    break;
                            }
                        }
                        result.TotalRecords = query.Count();
                        if (options.GetLimitOffset().HasValue && query.Count() != 0)
                        {
                            query = query.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value);
                        }
                        result.Items = query.ToList();
                    }
                    return result;
                })
            );

            //Agent Table
            MVCGridDefinitionTable.Add("agentTable", new MVCGridBuilder<AgentInfo>(defaults)
                .WithAuthorizationType(AuthorizationType.Authorized)
                .AddColumns(cols =>
                {
                    cols.Add("AgentId").WithHeaderText("Agent ID").WithValueExpression(p => p.AgentId).WithSorting(true);
                    cols.Add("AgentName").WithHeaderText("Agent Name").WithValueExpression(p => p.AgentName).WithSorting(true);
                    cols.Add("ContactName").WithHeaderText("Contact Name").WithValueExpression(p => p.ContactName).WithSorting(true);
                    cols.Add("MobileNo").WithHeaderText("Mobile No").WithValueExpression(p => p.MobileNo).WithSorting(true);
                    cols.Add("OfficeNo").WithHeaderText("Office No").WithValueExpression(p => p.OfficeNo).WithSorting(true);
                    cols.Add("Email").WithHeaderText("Email").WithValueExpression(p => p.Email).WithSorting(true);
                    cols.Add("UserName").WithHeaderText("Username").WithValueExpression(p => p.UserName).WithSorting(true);
                    cols.Add("Status").WithHeaderText("Status").WithValueExpression(p => p.Status > 0 ? "Active" : "Inactive").WithSorting(true);
                    cols.Add("ViewLink").WithSorting(false).WithHeaderText("Action").WithHtmlEncoding(false)
                        .WithValueExpression(p => p.Id.ToString()).WithValueTemplate(
                        "<a class='btn btn-sm m-b-0-25 btn-outline-primary' href='/Agents/Edit/{Value}'>Edit</a> " +
                        "<a class='btn btn-sm m-b-0-25 btn-outline-info' href='/Agents/Details/{Value}'>Details</a> " +
                        "<button class='btn btn-sm m-b-0-25 btn-outline-danger delete' data-id='{Value}'>Delete</button>"
                     );
                })
                .WithSorting(true, "AgentId")
                .WithPaging(true, 10, true, 100)
                .WithAdditionalQueryOptionNames("Search")
                .WithAdditionalSetting("RenderLoadingDiv", false)
                .WithRetrieveDataMethod((context) =>
                {
                    var options = context.QueryOptions;
                    var result = new QueryResult<AgentInfo>();
                    using (var db = new CrmDbContext())
                    {
                        var query = db.AgentInfos.AsQueryable();

                        var globalSearch = options.GetAdditionalQueryOptionString("Search");
                        if (!string.IsNullOrWhiteSpace(globalSearch))
                        {
                            query = query.Where(x =>
                                    x.AgentId.Contains(globalSearch)
                                    || x.AgentName.Contains(globalSearch)
                                    || x.ContactName.Contains(globalSearch)
                                    || x.MobileNo.Contains(globalSearch)
                                    || x.OfficeNo.Contains(globalSearch)
                                    || x.Email.Contains(globalSearch)
                                    || x.UserName.Contains(globalSearch)
                            );
                        }

                        if (!string.IsNullOrWhiteSpace(options.SortColumnName))
                        {
                            var direction = options.SortDirection;
                            switch (options.SortColumnName.ToLower())
                            {
                                case "agentid":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.AgentId) : query.OrderBy(p => p.AgentId);
                                    break;
                                case "agentname":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.AgentName) : query.OrderBy(p => p.AgentName);
                                    break;
                                case "contactname":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.ContactName) : query.OrderBy(p => p.ContactName);
                                    break;
                                case "mobileno":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.MobileNo) : query.OrderBy(p => p.MobileNo);
                                    break;
                                case "officeno":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.OfficeNo) : query.OrderBy(p => p.OfficeNo);
                                    break;
                                case "email":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.Email) : query.OrderBy(p => p.Email);
                                    break;
                                case "username":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.UserName) : query.OrderBy(p => p.UserName);
                                    break;
                                case "status":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.Status) : query.OrderBy(p => p.Status);
                                    break;
                            }
                        }
                        result.TotalRecords = query.Count();
                        if (options.GetLimitOffset().HasValue && query.Count() != 0)
                        {
                            query = query.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value);
                        }
                        result.Items = query.ToList();
                    }
                    return result;
                })
            );

            //Client Table
            MVCGridDefinitionTable.Add("clientTable", new MVCGridBuilder<ClientInfo>(defaults)
                .WithAuthorizationType(AuthorizationType.Authorized)
                .AddColumns(cols =>
                {
                    cols.Add("EntryDate").WithHeaderText("Entry Date").WithValueExpression(p => p.EntryDate.ToString("yyyy-MM-dd")).WithSorting(true);
                    cols.Add("CustomerId").WithHeaderText("CID").WithValueExpression(p => p.CustomerId).WithSorting(true);
                    cols.Add("FullName").WithHeaderText("Name").WithValueExpression(p => p.FullName).WithSorting(true);
                    cols.Add("ContactNo").WithHeaderText("Contact Number").WithValueExpression(p => p.ContactNo).WithSorting(true);
                    cols.Add("AirLineName").WithHeaderText("Air Line").WithValueExpression(p => p.AirLineInfo == null ? "" : p.AirLineInfo.AirLineName).WithSorting(true);
                    cols.Add("Service").WithHeaderText("Service").WithValueExpression(p => p.ServiceInfo == null ? "" : p.ServiceInfo.ServiceName).WithSorting(true);
                    cols.Add("ServiceCharge").WithHeaderText("Service Charge").WithValueExpression(p => p.ServiceCharge.ToString()).WithSorting(true);
                    cols.Add("UserName").WithHeaderText("Served By").WithValueExpression(p => p.UserServedBy == null ? "" : p.UserServedBy.UserName).WithSorting(true);
                    cols.Add("WorkingStatus").WithHeaderText("Working Status").WithValueExpression(p => p.WorkingStatus > 0 ? "Done" : "Pending").WithSorting(true);
                    cols.Add("InfoStatus").WithHeaderText("Update Status").WithValueExpression(p => p.InfoStatus > 0 ? "Updated" : "Not Updated").WithSorting(true);
                    cols.Add("DeliveryStatus").WithHeaderText("Delivery Status").WithValueExpression(p => p.DeliveryStatus > 0 ? "Delivery" : "Not Delivery").WithSorting(true);
                    cols.Add("DoneBy").WithHeaderText("Flight Time").WithValueExpression(p => string.Format("{0:yyyy-MM-dd HH:mm}",p.DoneBy)).WithSorting(true);
                    cols.Add("ViewLink").WithSorting(false).WithHeaderText("Action").WithHtmlEncoding(false)
                        .WithValueExpression(p => p.Id.ToString()).WithValueTemplate(
                            "<a class='btn btn-sm m-b-0-25 btn-outline-primary' href='/Clients/Edit/{Value}'>Edit</a> "
                            + "<a class='btn btn-sm m-b-0-25 btn-outline-info' href='/Clients/Details/{Value}'>Details</a> "
                            + "<button class='btn btn-sm m-b-0-25 btn-outline-danger delete' data-id='{Value}'>Delete</button>"
                        );
                    
                })
                .WithSorting(true, "EntryDate")
                .WithPaging(true, 10, true, 100)
                .WithAdditionalQueryOptionNames("Search")
                .WithAdditionalSetting("RenderLoadingDiv", false)
                .WithRetrieveDataMethod((context) =>
                {
                    var options = context.QueryOptions;
                    var result = new QueryResult<ClientInfo>();
                    using (var db = new CrmDbContext())
                    {
                        var query = db.ClientInfos.AsQueryable()
                            .Include(x => x.ServiceInfo)
                            .Include(x => x.AirLineInfo)
                            .Include(x => x.UserServedBy);

                        var globalSearch = options.GetAdditionalQueryOptionString("Search");
                        if (!string.IsNullOrWhiteSpace(globalSearch))
                        {
                            query = query.Where(x =>
                                    x.EntryDate.ToString().Contains(globalSearch)
                                    || x.CustomerId.Contains(globalSearch)
                                    || x.FirstName.Contains(globalSearch)
                                    || x.LastName.Contains(globalSearch)
                                    || x.ContactNo.Contains(globalSearch)
                                    || x.AirLineInfo.AirLineName.Contains(globalSearch)
                                    || x.ServiceInfo.ServiceName.Contains(globalSearch)
                                    || x.UserServedBy.UserName.Contains(globalSearch)
                                    //|| x.DoneBy.Contains(globalSearch)
                            );
                        }

                        if (!string.IsNullOrWhiteSpace(options.SortColumnName))
                        {
                            var direction = options.SortDirection;
                            switch (options.SortColumnName.ToLower())
                            {
                                case "entrydate":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.EntryDate) : query.OrderBy(p => p.EntryDate);
                                    break;
                                case "customerid":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.CustomerId) : query.OrderBy(p => p.CustomerId);
                                    break;
                                case "fullname":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.FirstName) : query.OrderBy(p => p.FirstName);
                                    break;
                                case "contactno":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.ContactNo) : query.OrderBy(p => p.ContactNo);
                                    break;
                                case "airlinename":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.AirLineInfo.AirLineName) : query.OrderBy(p => p.AirLineInfo.AirLineName);
                                    break;
                                case "service":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.ServiceInfo.ServiceName) : query.OrderBy(p => p.ServiceInfo.ServiceName);
                                    break;
                                case "servicecharge":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.ServiceCharge) : query.OrderBy(p => p.ServiceCharge);
                                    break;
                                case "username":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.UserServedBy.UserName) : query.OrderBy(p => p.UserServedBy.UserName);
                                    break;
                                case "workingstatus":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.WorkingStatus) : query.OrderBy(p => p.WorkingStatus);
                                    break;
                                case "InfoStatus":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.InfoStatus) : query.OrderBy(p => p.InfoStatus);
                                    break;
                                case "deliverystatus":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.DeliveryStatus) : query.OrderBy(p => p.DeliveryStatus);
                                    break;
                                //case "doneby":
                                //    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.DoneBy) : query.OrderBy(p => p.DoneBy);
                                //    break;
                            }
                        }
                        result.TotalRecords = query.Count();
                        if (options.GetLimitOffset().HasValue && query.Count() != 0)
                        {
                            query = query.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value);
                        }
                        result.Items = query.ToList();
                    }
                    return result;
                })
            );

            //BankAccount Table
            MVCGridDefinitionTable.Add("accountsTable", new MVCGridBuilder<BankAccount>(defaults)
                .WithAuthorizationType(AuthorizationType.Authorized)
                .AddColumns(cols =>
                {
                    cols.Add("AccountId").WithHeaderText("Account Id").WithValueExpression(p => p.AccountId).WithSorting(true);
                    cols.Add("AccountName").WithHeaderText("Account Name").WithValueExpression(p => p.AccountName).WithSorting(true);
                    cols.Add("AccountNumber").WithHeaderText("Account Number").WithValueExpression(p => p.AccountNumber).WithSorting(true);
                    cols.Add("BankName").WithHeaderText("Bank Name").WithValueExpression(p => p.BankName).WithSorting(true);
                    cols.Add("BranchName").WithHeaderText("Branch Name").WithValueExpression(p => p.BranchName).WithSorting(true);
                    cols.Add("Balance").WithHeaderText("Balance").WithValueExpression(p => p.Balance.ToString()).WithSorting(true);
                    cols.Add("Status").WithHeaderText("Status").WithValueExpression(p => p.Status > 0 ? "Active" : "Inactive").WithSorting(true);
                    cols.Add("ViewLink").WithSorting(false).WithHeaderText("Action").WithHtmlEncoding(false)
                        .WithValueExpression(p => p.Id.ToString()).WithValueTemplate(
                        "<a class='btn btn-sm m-b-0-25 btn-outline-primary' href='/Accounts/Edit/{Value}'>Edit</a> "
                        + "<a class='btn btn-sm m-b-0-25 btn-outline-info' href='/Accounts/Details/{Value}'>Details</a> "
                        + "<button class='btn btn-sm m-b-0-25 btn-outline-danger delete' data-id='{Value}'>Delete</button>"
                     );
                })
                .WithSorting(true, "AccountId")
                .WithPaging(true, 10, true, 100)
                .WithAdditionalQueryOptionNames("Search")
                .WithAdditionalSetting("RenderLoadingDiv", false)
                .WithRetrieveDataMethod((context) =>
                {
                    var options = context.QueryOptions;
                    var result = new QueryResult<BankAccount>();
                    using (var db = new CrmDbContext())
                    {
                        var query = db.BankAccounts.AsQueryable();

                        var globalSearch = options.GetAdditionalQueryOptionString("Search");
                        if (!string.IsNullOrWhiteSpace(globalSearch))
                        {
                            query = query.Where(x =>
                                    x.AccountId.Contains(globalSearch)
                                    || x.AccountName.Contains(globalSearch)
                                    || x.AccountNumber.Contains(globalSearch)
                                    || x.BankName.Contains(globalSearch)
                                    || x.BranchName.Contains(globalSearch)
                                    || x.Balance.ToString().Contains(globalSearch)
                            );
                        }

                        if (!string.IsNullOrWhiteSpace(options.SortColumnName))
                        {
                            var direction = options.SortDirection;
                            switch (options.SortColumnName.ToLower())
                            {
                                case "accountid":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.AccountId) : query.OrderBy(p => p.AccountId);
                                    break;
                                case "accountname":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.AccountName) : query.OrderBy(p => p.AccountName);
                                    break;
                                case "accountnumber":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.AccountNumber) : query.OrderBy(p => p.AccountNumber);
                                    break;
                                case "bankname":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.BankName) : query.OrderBy(p => p.BankName);
                                    break;
                                case "branchname":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.BranchName) : query.OrderBy(p => p.BranchName);
                                    break;
                                case "balance":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.Balance) : query.OrderBy(p => p.Balance);
                                    break;
                                case "status":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.Status) : query.OrderBy(p => p.Status);
                                    break;
                            }
                        }
                        result.TotalRecords = query.Count();
                        if (options.GetLimitOffset().HasValue && query.Count() != 0)
                        {
                            query = query.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value);
                        }
                        result.Items = query.ToList();
                    }
                    return result;
                })
            );

            //Deposits Table
            MVCGridDefinitionTable.Add("depositsTable", new MVCGridBuilder<TransactionsInfo>(defaults)
                .WithAuthorizationType(AuthorizationType.Authorized)
                .AddColumns(cols =>
                {
                    cols.Add("Date").WithHeaderText("Date").WithValueExpression(p => p.Date.NullDateToString()).WithSorting(true);
                    cols.Add("AccountTo").WithHeaderText("Account").WithValueExpression(p => p.BankAccountTo.AccountId).WithSorting(true);
                    cols.Add("Amount").WithHeaderText("Amount").WithValueExpression(p => p.Amount.ToString()).WithSorting(true);
                    cols.Add("Description").WithHeaderText("Description").WithValueExpression(p => p.Description).WithSorting(true);
                    cols.Add("ViewLink").WithSorting(false).WithHeaderText("Action").WithHtmlEncoding(false)
                        .WithValueExpression(p => p.Id.ToString()).WithValueTemplate(
                        "<a class='btn btn-sm m-b-0-25 btn-outline-primary' href='/Deposits/Edit/{Value}'>Edit</a> "
                        + "<a class='btn btn-sm m-b-0-25 btn-outline-info' href='/Deposits/Details/{Value}'>Details</a> "
                        + "<button class='btn btn-sm m-b-0-25 btn-outline-danger delete' data-id='{Value}'>Delete</button>"
                     );
                })
                .WithSorting(true, "Date")
                .WithPaging(true, 10, true, 100)
                .WithAdditionalQueryOptionNames("Search")
                .WithAdditionalSetting("RenderLoadingDiv", false)
                .WithRetrieveDataMethod((context) =>
                {
                    var options = context.QueryOptions;
                    var result = new QueryResult<TransactionsInfo>();
                    using (var db = new CrmDbContext())
                    {
                        var query = db.TransactionsInfos.Include(x => x.BankAccountTo).Where(x => x.TransactionType == TransactionType.Deposit).AsQueryable();

                        var globalSearch = options.GetAdditionalQueryOptionString("Search");
                        if (!string.IsNullOrWhiteSpace(globalSearch))
                        {
                            query = query.Where(x =>
                                    x.Date.NullDateToString().Contains(globalSearch)
                                    || x.BankAccountTo.AccountId.ToString().Contains(globalSearch)
                                    || x.Amount.ToString().Contains(globalSearch)
                                    || x.Description.Contains(globalSearch)
                            );
                        }

                        if (!string.IsNullOrWhiteSpace(options.SortColumnName))
                        {
                            var direction = options.SortDirection;
                            switch (options.SortColumnName.ToLower())
                            {
                                case "date":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.Date) : query.OrderBy(p => p.Date);
                                    break;
                                case "accountto":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.BankAccountTo.AccountId) : query.OrderBy(p => p.BankAccountTo.AccountId);
                                    break;
                                case "amount":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.Amount) : query.OrderBy(p => p.Amount);
                                    break;
                                case "description":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.Description) : query.OrderBy(p => p.Description);
                                    break;
                            }
                        }
                        result.TotalRecords = query.Count();
                        if (options.GetLimitOffset().HasValue && query.Count() != 0)
                        {
                            query = query.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value);
                        }
                        result.Items = query.ToList();
                    }
                    return result;
                })
            );

            //Expenses Table
            MVCGridDefinitionTable.Add("expensesTable", new MVCGridBuilder<TransactionsInfo>(defaults)
                .WithAuthorizationType(AuthorizationType.Authorized)
                .AddColumns(cols =>
                {
                    cols.Add("Date").WithHeaderText("Date").WithValueExpression(p => p.Date.NullDateToString()).WithSorting(true);
                    cols.Add("AccountFrom").WithHeaderText("Account").WithValueExpression(p => p.BankAccountFrom.AccountId).WithSorting(true);
                    cols.Add("Amount").WithHeaderText("Amount").WithValueExpression(p => p.Amount.ToString()).WithSorting(true);
                    cols.Add("Description").WithHeaderText("Description").WithValueExpression(p => p.Description).WithSorting(true);
                    cols.Add("ViewLink").WithSorting(false).WithHeaderText("Action").WithHtmlEncoding(false)
                        .WithValueExpression(p => p.Id.ToString()).WithValueTemplate(
                        "<a class='btn btn-sm m-b-0-25 btn-outline-primary' href='/Expenses/Edit/{Value}'>Edit</a> "
                        + "<a class='btn btn-sm m-b-0-25 btn-outline-info' href='/Expenses/Details/{Value}'>Details</a> "
                        + "<button class='btn btn-sm m-b-0-25 btn-outline-danger delete' data-id='{Value}'>Delete</button>"
                     );
                })
                .WithSorting(true, "Date")
                .WithPaging(true, 10, true, 100)
                .WithAdditionalQueryOptionNames("Search")
                .WithAdditionalSetting("RenderLoadingDiv", false)
                .WithRetrieveDataMethod((context) =>
                {
                    var options = context.QueryOptions;
                    var result = new QueryResult<TransactionsInfo>();
                    using (var db = new CrmDbContext())
                    {
                        var query = db.TransactionsInfos.Include(x => x.BankAccountFrom).Where(x => x.TransactionType == TransactionType.Expense).AsQueryable();

                        var globalSearch = options.GetAdditionalQueryOptionString("Search");
                        if (!string.IsNullOrWhiteSpace(globalSearch))
                        {
                            query = query.Where(x =>
                                    x.Date.NullDateToString().Contains(globalSearch)
                                    || x.BankAccountFrom.AccountId.ToString().Contains(globalSearch)
                                    || x.Amount.ToString().Contains(globalSearch)
                                    || x.Description.Contains(globalSearch)
                            );
                        }

                        if (!string.IsNullOrWhiteSpace(options.SortColumnName))
                        {
                            var direction = options.SortDirection;
                            switch (options.SortColumnName.ToLower())
                            {
                                case "date":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.Date) : query.OrderBy(p => p.Date);
                                    break;
                                case "accountfrom":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.BankAccountFrom.AccountId) : query.OrderBy(p => p.BankAccountFrom.AccountId);
                                    break;
                                case "amount":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.Amount) : query.OrderBy(p => p.Amount);
                                    break;
                                case "description":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.Description) : query.OrderBy(p => p.Description);
                                    break;
                            }
                        }
                        result.TotalRecords = query.Count();
                        if (options.GetLimitOffset().HasValue && query.Count() != 0)
                        {
                            query = query.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value);
                        }
                        result.Items = query.ToList();
                    }
                    return result;
                })
            );

            //Transfer Table
            MVCGridDefinitionTable.Add("transferTable", new MVCGridBuilder<TransactionsInfo>(defaults)
                .WithAuthorizationType(AuthorizationType.Authorized)
                .AddColumns(cols =>
                {
                    cols.Add("TransactionId").WithHeaderText("Deposit Id").WithValueExpression(p => p.TransactionId).WithSorting(true);
                    cols.Add("Date").WithHeaderText("Date").WithValueExpression(p => p.Date.NullDateToString()).WithSorting(true);
                    cols.Add("AccountFrom").WithHeaderText("From Account").WithValueExpression(p => p.BankAccountFrom.AccountName).WithSorting(true);
                    cols.Add("AccountTo").WithHeaderText("To Account").WithValueExpression(p => p.BankAccountTo.AccountName).WithSorting(true);
                    cols.Add("Amount").WithHeaderText("Amount").WithValueExpression(p => p.Amount.ToString()).WithSorting(true);
                    cols.Add("Description").WithHeaderText("Description").WithValueExpression(p => p.Description).WithSorting(true);
                    cols.Add("ViewLink").WithSorting(false).WithHeaderText("Action").WithHtmlEncoding(false)
                        .WithValueExpression(p => p.Id.ToString()).WithValueTemplate(
                        "<a class='btn btn-sm m-b-0-25 btn-outline-primary' href='/Transfers/Edit/{Value}'>Edit</a> "
                        + "<a class='btn btn-sm m-b-0-25 btn-outline-info' href='/Transfers/Details/{Value}'>Details</a> "
                        + "<button class='btn btn-sm m-b-0-25 btn-outline-danger delete' data-id='{Value}'>Delete</button>"
                     );
                })
                .WithSorting(true, "TransactionId")
                .WithPaging(true, 10, true, 100)
                .WithAdditionalQueryOptionNames("Search")
                .WithAdditionalSetting("RenderLoadingDiv", false)
                .WithRetrieveDataMethod((context) =>
                {
                    var options = context.QueryOptions;
                    var result = new QueryResult<TransactionsInfo>();
                    using (var db = new CrmDbContext())
                    {
                        var query = db.TransactionsInfos.Include(x => x.BankAccountFrom).Include(x=>x.BankAccountTo).Where(x => x.TransactionType == TransactionType.Transfer).AsQueryable();

                        var globalSearch = options.GetAdditionalQueryOptionString("Search");
                        if (!string.IsNullOrWhiteSpace(globalSearch))
                        {
                            query = query.Where(x =>
                                    x.TransactionId.Contains(globalSearch)
                                    || x.Date.NullDateToString().Contains(globalSearch)
                                    || x.BankAccountFrom.AccountName.ToString().Contains(globalSearch)
                                    || x.BankAccountTo.AccountName.ToString().Contains(globalSearch)
                                    || x.Amount.ToString().Contains(globalSearch)
                                    || x.Description.Contains(globalSearch)
                            );
                        }
                        if (!string.IsNullOrWhiteSpace(options.SortColumnName))
                        {
                            var direction = options.SortDirection;
                            switch (options.SortColumnName.ToLower())
                            {
                                case "transactionid":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.TransactionId) : query.OrderBy(p => p.TransactionId);
                                    break;
                                case "date":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.Date) : query.OrderBy(p => p.Date);
                                    break;
                                case "accountfrom":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.BankAccountFrom.AccountName) : query.OrderBy(p => p.BankAccountFrom.AccountName);
                                    break;
                                case "accountto":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.BankAccountTo.AccountName) : query.OrderBy(p => p.BankAccountTo.AccountName);
                                    break;
                                case "amount":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.Amount) : query.OrderBy(p => p.Amount);
                                    break;
                                case "description":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.Description) : query.OrderBy(p => p.Description);
                                    break;
                            }
                        }
                        result.TotalRecords = query.Count();
                        if (options.GetLimitOffset().HasValue && query.Count() != 0)
                        {
                            query = query.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value);
                        }
                        result.Items = query.ToList();
                    }
                    return result;
                })
            );

            //Expenses Table
            MVCGridDefinitionTable.Add("transactionTable", new MVCGridBuilder<TransactionView>(defaults)
                .WithAuthorizationType(AuthorizationType.Authorized)
                .AddColumns(cols =>
                {
                    cols.Add("TransactionId").WithHeaderText("Transaction Id").WithValueExpression(p => p.TransactionId).WithSorting(true);
                    cols.Add("TransactionType").WithHeaderText("Transaction Type").WithValueExpression(p => p.TransactionType).WithSorting(true);
                    cols.Add("AccountFrom").WithHeaderText("From Account").WithValueExpression(p => p.AccountFrom).WithSorting(true);
                    cols.Add("AccountTo").WithHeaderText("To Account").WithValueExpression(p => p.AccountTo).WithSorting(true);
                    cols.Add("Date").WithHeaderText("Date").WithValueExpression(p => p.Date.NullDateToString()).WithSorting(true);
                    cols.Add("Payer").WithHeaderText("Payer").WithValueExpression(p => p.Payer).WithSorting(true);
                    cols.Add("Method").WithHeaderText("Method").WithValueExpression(p => p.Method).WithSorting(true);
                    cols.Add("DepositAmount").WithHeaderText("Deposit").WithValueExpression(p => p.DepositAmount.ToString()).WithSorting(true);
                    cols.Add("ExpenseAmount").WithHeaderText("Expense").WithValueExpression(p => p.ExpenseAmount.ToString()).WithSorting(true);
                    cols.Add("TransferAmount").WithHeaderText("Transfer").WithValueExpression(p => p.TransferAmount.ToString()).WithSorting(true);
                    cols.Add("Description").WithHeaderText("Description").WithValueExpression(p => p.Description).WithSorting(true);
                    cols.Add("ViewLink").WithSorting(false).WithHeaderText("Action").WithHtmlEncoding(false)
                        .WithCellCssClassExpression(p => "customColumn")
                        .WithValueExpression(p => p.Id.ToString()).WithValueTemplate(
                        "<a class='btn btn-sm m-b-0-25 btn-outline-primary' href='/Transactions/Edit/{Value}'>Edit</a> "
                        + "<a class='btn btn-sm m-b-0-25 btn-outline-info' href='/Transactions/Details/{Value}'>Details</a> "
                        + "<button class='btn btn-sm m-b-0-25 btn-outline-danger delete' data-id='{Value}'>Delete</button>"
                     );
                })
                .WithSorting(true, "TransactionId")
                .WithPaging(true, 10, true, 100)
                .WithAdditionalQueryOptionNames("Search")
                .WithAdditionalSetting("RenderLoadingDiv", false)
                .WithRetrieveDataMethod((context) =>
                {
                    var options = context.QueryOptions;
                    var result = new QueryResult<TransactionView>();
                    using (var db = new CrmDbContext())
                    {
                        var query = db.TransactionView.AsQueryable();

                        var globalSearch = options.GetAdditionalQueryOptionString("Search");
                        if (!string.IsNullOrWhiteSpace(globalSearch))
                        {
                            query = query.Where(x =>
                                    x.TransactionId.Contains(globalSearch)
                                    || x.TransactionType.Contains(globalSearch)
                                    || x.AccountFrom.Contains(globalSearch)
                                    || x.AccountTo.Contains(globalSearch)
                                    //|| x.Date.NullDateToString().Contains(globalSearch)
                                    || x.Method.Contains(globalSearch)
                                    || x.Payer.Contains(globalSearch)
                                    || x.DepositAmount.ToString().Contains(globalSearch)
                                    || x.ExpenseAmount.ToString().Contains(globalSearch)
                                    || x.TransferAmount.ToString().Contains(globalSearch)
                                    || x.Description.Contains(globalSearch)
                            );
                        }

                        if (!string.IsNullOrWhiteSpace(options.SortColumnName))
                        {
                            var direction = options.SortDirection;
                            switch (options.SortColumnName.ToLower())
                            {
                                case "transactionid":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.TransactionId) : query.OrderBy(p => p.TransactionId);
                                    break;
                                case "transactiontype":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.TransactionType) : query.OrderBy(p => p.TransactionType);
                                    break;
                                case "accountfrom":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.AccountFrom) : query.OrderBy(p => p.AccountFrom);
                                    break;
                                case "accountto":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.AccountTo) : query.OrderBy(p => p.AccountTo);
                                    break;
                                case "date":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.Date) : query.OrderBy(p => p.Date);
                                    break;
                                case "method":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.Method) : query.OrderBy(p => p.Method);
                                    break;
                                case "payer":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.Payer) : query.OrderBy(p => p.Payer);
                                    break;
                                case "depositamount":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.DepositAmount) : query.OrderBy(p => p.DepositAmount);
                                    break;
                                case "expenseamount":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.ExpenseAmount) : query.OrderBy(p => p.ExpenseAmount);
                                    break;
                                case "transferamount":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.TransferAmount) : query.OrderBy(p => p.TransferAmount);
                                    break;
                                case "description":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.Description) : query.OrderBy(p => p.Description);
                                    break;
                            }
                        }
                        result.TotalRecords = query.Count();
                        if (options.GetLimitOffset().HasValue && query.Count() != 0)
                        {
                            query = query.Skip(options.GetLimitOffset().Value).Take(options.GetLimitRowcount().Value);
                        }
                        result.Items = query.ToList();
                    }
                    return result;
                })
            );
        }
    }
}