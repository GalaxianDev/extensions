﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Signum.Engine.Authorization;
using Signum.Entities;
using Signum.Entities.Authorization;
using Signum.Services;
using Signum.Utilities;
using Signum.React.Facades;
using Signum.React.Authorization;
using Signum.Entities.Omnibox;
using Signum.Entities.Files;
using Signum.Engine;
using System.Web;
using Signum.Engine.Files;
using System.IO;
using System.Net.Http.Headers;

namespace Signum.React.Files
{
    public class FilesController : ApiController
    {
        [Route("api/files/downloadFile/{fileId}"), HttpGet]
        public HttpResponseMessage DownloadFile(string fileId)
        {
            var file = Database.Retrieve<FileEntity>(PrimaryKey.Parse(fileId, typeof(FileEntity)));
            
            return GetHttpReponseMessage(new System.IO.MemoryStream(file.BinaryFile), file.FileName);

        }

        [Route("api/files/downloadFilePath/{filePathId}"), HttpGet]
        public HttpResponseMessage DownloadFilePath(string filePathId)
        {
            var filePath = Database.Retrieve<FilePathEntity>(PrimaryKey.Parse(filePathId, typeof(FilePathEntity)));

            return GetHttpReponseMessage(File.OpenRead(filePath.FullPhysicalPath), filePath.FileName);
        }

        [Route("api/files/downloadEmbeddedFilePath/{fileType}"), HttpGet]
        public HttpResponseMessage DownloadEmbeddedFilePath(string fileTypeKey, string suffix, string fileName)
        {
            var fileType = SymbolLogic<FileTypeSymbol>.ToSymbol(fileTypeKey);

            var virtualFile = new EmbeddedFilePathEntity(fileType)
            {
                Suffix = suffix,
                FileName = fileName
            };

            var pair = FileTypeLogic.FileTypes.GetOrThrow(fileType).GetPrefixPair(virtualFile);

            var fullPhysicalPath = Path.Combine(pair.PhysicalPrefix, suffix);
            
            return GetHttpReponseMessage(File.OpenRead(fullPhysicalPath), fullPhysicalPath);
        }


        public static HttpResponseMessage GetHttpReponseMessage(Stream stream, string fileName)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(stream);
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = Path.GetFileName(fileName)
            };
            var mime = MimeMapping.GetMimeMapping(fileName);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(mime);
            return response;
        }
    }
}