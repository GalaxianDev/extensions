﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;
using Signum.Utilities;
using Signum.Entities;
using System.Windows;
using System.Linq.Expressions;
using Signum.Entities.Reflection;
using Signum.Entities.Basics;

namespace Signum.Windows.UIAutomation
{
    public class NormalWindowProxy<T> : WindowProxy, ILineContainer<T> where T : ModifiableEntity
    {
        public WindowProxy ParentWindow { get { return this; } }
        public PropertyRoute PreviousRoute { get; set; }

        AutomationElement ILineContainer.Element
        {
            get { return this.MainControl; }
        }

        public NormalWindowProxy(AutomationElement element)
            : base(element)
        {
            element.AssertClassName("NormalWindow");

            Element.WaitDataContextSet(() => "DataContextSet for {0}".Formato(typeof(T).Name));
        }

        public ButtonBarProxy ButtonBar
        {
            get { return new ButtonBarProxy(Element.ChildById("buttonBar")); }
        }

        public LeftPanelProxy LeftExpander
        {
            get { return new LeftPanelProxy(Element.ChildById("widgetPanel").ChildById("expander")); }
        }

        AutomationElement mainControl;
        public AutomationElement MainControl
        {
            get { return mainControl ?? (mainControl = Element.Child(a => a.Current.ClassName == "ScrollViewer").Child(a => a.Current.ControlType != ControlType.ScrollBar)); }
        }

        public string EntityId
        {
            get { return Element.ChildById("entityTitle").ChildById("tbEntityId").Value(); }
        }

        public string EntityToStr
        {
            get { return Element.ChildById("entityTitle").ChildById("tbEntityToStr").Value(); }
        }

        public void Ok()
        {
            var entityId = EntityId;
            ButtonBar.OkButton.ButtonInvoke();
            Element.Wait(() => IsClosed,
            actionDescription: () => "Waiting to close window after OK {0}".Formato(entityId));
        }

        public AutomationElement OkCapture()
        {
            var entityId = EntityId;
            return Element.CaptureWindow(
            action: () => ButtonBar.OkButton.ButtonInvoke(),
            actionDescription: () => "Waiting to capture window after OK {0}".Formato(entityId));
        }

        public void Reload()
        {
            var entityId = EntityId;
            Element.WaitDataContextChangedAfter(
            action: () => ButtonBar.ReloadButton.ButtonInvoke(),
            actionDescription: () => "Reload " + entityId);
        }

        public void Reload(bool confirm)
        {
            ButtonBar.ReloadButton.ButtonInvoke();

            using (var mb = Element.WaitMessageBoxChild())
            {
                if (confirm)
                    mb.OkButton.ButtonInvoke();
                else
                    mb.CancelButton.ButtonInvoke();
            }
        }
        public void Execute(Enum operationKey, int? timeOut = null)
        {
            var time = timeOut ?? OperationTimeouts.ExecuteTimeout;
            var entityId = EntityId;
            Element.WaitDataContextChangedAfter(
            action: () => ButtonBar.GetButton(operationKey).ButtonInvoke(),
            actionDescription: () => "Executing {0} from {1}".Formato(OperationDN.UniqueKey(operationKey), entityId));
        }

        public AutomationElement ExecuteCapture(Enum operationKey, int? timeOut = null)
        {
            var time = timeOut ?? OperationTimeouts.ExecuteTimeout;
            var entityId = EntityId;
            return Element.CaptureWindow(
            action: () => ButtonBar.GetButton(operationKey).ButtonInvoke(),
            actionDescription: () => "Executing {0} from {1} and waiting to capture window".Formato(OperationDN.UniqueKey(operationKey), entityId));
        }


        public AutomationElement ConstructFromCapture(Enum operationKey, int? timeOut = null)
        {
            var time = timeOut ?? OperationTimeouts.ConstructFromTimeout;
            var entityId = EntityId;
            return Element.CaptureWindow(
                () => GetConstructorButton(operationKey).ButtonInvoke(),
                () => "Finding a window after {0} from {1} took more than {2} ms".Formato(OperationDN.UniqueKey(operationKey), entityId, time));
        }

        private AutomationElement GetConstructorButton(Enum operationKey)
        {
            var result = ButtonBar.TryGetButton(operationKey);
            if (result != null)
                return result;

            result = ButtonBar.TryGetGroupButton(operationKey, "Create");
            if (result != null)
                return result;

            result = (from sc in MainControl.Descendants(a => a.Current.ClassName == "SearchControl").Select(sc => new SearchControlProxy(sc))
                      select sc.GetOperationButton(operationKey)).NotNull().FirstOrDefault();
            if (result != null)
                return result;

            throw new ElementNotFoundException("Button for operation {0} not found on the ButtonBar or any visible SearchControl".Formato(OperationDN.UniqueKey(operationKey)));
        }

