using System.Collections.Generic;
using SabberStoneAICompetition.PartialObservation;
using SabberStoneAICompetition.Score;

namespace SabberStoneAICompetition.MCTSAlgorithm.MCTS
{
	/// <summary>
	/// The base class for performing the MCTS simulation.
	/// </summary>
	class MctsSimulator : AbstractMctsSimulator
	{
		/// <summary>
		/// The id of the player the MCTS simulates for.
		/// </summary>
		protected int _playerId;

		/// <summary>
		/// The scoring function of the player the MCTS simulates for.
		/// </summary>
		protected IScore _scoring;

		/// <summary>
		/// TODO: API
		/// </summary>
		protected double _deltaTime;



		public MctsSimulator(int playerId, IScore scoring, MctsParameters mctsParameters)
			: base (mctsParameters)
		{
			_playerId = playerId;
			_scoring = scoring;

			_deltaTime = (_mctsParameters.SimulationTime - 2 * _mctsParameters.AggregationTime);
		}

		public override MctsNode Simulate(POGame game)
		{
			POGame gameCopy = game.getCopy();

			// initials root node
			var initLeafs = new List<MctsNode>();
			var root = new MctsNode(_playerId, new List<MctsNode.ScoreExt> { new  MctsNode.ScoreExt(1.0, _scoring) }, gameCopy, null, null);

			// simulate
			MctsNode bestNode = Simulate(_deltaTime, root, ref initLeafs);

			return bestNode;
		}
	}
}
