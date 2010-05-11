﻿#region usings
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using Signum.Services;
using Signum.Utilities;
using Signum.Entities;
using Signum.Web;
using Signum.Engine;
using Signum.Engine.Operations;
using Signum.Entities.Operations;
using Signum.Engine.Basics;
using Signum.Web.Extensions.Properties;
using System.IO;
using Signum.Entities.Files;
using Signum.Entities.Basics;
using System.Text;
using System.Net;
using Signum.Engine.Files;
using System.Reflection;
using Signum.Web.Controllers;
#endregion

namespace Signum.Web.Files
{
    [HandleException]
    public class FileController : Controller
    {
        [AcceptVerbs(HttpVerbs.Post)]
        public PartialViewResult PartialView(
            string prefix,
            string fileType,
            int? sfId)
        {
            Type type = typeof(FilePathDN);
            FilePathDN entity = null;
            if (entity == null || entity.GetType() != type || sfId != (entity as IIdentifiable).TryCS(e => e.IdOrNull))
            {
                if (sfId.HasValue)
                    entity = Database.Retrieve<FilePathDN>(sfId.Value);
                else
                {
                    entity = new FilePathDN(EnumLogic<FileTypeDN>.ToEnum(fileType));
                }
            }
            ViewData["IdValueField"] = prefix;
            ViewData["FileType"] = fileType;

            string sfUrl = Navigator.Manager.EntitySettings[type].OnPartialViewName(entity);

            return Navigator.PartialView(this, entity, prefix, sfUrl);
        }

        public ActionResult Upload()
        {
            FilePathDN fp = null;
            string formFieldId = "";
            foreach (string file in Request.Files)
            {
                RuntimeInfo info = RuntimeInfo.FromFormValue((string)Request.Form[TypeContextUtilities.Compose(file, EntityBaseKeys.RuntimeInfo)]);
                if (info.RuntimeType != typeof(FilePathDN))
                    continue;

                if (info.IdOrNull.HasValue)
                    continue; //Only new files will come with content

                HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                if (hpf.ContentLength == 0)
                    continue;

                string fileType = (string)Request.Form[TypeContextUtilities.Compose(file, FileLineKeys.FileType)];
                if (!fileType.HasText())
                    throw new InvalidOperationException(Resources.CouldntCreateFilePathWithUnknownFileTypeForField0.Formato(file));

                formFieldId = file; //This is the uploaded file

                fp = new FilePathDN(EnumLogic<FileTypeDN>.ToEnum(fileType))
                {
                    FileName = Path.GetFileName(hpf.FileName),
                    BinaryFile = hpf.InputStream.ReadAllBytes()
                }.Save();
            }

            StringBuilder sb = new StringBuilder();
            //Use plain javascript not to have to add also the reference to jquery in the result iframe
            sb.AppendLine("<html><head><title>-</title></head><body>");
            sb.AppendLine("<script type='text/javascript'>");
            sb.AppendLine("var parDoc = window.parent.document;");

            if (fp.TryCS(f => f.IdOrNull) != null)
            {
                sb.AppendLine("parDoc.getElementById('{0}loading').style.display='none';".Formato(formFieldId));
                sb.AppendLine("parDoc.getElementById('{0}').innerHTML='{1}';".Formato(TypeContextUtilities.Compose(formFieldId, EntityBaseKeys.ToStrLink), fp.FileName));
                sb.AppendLine("parDoc.getElementById('{0}').value='{1}';".Formato(TypeContextUtilities.Compose(formFieldId, EntityBaseKeys.RuntimeInfo), new RuntimeInfo(fp).ToString()));
                sb.AppendLine("parDoc.getElementById('{0}DivNew').style.display='none';".Formato(formFieldId));
                sb.AppendLine("parDoc.getElementById('{0}DivOld').style.display='block';".Formato(formFieldId));
            }
            else
            {
                sb.AppendLine("parDoc.getElementById('{0}loading').style.display='none';".Formato(formFieldId));
                sb.AppendLine("window.alert('{0}');".Formato(Resources.ErrorSavingFile));
            }

            sb.AppendLine("</script>");
            sb.AppendLine("</body></html>");

            return Content(sb.ToString());
        }

        public FileResult Download(int? filePathID)
        {
            if (filePathID == null)
                throw new ArgumentException(Resources.ArgumentFilePathIDWasNotPassedToTheController);

            FilePathDN fp = Database.Retrieve<FilePathDN>(filePathID.Value);

            if (fp == null)
                throw new ArgumentException(Resources.ArgumentFilePathIDWasNotPassedToTheController);

            byte[] binaryFile;

            binaryFile = fp.FullWebPath != null ? new WebClient().DownloadData(fp.FullWebPath)
                : FilePathLogic.GetByteArray(fp);

            throw new NotImplementedException();
            //return File(binaryFile, SignumController.GetMimeType(Path.GetExtension(fp.FileName)), fp.FileName);
            return null;
        }

        
    }
}
