using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBlog.Domain.Commands;
using NBlog.Domain.Event;
using NUnit.Framework;

namespace NBlog.Domain.Specs.User
{
    [TestFixture]
    public class When_Creating_A_User : BaseCommandTest<CreateUserCommand>
    {
        protected override CreateUserCommand When()
        {
            _createUserCommand = new CreateUserCommand()
                                     {
                                         Email = "something@something.com",
                                         Name = "Tomas Something",
                                         UserId = "ThisIsOpen"
                                     };
            return _createUserCommand;
        }

        [Test]
        public void Then_A_User_Created_Event_Is_Published()
        {
            base.GetPublishedEvents().OfType<UserCreatedEvent>();
        }

        private CreateUserCommand _createUserCommand;
    }
}
