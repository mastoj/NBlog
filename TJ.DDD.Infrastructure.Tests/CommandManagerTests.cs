using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;
using TJ.DDD.Infrastructure.Command;
using TJ.DDD.Infrastructure.Exceptions;

namespace TJ.DDD.Infrastructure.Tests
{
    [TestFixture]
    public class When_Executing_An_Unregistered_Command
    {
        private CommandManager _commandManager;

        [TestFixtureSetUp]
        public void Setup()
        {
            _commandManager = new CommandManager(new CommandRepositoryStub());
        }

        [Test]
        public void Then_An_Unregistered_Command_Exception_Should_Be_Thrown()
        {
            // Assert
            Action act = () => _commandManager.Execute(new StubCommand());
            act.ShouldThrow<UnregisteredCommandException>();
        }
    }

    [TestFixture]
    public class When_Executing_A_Registered_Command
    {
        private CommandManager _commandManager;
        private StubCommandHandler _commandHandler;
        private StubCommand _command;

        [TestFixtureSetUp]
        public void Setup()
        {
            CommandRepositoryStub commandRepository = new CommandRepositoryStub();
            _commandHandler = new StubCommandHandler();
            commandRepository.AddHandler(_commandHandler);
            _commandManager = new CommandManager(commandRepository);
            _command = new StubCommand();
            _commandManager.Execute(_command);
        }

        [Test]
        public void Then_The_Command_Should_Be_Executed()
        {
            _commandHandler.ExecutedCommand.Should().BeSameAs(_command);
        }
    }
    public class StubCommandHandler : IHandle<StubCommand>
    {
        public StubCommand ExecutedCommand { get; set; }

        public void Execute(StubCommand command)
        {
            ExecutedCommand = command;
        }
    }

    public class StubCommand : Command.Command
    {
        public StubCommand()
            : base(Guid.NewGuid())
        {

        }
    }

    public class CommandRepositoryStub : ICommandRepository
    {
        private IDictionary<Type, object> _commandHandlers = new Dictionary<Type, object>();

        public IDictionary<Type, object> GetCommandHandlers()
        {
            return _commandHandlers;
        }

        public void AddHandler<TCommand>(IHandle<TCommand> commandHandler) where TCommand : ICommand
        {
            _commandHandlers.Add(typeof(TCommand), commandHandler);
        }
    }
}
