﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18051
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Signum.Web.Extensions.SMS.Views
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
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
    
    #line 1 "..\..\SMS\Views\SMSMessage.cshtml"
    using Signum.Engine;
    
    #line default
    #line hidden
    
    #line 2 "..\..\SMS\Views\SMSMessage.cshtml"
    using Signum.Entities.SMS;
    
    #line default
    #line hidden
    
    #line 4 "..\..\SMS\Views\SMSMessage.cshtml"
    using Signum.Utilities;
    
    #line default
    #line hidden
    
    #line 3 "..\..\SMS\Views\SMSMessage.cshtml"
    using Signum.Web;
    
    #line default
    #line hidden
    
    #line 5 "..\..\SMS\Views\SMSMessage.cshtml"
    using Signum.Web.SMS;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/SMS/Views/SMSMessage.cshtml")]
    public partial class SMSMessage : System.Web.Mvc.WebViewPage<dynamic>
    {
        public SMSMessage()
        {
        }
        public override void Execute()
        {





WriteLiteral("\r\n");


            
            #line 7 "..\..\SMS\Views\SMSMessage.cshtml"
Write(Html.RegisterUrls(new Dictionary<string, string> 
{ 
    { "getDictionaries", Url.Action<SMSController>(s => s.GetDictionaries()) },
    { "removeCharacters", Url.Action<SMSController>(s => s.RemoveNoSMSCharacters("")) }
}));

            
            #line default
            #line hidden
WriteLiteral("\r\n");


            
            #line 12 "..\..\SMS\Views\SMSMessage.cshtml"
Write(Html.ScriptCss("~/SMS/Content/SF_SMS.css"));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");


            
            #line 14 "..\..\SMS\Views\SMSMessage.cshtml"
 using (var e = Html.TypeContext<SMSMessageDN>())
{   
    
            
            #line default
            #line hidden
            
            #line 16 "..\..\SMS\Views\SMSMessage.cshtml"
Write(Html.ValueLine(e, s => s.MessageID, vl =>
    {
        vl.Visible = !e.Value.IsNew;
        vl.ReadOnly = true;
    }));

            
            #line default
            #line hidden
            
            #line 20 "..\..\SMS\Views\SMSMessage.cshtml"
      
    
            
            #line default
            #line hidden
            
            #line 21 "..\..\SMS\Views\SMSMessage.cshtml"
Write(Html.EntityLine(e, s => s.Template, vl =>
    {
        vl.Create = false;
        vl.Remove = false;
        vl.HideIfNull = true;
    }));

            
            #line default
            #line hidden
            
            #line 26 "..\..\SMS\Views\SMSMessage.cshtml"
      
    
            
            #line default
            #line hidden
            
            #line 27 "..\..\SMS\Views\SMSMessage.cshtml"
Write(Html.ValueLine(e, s => s.Certified, vl => vl.ReadOnly = (e.Value.State != SMSMessageState.Created)));

            
            #line default
            #line hidden
            
            #line 27 "..\..\SMS\Views\SMSMessage.cshtml"
                                                                                                        
    
            
            #line default
            #line hidden
            
            #line 28 "..\..\SMS\Views\SMSMessage.cshtml"
Write(Html.ValueLine(e, s => s.DestinationNumber, vl => vl.ReadOnly = !e.Value.IsNew));

            
            #line default
            #line hidden
            
            #line 28 "..\..\SMS\Views\SMSMessage.cshtml"
                                                                                    


            
            #line default
            #line hidden
WriteLiteral("    <div class=\"sf-sms-edit-container\">\r\n        ");


            
            #line 31 "..\..\SMS\Views\SMSMessage.cshtml"
   Write(Html.ValueLine(e, s => s.Message, vl =>
        {
            vl.ValueLineType = ValueLineType.TextArea;
            vl.ValueHtmlProps["cols"] = "30";
            vl.ValueHtmlProps["rows"] = "6";
            vl.ValueHtmlProps["class"] = "sf-sms-msg-text";
            vl.ReadOnly = (!e.Value.EditableMessage || e.Value.State != SMSMessageState.Created);
            vl.WriteHiddenOnReadonly = true;
        }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");


            
            #line 40 "..\..\SMS\Views\SMSMessage.cshtml"
         if (e.Value.State == SMSMessageState.Created && e.Value.EditableMessage)
        {

            
            #line default
            #line hidden
WriteLiteral("            <div class=\"sf-sms-characters-left\">\r\n                <p><span>");


            
            #line 43 "..\..\SMS\Views\SMSMessage.cshtml"
                    Write(SmsMessage.RemainingCharacters.NiceToString());

            
            #line default
            #line hidden
WriteLiteral("</span>: <span class=\"sf-sms-chars-left\"></span></p>\r\n            </div>\r\n");



WriteLiteral("            <div>\r\n                <input type=\"button\" class=\"sf-button sf-sms-r" +
"emove-chars\" value=\"");


            
            #line 46 "..\..\SMS\Views\SMSMessage.cshtml"
                                                                             Write(SmsMessage.RemoveNonValidCharacters.NiceToString());

            
            #line default
            #line hidden
WriteLiteral("\" />\r\n            </div>\r\n");


            
            #line 48 "..\..\SMS\Views\SMSMessage.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("    </div>\r\n");



WriteLiteral("    <br />\r\n");


            
            #line 51 "..\..\SMS\Views\SMSMessage.cshtml"
    
            
            #line default
            #line hidden
            
            #line 51 "..\..\SMS\Views\SMSMessage.cshtml"
Write(Html.ValueLine(e, s => s.From, vl => vl.ReadOnly = (e.Value.State != SMSMessageState.Created)));

            
            #line default
            #line hidden
            
            #line 51 "..\..\SMS\Views\SMSMessage.cshtml"
                                                                                                   

    if (e.Value.State != SMSMessageState.Created)
    {
        
            
            #line default
            #line hidden
            
            #line 55 "..\..\SMS\Views\SMSMessage.cshtml"
   Write(Html.ValueLine(e, s => s.SendDate, vl => vl.ReadOnly = true));

            
            #line default
            #line hidden
            
            #line 55 "..\..\SMS\Views\SMSMessage.cshtml"
                                                                     
        
            
            #line default
            #line hidden
            
            #line 56 "..\..\SMS\Views\SMSMessage.cshtml"
   Write(Html.ValueLine(e, s => s.State, vl => vl.ReadOnly = true));

            
            #line default
            #line hidden
            
            #line 56 "..\..\SMS\Views\SMSMessage.cshtml"
                                                                  
    }
}

            
            #line default
            #line hidden

            
            #line 59 "..\..\SMS\Views\SMSMessage.cshtml"
Write(Html.ScriptsJs("~/SMS/Scripts/SF_SMS.js"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");


        }
    }
}
#pragma warning restore 1591
