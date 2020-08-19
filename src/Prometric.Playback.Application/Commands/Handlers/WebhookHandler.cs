using Convey.CQRS.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Prometric.Playback.Application.Commands.Handlers
{
    public class WebhookHandler : ICommandHandler<Webhook>
    {
        public async Task HandleAsync(Webhook command)
        {
            
        }
    }
}   
