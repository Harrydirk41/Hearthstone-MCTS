using System;
using System.Collections.Generic;
using System.Linq;
using SabberStoneAICompetition.PartialObservation;
using SabberStoneAICompetition.Score;
using SabberStoneCore.Enums;
using SabberStoneCore.Model.Entities;
using SabberStoneCore.Tasks.PlayerTasks;

namespace SabberStoneAICompetition.MCTSAlgorithm.MCTS
{

    class MctsNode
    {
	    private int _playerId;

		private double _score;

		private List<ScoreExt> _scorings;

		private MctsNode _parent;

		private List<MctsNode> _children;

		private PlayerTask _task;

		private POGame _game;

		private POGame _gameCopy;

		private int _gameState;

		private int _endTurn;

		public int PlayerId
		{
			get
			{
				return _playerId;
			}
		}

		public double VisitCount
		{
			get; set;
		}

		public double TotalScore
		{
			get; set;
		}

		public double Score
		{
			get
			{
				return _score;
			}
		}

		public List<ScoreExt> Scorings
		{
			get
			{
				return _scorings;
			}
		}

		public bool IsLeaf
		{
			get
			{
				return (_children == null || (_children.Count < 1));
			}
		}

		public MctsNode Parent
		{
			get
			{
				return _parent;
			}
		}

		public List<MctsNode> Children
		{
			get
			{
				return _children;
			}
			set
			{
				_children = value;
			}
		}

		public POGame Game
		{
			get
			{
				return _game;
			}

			private set
			{
				_game = value;
			}
		}

		public Controller PlayerController
		{
			get
			{
				if (Game != null)
				{
					return (Game.CurrentPlayer.Id == _playerId) ?
						Game.CurrentPlayer : Game.CurrentOpponent;
				}
				return null;
			}
		}

		public bool IsEndTurn
		{
			get
			{
				return (_endTurn > 0 || (Task != null && Task.PlayerTaskType == PlayerTaskType.END_TURN));
			}
		}

		public bool IsRunning
		{
			get
			{
				return (_gameState == 0);
			}
		}

		public bool IsWon
		{
			get
			{
				return (_gameState > 0);
			}
		}

		public bool IsLost
		{
			get
			{
				return (_gameState < 0);
			}
		}

		public List<PlayerTask> Tasks
		{
			get
			{
				if (PlayerController != null)
				{
					return PlayerController.Options();
				}
				return new List<PlayerTask>();
			}
		}

		public PlayerTask Task
		{
			get
			{
				return _task;
			}
		}

		public MctsNode(POGame game, PlayerTask task, MctsNode parent)
			: this(parent.PlayerId, parent.Scorings, game, task, parent) { }

		public MctsNode(int playerId, List<ScoreExt> scorings, POGame game, PlayerTask task, MctsNode parent)
		{
			_parent = parent;
			_scorings = scorings;
			_playerId = playerId;
			_game = game.getCopy();
			_task = task;

			VisitCount = 1;

			if (Task != null)
			{
				Dictionary<PlayerTask, POGame> dir = Game.Simulate(new List<PlayerTask> { Task });
				POGame newGame = dir[Task];

				Game = newGame;
				// simulation has failed, maybe reduce score?
				if (Game == null)
				{

					_endTurn = 1;
				}
				else
				{

					_gameState = Game.State == SabberStoneCore.Enums.State.RUNNING ? 0
						: (PlayerController.PlayState == PlayState.WON ? 1 : -1);
					_endTurn = Game.CurrentPlayer.Id != _playerId ? 1 : 0;

					foreach (ScoreExt scoring in Scorings)
					{
						scoring.Controller = PlayerController;
						_score += scoring.Value * scoring.Rate();
					}
					_score /= Scorings.Count;
					TotalScore += _score;
				}
			}
		}

		public List<PlayerTask> GetSolution()
		{
			var solutions = new List<PlayerTask>();
			MctsNode bestChild = this;
			while (!bestChild.IsEndTurn && !bestChild.IsLeaf)
			{
				solutions.Add(bestChild.Task);
				bestChild = bestChild.Children.OrderByDescending(c => c.TotalScore).First();
			}

			if (bestChild.IsEndTurn || bestChild.IsLeaf)
			{
				solutions.Add(bestChild.Task);
				return solutions;
			}

			return solutions;
		}

		public int ParentCount()
		{
			int count = 0;
			MctsNode child = this;
			while (child.Parent != null)
			{
				child = child.Parent;
				count++;
			}
			return count;
		}

		public class ScoreExt : IScore
		{
			private IScore _score;

			public double Value
			{
				get; private set;
			}

			public Controller Controller
			{
				get
				{
					return _score.Controller;
				}

				set
				{
					_score.Controller = value;
				}

			}

			public ScoreExt(double value, IScore score)
			{
				Value = value;
				_score = score;
			}

			public Func<List<IPlayable>, List<int>> MulliganRule()
			{
				return _score.MulliganRule();
			}

			public int Rate()
			{
				return _score.Rate();
			}
		}

	}
}
