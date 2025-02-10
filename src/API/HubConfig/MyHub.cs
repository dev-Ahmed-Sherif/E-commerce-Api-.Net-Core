using Microsoft.AspNetCore.SignalR;

namespace E_commerce_Api.HubConfig
{
    public class MyHub : Hub
    {
        public async Task AskServer(string someTextFromClient)
        {
            string tempString;

            if (someTextFromClient == "hey")
            {
                tempString = "hello from server";
            }
            else
            {
                tempString = "message was something else";
            }

            await Clients.Client(this.Context.ConnectionId).SendAsync("askServerResponse", tempString);
        }
    }
}
