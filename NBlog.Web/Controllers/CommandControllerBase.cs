using System;
using System.Web.Mvc;
using TJ.CQRS.Messaging;
using TJ.Extensions;

namespace NBlog.Web.Controllers
{
    public abstract class CommandControllerBase : Controller
    {
        protected readonly ICommandBus _commandBus;
        public CommandControllerBase(ICommandBus commandBus)
        {
            _commandBus = commandBus;
        }

        protected ActionResult ValidateAndSendCommand(Command command, Func<ActionResult> successFunc, Func<ActionResult> failFunc, Func<ActionResult> validationFailFunc = null, Func<bool> preCondition = null, Func<ActionResult> preConditionResult = null)
        {
            if (preCondition.IsNull() || preCondition())
            {
                if (ModelState.IsValid)
                {
                    _commandBus.Send(command);
                    return successFunc();
                }
                else if(validationFailFunc.IsNotNull())
                {
                    return validationFailFunc();
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