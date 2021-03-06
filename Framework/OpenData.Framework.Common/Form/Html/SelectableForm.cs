﻿using OpenData.Framework.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenData.Framework.Common.Form.Html
{
    public class SelectableForm : ISchemaForm
    {
        static string Table = @"
@model Bzway.Web.Web.Areas.Contents.Models.SelectableViewModel
@using Bzway.Data.Query
@using Bzway.Data.Models
@using Bzway.Web.Web.Areas.Contents.Controllers
@{{
    var schema = (Bzway.Data.Models.Schema)ViewData[""Schema""];
    var folder = (Bzway.Data.Models.TextFolder)ViewData[""Folder""];
    var routes = ViewContext.RequestContext.AllRouteValues();

    var parentUUID = routes[""parentUUID""] ?? """";

    var parent = folder.Parent;

    var nameList = new List<string>() {{ folder.FriendlyText }};

    while (parent != null)
    {{
        parent = Bzway.Data.Models.IPersistableExtensions.AsActual(parent);
        if (parent != null)
        {{
            nameList.Add(parent.FriendlyText);
            parent = parent.Parent;
        }}
        
    }}

    nameList.Reverse();   

   var childFolders = Entity.ChildFolders==null? new TextFolder[0]:Entity.ChildFolders.ToArray();
}}

<div class=""common-table fixed"">
 <div class=""thead"">
    <table>
        <thead>
            <tr>
                <th class=""checkbox"">
                    <input type=""checkbox"" class=""select-all"" />
                </th>
                {0}
            </tr>
        </thead>
    </table>
</div>
<div class=""tbody"">
    <table>
        <tbody>
        @if (childFolders.Length == 0 && Entity.Contents.TotalItemCount == 0)
        {{
            <tr class=""empty"">
                <td>
                    @(""Empty"".Localize())
                </td>
            </tr>
        }}
        else{{
              foreach (dynamic item in childFolders)
                {{
                    <tr class=""folderTr"">
                        <td>                            
                        </td>
                        <td>
                            @if (!string.IsNullOrEmpty(item.SchemaName))
                            {{
                                <a href=""@this.Url.Action(""SelectCategories"", ViewContext.RequestContext.AllRouteValues().Merge(""FolderName"", (object)(item.FullName)).Merge(""FullName"", (object)(item.FullName)))"" >
                                   @Html.IconImage(""folder"") @Bzway.Data.Models.IPersistableExtensions.AsActual(item).FriendlyText</a>
                            }}
                            else
                            {{
                                <a href=""@this.Url.Action(""SelectCategories"", ViewContext.RequestContext.AllRouteValues().Merge(""controller"", ""TextFolder"").Merge(""FolderName"", (object)(item.FullName)).Merge(""FullName"", (object)(item.FullName)))"" >
                                   @Html.IconImage(""folder"") @Bzway.Data.Models.IPersistableExtensions.AsActual(item).FriendlyText</a>
                            }}
                        </td>
                        <td colspan=""{2}"">
                        </td>                      
                    </tr>
                }} 
        }}
           @AddTreeItems(Entity.Contents, schema, folder, true)
        </tbody>
    </table>
</div>
</div>
@helper AddTreeItems(IEnumerable<TextContent> items, Schema schema, TextFolder folder, bool isRoot)
    {{
        {3}    
        if (items.Count() > 0)
        {{
            foreach (dynamic item in items)
            {{
    <tr id=""@item.Id"" class= ""doctr  @((item.IsLocalized != null && item.IsLocalized == false) ? ""unlocalized"" : ""localized"") @((item.Published == null || item.Published == false) ? ""unpublished"" : ""published"")"">
        <td class='checkbox'>
            @if (Entity.SingleChoice)
            {{
                <input type=""radio"" value='@item[""Id""]' name=""select"" class=""select doc""/> 
            }}
            else
            {{
                <input type=""checkbox"" value='@item[""Id""]' name=""select"" class=""select docs""/>
            }}
        </td>
        {1}
    </tr>     
            }}
        }}
}}

<table id=""treeNode-template"" style=""display: none"" data-model=""JsonModel"">
    <tbody data-bind=""foreach:{{data:Entity,as:'item'}}"">  
           
      <tr data-bind=""attr:{{id:item.Id,parentChain:item._ParentChain_}}"">
        <td class='checkbox'>
            @if (Entity.SingleChoice)
            {{
                <input type=""radio"" name=""select"" data-bind=""attr:{{value:item.Id}}"" class=""select doc""/> 
            }}
            else
            {{                
               <input type=""checkbox"" name=""select"" class=""select docs"" data-bind=""attr:{{value:item.Id}}""/>
            }}
        </td>
{4}
        <td class=""date"" data-bind=""html:item._LocalCreationDate_""></td>
        <td><span data-bind=""text : (item.Published == true?'YES': '-')""></span></td>
    </tr>     
    </tbody>
</table>
";


        public string Generate(ISchema schema)
        {

            StringBuilder sb_head = new StringBuilder();
            StringBuilder sb_body = new StringBuilder();
            StringBuilder columnDataSource = new StringBuilder();
            StringBuilder sb_koTml = new StringBuilder();
            int colspan = 0;
            //StringBuilder sb_categoryData = new StringBuilder();
            foreach (var item in schema.Columns)
            {
                if (item.ShowInGrid)
                {
                    string columnValue = string.Format("@Bzway.Web.Form.Html.HtmlCodeHelper.RenderColumnValue(item.{0})", item.Name);
                    if (HasDataSource(item.ControlType))
                    {
                        if (!string.IsNullOrEmpty(item.SelectionFolder))
                        {
                            //                        sb_categoryData.AppendFormat(@"
                            //                          
                            //                         ", item.Name, item.SelectionFolder);
                            columnDataSource.AppendFormat(@"var {0}_data = (new TextFolder(Repository.Current,""{1}"")).CreateQuery().ToArray();", item.Name, item.SelectionFolder);
                            columnValue = string.Format(@"@{{
                        string {0}_rawValue = (item.{0} ?? """").ToString();
                        string[] {0}_value = {0}_rawValue.Split(new[] {{ ',' }}, StringSplitOptions.RemoveEmptyEntries);                        
                        var {0}_values = {0}_data.Where(it =>
                            {0}_value.Any(s =>
                                s.EqualsOrNullEmpty(it.Id, StringComparison.OrdinalIgnoreCase))).ToArray();}}
                        @if ({0}_values.Length > 0)
                        {{
                            @string.Join("","", {0}_values.Select(it => it.GetSummary()))
                        }}
                        else
                        {{
                            {1}
                        }}", item.Name, columnValue, item.SelectionFolder);
                        }
                        else if (item.SelectionItems != null && item.SelectionItems.Length > 0)
                        {
                            columnDataSource.AppendFormat(@"var {0}_data = schema[""{0}""].SelectionItems;", item.Name);
                            columnValue = string.Format(@"@{{
                        string {0}_rawValue = (item.{0} ?? """").ToString();
                        string[] {0}_value = {0}_rawValue.Split(new[] {{ ',' }}, StringSplitOptions.RemoveEmptyEntries);
                      
                        var {0}_values = {0}_data.Where(it =>
                            {0}_value.Any(s =>
                                s.EqualsOrNullEmpty(it.Value, StringComparison.OrdinalIgnoreCase))).ToArray();}}
                        @if ({0}_values.Length > 0)
                        {{
                            @string.Join("","", {0}_values.Select(it => it.Text))
                        }}
                        else
                        {{
                            {1}
                        }}", item.Name, columnValue, item.SelectionFolder);
                        }
                    }
                    sb_head.AppendFormat("\t\t<th class=\"{1} @SortByExtension.RenderSortHeaderClass(ViewContext.RequestContext, \"{1}\",-1)\">@SortByExtension.RenderGridHeader(ViewContext.RequestContext, \"{0}\", \"{1}\", -1)</th>\r\n", string.IsNullOrEmpty(item.Label) ? item.Name : item.Label, item.Name, colspan);
                    if (item.Name.EqualsOrNullEmpty("Published", StringComparison.CurrentCultureIgnoreCase))
                    {
                        sb_body.AppendFormat("\t\t<td>{0}</td>", columnValue);
                    }
                    else if (item.Name.EqualsOrNullEmpty("UtcCreationDate", StringComparison.CurrentCultureIgnoreCase))
                    {
                        sb_body.AppendFormat("\t\t<td class=\"date\">@(DateTime.Parse(item[\"{0}\"].ToString()).ToLocalTime().ToShortDateString())</td>\r\n", item.Name);
                    }
                    else if (item.DataType == ColumnType.DateTime)
                    {
                        sb_body.AppendFormat("\t\t<td class=\"date\">@(item[\"{0}\"] == null?\"\":((DateTime)item[\"{0}\"]).ToLocalTime().ToShortDateString())</td>\r\n", item.Name);
                    }
                    else
                    {
                        if (colspan == 0)
                        {
                            sb_body.AppendFormat("\t\t<td>@if(Entity.ShowTreeStyle){{\n\t\t<span class=\"expander\">@Html.IconImage(\"arrow\")</span>}}\n {0}</td>\r\n"
                                        , columnValue);

                            sb_koTml.AppendFormat("\t\t<td class=\"treeStyle\">\n\t\t<span class=\"expander\">@Html.IconImage(\"arrow\")</span>\n\t\t@Html.IconImage(\"file document\")<span data-bind=\"html:item.{0}\"></span></td>"
                                , item.Name);
                        }
                        else
                        {
                            sb_body.AppendFormat("\t\t<td>{0}</td>\r\n", columnValue);
                            sb_koTml.AppendFormat("\t\t<td data-bind=\"html:item.{0}\"></td>", item.Name);
                        }


                    }
                    colspan++;
                }
            }
            return string.Format(Table, sb_head, sb_body, colspan - 1, columnDataSource, sb_koTml);
        }

        private bool HasDataSource(string controlType)
        {
            if (string.IsNullOrEmpty(controlType))
            {
                return false;
            }
            var control = ControlHelper.Resolve(controlType);
            if (control == null)
            {
                return false;
            }
            return control.HasDataSource;
        }
    }
}
