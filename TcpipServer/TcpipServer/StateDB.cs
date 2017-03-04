using System;
namespace TcpipServer
{
	public interface IStateDB
	{
		string Locking(StateDB sdb);
		string Unlocking(StateDB sdb);
	}

	public class StateDB
	{
		public IStateDB _state { get; set; }

		public StateDB(IStateDB sdb)
		{
			_state = sdb;
		}

		public void Locking()
		{
			_state.Locking(this);
		}

		public void Unlocking()
		{
			_state.Unlocking(this);
		}
	}

	public class LockDBState : IStateDB
	{
		public string Locking(StateDB state)
		{
			return "Бд уже блокировано";
		}

		public string Unlocking(StateDB state)
		{
			state._state = new UnlockDBState();
			return "Разблокируем бд";
		}
	}

	public class UnlockDBState : IStateDB
	{
		public string Locking(StateDB state)
		{
			state._state = new LockDBState();
			return "Блокируем бд";
		}

		public string Unlocking(StateDB state)
		{
			return "Бд уже разблокировано";
		}
	}
}