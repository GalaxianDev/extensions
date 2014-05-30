﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Signum.Web.Extensions.Translation.Views
{
    using System;
    using System.Collections.Generic;
    
    #line 2 "..\..\Translation\Views\Sync.cshtml"
    using System.Globalization;
    
    #line default
    #line hidden
    using System.IO;
    using System.Linq;
    using System.Net;
    
    #line 3 "..\..\Translation\Views\Sync.cshtml"
    using System.Reflection;
    
    #line default
    #line hidden
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using Signum.Entities;
    
    #line 7 "..\..\Translation\Views\Sync.cshtml"
    using Signum.Entities.Translation;
    
    #line default
    #line hidden
    
    #line 4 "..\..\Translation\Views\Sync.cshtml"
    using Signum.Utilities;
    
    #line default
    #line hidden
    using Signum.Web;
    
    #line 5 "..\..\Translation\Views\Sync.cshtml"
    using Signum.Web.Translation;
    
    #line default
    #line hidden
    
    #line 6 "..\..\Translation\Views\Sync.cshtml"
    using Signum.Web.Translation.Controllers;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Translation/Views/Sync.cshtml")]
    public partial class Sync : System.Web.Mvc.WebViewPage<LocalizedAssemblyChanges>
    {
        public Sync()
        {
        }
        public override void Execute()
        {
            
            #line 8 "..\..\Translation\Views\Sync.cshtml"
  
    CultureInfo culture = ViewBag.Culture;
    int totalTypes = ViewBag.TotalTypes;

    ViewBag.Title = TranslationMessage.Synchronize0In1.NiceToString().Formato(Model.LocalizedAssembly.Assembly.GetName().Name, Model.LocalizedAssembly.Culture.DisplayName);

    if(Model.Types.Count < totalTypes)
    {
        ViewBag.Title = ViewBag.Title + " [{0}/{1}]".Formato(Model.Types.Count, totalTypes); 
    }
    
    Func<IEnumerable<string>, List<SelectListItem>> selectListItems = values =>
    {
        var items = values.Select(s => new SelectListItem { Value = s, Text = s }).ToList();

        if (values.Count() > 1 && values.Distinct().Count() == 1 || TranslationClient.Translator.AutoSelect())
        {
            items.First().Selected = true;
            items.Insert(0, new SelectListItem { Value = "", Text = "-" });
        }
        else
        {
            items.Insert(0, new SelectListItem { Value = "", Text = "-", Selected = true });
        }

        return items;
    };

    Func<LocalizedType, string> locKey = lt => lt.Type.Name + "." + lt.Assembly.Culture.Name;

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

            
            #line 39 "..\..\Translation\Views\Sync.cshtml"
Write(Html.ScriptCss("~/Translation/Content/Translation.css"));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

            
            #line 41 "..\..\Translation\Views\Sync.cshtml"
 if (Model.Types.IsEmpty())
{

            
            #line default
            #line hidden
WriteLiteral("    <h2>");

            
            #line 43 "..\..\Translation\Views\Sync.cshtml"
   Write(TranslationMessage._0AlreadySynchronized.NiceToString().Formato(Model.LocalizedAssembly.Assembly.GetName().Name));

            
            #line default
            #line hidden
WriteLiteral("</h2>   \r\n");

            
            #line 44 "..\..\Translation\Views\Sync.cshtml"
}
else
{

            
            #line default
            #line hidden
WriteLiteral("    <h2>");

            
            #line 47 "..\..\Translation\Views\Sync.cshtml"
   Write(ViewBag.Title);

            
            #line default
            #line hidden
WriteLiteral("</h2>\r\n");

            
            #line 48 "..\..\Translation\Views\Sync.cshtml"
    
    using (Html.BeginForm((TranslationController c) => c.SaveSync(Model.LocalizedAssembly.Assembly.GetName().Name, culture.Name)))
    {

            
            #line default
            #line hidden
WriteLiteral("    <table");

WriteLiteral(" id=\"results\"");

WriteLiteral(" style=\"width: 100%; margin: 0px\"");

WriteLiteral(" class=\"st\"");

WriteLiteral(" \r\n        data-pluralAndGender=\"");

            
            #line 52 "..\..\Translation\Views\Sync.cshtml"
                         Write(Url.Action((TranslationController tc) => tc.PluralAndGender()));

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(" \r\n        data-feedback=\"");

            
            #line 53 "..\..\Translation\Views\Sync.cshtml"
                  Write(Url.Action("Feedback", "Translation"));

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(" \r\n        data-culture=\"");

            
            #line 54 "..\..\Translation\Views\Sync.cshtml"
                 Write(culture.Name);

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(">\r\n");

            
            #line 55 "..\..\Translation\Views\Sync.cshtml"
        
            
            #line default
            #line hidden
            
            #line 55 "..\..\Translation\Views\Sync.cshtml"
         foreach (var typeChanges in Model.Types)
        {

            
            #line default
            #line hidden
WriteLiteral("            <thead>\r\n                <tr>\r\n                    <th");

WriteLiteral(" class=\"leftCell\"");

WriteLiteral(">");

            
            #line 59 "..\..\Translation\Views\Sync.cshtml"
                                    Write(TranslationMessage.Type.NiceToString());

            
            #line default
            #line hidden
WriteLiteral("</th>\r\n                    <th");

WriteLiteral(" colspan=\"4\"");

WriteLiteral(" class=\"titleCell\"");

WriteLiteral(">");

            
            #line 60 "..\..\Translation\Views\Sync.cshtml"
                                                 Write(typeChanges.Type.Type.Name);

            
            #line default
            #line hidden
WriteLiteral("</th>\r\n                </tr>\r\n            </thead>\r\n");

            
            #line 63 "..\..\Translation\Views\Sync.cshtml"
            
            if (typeChanges.TypeConflict != null)
            {

                bool hasGenderOption = typeChanges.Type.Options.IsSet(DescriptionOptions.Gender);
                bool hasPlural = typeChanges.Type.Options.IsSet(DescriptionOptions.PluralDescription);

                foreach (var tc in typeChanges.TypeConflict)
                {
                    var locType = tc.Value.Original;

                    var hasGender = hasGenderOption && NaturalLanguageTools.HasGenders(tc.Key);

            
            #line default
            #line hidden
WriteLiteral("            <tr>\r\n                <th");

WriteLiteral(" class=\"leftCell\"");

WriteLiteral(">");

            
            #line 76 "..\..\Translation\Views\Sync.cshtml"
                                Write(tc.Key.Name);

            
            #line default
            #line hidden
WriteLiteral("</th>\r\n                <th");

WriteLiteral(" class=\"smallCell monospaceCell\"");

WriteLiteral(">\r\n");

            
            #line 78 "..\..\Translation\Views\Sync.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 78 "..\..\Translation\Views\Sync.cshtml"
                     if (hasGender)
                    {
                        
            
            #line default
            #line hidden
            
            #line 80 "..\..\Translation\Views\Sync.cshtml"
                    Write(locType.Gender != null ? NaturalLanguageTools.GetPronom(locType.Gender.Value, plural: false, culture: locType.Assembly.Culture) : "-");

            
            #line default
            #line hidden
            
            #line 80 "..\..\Translation\Views\Sync.cshtml"
                                                                                                                                                                
                    }

            
            #line default
            #line hidden
WriteLiteral("                </th>\r\n                <th");

WriteLiteral(" class=\"monospaceCell\"");

WriteLiteral(">\r\n");

WriteLiteral("                    ");

            
            #line 84 "..\..\Translation\Views\Sync.cshtml"
               Write(locType.Description);

            
            #line default
            #line hidden
WriteLiteral("\r\n                </th>\r\n                <th");

WriteLiteral(" class=\"smallCell monospaceCell\"");

WriteLiteral(">\r\n");

            
            #line 87 "..\..\Translation\Views\Sync.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 87 "..\..\Translation\Views\Sync.cshtml"
                     if (hasGender && hasPlural)
                    {
                        
            
            #line default
            #line hidden
            
            #line 89 "..\..\Translation\Views\Sync.cshtml"
                    Write(locType.Gender != null ? NaturalLanguageTools.GetPronom(locType.Gender.Value, plural: true, culture: locType.Assembly.Culture) : "-");

            
            #line default
            #line hidden
            
            #line 89 "..\..\Translation\Views\Sync.cshtml"
                                                                                                                                                               
                    }

            
            #line default
            #line hidden
WriteLiteral("                </th>\r\n                <th");

WriteLiteral(" class=\"monospaceCell\"");

WriteLiteral(">\r\n");

            
            #line 93 "..\..\Translation\Views\Sync.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 93 "..\..\Translation\Views\Sync.cshtml"
                     if (hasPlural)
                    {
                        
            
            #line default
            #line hidden
            
            #line 95 "..\..\Translation\Views\Sync.cshtml"
                    Write(locType.PluralDescription ?? "-");

            
            #line default
            #line hidden
            
            #line 95 "..\..\Translation\Views\Sync.cshtml"
                                                           
                    }

            
            #line default
            #line hidden
WriteLiteral("                </th>\r\n            </tr>\t \r\n");

            
            #line 99 "..\..\Translation\Views\Sync.cshtml"
                }

                {
                    var locType = typeChanges.Type;

                    var hasGender = hasGenderOption && NaturalLanguageTools.HasGenders(culture);
                    

            
            #line default
            #line hidden
WriteLiteral("            <tr>\r\n                <th");

WriteLiteral(" class=\"leftCell\"");

WriteLiteral(">");

            
            #line 107 "..\..\Translation\Views\Sync.cshtml"
                                Write(culture.Name);

            
            #line default
            #line hidden
WriteLiteral("</th>\r\n                <th");

WriteLiteral(" class=\"smallCell monospaceCell\"");

WriteLiteral(">\r\n");

            
            #line 109 "..\..\Translation\Views\Sync.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 109 "..\..\Translation\Views\Sync.cshtml"
                     if (hasGender)
                    {
                        var gd = NaturalLanguageTools.GenderDetectors.TryGetC(locType.Assembly.Culture.TwoLetterISOLanguageName);

                        var list = gd.Try(a => a.Pronoms).EmptyIfNull()
                                .Select(pi => new SelectListItem { Value = pi.Gender.ToString(), Text = pi.Singular, Selected = pi.Gender == locType.Gender }).ToList();

                        if (typeChanges.Type.Gender == null)
                        {
                            list.Insert(0, new SelectListItem { Value = "", Text = "-", Selected = true });
                        }
                        
            
            #line default
            #line hidden
            
            #line 120 "..\..\Translation\Views\Sync.cshtml"
                   Write(Html.DropDownList(locKey(typeChanges.Type) + ".Gender", list));

            
            #line default
            #line hidden
            
            #line 120 "..\..\Translation\Views\Sync.cshtml"
                                                                                      ;
                    }

            
            #line default
            #line hidden
WriteLiteral("\r\n                </th>\r\n                <th");

WriteLiteral(" class=\"monospaceCell\"");

WriteLiteral(">\r\n");

            
            #line 125 "..\..\Translation\Views\Sync.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 125 "..\..\Translation\Views\Sync.cshtml"
                        
                    var items = selectListItems(typeChanges.TypeConflict.Values.Select(a => a.Translated));

                    var elemName = locKey(typeChanges.Type) + ".Description";
                    if(items.Count ==1){

            
            #line default
            #line hidden
WriteLiteral("                        <textarea");

WriteAttribute("name", Tuple.Create(" name=\"", 5426), Tuple.Create("\"", 5442)
            
            #line 130 "..\..\Translation\Views\Sync.cshtml"
, Tuple.Create(Tuple.Create("", 5433), Tuple.Create<System.Object, System.Int32>(elemName
            
            #line default
            #line hidden
, 5433), false)
);

WriteLiteral(" style=\"width:90%\"");

WriteLiteral(">");

            
            #line 130 "..\..\Translation\Views\Sync.cshtml"
                                                                Write(items.First());

            
            #line default
            #line hidden
WriteLiteral("</textarea>\r\n");

WriteLiteral("                        <button");

WriteLiteral(" class=\"rememberChange\"");

WriteLiteral(">");

            
            #line 131 "..\..\Translation\Views\Sync.cshtml"
                                                  Write(TranslationJavascriptMessage.RememberChange.NiceToString());

            
            #line default
            #line hidden
WriteLiteral("</button>\r\n");

            
            #line 132 "..\..\Translation\Views\Sync.cshtml"
                    }else{
                        
            
            #line default
            #line hidden
            
            #line 133 "..\..\Translation\Views\Sync.cshtml"
                   Write(Html.DropDownList(elemName, items, new { style = "width:90%" }));

            
            #line default
            #line hidden
            
            #line 133 "..\..\Translation\Views\Sync.cshtml"
                                                                                        ;

            
            #line default
            #line hidden
WriteLiteral("                         <a");

WriteLiteral(" href=\"#\"");

WriteLiteral(" class=\"edit\"");

WriteLiteral(">");

            
            #line 134 "..\..\Translation\Views\Sync.cshtml"
                                             Write(TranslationMessage.Edit.NiceToString());

            
            #line default
            #line hidden
WriteLiteral("</a>\r\n");

            
            #line 135 "..\..\Translation\Views\Sync.cshtml"
                    }
                    
            
            #line default
            #line hidden
WriteLiteral("\r\n                   \r\n                </th>\r\n                <th");

WriteLiteral(" class=\"smallCell monospaceCell\"");

WriteLiteral(">\r\n");

            
            #line 140 "..\..\Translation\Views\Sync.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 140 "..\..\Translation\Views\Sync.cshtml"
                     if (hasGender && hasPlural)
                    {
                        
            
            #line default
            #line hidden
            
            #line 142 "..\..\Translation\Views\Sync.cshtml"
                    Write(locType.Gender != null ? NaturalLanguageTools.GetPronom(locType.Gender.Value, plural: true, culture: locType.Assembly.Culture) : "-");

            
            #line default
            #line hidden
            
            #line 142 "..\..\Translation\Views\Sync.cshtml"
                                                                                                                                                                  
                    }

            
            #line default
            #line hidden
WriteLiteral("                </th>\r\n                <th");

WriteLiteral(" class=\"monospaceCell\"");

WriteLiteral(">\r\n");

            
            #line 146 "..\..\Translation\Views\Sync.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 146 "..\..\Translation\Views\Sync.cshtml"
                     if (hasPlural)
                    {
                        
            
            #line default
            #line hidden
            
            #line 148 "..\..\Translation\Views\Sync.cshtml"
                   Write(Html.TextArea(locKey(locType) + ".PluralDescription", locType.PluralDescription, new { style = "width:90%;height:16px" }));

            
            #line default
            #line hidden
            
            #line 148 "..\..\Translation\Views\Sync.cshtml"
                                                                                                                                                     
                    }

            
            #line default
            #line hidden
WriteLiteral("                </th>\r\n            </tr>\r\n");

            
            #line 152 "..\..\Translation\Views\Sync.cshtml"
                
                }
            }

            {

                var locType = typeChanges.Type;
                foreach (var conflict in typeChanges.MemberConflicts)
                {


            
            #line default
            #line hidden
WriteLiteral("            <tr>\r\n                <th");

WriteLiteral(" class=\"leftCell\"");

WriteLiteral(">");

            
            #line 163 "..\..\Translation\Views\Sync.cshtml"
                                Write(TranslationMessage.Member.NiceToString());

            
            #line default
            #line hidden
WriteLiteral("\r\n                </th>\r\n                <th");

WriteLiteral(" colspan=\"4\"");

WriteLiteral(">");

            
            #line 165 "..\..\Translation\Views\Sync.cshtml"
                           Write(conflict.Key);

            
            #line default
            #line hidden
WriteLiteral("\r\n                </th>\r\n            </tr>\r\n");

            
            #line 168 "..\..\Translation\Views\Sync.cshtml"
                    foreach (var mc in conflict.Value)
                    {

            
            #line default
            #line hidden
WriteLiteral("            <tr>\r\n                <td");

WriteLiteral(" class=\"leftCell\"");

WriteLiteral(">");

            
            #line 171 "..\..\Translation\Views\Sync.cshtml"
                                Write(mc.Key.Name);

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n                <td");

WriteLiteral(" colspan=\"4\"");

WriteLiteral(" class=\"monospaceCell\"");

WriteLiteral(">\r\n");

WriteLiteral("                    ");

            
            #line 173 "..\..\Translation\Views\Sync.cshtml"
               Write(mc.Value.Original);

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n                </td>\r\n            </tr>\r\n");

            
            #line 177 "..\..\Translation\Views\Sync.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral("            <tr>\r\n                <td");

WriteLiteral(" class=\"leftCell\"");

WriteLiteral(">");

            
            #line 179 "..\..\Translation\Views\Sync.cshtml"
                                Write(culture.Name);

            
            #line default
            #line hidden
WriteLiteral("</td>\r\n                <td");

WriteLiteral(" colspan=\"4\"");

WriteLiteral(" class=\"monospaceCell\"");

WriteLiteral(">\r\n");

            
            #line 181 "..\..\Translation\Views\Sync.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 181 "..\..\Translation\Views\Sync.cshtml"
                        
                    var items = selectListItems(conflict.Value.Values.Select(a => a.Translated));
                    var elemName = locKey(typeChanges.Type) + ".Member." + conflict.Key;
                    if(items.Count == 1)
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <textarea");

WriteAttribute("name", Tuple.Create(" name=\"", 7757), Tuple.Create("\"", 7773)
            
            #line 186 "..\..\Translation\Views\Sync.cshtml"
, Tuple.Create(Tuple.Create("", 7764), Tuple.Create<System.Object, System.Int32>(elemName
            
            #line default
            #line hidden
, 7764), false)
);

WriteLiteral(" style=\"width:90%\"");

WriteLiteral(">");

            
            #line 186 "..\..\Translation\Views\Sync.cshtml"
                                                                Write(items.First());

            
            #line default
            #line hidden
WriteLiteral("</textarea>\r\n");

WriteLiteral("                        <button");

WriteLiteral(" class=\"rememberChange\"");

WriteLiteral(">");

            
            #line 187 "..\..\Translation\Views\Sync.cshtml"
                                                  Write(TranslationJavascriptMessage.RememberChange.NiceToString());

            
            #line default
            #line hidden
WriteLiteral("</button>\r\n");

            
            #line 188 "..\..\Translation\Views\Sync.cshtml"
                    }else{
                         
            
            #line default
            #line hidden
            
            #line 189 "..\..\Translation\Views\Sync.cshtml"
                    Write(Html.DropDownList(elemName, items));

            
            #line default
            #line hidden
            
            #line 189 "..\..\Translation\Views\Sync.cshtml"
                                                            ;

            
            #line default
            #line hidden
WriteLiteral("                        <a");

WriteLiteral(" href=\"#\"");

WriteLiteral(" class=\"edit\"");

WriteLiteral(">");

            
            #line 190 "..\..\Translation\Views\Sync.cshtml"
                                            Write(TranslationMessage.Edit.NiceToString());

            
            #line default
            #line hidden
WriteLiteral("</a>\r\n");

            
            #line 191 "..\..\Translation\Views\Sync.cshtml"
                    }
                    
            
            #line default
            #line hidden
WriteLiteral("\r\n                  \r\n                </td>\r\n            </tr>\r\n");

            
            #line 196 "..\..\Translation\Views\Sync.cshtml"
                }
            }

        }

            
            #line default
            #line hidden
WriteLiteral("    </table>\r\n");

WriteLiteral("    <input");

WriteLiteral(" type=\"submit\"");

WriteAttribute("value", Tuple.Create(" value=\"", 8323), Tuple.Create("\"", 8370)
            
            #line 201 "..\..\Translation\Views\Sync.cshtml"
, Tuple.Create(Tuple.Create("", 8331), Tuple.Create<System.Object, System.Int32>(TranslationMessage.Save.NiceToString()
            
            #line default
            #line hidden
, 8331), false)
);

WriteLiteral(" />\r\n");

            
            #line 202 "..\..\Translation\Views\Sync.cshtml"
    }
}

            
            #line default
            #line hidden
WriteLiteral("\r\n<script>\r\n    $(function () {\r\n");

WriteLiteral("        ");

            
            #line 207 "..\..\Translation\Views\Sync.cshtml"
    Write(TranslationClient.Module["pluralAndGender"]());

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("        ");

            
            #line 208 "..\..\Translation\Views\Sync.cshtml"
    Write(TranslationClient.Module["editAndRemember"](TranslationClient.Translator is ITranslatorWithFeedback));

            
            #line default
            #line hidden
WriteLiteral("\r\n    });\r\n</script>\r\n");

        }
    }
}
#pragma warning restore 1591
