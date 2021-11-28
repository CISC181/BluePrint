using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Blazor.Components;

namespace BluePrint.Client.Shared
{
    public partial class MainLayout : LayoutComponentBase
    {
        [Inject]
        NavigationManager NavManager { get; set; }

        TelerikDrawer<DrawerItem> DrawerRef { get; set; }

        List<DrawerItem> NavigablePages { get; set; } =
        new List<DrawerItem>
        {
            new DrawerItem { Text = "Home", Title="Home", Url = "/", Icon = "home" },
            new DrawerItem { IsSeparator = true, Url = string.Empty  },
            new DrawerItem { Text = "Students", Title="Students", Url = "Students", Icon = "user" },
            new DrawerItem { Text = "Post Accounting", Title="Post Accounting", Url = "postaccounting", Icon = "dollar" },
            new DrawerItem { Text = "Transfers", Title="Transfers", Url = "transfers", Icon = "arrows-swap" },
            new DrawerItem { Text = "Speakers", Title="Speakers", Url = "speakers", Icon = "myspace" },
            new DrawerItem { Text = "Reports", Title="Reports", Url = "reports", Icon = "subreport" },
            new DrawerItem { IsSeparator = true, Url = string.Empty  },
            new DrawerItem { Text = "Administration", Title="Administration", Url = "administration", Icon = "gear" }
            };

        DrawerItem SelectedItem { get; set; }

        string BuildNumner { get; set; } = "v01.0.0";
        string TopBarTitle { get; set; } = "SPARC";

        public class DrawerItem
        {
            public string Text { get; set; }
            public string Title { get; set; }
            public string Url { get; set; }
            public string Icon { get; set; }
            public bool IsSeparator { get; set; }
        }

        protected override void OnInitialized()
        {
            string currPage = GetCurrentPage().ToLowerInvariant();

            DrawerItem ActivePage = NavigablePages.FirstOrDefault(p => p.Url.ToLowerInvariant() == currPage);
            if (ActivePage != null)
            {
                SelectedItem = ActivePage;
            }

            base.OnInitialized();
        }

        public string GetCurrentPage()
        {
            string uriWithoutQueryString = NavManager.Uri.Split("?")[0];
            string currPage = uriWithoutQueryString.Substring(Math.Min(NavManager.Uri.Length, NavManager.BaseUri.Length));
            if (currPage.Contains("/"))
            {
                currPage = currPage.Split("/")[0];
            }
            return string.IsNullOrWhiteSpace(currPage) ? "/" : currPage;
        }
    }
}
