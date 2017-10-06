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
                    cols.Add("Status").WithHeaderText("Status").WithValueExpression(p => p.Status == Status.Active ? "Active" : "Inactive");
                    //cols.Add("DelStatus").WithHeaderText("Delete Status").WithValueExpression(p => p.DelStatus.ToString());
                    //cols.Add("EntryDate").WithHeaderText("Entry Date").WithValueExpression(p => p.EntryDate.ToString()).WithSorting(true);
                    //cols.Add("EntryBy").WithHeaderText("Entry By").WithValueExpression(p => p.EntryBy.GetUserName()).WithSorting(true);
                    cols.Add("ViewLink").WithSorting(false).WithHeaderText("Action").WithHtmlEncoding(false)
                        .WithValueExpression(p => p.Id.ToString()).WithValueTemplate(
                        "<a class='btn btn-sm btn-outline-primary' href='/Branch/Edit/{Value}'>Edit</a> "
                        + "<a class='btn btn-sm btn-outline-info' href='/Branch/Details/{Value}'>Details</a> "
                        + "<button class='btn btn-sm btn-outline-danger delete' data-id='{Value}'>Delete</button>"
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
                        "<a class='btn btn-sm btn-outline-primary' href='/Designations/Edit/{Value}'>Edit</a> "
                        + "<a class='btn btn-sm btn-outline-primary' href='/Designations/Details/{Value}'>Details</a> "
                        + "<button class='btn btn-sm btn-outline-danger delete' data-id='{Value}'>Delete</button>"
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
                        "<a class='btn btn-sm btn-outline-primary' href='/Services/Edit/{Value}'>Edit</a> "
                        + "<a class='btn btn-sm btn-outline-primary' href='/Services/Details/{Value}'>Details</a> "
                        + "<button class='btn btn-sm btn-outline-danger delete' data-id='{Value}'>Delete</button>"
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
                        "<a class='btn btn-sm btn-outline-primary' href='/Sectors/Edit/{Value}'>Edit</a> "
                        + "<a class='btn btn-sm btn-outline-primary' href='/Sectors/Details/{Value}'>Details</a> "
                        + "<button class='btn btn-sm btn-outline-danger delete' data-id='{Value}'>Delete</button>"
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
                        "<a class='btn btn-sm btn-outline-primary' href='/Airlines/Edit/{Value}'>Edit</a> "
                        + "<a class='btn btn-sm btn-outline-primary' href='/Airlines/Details/{Value}'>Details</a> "
                        + "<button class='btn btn-sm btn-outline-danger delete' data-id='{Value}'>Delete</button>"
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
                        "<a class='btn btn-sm btn-outline-primary' href='/Suppliers/Edit/{Value}'>Edit</a> "
                        + "<a class='btn btn-sm btn-outline-primary' href='/Suppliers/Details/{Value}'>Edit</a> "
                        + "<button class='btn btn-sm btn-outline-danger delete' data-id='{Value}'>Delete</button>"
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
                        "<a class='btn btn-sm btn-outline-primary' href='/PaymentMethods/Edit/{Value}'>Edit</a> "
                        + "<a class='btn btn-sm btn-outline-primary' href='/PaymentMethods/Details/{Value}'>Details</a> "
                        + "<button class='btn btn-sm btn-outline-danger delete' data-id='{Value}'>Delete</button>"
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
                        "<a class='btn btn-sm btn-outline-primary' href='/Agents/Edit/{Value}'>Edit</a> " +
                        "<a class='btn btn-sm btn-outline-info' href='/Agents/Details/{Value}'>Details</a> " +
                        "<button class='btn btn-sm btn-outline-danger delete' data-id='{Value}'>Delete</button>"
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
                    cols.Add("DoneBy").WithHeaderText("Flight Time").WithValueExpression(p => p.DoneBy).WithSorting(true);
                    cols.Add("ViewLink").WithSorting(false).WithHeaderText("Action").WithHtmlEncoding(false)
                        .WithValueExpression(p => p.Id.ToString()).WithValueTemplate(
                        "<a class='btn btn-sm btn-outline-primary' href='/Clients/Edit/{Value}'>Edit</a> "
                        + "<a class='btn btn-sm btn-outline-info' href='/Clients/Details/{Value}'>Details</a> "
                        + "<button class='btn btn-sm btn-outline-danger delete' data-id='{Value}'>Delete</button>"
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
                                    || x.DoneBy.Contains(globalSearch)
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
                                case "doneby":
                                    query = direction == SortDirection.Dsc ? query.OrderByDescending(p => p.DoneBy) : query.OrderBy(p => p.DoneBy);
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