using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TJ.CQRS.Messaging;
using TJ.Extensions;

namespace NBlog.Areas.Admin.Controllers
{
    public abstract class CommandControllerBase : Controller
    {
        protected readonly ICommandBus _commandBus;
        public CommandControllerBase()
        {
        }
        public CommandControllerBase(ICommandBus commandBus)
        {
            _commandBus = commandBus;
        }

        protected ActionResult ValidateAndSendCommand(Command command, Func<ActionResult> successFunc, Func<ActionResult> failFunc, Func<bool> preCondition = null, Func<ActionResult> preConditionResult = null)
        {
            if (preCondition.IsNull() || preCondition())
            {
                if (ModelState.IsValid)
                {
                    _commandBus.Send(command);
                    return successFunc();
                }
            }
            else if (preConditionResult.IsNotNull())
            {
                return preConditionResult();
            }
            return failFunc();
        }
    }
}
