using App.Entity.Models;
using App.Web.Context;
using App.Web.Helper;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(App.Web.MVCGridConfig), "RegisterGrids")]

namespace App.Web
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Linq;
    using System.Collections.Generic;

    using MVCGrid.Models;
    using MVCGrid.Web;

    public static class MVCGridConfig 
    {
        public static void RegisterGrids()
        {
            var defaults = new GridDefaults()
            {
                Paging = true,
                ItemsPerPage = 20,
                Sorting = true,
                NoResultsMessage = "Sorry, no results were found"
            };

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
            })
            .WithSorting(true, "BranchId")
            //.WithAdditionalQueryOptionNames("Search")
            //.WithAdditionalSetting("RenderLoadingDiv", false)
            .WithRetrieveDataMethod((context) =>
            {
                var options = context.QueryOptions;
                var result = new QueryResult<BranchInfo>();
                using (var db = new CrmDbContext())
                {
                    var query = db.BranchInfos.AsQueryable();
                    result.TotalRecords = query.Count();
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
                    if (options.GetLimitOffset().HasValue)
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