        public NormalWindowProxy<R> ConstructFrom<R>(Enum operationKey, int? timeOut = null) where R : IdentifiableEntity
        {
            AutomationElement element = ConstructFromCapture(operationKey, timeOut);

            return new NormalWindowProxy<R>(element);
        }


        public override void Dispose()
        {
            if (base.Close())
            {
                MessageBoxProxy confirmation = null;

                Element.Wait(() =>
                {
                    if (IsClosed)
                        return true;

                    confirmation = Element.TryMessageBoxChild();

                    if (confirmation != null)
                        return true;

                    return false;
                }, () => "Waiting for normal window to close or show confirmation dialog");


                if (confirmation != null && !confirmation.IsError)
                {
                    confirmation.OkButton.ButtonInvoke();
                }
            }

            OnDisposed();
        }

        public void CloseLooseChanges()
        {
            Element.CaptureChildWindow(
                () => Close(), 
                ()=>"Waiting for loose changes dialog");
        }
    }

    public static class NormalWindowExtensions
    {
        public static NormalWindowProxy<T> ToNormalWindow<T>(this AutomationElement element) where T : ModifiableEntity
        {
            return new NormalWindowProxy<T>(element);
        }

        public static Lite<IIdentifiable> ParseLiteHash(string itemStatus)
        {
            if (string.IsNullOrEmpty(itemStatus))
                return null;

            return Signum.Entities.Lite.Parse(itemStatus.Split(new[] { " Hash:" }, StringSplitOptions.None)[0]);
        }

        public static Lite<T> Lite<T>(this NormalWindowProxy<T> entity) where T : IdentifiableEntity
        {
            return (Lite<T>)NormalWindowExtensions.ParseLiteHash(entity.Element.Current.ItemStatus);
        }
    }


    public class OperationTimeouts
    {
        public static int ExecuteTimeout = 3 * 1000;
        public static int ConstructFromTimeout = 2 * 1000;
    }


    public class ButtonBarProxy
    {
        public AutomationElement Element { get; private set; }

        public ButtonBarProxy(AutomationElement element)
        {
            Element = element;
        }

        public AutomationElement OkButton
        {
            get { return Element.ChildById("btOk"); }
        }

        public AutomationElement ReloadButton
        {
            get { return Element.ChildById("btReload"); }
        }

        public AutomationElement GetButton(Enum operationKey)
        {
            return Element.Child(a => a.Current.ControlType == ControlType.Button && a.Current.Name == OperationDN.UniqueKey(operationKey));
        }

        public AutomationElement TryGetButton(Enum operationKey)
        {
            return Element.TryChild(a => a.Current.ControlType == ControlType.Button && a.Current.Name == OperationDN.UniqueKey(operationKey));
        }

        public AutomationElement TryGetGroupButton(Enum operationKey, string groupName)
        {
            var groupButton = Element.TryChild(a => a.Current.ControlType == ControlType.Button && a.Current.Name == groupName);

            if (groupButton == null)
                return null;

            var window = Element.CaptureChildWindow(
                () => groupButton.ButtonInvoke(),
                actionDescription: () => "Waiting for ContextMenu after click on {0}".Formato(groupName));

            var menuItem = window
                .Child(a => a.Current.ControlType == ControlType.Menu)
                .TryChild(a => a.Current.ControlType == ControlType.MenuItem && a.Current.Name == OperationDN.UniqueKey(operationKey));

            return menuItem;
        }

        public AutomationElement GetGroupButton(Enum operationKey, string groupName)
        {
            var groupButton = Element.Child(a => a.Current.ControlType == ControlType.Button && a.Current.Name == groupName);

            var window = Element.CaptureChildWindow(
                () => groupButton.ButtonInvoke(),
                actionDescription: () => "Waiting for ContextMenu after click on {0}".Formato(groupName));

            var menuItem = window
                .Child(a => a.Current.ControlType == ControlType.Menu)
                .Child(a => a.Current.ControlType == ControlType.MenuItem && a.Current.Name == OperationDN.UniqueKey(operationKey));

            return menuItem;
        }
    }

    public class LeftPanelProxy
    {
        public AutomationElement Element {get;set;}

        public LeftPanelProxy(AutomationElement element)
        {
            this.Element = element;
        }

        public AutomationElement LeftExpanderButton
        {
            get { return Element.ChildById("HeaderSite"); }
        }
    }

    public static class QuickLinkExtensions
    {
        public static AutomationElement Button(this LeftPanelProxy left, string name)
        {
            return left.Element.Descendant(el => el.Current.ControlType == ControlType.Button && el.Current.Name == name);
        }

        public static AutomationElement InvokeQuickLink(this LeftPanelProxy left, string name)
        {
            return left.Element.CaptureWindow(
            action: () => left.Button(name).ButtonInvoke(),
            actionDescription: () => "Waiting to capture window after click {0} on LeftPanel".Formato(name));
        }
    }
}
