using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BluePrint.Client.Pages
{
    public partial class Authentication : ComponentBase
    {
        [Parameter] public string Action { get; set; }

        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationState { get; set; }

        public async void OnLogInSucceeded()
        {
            int a = 1;
        }
        public async void OnLogOutSucceeded()
        {

        }
    }
}
