﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Signum.Entities.Authorization;
using Signum.Utilities;
using System.Windows;
using Signum.Services;
using System.Reflection;
using System.Collections;
using Signum.Windows;
using System.Windows.Controls;
using Signum.Entities.Basics;
using Signum.Entities;
using Signum.Utilities.Reflection;
using Signum.Utilities.DataStructures;
using Signum.Windows.Operations;
using Signum.Windows.Omnibox;
using Signum.Windows.Extensions.Properties;
using System.IO;
using Microsoft.Win32;

namespace Signum.Windows.Authorization
{
    public static class AuthClient
    {
        public static event Action UpdateCacheEvent;

        public static void UpdateCache()
        {
            if (UpdateCacheEvent != null)
                UpdateCacheEvent();
        }

        public static void Start(bool types, bool property, bool queries, bool permissions, bool operations, bool facadeMethods, bool defaultPasswordExpiresLogic)
        {
            if (Navigator.Manager.NotDefined(MethodInfo.GetCurrentMethod()))
            {
                Server.Connecting += UpdateCache;

                if (types) TypeAuthClient.Start();
                if (property) PropertyAuthClient.Start();
                if (queries) QueryAuthClient.Start();
                if (permissions) PermissionAuthClient.Start();
                if (facadeMethods) FacadeMethodAuthClient.Start();
                if (operations) OperationAuthClient.Start();

                UpdateCache();

                Navigator.AddSetting(new EntitySettings<UserDN> { View = e => new User() });
                Navigator.AddSetting(new EntitySettings<RoleDN> { View = e => new Role() });

                if (defaultPasswordExpiresLogic)
                    Navigator.AddSetting(new EntitySettings<PasswordExpiresIntervalDN> { View = e => new PasswordExpiresInterval() });

                OperationClient.AddSetting(new EntityOperationSettings(UserOperation.SaveNew)
                {
                    IsVisible = e => e.Entity.IsNew,
                });

                OperationClient.AddSetting(new EntityOperationSettings(UserOperation.Save)
                {
                    IsVisible = e => !e.Entity.IsNew,
                });

                SpecialOmniboxProvider.Register(new SpecialOmniboxAction("UpdateAuthCache",
                    () => true,
                    win =>
                    {
                        UpdateCache();

                        MessageBox.Show(Resources.AuthorizationCacheSuccessfullyUpdated);
                    }));

                SpecialOmniboxProvider.Register(new SpecialOmniboxAction("DownloadAuthRules",
                    () => BasicPermission.AdminRules.TryIsAuthorized() ?? true,
                    win =>
                    {
                        SaveFileDialog sfc = new SaveFileDialog();

                        sfc.FileName = "AuthRules.xml";

                        if (sfc.ShowDialog() == true)
                        {
                            var bytes = Server.Return((ILoginServer ls) => ls.DownloadAuthRules());

                            File.WriteAllBytes(sfc.FileName, bytes);
                        }
                    }));
            }
        }
    }
}
